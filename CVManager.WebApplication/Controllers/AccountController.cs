using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using CVManager.DAL.Context;
using CVManager.DAL.Entities;
using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString(); // Lägger in alla olästa meddelanden i viewbag
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.AnvandarNamn, loginViewModel.Losenord,
                    isPersistent: loginViewModel.RememberMe, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Losenord", "Felaktigt lösenord, vänligen försök igen.");
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
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
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

                        var cv = new CV //Skapar nytt CV kopplat till användaren
                        {
                            Summary = "",
                            UserId = user.Id
                        };

                        cVContext.CVs.Add(cv); //Lägger till entitet
                        await cVContext.SaveChangesAsync(); //Sparar entitet i databas


                        return RedirectToAction("Index", "Home");
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
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
            var user = await userManager.GetUserAsync(User); //Hämtar aktuell användare

            if (user == null)
            {
                Console.WriteLine("Användare ej inloggad");
            }

            KontoAltViewModel kontoAltViewModel = new KontoAltViewModel //Skapar en ny vy med hjälp av data som är kopplad till användare
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

            var passwordCheck = await userManager.CheckPasswordAsync(user, kontoAltViewModel.CurrentPassword); //Används för att kolla ifall det bekräftade lösenordet stämmer
            if (!passwordCheck)
            {
                ModelState.AddModelError("CurrentPassword", "Nuvarande lösenord är felinmatat.");
                return View(kontoAltViewModel);
            }

            if (!string.IsNullOrEmpty(kontoAltViewModel.Losenord)) //Kodblock för att kolla ifall det inmatade lösenordet är null eller tomt, då ska ingen ändring av lösenordet ske
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

            //Här läggs all data i kontot

            user.FirstName = kontoAltViewModel.FirstName;
            user.LastName = kontoAltViewModel.LastName;
            user.Address = kontoAltViewModel.Address;
            user.IsPrivateProfile = kontoAltViewModel.IsPrivateProfile;
            user.Email = kontoAltViewModel.Email;
            user.Phone = kontoAltViewModel.Phone;

            var updateResult = await userManager.UpdateAsync(user); //Här updateras/sparas datan

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
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Användare ej inloggad");
                return NotFound();
            }

            var userId = user.Id;
            var cv = await cVContext.CVs
                .FirstOrDefaultAsync(c => c.UserId == userId); //Hämtar alla CV som hör till användare som hämtats tidigare



            var skill = await cVContext.Skills
                .FirstOrDefaultAsync(c => c.CVId == cv.CVId); //Hämtar skills som hör till CV

            if (cv == null)
            {
                Console.WriteLine("CV hittades inte för användaren");
                return NotFound(); // Eller hantera detta
            }

            CVAltViewModel cVAltViewModel = new CVAltViewModel();

            cVAltViewModel.Summary = cv.Summary;
            //cVAltViewModel.ProfilePicturePath = cv.ProfilePicturePath;

            //Skapar en viewmodel med överensstämmande data



            return View(cVAltViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CVAlt(CVAltViewModel cVAltViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    Console.WriteLine("Ej inloggad");
                    return NotFound();
                }
                var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

                // Spara uppladad bild
                if (cVAltViewModel.FileUpload != null && cVAltViewModel.FileUpload.FormFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await cVAltViewModel.FileUpload.FormFile.CopyToAsync(memoryStream);

                        // Upload the file if it's less than 2 MB
                        if (memoryStream.Length < 2097152)
                        {
                            cv.ProfilePicture = memoryStream.ToArray();
                        }
                        else
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                    }
                }
                var existingSkill = await cVContext.Skills
                 .FirstOrDefaultAsync(s => s.SkillName == cVAltViewModel.SkillName && s.CVId == cv.CVId);


                var existingEdNa = await cVContext.Educations
                 .FirstOrDefaultAsync(s => s.EducationName == cVAltViewModel.EducationName && s.CVId == cv.CVId);

                var existingEdIn = await cVContext.Educations
                 .FirstOrDefaultAsync(s => s.Institution == cVAltViewModel.Institution && s.CVId == cv.CVId);

                var existingExCoNa = await cVContext.Experiences
                 .FirstOrDefaultAsync(s => s.CompanyName == cVAltViewModel.CompanyName && s.CVId == cv.CVId);

                var existingExCoRo = await cVContext.Experiences
                 .FirstOrDefaultAsync(s => s.Role == cVAltViewModel.Role && s.CVId == cv.CVId);

                if (existingSkill == null)
                {

                    //skapar objekt
                    Skill skill = new Skill()
                    {
                        SkillName = cVAltViewModel.SkillName,
                        CVId = cv.CVId


                    };//tittar ifall skillfältet skrevs in
                    if (cVAltViewModel.SkillName == null)
                    {
                        Console.WriteLine("Inget hände");
                    }
                    else
                    {//sparar skill
                        cVContext.Skills.Update(skill);
                        await cVContext.SaveChangesAsync();


                    }



                }
                else
                {
                    Console.WriteLine("Färdigheten finns redan i CV:t");
                }

                if (existingEdNa == null || existingEdIn == null) //kollar ifall det redan existerar i cvt
                {


                    Education education = new Education()
                    {

                        Institution = cVAltViewModel.Institution,
                        EducationName = cVAltViewModel.EducationName,
                        CVId = cv.CVId

                    };
                    if (cVAltViewModel.Institution == null && cVAltViewModel.EducationName == null) //tittar fall fälten skrevs in
                    {
                        Console.WriteLine("Inget hände");
                    }
                    else
                    {
                        cVContext.Educations.Update(education);
                        await cVContext.SaveChangesAsync();


                    }

                }
                else
                {
                    Console.WriteLine("Education finns redan i CV:t");
                }


                if (existingExCoNa == null || existingExCoRo == null)
                {
                    Experience experience = new Experience()
                    {

                        CompanyName = cVAltViewModel.CompanyName,
                        Role = cVAltViewModel.Role,
                        CVId = cv.CVId

                    };
                    if (cVAltViewModel.CompanyName == null && existingExCoRo == null)
                    {
                        Console.WriteLine("Inget hände");
                    }
                    else
                    {
                        cVContext.Experiences.Update(experience);
                        await cVContext.SaveChangesAsync();


                    }


                }
                else
                {
                    Console.WriteLine("Education finns redan i CV:t");
                }



                if (cv == null)
                {
                    Console.WriteLine("CV kan ej hittas");
                    return NotFound();
                }


                cv.Summary = cVAltViewModel.Summary;
                //cv.ProfilePicturePath=cv.ProfilePicturePath;

                cVContext.CVs.Update(cv);
                await cVContext.SaveChangesAsync();

                return View(cVAltViewModel);
            }
            return View(cVAltViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CVAltTaBortSkills(CVAltViewModel cVAltViewModel)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Ej inloggad");
                return NotFound();
            }

            // Hämta CV kopplat till användaren
            var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cv == null)
            {
                Console.WriteLine("CV kan ej hittas");
                return NotFound();
            }

            // Hämtar alla skills kopplade till CVt
            var skillsToRemove = await cVContext.Skills
                .Where(s => s.CVId == cv.CVId)
                .ToListAsync();

            if (skillsToRemove.Any())
            {
                // Tar bort alla skills 
                cVContext.Skills.RemoveRange(skillsToRemove);
                await cVContext.SaveChangesAsync();
                Console.WriteLine("Alla färdigheter har tagits bort.");
            }
            else
            {
                Console.WriteLine("Inga färdigheter att ta bort.");
            }


            return RedirectToAction("CVAlt", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CVAltTaBortEducations(CVAltViewModel cVAltViewModel)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Ej inloggad");
                return NotFound();
            }


            var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cv == null)
            {
                Console.WriteLine("CV kan ej hittas");
                return NotFound();
            }


            var EducationsToRemove = await cVContext.Educations
                .Where(s => s.CVId == cv.CVId)
                .ToListAsync();

            if (EducationsToRemove.Any())
            {

                cVContext.Educations.RemoveRange(EducationsToRemove);
                await cVContext.SaveChangesAsync();

            }


            return RedirectToAction("CVAlt", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CVAltTaBortExperiences(CVAltViewModel cVAltViewModel)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Ej inloggad");
                return NotFound();
            }


            var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cv == null)
            {
                Console.WriteLine("CV kan ej hittas");
                return NotFound();
            }


            var ExperiencesToRemove = await cVContext.Experiences
                .Where(s => s.CVId == cv.CVId)
                .ToListAsync();

            if (ExperiencesToRemove.Any())
            {

                cVContext.Experiences.RemoveRange(ExperiencesToRemove);
                await cVContext.SaveChangesAsync();
            }



            return RedirectToAction("CVAlt", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> ProjektAlt()
        {
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
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
                .Where(p => p.ownerId == user.Id) //Hämtar alla projekt som är skapade/ägda av den inloggade användaren
                .Select(p => new ProjektAltViewModel
                {
                    ProjectName = p.ProjectName, //Här skapas ny viewmodel som har värdena av projekt i databasen
                    ProjectId = p.ProjectId,
                    ProjectDescription = p.ProjectDescription
                })
                .ToList();

            return View(allaProjekt);
        }


        [HttpGet]
        public async Task<IActionResult> ChangeProject(int id)
        {
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();
            Console.WriteLine($"Project ID: {id}");

            var projekt = await cVContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == id); // Fetch project based on the id

            if (projekt == null)
            {
                return NotFound();
            }


            var user = await userManager.GetUserAsync(User);

            if (user == null || projekt.ownerId != user.Id)
            {
                TempData["ErrorMessage"] = "You do not have the authority to change contents of this project.";
                return RedirectToAction("Index", "Home");
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
            Console.WriteLine("Aktivt Projekt =" + changeProjectViewModel.ProjectId.ToString());

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


                // Update project details
                projekt.ProjectName = changeProjectViewModel.ProjectName;
                projekt.ProjectDescription = changeProjectViewModel.ProjectDescription;

                cVContext.Projects.Update(projekt);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(changeProjectViewModel);
        }



    }
}
