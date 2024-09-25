using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Security.Identity
{
    public interface IPasswordHasher
    {
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }
}
