using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons.BaseEntities
{
    public interface ITrackable
    {
        byte[] RowVersion { get; set; }

        DateTimeOffset? CreatedAt { get; set; }

        DateTimeOffset? UpdatedAt { get; set; }

        User? CreatedBy { get; set; }

        User? UpdatedBy { get; set; }

    }
}
