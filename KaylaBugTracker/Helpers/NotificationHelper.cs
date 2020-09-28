using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;

namespace KaylaBugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserHelper userHelper = new UserHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        EmailService svc = new EmailService();
        string from = "Kayla Bug Tracker";

        public async Task ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketAssigned = oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null;
            var ticketUnassigned = oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId == null;
            var ticketReassinged = oldTicket.DeveloperId != newTicket.DeveloperId && oldTicket.DeveloperId != null && newTicket.DeveloperId != null;
            if (ticketAssigned)
            {
                await AddAssignmentNotification(newTicket);
            }
            else if (ticketUnassigned)
            {
                await AddUnassignmentNotification(oldTicket);
            }
            else if (ticketReassinged)
            {
                await AddAssignmentNotification(newTicket);
                await AddUnassignmentNotification(oldTicket);
            }
        }
        private async Task AddAssignmentNotification(Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                UserId = newTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"You have been assigned to Ticket Id {newTicket.Id} on Project {newTicket.Project.Name}. <br/>This is a {newTicket.TicketPriority.Name} priority ticket."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;
            try
            {
                var email = new MailMessage(from, userEmail)
                {
                    Subject = "Ticket Assignment",
                    Body = notification.Message,
                    IsBodyHtml = true,
                };
                await svc.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        private async Task AddUnassignmentNotification(Ticket oldTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = oldTicket.Id,
                IsRead = false,
                UserId = oldTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"You have been removed from Ticket Id {oldTicket.Id} on Project {oldTicket.Project.Name}."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;
            try
            {
                var email = new MailMessage(from, userEmail)
                {
                    Subject = "Ticket Unassignment",
                    Body = notification.Message,
                    IsBodyHtml = true,
                };
                await svc.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public List<TicketNotification> ListUsersNotifications(string userId)
        {
            return db.TicketNotifications.Where(n => n.UserId == userId).OrderByDescending(n => n.Created).ToList();
        }

        public void MarkAsRead(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
        }
    }
}