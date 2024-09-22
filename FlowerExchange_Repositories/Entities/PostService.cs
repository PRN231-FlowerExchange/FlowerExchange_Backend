using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("PostService")]
    public class PostService
    {
        [Key]
        public Guid Id { get; set; }

        public PostServiceStatus Status { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public Guid ServiceId { get; set; }

        public Service Service { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public ServiceOrder? ServiceOrder { get; set; }


    }
}
