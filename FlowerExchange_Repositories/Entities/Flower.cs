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
    [Table("Flower")]
    public class Flower
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }

        //public Guid CreateBy { get; set; }

        public DateTime UpdateAt { get; set; }

        //public Guid UpdateBy { get; set; }

        public Currency Currency { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public FlowerOrder FlowerOrder { get; set; }


    }
}
