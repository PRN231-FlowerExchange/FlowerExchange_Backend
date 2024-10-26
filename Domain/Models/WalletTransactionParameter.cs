namespace Domain.Models
{
    public class WalletTransactionParameter : QueryStringParameters
    {
        public string? Title { get; set; } = null;
        public WalletTransactionParameter()
        {
            OrderBy = "CreatedAt";
        }
    }
}
