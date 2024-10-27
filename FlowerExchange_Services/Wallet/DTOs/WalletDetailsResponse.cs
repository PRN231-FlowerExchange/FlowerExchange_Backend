using Domain.Constants.Enums;

namespace Application.Wallet.DTOs;

public class WalletDetailsResponse
{
    public double TotalBalance { get; set; }

    public string Currency { get; set; }

    public Guid UserId { get; set; }
}