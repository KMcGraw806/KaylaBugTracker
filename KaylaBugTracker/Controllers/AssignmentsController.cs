﻿using KaylaBugTracker.Helpers;
using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaylaBugTracker.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        #region Role Assignments
        // GET: Assignments
        [Authorize]
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");
            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds == null)
                return RedirectToAction("RolesIndex");

            foreach(var userId in userIds)
            {
                foreach(var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
                
            }
            return RedirectToAction("ManageRoles");
        }
        #endregion

        #region Project Assignments
        public ActionResult ManageProjectUsers()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");
            var users = db.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<string> userIds, ICollection<int> projectIds)
        {
            if(userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectUsers");
            }

            foreach(var userId in userIds)
            {
                foreach(var projectId in projectIds)
                    {
                        projectHelper.AddUserToProject(userId, projectId);
                    }
            }
            

            return RedirectToAction("ManageProjectUsers");
        }
        #endregion
    }
}