using Admin.Dashboard.Models.Roles;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class RolesController(RoleManager<IdentityRole> _roleManager,
                                 UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Validation Error !");
                return RedirectToAction(nameof(Index), await _roleManager.Roles.ToListAsync());
            }

            var roleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = model.Name });
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.AddModelError("Name", "Role Already Exist");
            return RedirectToAction(nameof(Index), await _roleManager.Roles.ToListAsync());
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                TempData["ErrorMessage"] = "Role not found";
                return RedirectToAction(nameof(Index));
            }
            var VM = new UpdateRoleViewModel
            {
                Id = id,
                Name = role.Name!
            };
            return View(VM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExist)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);

                    if (role is not null)
                    {
                        role.Name = model.Name;
                        var result = await _roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Role Edited Successfully";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Already Exist");
                    return View(model);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role Deleted Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete role";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Role not found";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
