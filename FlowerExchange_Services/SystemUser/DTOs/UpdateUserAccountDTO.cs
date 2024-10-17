using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SystemUser.DTOs
{
    public class UpdateUserAccountDTO
    {
        public Guid UserId { get; set; }
        public string? Fullname { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? NewEmail { get; set; }
        public string? NewUsername { get; set; }
    }
}
