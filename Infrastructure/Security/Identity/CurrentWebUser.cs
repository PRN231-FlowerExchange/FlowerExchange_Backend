using Domain.Security.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Infrastructure.Security.Identity
{
    public class CurrentWebUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context; //HttpContext instance — which only exists during an API request. 

        public CurrentWebUser(IHttpContextAccessor context)
        {
            _context = context;
        }

        public bool IsAuthenticated
        {
            get
            {
                return _context
                            .HttpContext?
                            .User
                            .Identity?
                            .IsAuthenticated ??
                            throw new ApplicationException("User context is unvalable");

            }

        }

        public string UserEmail
        {
            get
            {
                var userEmail = _context
                                .HttpContext?
                                .User
                                .FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                            _context
                                .HttpContext
                                .User
                                .FindFirst("sub")?.Value ?? throw new ApplicationException("User Email context is unvalable");

                return userEmail;

            }
        }

        public Guid UserId
        {
            get
            {
                var userId = _context
                                .HttpContext?
                                .User
                                .FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? throw new ApplicationException("User context is unvalable");

                return Guid.Parse(userId);

            }
        }
    }
}
