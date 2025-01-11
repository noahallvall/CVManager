using System.Diagnostics;
using System.Linq;
using CVManager.DAL.Context;
using CVManager.DAL.Entities;
using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace CVManager.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private CVContext cVContext;
        private ILogger<AccountController> _logger;
        private int AktivtProjekt;

        public AccountController(UserManager<User> userMngr, SignInManager<User> signInMngr, ILogger<AccountController> logger, CVContext context)
        {
            this.userManager = userMngr;
            this.signInManager = signInMngr;
            this.cVContext = context;
            this._logger = logger;
            AktivtProjekt = 0;
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
        public IActionResult Registrera()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrera(UserRegisterViewModel userRegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = userRegisterViewModel.AnvandarNamn,
                    IsPrivateProfile = userRegisterViewModel.IsPrivateProfile // Tilldela här
                };

                var result = await userManager.CreateAsync(user, userRegisterViewModel.Losenord);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    Console.WriteLine(user.Id);

                    if (ModelState.IsValid)
                    {
                        Console.WriteLine("Funkar");

                        var cv = new CV
                        {
                            ProfilePicturePath = null, 
                            Summary = "",
                            UserId = user.Id
                        };

                        cVContext.CVs.Add(cv);
                        await cVContext.SaveChangesAsync();


                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Code} - {error.Description}");
                        }
                    }

                }
            }
            return View(userRegisterViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> LoggaUt()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> KontoAlt()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Användare ej inloggad");
            }

            KontoAltViewModel kontoAltViewModel = new KontoAltViewModel
            {
                FirstName = user?.FirstName,
                LastName = user?.LastName,
                Address = user?.Address,
                Email = user?.Email,
                Phone = user?.Phone
                
            };

            return View(kontoAltViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> KontoAlt(KontoAltViewModel kontoAltViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(kontoAltViewModel);
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var passwordCheck = await userManager.CheckPasswordAsync(user, kontoAltViewModel.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError("CurrentPassword", "Nuvarande lösenord är felinmatat.");
                return View(kontoAltViewModel); 
            }

            if (!string.IsNullOrEmpty(kontoAltViewModel.Losenord))
            {
                var passwordChangeResult = await userManager.ChangePasswordAsync(user, kontoAltViewModel.CurrentPassword, kontoAltViewModel.Losenord);
                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    return View(kontoAltViewModel);
                }
            }

            user.FirstName = kontoAltViewModel.FirstName;
            user.LastName = kontoAltViewModel.LastName;
            user.Address = kontoAltViewModel.Address;
            user.IsPrivateProfile = kontoAltViewModel.IsPrivateProfile;
            user.Email = kontoAltViewModel.Email;
            user.Phone = kontoAltViewModel.Phone;

            var updateResult = await userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return RedirectToAction("Index", "Home"); 
            }

            foreach (var error in updateResult.Errors)
            {
                Console.WriteLine($"Error: {error}");
            }

            return View(kontoAltViewModel); 
        }

        [HttpGet]
        public async Task<IActionResult> CVAlt()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Användare ej inloggad");
                return NotFound();
            }

            var userId = user.Id;
            var cv = await cVContext.CVs
                .FirstOrDefaultAsync(c => c.UserId == userId);

            

            var skill = await cVContext.Skills
                .FirstOrDefaultAsync(c => c.CVId == cv.CVId);

            if (cv == null)
            {
                Console.WriteLine("CV hittades inte för användaren");
                return NotFound(); // Eller hantera detta
            }

           CVAltViewModel cVAltViewModel = new CVAltViewModel();
            
            cVAltViewModel.Summary = cv.Summary;
            cVAltViewModel.ProfilePicturePath = cv.ProfilePicturePath;
            

            return View(cVAltViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CVAlt(CVAltViewModel cVAltViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync (User);

                if (user == null)
                {
                    Console.WriteLine("Ej inloggad");
                    return NotFound();
                }

                var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

                Skill skill = new Skill()
                {

                    SkillName = cVAltViewModel.SkillName,
                    CVId = cv.CVId

                };

                Education education = new Education()
                {

                    Institution = cVAltViewModel.Institution,
                    EducationName= cVAltViewModel.EducationName,
                    CVId = cv.CVId

                };

                Experience experience = new Experience()
                {

                    CompanyName = cVAltViewModel.CompanyName,
                    Role = cVAltViewModel.Role,
                    CVId = cv.CVId

                };

                if (cv == null)
                {
                    Console.WriteLine("CV kan ej hittas"); 
                    return NotFound();
                }

                cv.Summary = cVAltViewModel.Summary;
                cv.ProfilePicturePath=cv.ProfilePicturePath;

                cVContext.Experiences.Update(experience);
                await cVContext.SaveChangesAsync();
                cVContext.Educations.Update(education);
                cVContext.Skills.Update(skill);
                await cVContext.SaveChangesAsync();
                cVContext.CVs.Update(cv);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(cVAltViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProjektAlt()
        {
            User user = new User();

            if (ModelState.IsValid)
            {
                user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    Console.WriteLine("Ej inloggad");
                    return NotFound();
                }  
            }


            var allaProjekt = cVContext.Projects
                .Where(p => p.ownerId == user.Id)
                .Select(p => new ProjektAltViewModel
                {
                    ProjectName = p.ProjectName,
                    ProjectId = p.ProjectId,
                    ProjectDescription = p.ProjectDescription
                })
                .ToList();

            return View(allaProjekt);
        }

        
        [HttpGet]
        public IActionResult ChangeProject(int id)
        {
            Console.WriteLine($"Project ID: {id}");

            var projekt = cVContext.Projects.FirstOrDefault(p => p.ProjectId == id);

            if (projekt == null)
            {
                return NotFound();
            }

            ChangeProjectViewModel changeProjectViewModel = new ChangeProjectViewModel
            {
                ProjectId = projekt.ProjectId,
                ProjectDescription = projekt.ProjectDescription,
                ProjectName = projekt.ProjectName
            };

            return View(changeProjectViewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProject(ChangeProjectViewModel changeProjectViewModel)
        {

            Console.WriteLine("Aktivt Projekt =" + changeProjectViewModel.ProjectId.ToString() );
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error);
                }

                var projekt = await cVContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == changeProjectViewModel.ProjectId);

                if (projekt == null)
                {
                    Console.WriteLine("Projektet hittades inte.");
                }

                projekt.ProjectName =changeProjectViewModel.ProjectName;
                projekt.ProjectDescription = changeProjectViewModel.ProjectDescription;

                cVContext.Projects.Update(projekt);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Home", "Index");
            }

            return View(changeProjectViewModel);
        }
    }
}
