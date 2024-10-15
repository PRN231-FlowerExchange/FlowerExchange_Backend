using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Post.DTOs
{
    public class StoreDTO
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}
