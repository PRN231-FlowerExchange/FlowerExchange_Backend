using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class PostCategoryModel
    {
        public Guid PostId { get; set; }

        public PostModel Post { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryModel Category { get; set; }  
    }
}
