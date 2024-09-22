using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("PostCategory")]
    public class PostCategory
    {
        [Key]
        public Guid PostId { get; set; }

        public Post Post { get; set; }

        [Key]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }  
    }
}
