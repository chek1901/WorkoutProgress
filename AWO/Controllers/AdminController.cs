using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWO.Services;
using AWO.Services.Interfaces;
using AWO.ViewModels;
using AWO.ViewModels.Admin;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AWO.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class AdminController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GymadminContext _context;
        private readonly IAdminService _adminService;

        public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
            GymadminContext context, IAdminService adminService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var adminManagementModel = new AdminManageViewModel();

            var userModel = new UserListViewModel
            {
                Users = _userManager.Users.OrderBy(x => x.UserName),
                Roles = _roleManager.Roles.OrderBy(x => x.OriginDate)
            };

            var roleModel = new RoleListViewModel()
            {
                Roles = await _adminService.GetRoles()
            };

            adminManagementModel.RoleListViewModel = roleModel;
            adminManagementModel.UserListViewModel = userModel;

            return View(adminManagementModel);
        }

        [HttpGet]
        public IActionResult CreateRole() => PartialView("_CreateRolePartial", new CreateRoleViewModel());

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleResult = await _adminService.CreateRole(model.RoleName);

                switch (roleResult)
                {
                    case RoleCreation.Success:
                        return PartialView("_CreateRolePartial", model);

                    case RoleCreation.RoleExist:
                        ModelState.AddModelError("RoleName", "Rolename already exists");
                        return PartialView("_CreateRolePartial", model);

                    case RoleCreation.CreationError:
                        ModelState.AddModelError("RoleName", "Special error occured with the server");
                        return PartialView("_CreateRolePartial", model);
                }
            }

            return PartialView("_CreateRolePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleToDelete = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                CreateNotification("Role Deleted");

                var returnModel = new RoleListViewModel()
                {
                    Roles = await _adminService.GetRoles()
                };

                return PartialView("_ListRolePartial", returnModel);
            }

            return View("Index");

        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var viewModel = new RoleListViewModel()
            {
                Roles = await _adminService.GetRoles()
            };

            return PartialView("_ListRolePartial", viewModel);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            return PartialView("_ListUserPartial", new UserListViewModel
            {
                Users = _userManager.Users.OrderBy(x => x.UserName),
                Roles = _roleManager.Roles.OrderBy(r => r.OriginDate)
            });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var model = new UpdateUserViewModel();
            var user = await _adminService.GetUser(userId);
           
            if(user!= null)
            {
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Roles = _adminService.GetRolesAsSelectList(user.RoleName);
                
                return PartialView("Users/_UpdateUserPartial", model);
            }

            return RedirectToAction("ListUsers");
           
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateResult = await _adminService.UpdateUser(model);

                switch (updateResult)
                {
                    case UpdateUserEnum.Success:
                        return PartialView("Users/_UpdateUserPartial", model);

                    case UpdateUserEnum.UpdateError:
                        ModelState.AddModelError("UserName", "Something went wrong, try again!");
                        return PartialView("Users/_UpdateUserPartial", model);

                    case UpdateUserEnum.NoUserToUpdate:
                        ModelState.AddModelError("UserName", "No user to update");
                        return PartialView("Users/_UpdateUserPartial", model);

                }
            }

            return PartialView("_CreateUserPartial", new UpdateUserViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> ListUsersInRole(string roleId)
        {
            var userList = new List<UserRoleViewModel>();
            var role = await _roleManager.FindByIdAsync(roleId);
            
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} cannot be found";
                return PartialView("_ListUsersInRole", new { });
            }

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userList.Add(userRoleViewModel);
                }
            }

            return PartialView("_UsersInRolePartial", userList);
        }

        [NonAction]
        public IActionResult Notifications()
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as IEnumerable<string> ?? Enumerable.Empty<string>();
            return PartialView("_NotificationsPartial", notifications);
        }

        [NonAction]
        private void CreateNotification(string message)
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as List<string> ?? new List<string>();
            notifications.Add(message);
            TempData["Notifications"] = notifications;
        }
    }
}
