using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security.TokenProvider
{
    public class JwtConfigOptions
    {
        public string JwtSecret { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtValidAudience { get; set; }
    }
}
