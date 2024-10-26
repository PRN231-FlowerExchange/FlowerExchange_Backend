using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

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
