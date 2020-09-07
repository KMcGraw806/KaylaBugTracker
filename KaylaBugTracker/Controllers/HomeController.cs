using KaylaBugTracker.Helpers;
using KaylaBugTracker.Models;
using KaylaBugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaylaBugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        //[Authorize]
        public ActionResult Index()
        {
            var allTickets = db.Tickets.ToList();
            var dashboardVM = new DashboardViewModel()
            {
                TicketCount = allTickets.Count,
                HighPriorityTicketCount = allTickets.Where(t => t.TicketPriority.Name == "High").Count(),
                NewTicketCount = allTickets.Where(t => t.TicketStatus.Name == "New").Count(),
                TotalComments = db.TicketComments.Count(),
                AllTickets = db.Tickets.ToList()
            };
            dashboardVM.ProjectVM.ProjectCount = db.Projects.Count();
            dashboardVM.ProjectVM.AllProjects = db.Projects.ToList();
            dashboardVM.ProjectVM.AllPMs = roleHelper.UsersInRole("Project Manager").ToList();

            return View(dashboardVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}