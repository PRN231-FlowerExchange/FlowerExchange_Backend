using System.Text.Json.Serialization;

namespace Application.Payment.DTOs
{
    public class IPNResponseVNPAY
    {
        [JsonPropertyName("RspCode")]
        public string RspCode { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}
