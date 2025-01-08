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

        [HttpPost]
        public async Task<IActionResult> Projekt(ProjektViewModel projektViewModel)
        {
            if(ModelState.IsValid)
            {
                var projekt = new Project
                {
                    ProjectName = projektViewModel.ProjectName,
                    ProjectDescription = projektViewModel.ProjectDescription
                };

                cVContext.Projects.Add(projekt);
                await cVContext.SaveChangesAsync();

                var cvProject = new CVProject
                {
                    CVId = 1, // Glöm inte att ändra till ett cv id som är kopplat till användaren!
                    ProjectId = projekt.ProjectId
                };

                cVContext.CVProjects.Add(cvProject);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(projektViewModel);
        }
    }
}
