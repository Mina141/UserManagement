using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserManagement.ViewModel;

namespace UserManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager) 
        { 
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
       
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormVM model) 
        {
            var roles = await _roleManager.Roles.ToListAsync();


            if (!ModelState.IsValid) 
            {
                return View(nameof(Index),roles);
               
            }
            if(await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Role name is exists!");
                return View(nameof(Index),roles);
                
            }
            await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            return RedirectToAction(nameof(Index));
        }

    }
}
