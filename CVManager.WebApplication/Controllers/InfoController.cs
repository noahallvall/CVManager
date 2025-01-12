﻿using CVManager.DAL.Context;
using CVManager.DAL.Entities;
using CVManager.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CVManager.WebApplication.Controllers
{
    public class InfoController : Controller
    {
        private readonly CVContext cVContext;
        private UserManager<User> userManager;
        private string reciever;
        private string sender;

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
        public async Task<IActionResult> CV(int id)
        {

            Console.WriteLine(id.ToString());
            if (id != 0)
            {
                Console.WriteLine(id.ToString());

                // Hämta användaren och deras enda CV
                var cv = cVContext.CVs
                    .Include(c => c.User)
                    .Include(c => c.CVProjects)
                    .ThenInclude(cp => cp.Project)
                    .FirstOrDefault(c => c.CVId == id);

                User user;
                CvViewModel cvViewModel = new CvViewModel();


                user = await cVContext.Users.FindAsync(cv.UserId);

                cvViewModel = new CvViewModel()
                {
                    User = user,
                    CV = user.CV
                };

                // Skicka användardata och det kopplade cv:t till vyn 

                return View(cvViewModel);
            } else if (id == 0)
            {


                var user = await userManager.GetUserAsync(User);

                var cv = cVContext.CVs
                    .Include(c => c.User)
                    .Include(c => c.CVProjects)
                    .ThenInclude(cp => cp.Project)
                    .FirstOrDefault(c => c.UserId == user.Id);

                CvViewModel cvViewModel = new CvViewModel()
                {
                    User = user,
                    CV = user.CV
                };
                return View(cvViewModel);
            }


            return RedirectToAction("Index", "Home");

        }


        [HttpPost]
        public async Task<IActionResult> Projekt(ProjektViewModel projektViewModel)
        {
            if (ModelState.IsValid)
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

        [HttpGet]
        public async Task<IActionResult> Message(string id)
        {
            var user = await userManager.GetUserAsync(User);

            var userRecieve = cVContext.Users.FirstOrDefault(u => u.Id == id);

            string messageId = Guid.NewGuid().ToString();


            reciever = userRecieve.Id;

            MessageViewModel messageViewModel = new MessageViewModel()
            {
                MessageId = messageId,
                Reciever = reciever
            };



            return View(messageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Message(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var userRecieve = cVContext.Users.FirstOrDefault(u => u.Id == messageViewModel.Reciever);

                if (user != null)
                {
                    sender = user.Id;
                }

                reciever = userRecieve.Id;




                var senderCVId = cVContext.CVs
                    .Where(cv => cv.UserId == sender) // Filtrera på UserId
                    .Select(cv => cv.CVId) // Välj endast CVId
                    .FirstOrDefault();

                var recieverCVId = cVContext.CVs
                    .Where(cv => cv.UserId == reciever)
                    .Select(cv => cv.CVId)
                    .FirstOrDefault();



                Message message = new Message()
                {
                    MessageId = messageViewModel.MessageId,
                    MessageContent = messageViewModel.MessageContent,

                    CVRecievedId = recieverCVId,
                    IsRead = false
                };

                if (user != null)
                {
                    message.CVSentId = senderCVId;
                } else
                {
                    message.SendersName = messageViewModel.SenderName;
                    message.CVSentId = null;
                }

                cVContext.Messages.Add(message);
                await cVContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");

            }

            return View(messageViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Konversationer()
        {
            var user = await userManager.GetUserAsync(User);

            var reciever = cVContext.CVs
                .Where(cv => cv.UserId == user.Id)
                .FirstOrDefault();

            var messages = cVContext.Messages
                .Where(m => m.CVRecievedId == reciever.CVId)
                .ToList();

            List<CV> CVsAttSkicka = new List<CV>();

            foreach (var message in messages) {
                var matchandeCVs = cVContext.CVs
                    .Where(c => c.CVId == message.CVSentId)
                    .ToList();

                CVsAttSkicka.AddRange(matchandeCVs);
            }

            List<User> users = new List<User>();

            foreach (var CV in CVsAttSkicka)
            {
                var matchandeUser = cVContext.Users
                    .Where(u => u.Id == CV.UserId)
                    .ToList();

                users.AddRange(matchandeUser);
            }

            List<string> AllaNamn = new List<string>(); 

            foreach(var uuser in users)
            {
                AllaNamn.Add(uuser.FirstName + " "+ uuser.LastName);
            }

            

            return View();
        }
    }
}
