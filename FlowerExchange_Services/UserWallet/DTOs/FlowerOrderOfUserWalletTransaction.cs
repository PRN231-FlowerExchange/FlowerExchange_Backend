using Application.PostFlower.DTOs;
using Domain.Entities;

namespace Application.UserWallet.DTOs;

public class FlowerOrderOfUserWalletTransaction
{
    public Guid Id { get; set; }
    
    public double Amount { get; set; }

    public bool IsRefund { get; set; }

    public string Status { get; set; }

    public Guid BuyerId { get; set; }

    public string BuyerName { get; set; }

    public Guid SellerId { get; set; }

    public string SellerName { get; set; }

    public Guid FlowerId { get; set; }

     public virtual FlowerDTO Flower { get; set; }
}