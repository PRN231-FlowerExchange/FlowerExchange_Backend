using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{
    public class PostService : BaseEntity<PostService, Guid>
    {
        public PostServiceStatus Status { get; set; }

        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }

        public Guid ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public virtual ServiceOrder? ServiceOrder { get; set; }

        public DateTime CreteaAt { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
