using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CVManager.DAL.Context;
using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly CVContext CVContext;

        public HomeController(ILogger<HomeController> logger,CVContext context)
        {
            _logger = logger;
            CVContext = context;
        }

        public IActionResult Index()
        {

            //Här ska startsidans lista med CVs och projekt returneras.
            //Hämta data från cvcontext --> bind till HomeViewModel --> returnera 
            //HomeViewModel till startvyn index. 


            // Hämta data från databasen
            var cvList = CVContext.CVs
                .Include(cv => cv.User) // Ladda användarinformation
                .Include(cv => cv.CVProjects) // Ladda sambandstabellen
                .ThenInclude(cp => cp.Project) // Ladda kopplade projekt
                .ToList();

            // Skapa och fyll HomeViewModel
            var homeViewModel = new HomeViewModel
            {
                CVs = cvList,
                Projects = cvList.SelectMany(cv => cv.CVProjects.Select(cp => cp.Project)).ToList()
            };
        

            return View(homeViewModel);
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
