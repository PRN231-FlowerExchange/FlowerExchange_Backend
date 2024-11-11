namespace Application.UserWallet.DTOs;

public class ServiceOrderOfUserWalletTransaction
{
    public Guid Id { get; set; }
    
    public double Amount { get; set; }

    public string Status { get; set; }

    public Guid BuyerId { get; set; }

    public string BuyerName { get; set; }
    //
    // public virtual ICollection<PostService>? PostServices { get; set; }
}