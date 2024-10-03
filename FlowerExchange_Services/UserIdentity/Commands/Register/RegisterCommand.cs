using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Persistence;
using Persistence.RepositoryAdapter;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ValidationException = Domain.Exceptions.ValidationException;


namespace Application.UserIdentity.Commands.Register
{
    public record class RegisterCommand : IRequest<string>
    {
        [EmailAddress(ErrorMessage = "Invalid Email !")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        [Required]
        public string Fullname { get; init; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterCommand> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;
        private readonly RoleManager<Role> _roleManager;


        public RegisterCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<RegisterCommand>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Email = request.Email.ToLower(),
                Fullname = request.Fullname,
                PhoneNumber = request.PhoneNumber ?? null,
                Status = UserStatus.Active
            };

            // Start a transaction using DbContext
            using var transaction = await _unitofwork.Context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Starting transaction for user registration.");

                // Check if role exists
                var roleExists = await _roleManager.RoleExistsAsync(RoleType.Customer.GetDisplayName());

                if (!roleExists)
                {
                    var role = new Role { RoleType = RoleType.Customer, Name = RoleType.Customer.GetDisplayName() };
                    var roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Failed to create role.");
                    }
                }

                // Set username for the user
                await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);

                // Create user in UserManager
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    List<ValidationFailure> errors = result.Errors.Select(e => new ValidationFailure(e.Code, e.Description)).ToList();
                    throw new ValidationException(errors);
                }

                // Retrieve saved user
                var userSaved = await _userManager.FindByEmailAsync(request.Email);

                // Add the user to the role
               await _userManager.AddToRoleAsync(user, RoleType.Customer.GetDisplayName());

                // Commit the transaction if everything succeeded
                await _unitofwork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();

                return "Register successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user. Rolling back transaction.");

                // Rollback transaction if any error occurs
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
