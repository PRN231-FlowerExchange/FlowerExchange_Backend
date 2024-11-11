using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Flower : BaseEntity<Flower, Guid>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public Currency Currency { get; set; }

        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }
        
        public virtual FlowerOrder FlowerOrder { get; set; }

        //TODO: Flower Status bỏ vô category


    }
}
