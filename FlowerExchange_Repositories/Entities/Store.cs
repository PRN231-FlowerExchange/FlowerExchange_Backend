using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Store")]
    public class Store
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public Guid OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<Post>? Posts { get; set; }

    }
}
