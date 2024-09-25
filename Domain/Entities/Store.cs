using Domain.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Store : BaseEntity<Store, Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public Guid OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

    }
}
