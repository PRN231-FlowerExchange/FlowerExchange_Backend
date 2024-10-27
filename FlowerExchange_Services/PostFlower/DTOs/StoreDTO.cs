using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.PostFlower.DTOs
{
    public class StoreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}
