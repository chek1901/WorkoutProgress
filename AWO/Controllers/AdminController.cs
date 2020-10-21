using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWO.Services;
using AWO.Services.Interfaces;
using AWO.ViewModels;
using AWO.ViewModels.Admin;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AWO.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GymadminContext context;
        private readonly IAdminService adminService;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            GymadminContext context, IAdminService adminService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
            this.adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var adminManagementModel = new AdminManageViewModel();

            var userModel = new UserListViewModel
            {
                Users = userManager.Users.OrderBy(x => x.UserName)
            };

            var roleModel = new RoleListViewModel()
            {
                Roles = await adminService.GetRoles()
            };

            adminManagementModel.RoleListViewModel = roleModel;
            adminManagementModel.UserListViewModel = userModel;

            return View(adminManagementModel);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            var model = new CreateRoleViewModel();
            return PartialView("_CreateRolePartial", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleResult = await adminService.CreateRole(model.RoleName);

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
            //If modelstate is not valid.
            return PartialView("_CreateRolePartial", model);
        }
       
        [HttpGet]
        public IActionResult ListUser()
        {
            var users = userManager.Users.OrderBy(x => x.UserName);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var viewModel = new RoleListViewModel()
            {
                Roles = await adminService.GetRoles()
            };

            return PartialView("_ListRolePartial", viewModel);
        }
   
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleToDelete = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            var result = await roleManager.DeleteAsync(roleToDelete);

            if (result.Succeeded)
            {
                await context.SaveChangesAsync();
                CreateNotification("Role Deleted");

                var returnModel = new RoleListViewModel()
                {
                    Roles = await adminService.GetRoles()
                };

                return PartialView("_ListRolePartial", returnModel);
            }

            return View("Index");

        }

        [HttpGet]
        public async Task<IActionResult> ListUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} cannot be found";
                return PartialView("_ListUsersInRole", new { });
            }
            var userList = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userList.Add(userRoleViewModel);
                }
            }
            return PartialView("_UsersInRolePartial", userList);

        }


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
