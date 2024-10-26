using Application.PostFlower.DTOs;
using Domain.Constants.Enums;

namespace Application.PlatformService.DTOs
{
    public class ServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public Currency Currency { get; set; }

        public ServiceStatus Status { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public Guid CreateBy { get; set; }

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public Guid UpdateBy { get; set; }

        public List<PostServiceModel>? PostServices { get; set; }
    }
}
