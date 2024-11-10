namespace Application.UserWallet.DTOs;

public class FlowerOrderOfUserWalletTransaction
{
    public double Amount { get; set; }

    public bool IsRefund { get; set; }

    public string Status { get; set; }

    public Guid BuyerId { get; set; }

    // public virtual User Buyer { get; set; }

    public Guid SellerId { get; set; }

    // public virtual User Seller { get; set; }

    public Guid FlowerId { get; set; }

    // public virtual Flower Flower { get; set; }
}