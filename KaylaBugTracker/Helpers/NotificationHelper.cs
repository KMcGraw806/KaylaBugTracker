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
        EmailService svc = new EmailService();
        string from = "Kayla Bug Tracker";
        public async Task ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketAssigned = oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null;
            var ticketUnassigned = oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId == null;
            var ticketReassinged = oldTicket.DeveloperId != newTicket.DeveloperId && oldTicket.DeveloperId != null && newTicket.DeveloperId != null;
            if (ticketAssigned)
            {
                AddAssignmentNotification(newTicket);
            }
            else if (ticketUnassigned)
            {
                AddUnassignmentNotification(oldTicket);
            }
            else if (ticketReassinged)
            {
                AddAssignmentNotification(newTicket);
                AddUnassignmentNotification(oldTicket);
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

            try
            {
                var email = new MailMessage(from, notification.User.Email)
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
        private async Task AddUnassignmentNotification(Ticket newTicket)
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

            try
            {
                var email = new MailMessage(from, notification.User.Email)
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
    }
}