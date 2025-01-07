using System.Diagnostics;
using CVManager.DAL.Entities;
using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CVManager.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userMngr, SignInManager<User> signInMngr, ILogger<AccountController> logger)
        {
            this.userManager = userMngr;
            this.signInManager = signInMngr;
            this._logger = logger;
        }

        [HttpGet]

        public IActionResult LogIn()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.AnvandarNamn, loginViewModel.Losenord, 
                    isPersistent: loginViewModel.RememberMe, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]

        public IActionResult Register()
        {
            UserRegisterViewModel registerViewModel = new UserRegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Register(UserRegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.UserName = registerViewModel.AnvandarNamn;

                var result = await userManager.CreateAsync(user, registerViewModel.Losenord);

                if (result.Succeeded)
                {
                    Debug.WriteLine("Det gick");
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(registerViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> LoggaUt()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
