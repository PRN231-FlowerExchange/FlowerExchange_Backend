namespace Application.UserStore.DTOs
{
    public class StoreCreateDTO
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string CoverPhotoUrl { get; set; }

        public string AvatarUrl { get; set; }

        public string Descriptions { get; set; }

        public string Slug { get; set; }

        public HashSet<string>? Phones { get; set; }

    }
}
