using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.IdentityModule;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class AccountController(UserManager<ApplicationUser> _userManager,
                                 SignInManager<ApplicationUser> _signInManager) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(loginDTO);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
            if (!result.Succeeded || (!await _userManager.IsInRoleAsync(user, "Admin") && (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))))
            {
                ModelState.AddModelError("", "You Are Not Authorized");
                return View(loginDTO);
            }
            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
