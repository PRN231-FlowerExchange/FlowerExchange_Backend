namespace Application.UserWallet.DTOs;

public class WalletTransactionOfUserListResponse
{
    public Guid Id { get; set; }
    
    public string Type { get; set; }
    
    public double Amount { get; set; }
}