using Microsoft.AspNetCore.Identity;

namespace MultiTenancyApp.Extensions
{
    public static class ExtensionType
    {

        public static bool SkipTenantValidation(this Type t)
        {
            var listBool = new List<bool>
            {
                t.IsAssignableFrom(typeof(IdentityUser)),
                t.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                t.IsAssignableFrom(typeof(IdentityRole)),
                t.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                t.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                t.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                t.IsAssignableFrom(typeof(IdentityUserClaim<string>))
            };

            var result = listBool.Aggregate((b1, b2) => b1 || b2);
            return result;

        }
        
    }
}
