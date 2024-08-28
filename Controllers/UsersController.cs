using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.Models;
using UserManagement.ViewModel;

namespace UserManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserVM { 
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            
                }).ToListAsync();
            
            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.Select(r=> new RoleVM {  RoleName = r.Name}).ToListAsync();

            var userRolesVM = new AddUserVM
            {
                Roles = roles
            };

            return View(userRolesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserVM model) 
        { 
            if(!ModelState.IsValid)
                return View(model);
            
            if(!model.Roles.Any(r=> r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Select at least one role");
                return View(model);
            }
            var email =await _userManager.FindByEmailAsync(model.Email);
            if (email != null)
            {
                ModelState.AddModelError("Email", "This email is already exist");
                return View(model);
            }    
            if(await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "Username is already exist");
                return View(model);
            }

            var user = new ApplicationUser
            { UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,

            };
            
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRolesAsync(user, model.Roles.Where(r=> r.IsSelected).Select(r => r.RoleName));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var userVM = new EditProfileVM {
                Id = userId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
            return View(userVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
            {
                ModelState.AddModelError("Email", "This email is assigned to another user");
                return View(model);
            }
            
            var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);
           
            if (userWithSameUserName != null && userWithSameUserName.Id != user.Id)
            {
                ModelState.AddModelError("UserName", "This Username is assigned to another user");
                return View(model);
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound();
            var roles = await _roleManager.Roles.ToListAsync();

            var userRolesVM = new UserRolesVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select( role => new RoleVM
                {
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user,role.Name).Result
                }).ToList()
            };

            return View(userRolesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesVM model) 
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();
            var userRoles = await _roleManager.Roles.ToListAsync();
            foreach(var role in model.Roles) //model.Roles == all roles in the database
            { 
                if(userRoles.Any(r => r.Name == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user , role.RoleName);
                
                if(userRoles.Any(r => r.Name == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user , role.RoleName);     
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
