using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiTenancyApp.Models;
using MultiTenancyApp.Utils;
using System.Security.Claims;

namespace MultiTenancyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
            };
            var resultOfAddToUser = await _userManager.CreateAsync(user, model.Password);

            if (resultOfAddToUser.Succeeded)
            {
                //add Claims to user

                var userClaims = (await _userManager.GetClaimsAsync(user)).Select(x => x.Value).ToList();
                var allClaims = Claims.Names.InitialAppMultiTenancy.GetAll();
                var claimsToInsert = allClaims
                    .Except(userClaims)
                    .Except(new[]
                    {
                            "Permission",
                            Claims.Names.InitialAppMultiTenancy.AuthorizationSwagger,
                            Claims.Names.InitialAppMultiTenancy.AuthorizationHttpClient
                    })
                    .ToList();

                if (claimsToInsert.Any())
                {
                    var newClaims = claimsToInsert.Select(claimName => new Claim("Permission", claimName)).ToList();
                    var TenantId = 1.ToString();
                    newClaims.Add(new Claim("TenantId", TenantId));
                    await _userManager.AddClaimsAsync(user, newClaims);
                }

                var listClaims = new List<Claim>
                {
                    new Claim(Claims.Names.ClaimTenantId,user.Id.ToString())
                };


                var resultOfAddClaimsToUser = await _userManager.AddClaimsAsync(user, listClaims);

                if (resultOfAddClaimsToUser.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultOfAddClaimsToUser.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                foreach (var error in resultOfAddToUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "The Email or Password is a incorrect");
                return View(model);
            }
        }
    }
}
