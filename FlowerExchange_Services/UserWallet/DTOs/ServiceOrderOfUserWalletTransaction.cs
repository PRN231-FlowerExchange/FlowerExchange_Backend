namespace Application.UserWallet.DTOs;

public class ServiceOrderOfUserWalletTransaction
{
    public double Amount { get; set; }

    public string Status { get; set; }

    public Guid BuyerId { get; set; }

    // public virtual User Buyer { get; set; }
    //
    // public virtual ICollection<PostService>? PostServices { get; set; }
}