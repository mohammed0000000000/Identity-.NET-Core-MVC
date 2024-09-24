using IdentityDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Controllers
{
    [Authorize(Roles = "Admin,Hr")] // Cookie foud or not ==> claim type == "Admin"
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager) {
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> TestRoles() {
            IdentityRole identityRole = new IdentityRole() {
                Name = "Admin"
            };
            await roleManager.CreateAsync(identityRole);
            var roles = await roleManager.Roles.ToListAsync();
            return Json(roles);
        }
        [AllowAnonymous]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult New() {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(RoleViewModel roleViewModel){
            if(ModelState.IsValid){
                IdentityRole roleModel = new IdentityRole() { Name = roleViewModel.RoleName };
                var result = await roleManager.CreateAsync(roleModel);

                if(result.Errors.Count() > 0){
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError($"{error.Code}", error.Description);
                    }
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View("New", roleViewModel);
        }
    }
}
