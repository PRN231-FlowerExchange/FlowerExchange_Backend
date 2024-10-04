using Domain.Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security.Identity
{
    public class AnoymousUser : ICurrentUser
    {
        public bool IsAuthenticated => false;

        public Guid UserId => Guid.Empty;

        public string UserEmail => null;
    }
}
