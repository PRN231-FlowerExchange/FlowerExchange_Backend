using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Security.IdentityProvider
{
    public interface IUserAccount
    {
        Guid Id { get; set; }

        string Username { get; set; }

        string Email { get; set; }

        string Password { get; set; }

        string Fullname { get; set; }

    }
}
