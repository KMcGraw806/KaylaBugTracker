namespace KaylaBugTracker.Migrations
{
    using KaylaBugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<KaylaBugTracker.Models.ApplicationDbContext>
    {
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

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
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
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];

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

            if (!context.Users.Any(u => u.Email == "annieedison@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "annieedison@mailinator.com",
                    Email = "annieedison@mailinator.com",
                    FirstName = "Annie",
                    LastName = "Edison",
                }, "Password1");
            }
            if (!context.Users.Any(u => u.Email == "troy_barnes@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "troy_barnes@mailinator.com",
                    Email = "troy_barnes@mailinator.com",
                    FirstName = "Troy",
                    LastName = "Barnes",
                }, "Password1");
            }
            if (!context.Users.Any(u => u.Email == "abed.nadir@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "abed.nadir@mailinator.com",
                    Email = "abed.nadir@mailinator.com",
                    FirstName = "Abed",
                    LastName = "Nadir",
                }, "Password1");
            }

            #endregion

            #region Role Assignment
            var adminId = userManager.FindByEmail("kayla_mcgraw@hotmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var projManId = userManager.FindByEmail("annieedison@mailinator.com").Id;
            userManager.AddToRole(projManId, "ProjectManager");
            
            var devId = userManager.FindByEmail("troy_barnes@mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");
            
            var subId = userManager.FindByEmail("abed.nadir@mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");
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
        }
    }
}
