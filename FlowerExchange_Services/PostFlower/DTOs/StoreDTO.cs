namespace Application.PostFlower.DTOs
{
    public class StoreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public Guid OwnerId { get; set; }

    }
}
