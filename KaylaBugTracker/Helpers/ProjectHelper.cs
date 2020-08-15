using KaylaBugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        //What do we need to do?
        //Add one or more users to a project
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project project = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                project.Users.Add(user);
                db.SaveChanges();
            }
        }
        //Remove one or more users from a project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            var result = project.Users.Remove(user);
            db.SaveChanges();
            return result;
        }
    
        
        //List users on a project
        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }
        //List users not on a project
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            foreach(var user in db.Users.ToList())
            {
                if (IsUserOnProject(user.Id, projectId))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            return project.Users.Contains(user);
        }
        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }
        //OPTIONAL: List users on a project that occupy a specific role
        public List<ApplicationUser> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        //OPTIONAL: List users not on a project in a specific role
        //OPTIONAL: Filter for Project Manager role - if you code for only one allowed
    }
}