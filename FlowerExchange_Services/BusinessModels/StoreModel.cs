using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class StoreModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public Guid OwnerId { get; set; }

        public UserModel Owner { get; set; }

        public List<PostModel>? Posts { get; set; }

    }
}
