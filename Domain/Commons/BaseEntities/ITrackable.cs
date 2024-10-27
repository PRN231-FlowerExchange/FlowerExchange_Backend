using Domain.Entities;

namespace Domain.Commons.BaseEntities
{
    public interface ITrackable
    {
        string ConcurrencyStamp { get; set; }

        DateTimeOffset? CreatedAt { get; set; }

        DateTimeOffset? UpdatedAt { get; set; }

        User? CreatedBy { get; set; }

        User? UpdatedBy { get; set; }

    }
}
