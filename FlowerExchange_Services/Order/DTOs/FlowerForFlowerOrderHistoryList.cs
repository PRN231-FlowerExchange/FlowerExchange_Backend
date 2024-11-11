namespace Application.Order.DTOs;

public class FlowerForFlowerOrderHistoryList
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public double Price { get; set; }

    public string Currency { get; set; }

    public Guid PostId { get; set; }

    // public virtual Post Post { get; set; }
}