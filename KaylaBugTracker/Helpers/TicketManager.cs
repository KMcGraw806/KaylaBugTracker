using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace KaylaBugTracker.Helpers
{
    public class TicketManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        public List<Ticket> GetMyTickets()
        {
            var tickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            List<Ticket> model = new List<Ticket>();

            switch (myRole)
            {
                case "Admin":
                    tickets.AddRange(db.Tickets);
                    break;
                case "Project Manager":
                    tickets.AddRange(db.Users.Find(userId).Projects.SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                    tickets.AddRange(db.Tickets.Where(t => t.DeveloperId == userId));
                    break;
                case "Submitter":
                    tickets.AddRange(db.Tickets.Where(t => t.SubmitterId == userId));
                    break;

                default:

                    break;
            }
            return tickets;
        }
    }
}