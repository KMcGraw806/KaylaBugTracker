namespace KaylaBugTracker.Migrations
{
    using KaylaBugTracker.Helpers;
    using KaylaBugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<KaylaBugTracker.Models.ApplicationDbContext>
    {
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        Random random = new Random();
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(KaylaBugTracker.Models.ApplicationDbContext context)
        {
            #region Role Creation
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion

            #region User Creation
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var demoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];
            var demoPMPassword = WebConfigurationManager.AppSettings["DemoPMPassword"];
            var demoDevPassword = WebConfigurationManager.AppSettings["DemoDevPassword"];
            var demoSubPassword = WebConfigurationManager.AppSettings["DemoSubPassword"];

            var adminEmail = WebConfigurationManager.AppSettings["DemoAdminEmail"];

            var pmEmail = WebConfigurationManager.AppSettings["DemoPMEmail"];
            var pm2Email = WebConfigurationManager.AppSettings["DemoPM2Email"];

            var devEmail = WebConfigurationManager.AppSettings["DemoDevEmail"];
            var dev2Email = WebConfigurationManager.AppSettings["DemoDev2Email"];
            var dev3Email = WebConfigurationManager.AppSettings["DemoDev3Email"];

            var subEmail = WebConfigurationManager.AppSettings["DemoSubEmail"];
            var sub2Email = WebConfigurationManager.AppSettings["DemoSub2Email"];
            var sub3Email = WebConfigurationManager.AppSettings["DemoSub3Email"];

            //Now I need to go out and look for the presence of a user with a specific Email...
            //If and only if it IS NOT found will I create a user with that email
            if (!context.Users.Any(u => u.Email == "kayla_mcgraw@hotmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "kayla_mcgraw@hotmail.com",
                    Email = "kayla_mcgraw@hotmail.com",
                    FirstName = "Kayla",
                    LastName = "McGraw",
                }, "Simone2410");
            }

            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "annieedison@mailinator.com",
                    Email = "annieedison@mailinator.com",
                    FirstName = "Annie",
                    LastName = "Edison",
                    AvatarPath = "/Avatars/Annie.png"
                }, demoAdminPassword);
            }
            if (!context.Users.Any(u => u.Email == pmEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "troy_barnes@mailinator.com",
                    Email = "troy_barnes@mailinator.com",
                    FirstName = "Troy",
                    LastName = "Barnes",
                    AvatarPath = "/Avatars/Troy.png"
                }, demoPMPassword);
            }
            if (!context.Users.Any(u => u.Email == pm2Email))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "abed.nadir@mailinator.com",
                    Email = "abed.nadir@mailinator.com",
                    FirstName = "Abed",
                    LastName = "Nadir",
                    AvatarPath = "/Avatars/Abed.png"
                }, demoPMPassword);
            }
            
            if (!context.Users.Any(u => u.Email == devEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "jeffwinger@mailinator.com",
                    Email = "jeffwinger@mailinator.com",
                    FirstName = "Jeff",
                    LastName = "Winger",
                    AvatarPath = "/Avatars/Jeff.png"
                }, demoDevPassword);
            }
            if (!context.Users.Any(u => u.Email == dev2Email))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "brittaperry@mailinator.com",
                    Email = "brittaperry@mailinator.com",
                    FirstName = "Britta",
                    LastName = "Perry",
                    AvatarPath = "/Avatars/Britta.png"
                }, demoDevPassword);
            }
            if (!context.Users.Any(u => u.Email == dev3Email))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "deancraigpelton@mailinator.com",
                    Email = "deancraigpelton@mailinator.com",
                    FirstName = "Craig",
                    LastName = "Pelton",
                    AvatarPath = "/Avatars/Dean.png"
                }, demoDevPassword);
            }
            if (!context.Users.Any(u => u.Email == subEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "piercehawthorne@mailinator.com",
                    Email = "piercehawthorne@mailinator.com",
                    FirstName = "Pierce",
                    LastName = "Hawthorne",
                    AvatarPath = "/Avatars/Pierce.png"
                }, demoSubPassword);
            }
            if (!context.Users.Any(u => u.Email == sub2Email))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "shirleybennet@mailinator.com",
                    Email = "shirleybennet@mailinator.com",
                    FirstName = "Shirley",
                    LastName = "Bennett",
                    AvatarPath = "/Avatars/Shirley.png"
                }, demoSubPassword);
            }
            if (!context.Users.Any(u => u.Email == sub3Email))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "benchang@mailinator.com",
                    Email = "benchang@mailinator.com",
                    FirstName = "Ben",
                    LastName = "Chang",
                    AvatarPath = "/Avatars/Chang.png"
                }, demoSubPassword);
            }

            #endregion

            #region Role Assignment
            var adminId = userManager.FindByEmail("kayla_mcgraw@hotmail.com").Id;
            userManager.AddToRole(adminId, "Admin");
            
            var admin2Id = userManager.FindByEmail(adminEmail).Id;
            userManager.AddToRole(admin2Id, "Admin");

            var projManId = userManager.FindByEmail(pmEmail).Id;
            userManager.AddToRole(projManId, "Project Manager");
            var projMan2Id = userManager.FindByEmail(pm2Email).Id;
            userManager.AddToRole(projMan2Id, "Project Manager");
            
            var devId = userManager.FindByEmail(devEmail).Id;
            userManager.AddToRole(devId, "Developer");
            var dev2Id = userManager.FindByEmail(dev2Email).Id;
            userManager.AddToRole(dev2Id, "Developer");
            var dev3Id = userManager.FindByEmail(dev3Email).Id;
            userManager.AddToRole(dev3Id, "Developer");
            
            var subId = userManager.FindByEmail(subEmail).Id;
            userManager.AddToRole(subId, "Submitter");
            var sub2Id = userManager.FindByEmail(sub2Email).Id;
            userManager.AddToRole(sub2Id, "Submitter");
            var sub3Id = userManager.FindByEmail(sub3Email).Id;
            userManager.AddToRole(sub3Id, "Submitter");
            #endregion
            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Other" },
                new TicketType() { Name = "Feature Request" }
                );
            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                tp => tp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );
            #endregion

            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );
            #endregion

            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45) },
                new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7) }
                );
            #endregion
            context.SaveChanges();

            #region Ticket Seed
            List<Ticket> ticketList = new List<Ticket>();
            List<ApplicationUser> projectManagers = new List<ApplicationUser>();
            List<ApplicationUser> developers = new List<ApplicationUser>();
            List<ApplicationUser> submitters = new List<ApplicationUser>();
            projectManagers.AddRange(roleHelper.UsersInRole("Project Manager"));
            developers.AddRange(roleHelper.UsersInRole("Developer"));
            submitters.AddRange(roleHelper.UsersInRole("Submitter"));

            #region Assigning users to projects by role
            foreach (var project in context.Projects)
            {
                foreach(var user in roleHelper.UsersInRole("Admin"))
                {
                    projectHelper.AddUserToProject(user.Id, project.Id);
                }
                projectHelper.AddUserToProject(projectManagers[random.Next(projectManagers.Count)].Id, project.Id);
                //Developer Assignment
                var firstDev = developers[random.Next(developers.Count)].Id;
                var secondDev = developers[random.Next(developers.Count)].Id;
                while(firstDev == secondDev)
                {
                    secondDev = developers[random.Next(developers.Count)].Id;
                }
                projectHelper.AddUserToProject(firstDev, project.Id);
                projectHelper.AddUserToProject(secondDev, project.Id);
                //Submitter Assignment
                var firstSub = submitters[random.Next(submitters.Count)].Id;
                var secondSub = submitters[random.Next(submitters.Count)].Id;
                while (firstSub == secondSub)
                {
                    secondSub = submitters[random.Next(submitters.Count)].Id;
                }
                projectHelper.AddUserToProject(firstSub, project.Id);
                projectHelper.AddUserToProject(secondSub, project.Id);
            }
            #endregion

            SeedHelper seedHelper = new SeedHelper(context);
            seedHelper.SeedTickets();

            //foreach (var project in context.Projects.ToList())
            //{
            //    projectManagers = projectHelper.ListUsersOnProjectInRole(project.Id, "Project Manager");
            //    developers = projectHelper.ListUsersOnProjectInRole(project.Id, "Developer");
            //    submitters = projectHelper.ListUsersOnProjectInRole(project.Id, "Submitter");
            //    foreach (var type in context.TicketTypes.ToList())
            //    { 
            //        foreach(var status in context.TicketStatuses.ToList())
            //        {
            //            foreach(var priority in context.TicketPriorities.ToList())
            //            {
            //                var developerId = developers[random.Next(developers.Count)].Id;
            //                if(status.Name == "Open")
            //                {
            //                    developerId = null;
            //                }
            //                var resolved = false;
            //                var archived = false;
            //                if(status.Name == "Resolved")
            //                {
            //                    resolved = true;
            //                }
            //                if(status.Name == "Archived" || project.IsArchived)
            //                {
            //                    archived = true;
            //                }
            //                var newTicket = new Ticket()
            //                {
            //                    ProjectId = project.Id,
            //                    TicketPriorityId = priority.Id,
            //                    TicketTypeId = type.Id,
            //                    TicketStatusId = status.Id,
            //                    SubmitterId = submitters[random.Next(submitters.Count)].Id,
            //                    DeveloperId = developerId,
            //                    Created = DateTime.Now,
            //                    Issue = $"This is a seeded ticket of type {type.Name} with a status of {status.Name} and {priority.Name} priority",
            //                    IssueDescription = $"This is a description of a ticket of type {type.Name} on {project.Name}",
            //                    IsResolved = resolved,
            //                    IsArchived = archived
            //                };
            //                ticketList.Add(newTicket);
            //            }
            //        }
            //    }
            //}
            context.Tickets.AddRange(ticketList);
            context.SaveChanges();
            #endregion
        }
    }
}
