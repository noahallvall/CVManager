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

        [HttpPost]
        public async Task<IActionResult> LinkCvToProject(int projectId)
        {
            try
            {
                // Hämta den inloggade användaren
                var user = await userManager.GetUserAsync(User);

                // Hämta användarens CV
                var cv = await cVContext.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (cv == null)
                {
                    TempData["ErrorMessage"] = "Du måste ha ett CV för att kunna koppla till ett projekt.";
                    return RedirectToAction("Index", "Home");
                }

                // Kontrollera att projektet finns
                var project = await cVContext.Projects.FindAsync(projectId);
                if (project == null)
                {
                    TempData["ErrorMessage"] = "Projektet finns inte.";
                    return RedirectToAction("Index", "Home");
                }

                // Kontrollera om kopplingen redan finns
                var existingLink = await cVContext.CVProjects
                    .FirstOrDefaultAsync(cp => cp.CVId == cv.CVId && cp.ProjectId == project.ProjectId);

                if (existingLink != null)
                {
                    TempData["ErrorMessage"] = "Du är redan kopplad till detta projekt.";
                    return RedirectToAction("Index", "Home");
                }

                // Skapa kopplingen mellan CV och projekt
                var cvProject = new CVProject
                {
                    CVId = cv.CVId,
                    ProjectId = project.ProjectId
                };

                // Lägg till användarens UserId till projektet
                project.ownerId = user.Id;

                cVContext.CVProjects.Add(cvProject);
                await cVContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ditt CV har kopplats till projektet!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ett fel uppstod: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
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
