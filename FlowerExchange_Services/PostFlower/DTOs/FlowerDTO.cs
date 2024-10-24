using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PostFlower.DTOs
{
    public class FlowerDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Currency Currency { get; set; }
    }

}
