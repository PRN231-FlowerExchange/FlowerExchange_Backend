using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.DTOs
{
    public class PostServicePaymentRequest
    {
        public Guid[] PostIds { get; set; } = [];

        public Guid[] ServiceIds { get; set; } = [];

    }
}
