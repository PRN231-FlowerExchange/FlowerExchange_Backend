using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Security.Identity
{
    public interface ICurrentUser
    {
        public bool IsAuthenticated { get; }

        public string UserEmail { get; }

        public Guid UserId { get; }
    }
}
