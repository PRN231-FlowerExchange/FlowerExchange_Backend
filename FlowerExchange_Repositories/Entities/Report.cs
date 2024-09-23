using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Detail {  get; set; }

        public ReportStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid CreateById { get; set; }

        public User CreateBy { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid UpdateById { get; set; }

        public User UpdateBy { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}
