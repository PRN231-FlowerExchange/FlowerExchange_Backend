using Application.PostFlower.DTOs;

namespace Application.UserApplication.DTOs
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
