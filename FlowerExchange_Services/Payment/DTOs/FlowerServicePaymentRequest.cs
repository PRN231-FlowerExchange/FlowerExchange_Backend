using System.ComponentModel.DataAnnotations;

namespace Application.Payment.DTOs;

public class FlowerServicePaymentRequest
{
    [Required]
    public Guid PostId { get; set; }
}