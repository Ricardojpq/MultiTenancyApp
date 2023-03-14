using System.Reflection;
using System.Security.Claims;

namespace MultiTenancyApp.Utils
{
    public class Claims
    {
        //Claims 
        public static readonly Claim AuthorizationProfile = new Claim("Permission", Names.InitialAppMultiTenancy.AuthorizationProfile);
        public static readonly Claim AuthorizationHttpClient = new Claim("Permission", Names.InitialAppMultiTenancy.AuthorizationHttpClient);
        public static readonly Claim AuthorizationSwagger = new Claim("Permission", Names.InitialAppMultiTenancy.AuthorizationSwagger);
        //

        public static class Names
        {
            public const string ClaimTenantId = "TenantId";

            public static class InitialAppMultiTenancy
            {
                public const string AuthorizationProfile = "Profile";
                public const string AuthorizationHttpClient = "HttpClientAuthorization";
                public const string AuthorizationSwagger = "SwaggerAuthorization";
                public static string?[] GetAll()
                {
                    return typeof(InitialAppMultiTenancy)
                        .GetFields(BindingFlags.Public | BindingFlags.Static)
                        .Where(x => x.IsLiteral && x.FieldType == typeof(string))
                        .Select(x => x.GetValue(null)?.ToString())
                        .ToArray();
                }
            }
        }
    }
}
