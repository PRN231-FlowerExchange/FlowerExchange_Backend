using Domain.Constants.Enums;

namespace Application.UserWallet.DTOs;

public class WalletTransactionListResponse
{
    public Guid Id { get; set; }
    
    public double Amount { get; set; }

    public string Status { get; set; }

    public string Type { get; set; }

    public Guid? FromWallet { get; set; }

    public Guid? ToWallet { get; set; }
}