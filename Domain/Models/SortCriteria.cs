using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SortCriteria
    {
        public string SortBy { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
    }
}
