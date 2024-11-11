namespace Application.UserWallet.DTOs;

public class WalletTransactionOfUserDetailsResponse
{
    public Guid Id { get; set; }
    
    public string Type { get; set; }
    
    public double Amount { get; set; }
    
    public string Direction { get; set; }
    
    public string Status { get; set; }
    
    public Guid FromWallet { get; set; }
    
    public string? FromUserFullName { get; set; }
    
    public Guid ToWallet { get; set; }  
    
    public string? ToUserFullName { get; set; }
    
    public string? CreateAt { get; set; }
    
    public ServiceOrderOfUserWalletTransaction? ServiceOrder { get; set; }
    
    public FlowerOrderOfUserWalletTransaction? FlowerOrder { get; set; }
}