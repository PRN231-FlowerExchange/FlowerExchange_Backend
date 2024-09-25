using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Service : BaseEntity<Service, Guid>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public Currency Currency { get; set; }

        public ServiceStatus Status { get; set; }

        public virtual ICollection<PostService>? PostServices { get; set; }
    }
}
