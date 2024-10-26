namespace Application.Payment.DTOs
{
    public class PostServicePaymentRequest
    {
        public Guid[] PostIds { get; set; } = [];

        public Guid[] ServiceIds { get; set; } = [];

    }
}
