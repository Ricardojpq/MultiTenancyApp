using MultiTenancyApp.Services.Interfaces;
using System.Security.Claims;

namespace MultiTenancyApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpContext _httpContext;

        public UserService(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetUserId()
        {
            if (_httpContext.User.Identity!.IsAuthenticated)
            {
                var claimId = _httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                if (claimId is not null)
                {
                    throw new Exception("El usuario no tiene el claim del ID");
                }

                return claimId.Value;
            }
            else
            {
                throw new Exception("El usuario no esta autenticado");
            }
        }
    }
}
