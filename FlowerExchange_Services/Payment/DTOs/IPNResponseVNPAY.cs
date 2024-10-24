using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
