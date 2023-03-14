using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiTenancyApp.Persistence;
using MultiTenancyApp.Services.Interfaces;
using MultiTenancyApp.Utils;
using System.Security.Claims;

namespace MultiTenancyApp.Services.Implementation
{
    public class ChangeTenantService : IChangeTenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChangeTenantService(ApplicationDbContext context, UserManager<IdentityUser> userManager, 
                                    SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ReplaceTenant(Guid companyId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var claimTenantExists = await _context.UserClaims.
                FirstOrDefaultAsync(x => x.ClaimType == Claims.Names.ClaimTenantId && x.UserId == userId);

            if (claimTenantExists is not null)
            {
                _context.Remove(claimTenantExists);
            }

            var newClaimTenant = new Claim(Claims.Names.ClaimTenantId, companyId.ToString());
            await _userManager.AddClaimAsync(user, newClaimTenant);
            await _signInManager.SignInAsync(user, isPersistent: true);
        }
    }
}
