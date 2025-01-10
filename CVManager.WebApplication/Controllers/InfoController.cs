using CVManager.DAL.Context;
using CVManager.DAL.Entities;
using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVManager.WebApplication.Controllers
{
    public class InfoController : Controller
    {
        private readonly CVContext cVContext;
        private UserManager<User> userManager;

        public InfoController(UserManager<User> userMngr, CVContext context)
        {
            this.cVContext = context;
            this.userManager = userMngr;
        }

        [HttpGet]
        public IActionResult Projekt()
        {
            ProjektViewModel projektViewModel = new ProjektViewModel();
            return View(projektViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CV()
        {

            var uuser = await userManager.GetUserAsync(User);

            if (uuser == null)
            {
                Console.WriteLine("Ej inloggad");
                return NotFound();
            }

            var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == uuser.Id);

            // Hämta användaren och deras enda CV
            var user = cVContext.Users
                .Include(u => u.CV)
                .ThenInclude(cv => cv.CVProjects)
                .ThenInclude(cp => cp.Project)
                .FirstOrDefault(u => u.Id == uuser.Id);

            if (user == null || user.CV == null)
            {
                return NotFound("Användaren eller deras CV kunde inte hittas.");
            }


            var cvViewModel = new CvViewModel
            {
                User = user,
                CV = user.CV
            };

            // Skicka användardata och det kopplade cv:t till vyn 

            return View(cvViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Projekt(ProjektViewModel projektViewModel)
        {
            if(ModelState.IsValid)
            {

                var uuser = await userManager.GetUserAsync(User);

                var cv = await cVContext.CVs.FirstOrDefaultAsync
                    (c => c.UserId == uuser.Id);

                var projekt = new Project
                {
                    ProjectName = projektViewModel.ProjectName,
                    ProjectDescription = projektViewModel.ProjectDescription,
                    UploadDate = DateTime.Now,
                    ownerId = uuser.Id
                };

                cVContext.Projects.Add(projekt);
                await cVContext.SaveChangesAsync();

                

                var cvProject = new CVProject
                {
                    CVId = cv.CVId, // Glöm inte att ändra till ett cv id som är kopplat till användarens cvq!
                    ProjectId = projekt.ProjectId
                };

                cVContext.CVProjects.Add(cvProject);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(projektViewModel);
        }


        [HttpGet]
        public IActionResult VisaProjekt()
        {
            var allaProjekt = cVContext.Projects
                .Select(p => new VisaProjektViewModel
                {
                    ProjectID = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    UploadDate = p.UploadDate
                })
                .ToList();

            return View(allaProjekt);
        }

    }
}
