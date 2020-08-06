using KaylaBugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace KaylaBugTracker.Helper
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FullName;
        }

        public string GetAvatarPath()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            return user.AvatarPath;
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            return db.Projects.Where(p => p.Users.Contains(user)).ToList();
        }

        public string GetUserRole()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var roleId = user.Roles.Where(u => u.UserId == userId);
            return null;
        }
    }
}