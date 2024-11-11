namespace Application.Order.DTOs;

public class FlowerOrderHistoryListResponse
{
    public Guid Id { get; set; }
    
    public double Amount { get; set; }

    public bool IsRefund { get; set; }

    public string Status { get; set; }

    public Guid BuyerId { get; set; }

    public string? BuyerFullName { get; set; }

    public Guid SellerId { get; set; }

    public string? SellerFullName { get; set; }

    public Guid FlowerId { get; set; }

    public FlowerForFlowerOrderHistoryList Flower { get; set; }
    
}