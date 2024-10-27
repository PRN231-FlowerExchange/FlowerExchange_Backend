using Domain.Constants.Enums;

namespace Application.PostFlower.DTOs
{
    public class FlowerDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Currency Currency { get; set; }
    }

}
