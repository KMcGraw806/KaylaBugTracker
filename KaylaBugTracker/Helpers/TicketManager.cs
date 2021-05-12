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

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            //Scenario 1: A new assignment - oldTicket.DeveloperId = null newTicket.DeveloperId is not
            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"You have been assigned Ticket Id: {newTicket.Id}",
                    Body = $"Heads up {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '' on Project ''"
                };

                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();

            }

            //Scenario 2: An unassignment
            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId == null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"You have been from {newTicket.Id}",
                    Body = $"Heads up {newTicket.Developer.FullName}, you have been removed from Ticket Id {newTicket.Id} titled '' on Project ''"
                };
            }

            //Scenario 3: A reassignment => could create two notifications, one for the new guy and one for the old girl
        }
    }
}