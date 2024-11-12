using AutoMapper.Configuration.Annotations;
using Domain.Constants.Enums;
using System.Text.Json.Serialization;

namespace Application.PostFlower.DTOs
{
    public class PostUpdateDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public DateTime ExpiredAt { get; set; }


        public string UnitMeasure { get; set; }

        public FlowerDTO Flower { get; set; }
    }
}
