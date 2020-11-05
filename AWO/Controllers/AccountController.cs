using System;
using System.Linq;
using System.Threading.Tasks;
using AWO.Services;
using AWO.Services.GymUserServices;
using AWO.ViewModels;
using AWO.ViewModels.Account;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AWO.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly GymadminContext context;
        private readonly IGymUserServices _gymUserService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            GymadminContext context, IGymUserServices gymUserService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            _gymUserService = gymUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            try
            {
                var loggedUser = await userManager.GetUserAsync(User);
                var gymUser = _gymUserService.Get(loggedUser.GymUserId).Result;

                var viewModel = new ManageAccountViewModel
                {
                    GymUserId = gymUser.GymUserId,
                    Email = gymUser.Email,
                    FirstName = gymUser?.FirstName ?? string.Empty,
                    LastName = gymUser?.LastName ?? string.Empty,
                    Telephone = gymUser?.Telephone ?? string.Empty,
                    User = loggedUser
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Manage(ManageAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(!context.GymUsers.Any(x => x.Email == model.Email))
                {
                    await _gymUserService.Create(model.FirstName, model.LastName, model.Email, model.Telephone);
                }
                else
                {
                    await _gymUserService.Update(model);
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!context.Users.Any(x => x.UserName == model.UserName))
                {
                    UserExceptionMsg gymUserResult = await _gymUserService.Create(string.Empty, string.Empty, model.Email, string.Empty);

                    switch (gymUserResult)
                    {
                        case UserExceptionMsg.NameExists:
                            ModelState.AddModelError("UserName", "GymUser not able to be created");
                            return View();
                        case UserExceptionMsg.Success:
                            break;
                        case UserExceptionMsg.Error:
                            ModelState.AddModelError("UserName", "GymUser not able to be created");
                            return View();
                    }

                    var gymId = context.GymUsers.Where(gymUser => gymUser.Email == model.Email)
                        .Select(user => user.GymUserId).SingleOrDefault();

                    var newUser = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        EmailConfirmed = true,
                        RoleName = Enum.GetName(typeof(RoleEnum), RoleEnum.User),
                        GymUserId = gymId
                    };

                    var result = await userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(newUser, isPersistent: false);
                        return RedirectToAction("Index", "Home");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("customError", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "User already exists");
                    return View();
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
