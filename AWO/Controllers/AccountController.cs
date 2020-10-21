using System.Linq;
using System.Threading.Tasks;
using AWO.ViewModels;
using AWO.ViewModels.Account;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AWO.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly GymadminContext context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            GymadminContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var loggedUser = await userManager.GetUserAsync(User);
            //var role = await userManager.GetRolesAsync(loggedUser);
            var gymUser = await context.GymUsers.SingleOrDefaultAsync(user => user.GymUserId == loggedUser.GymUserId);

            var viewModel = new ManageAccountViewModel
            {
                GymUserId = gymUser.GymUserId,
                Email = gymUser.Email,
                FirstName = gymUser?.FirstName ?? "", 
                LastName = gymUser?.LastName ?? "",
                Telephone = gymUser?.Telephone ?? "",
                User = loggedUser
                
            };
            return View(viewModel);
        }   

        [HttpPost]
        public async Task<IActionResult> Manage(ManageAccountViewModel model)
        {

            if (ModelState.IsValid)
            {
                if(!context.GymUsers.Any(x => x.Email == model.Email))
                {
                    var newGymUser = new GymUsers()
                    {
                        Email = model.Email,
                        Telephone = model.Telephone,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };
                    await context.GymUsers.AddAsync(newGymUser);
                }
                else
                {
                    var user = context.GymUsers.SingleOrDefault(user => user.Email == model.Email);
                    user.Telephone = model?.Telephone ?? "";
                    user.FirstName = model?.FirstName ?? "";
                    user.LastName = model?.LastName ?? "";
                    context.GymUsers.Update(user);
                }
                await context.SaveChangesAsync();
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!context.Users.Any(x => x.UserName == model.UserName))
                {

                    var gymUser = new GymUsers()
                    {
                        Email = model.Email
                    };
                    await context.AddAsync(gymUser);
                    var creationOk = await context.SaveChangesAsync();

                    var gymId = context.GymUsers.Where(gymUser => gymUser.Email == model.Email)
                        .Select(user => user.GymUserId).SingleOrDefault();

                    var newUser = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        EmailConfirmed = true,
                        GymUserId = gymId
                    };

                    if(creationOk < 1)
                    {
                        ModelState.AddModelError("UserName", "GymUser not able to be created");
                        return View();
                    }


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
        public IActionResult Login()
        {
            return View();
        }
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
