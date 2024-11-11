using Application.Category.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PostFlower.DTOs
{
    public class CategoryDetailDTO
    {
        public Guid CategoryId { get; set; }
        public CategoryDetailDTO Category { get; set; }
    }
}
