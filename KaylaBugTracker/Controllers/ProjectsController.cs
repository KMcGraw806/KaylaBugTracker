﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KaylaBugTracker.Helpers;
using KaylaBugTracker.ViewModels;
using KaylaBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace KaylaBugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();


        // GET: Projects
        public ActionResult Index()
        {
            var model = new List<Project>();
            if (User.IsInRole("Admin"))
            {
                model = db.Projects.ToList();
            }
            if (User.IsInRole("Project Manager") || User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId();
                model = projectHelper.ListUserProjects(userId).ToList();
            }
            return View(model);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();
                projectHelper.AddUserToProject(User.Identity.GetUserId(), project.Id);
                return RedirectToAction("Index");
            }

            return View(project);
        }

        public ActionResult ProjectWizard()
        {
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "FullName");
            ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
            ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");
            ViewBag.Errors = "";
            var model = new ProjectWizardVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectWizard(ProjectWizardVM model)
        {
            #region Fail Cases
            ViewBag.Errors = "";
            if (model.ProjectManagerId == null)
            {
                ViewBag.Errors += "<p>You must select a Project Manager</p>";
            }
            if(model.DeveloperIds.Count == 0)
            {
                ViewBag.Errors += "<p>You must select at least one Developer</p>";
            }
            if(model.SubmitterIds.Count == 0)
            {
                ViewBag.Errors += "<p>You must select at least one Submitter</p>";
            }
            if(ViewBag.Errors.Length > 0)
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
                ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");
                return View(model);
            }
            #endregion
            if (ModelState.IsValid)
            {

                Project project = new Project();
                project.Name = model.Name;
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();
                projectHelper.AddUserToProject(User.Identity.GetUserId(), project.Id);

                projectHelper.AddUserToProject(model.ProjectManagerId, project.Id);
                foreach(var userId in model.DeveloperIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }
                foreach(var userId in model.SubmitterIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
                ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");
                return View(model);
            }
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public PartialViewResult _ProjectModal()
        {
            var model = new Project();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditProjectName(string projectName, int projectId)
        {
            db.Projects.Find(projectId).Name = projectName;
            db.SaveChanges();

            return RedirectToAction("Details", "Projects", new { Id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditProject(List<string> managerIds, List<string> developerIds, List<string> submitterIds, string projectName, int projectId)
        {
            // if managerIds is empty, we still want to remove all the users from this project,
            // because that could be the user's intent
            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Project Manager"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            if (managerIds.Count == 0)
            {
                return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
            }

            foreach (var Id in managerIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }

            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Developer"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            foreach (var Id in developerIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }

            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Submitter"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            foreach (var Id in submitterIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }
            db.SaveChanges();

            return RedirectToAction("Details", "Projects", new { Id = projectId });
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
