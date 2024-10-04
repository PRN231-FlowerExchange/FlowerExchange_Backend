using Domain.Constants.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<FlowerExchangeDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }
    public class FlowerExchangeDbContextInitialiser
    {
        private readonly ILogger<FlowerExchangeDbContextInitialiser> _logger;
        private readonly FlowerExchangeDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public FlowerExchangeDbContextInitialiser(ILogger<FlowerExchangeDbContextInitialiser> logger, FlowerExchangeDbContext context, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {

            // Seed data weather
            // Seed, if necessary
            IEnumerable<WeatherForecast> list = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });

            if (!_context.WeatherForecast.Any())
            {
                _context.WeatherForecast.AddRange(list);

                await _context.SaveChangesAsync();
            }


            var roleManager = _serviceProvider.GetService<RoleManager<Role>>();
            IEnumerable<Role> roles = Enum.GetValues(typeof(RoleType))
            .Cast<RoleType>()
            .Select(roleType => new Role
            {
                RoleType = roleType,
                Name = roleType.GetDisplayName()
            });

            if (!_context.Roles.Any())
            {
                foreach (var role in roles)
                {
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

    }
}
