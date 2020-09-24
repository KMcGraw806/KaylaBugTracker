using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.Helpers
{
    public class SeedHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public SeedHelper(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        private Random random = new Random();

        public void SeedTickets()
        {
            ProjectHelper projectHelper = new ProjectHelper();
            var projects = db.Projects.ToList();
            var ticketPriorities = db.TicketPriorities.ToList();
            var ticketTypes = db.TicketTypes.ToList();
            var ticketStatuses = db.TicketStatuses.ToList();
            for (var i = 0; i < 50; i++)
            {
                var aProject = projects[random.Next(projects.Count())];
                var projectSubmitters = projectHelper.ListUsersOnProjectInRole(aProject.Id, "Submitter");
                var projectDevelopers = projectHelper.ListUsersOnProjectInRole(aProject.Id, "Developer");
                var ticketStatus = ticketStatuses[random.Next(ticketStatuses.Count())];

                var developerId = "";
                if (projectDevelopers.Count != 0)
                {
                    developerId = projectDevelopers[random.Next(projectDevelopers.Count)].Id;
                }

                if (ticketStatus.Name == "Open")
                {
                    developerId = null;
                }

                var resolved = false;
                if (ticketStatus.Name == "Resolved")
                {
                    resolved = true;
                }

                var archived = false;
                if (ticketStatus.Name == "Archived" || aProject.IsArchived)
                {
                    archived = true;
                }

                var newTicket = new Ticket()
                {
                    ProjectId = aProject.Id,
                    TicketPriorityId = ticketPriorities[random.Next(ticketPriorities.Count())].Id,
                    TicketTypeId = ticketTypes[random.Next(ticketTypes.Count())].Id,
                    TicketStatusId = ticketStatus.Id,
                    SubmitterId = projectSubmitters[random.Next(projectSubmitters.Count)].Id,
                    DeveloperId = developerId,
                    Created = DateTime.Now.AddDays(-2),
                    Updated = DateTime.Now,
                    Issue = $"Seeded Ticket {i}",
                    IssueDescription = "This is a description of Ticket.",
                    IsResolved = resolved,
                    IsArchived = archived
                };

                db.Tickets.Add(newTicket);
            }

            db.SaveChanges();
        }
    }
}