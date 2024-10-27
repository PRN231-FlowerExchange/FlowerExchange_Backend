using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{

    public class Report : BaseEntity<Report, Guid>
    {
        public int Rating { get; set; }

        public string Detail { get; set; }

        public ReportStatus Status { get; set; }

        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }

        public Guid reportByUserId { get; set; }

        public virtual User reportBy { get; set; }
    }
}
