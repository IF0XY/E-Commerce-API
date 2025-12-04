using Admin.Dashboard.Models.Roles;
using Admin.Dashboard.Models.Users;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class UsersController(RoleManager<IdentityRole> _roleManager,
                                 UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Email = u.Email,
                UserName = u.UserName,
                Roles = _userManager.GetRolesAsync(u).Result,
            }).ToListAsync();
            return View(users);
        }

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var roles = await _roleManager.Roles.ToListAsync();

            var userModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.DisplayName,
                Roles = roles.Select(r => new UpdateRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result,
                }).ToList()
            };
            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return RedirectToAction(nameof(Index));

            var rolesForUser = await _userManager.GetRolesAsync(user);

            // Role was granted -> UnCheck for the Role (Remove this Role)
            // Role was not granted -> Check For The Role (Add This Role)
            foreach (var role in model.Roles)
            {
                if (rolesForUser.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!rolesForUser.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Index));
        } 
        #endregion

    }
}
