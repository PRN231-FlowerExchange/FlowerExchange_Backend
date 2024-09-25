using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Report : BaseEntity<Report, Guid>
    {
        public int Rating { get; set; }

        public string Detail { get; set; }

        public ReportStatus Status { get; set; }

        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }

        public Guid reportByUserId {  get; set; }

        public virtual User reportBy { get; set; }
    }
}
