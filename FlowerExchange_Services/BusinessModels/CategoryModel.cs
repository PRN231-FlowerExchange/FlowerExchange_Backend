using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CategoryStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        public List<PostCategoryModel>? PostCategories { get; set; }
    }
}
