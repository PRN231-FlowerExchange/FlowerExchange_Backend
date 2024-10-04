using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PostFlower.DTOs
{
    public class GetPostQueryRequest
    {
        public string StoreId { get; set; }
        public string SellerId { get; set; }
    }
}
