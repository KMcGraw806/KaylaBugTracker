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
                    AvatarPath = "/Avatars/default_user.png",
                }, "Simone2410");
            }

            if (!context.Users.Any(u => u.Email == "chris.cwm.1022@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    UserName = "chris.cwm.1022@gmail.com",
                    Email = "chris.cwm.1022@gmail.com",
                    FirstName = "Christopher",
                    LastName = "McGraw",
                    AvatarPath = "/Avatars/default_user.png"
                }, "100221Cm!");
            }

            #endregion

            var adminId = userManager.FindByEmail("kayla_mcgraw@hotmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var modId = userManager.FindByEmail("chris.cwm.1022@gmail.com").Id;
            userManager.AddToRole(modId, "Moderator");
        }
    }
}
