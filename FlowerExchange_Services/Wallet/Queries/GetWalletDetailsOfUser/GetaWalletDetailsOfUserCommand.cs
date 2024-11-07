using System.ComponentModel.DataAnnotations;
using Application.Wallet.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Wallet.Queries.GetWalletDetailsOfUser;

public record GetaWalletDetailsOfUserCommand : IRequest<WalletDetailsResponse>
{
    [Required]
    public Guid UserId { get; init; }

    public GetaWalletDetailsOfUserCommand(Guid userId)
    {
        UserId = userId;
    }
}

public class GetaWalletDetailsOfUserCommandHandler : IRequestHandler<GetaWalletDetailsOfUserCommand, WalletDetailsResponse>
{
    private IWalletRepository _walletRepository;
    private IUserRepository _userRepository;
    private IMapper _mapper;

    public GetaWalletDetailsOfUserCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository, IMapper mapper)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<WalletDetailsResponse> Handle(GetaWalletDetailsOfUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user 
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.UserId} not found in " +
                                            $"GetaWalletDetailsOfUserCommand!");
            }
        
            // Get wallet of user
            var wallet = await _walletRepository.GetByUserId(request.UserId);
            if (wallet == null)
            {
                throw new NotFoundException($"Wallet of user with id {request.UserId} not found in " +
                                            $"GetaWalletDetailsOfUserCommand!");
            }
        
            // Mapping to WalletDetailsResponse to response
            var response = _mapper.Map<WalletDetailsResponse>(wallet);
            return response;
        }
        catch
        {
            throw;
        }
    }
}