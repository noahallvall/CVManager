using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CVManager.DAL.Context;
using CVManager.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using AspNetCoreGeneratedDocument;

namespace CVManager.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly CVContext CVContext;
        private readonly UserManager<User> userManager;


        public HomeController(ILogger<HomeController> logger,CVContext context, UserManager<User> userManager)
        {
            _logger = logger;
            CVContext = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            //Här ska startsidans lista med CVs och projekt returneras.
            //H�mta data från cvcontext --> bind till HomeViewModel --> returnera 
            //HomeViewModel till startvyn index. 

            var isAuthenticated = User.Identity.IsAuthenticated;

            // H�mta data från databasen
            var cvList = CVContext.CVs
             
                .Include(cv => cv.User) // Ladda användarinformation
                .Include(cv => cv.CVProjects) // Ladda sambandstabellen
                .ThenInclude(cp => cp.Project) // Ladda kopplade projekt
                .Include(cv => cv.Skills)
                .Include(cv => cv.Experiences)
                .Include(cv => cv.Educations)
                .Where(cv => !cv.User.IsPrivateProfile.HasValue || !cv.User.IsPrivateProfile.Value || isAuthenticated)
                .ToList();

            var sortedProjects = cvList
                 .SelectMany(cv => cv.CVProjects)
                 .Where(cp => cp.Project != null) 
                 .Select(cp => cp.Project) 
                 .OrderByDescending(p => p.UploadDate) 
                 .ToList();

            // Skapa och fyll HomeViewModel
            var homeViewModel = new HomeViewModel
            {
                CVs = cvList,
                Projects = sortedProjects

            };


            GlobalData.OlastaMeddelandenCount = await OlastaMeddelanden();
            ViewBag.Antal = GlobalData.OlastaMeddelandenCount.ToString();

            return View(homeViewModel);
        }

        public async Task<int> OlastaMeddelanden()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var CV = CVContext.CVs
                    .Where(cv => cv.UserId == user.Id)
                    .FirstOrDefault();

                var Messages = CVContext.Messages
                    .Where(m => m.CVRecievedId == CV.CVId)
                    .Where(m => m.IsRead == false)
                    .ToList();



                return Messages.Count();
            }
            return 0;
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult FirstPage()
        {
            return View();
        }

        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
