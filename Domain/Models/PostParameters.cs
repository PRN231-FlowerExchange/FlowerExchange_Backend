namespace Domain.Models
{
    public class PostParameters : QueryStringParameters
    {
        public string? Title { get; set; } = null;
        public PostParameters()
        {
            OrderBy = "Title";
        }
    }
}
