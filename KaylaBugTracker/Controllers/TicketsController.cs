﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KaylaBugTracker.Helpers;
using KaylaBugTracker.Models;
using KaylaBugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace KaylaBugTracker.Controllers
{
    //[Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketManager ticketManager = new TicketManager();
        private HistoryHelper historyHelper = new HistoryHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            //I want to show All Tickets AND my Tickets

            //var myUserId = User.Identity.GetUserId();
            //var myTickets = ticketManager.GetMyTickets(myUserId);
            return View(db.Tickets.ToList());
        }

        public ActionResult GetProjectTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
            return View("Index", ticketList);
        }

        public ActionResult GetMyTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            if (User.IsInRole("Developer"))
            {
                ticketList = db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                return View("Index", ticketList);
            }
            if(User.IsInRole("Submitter"))
            {
                ticketList = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                return View("Index", ticketList);
            }
            else
            {
                return RedirectToAction("GetProjectTickets");
            }
        }

        // GET: Tickets/Details/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        //[Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            if(userId == null)
            {
                return RedirectToAction("Index");
            }
            var projectList = projectHelper.ListUserProjects(userId);
            ViewBag.ProjectId = new SelectList(projectList, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketTypeId,Issue,IssueDescription,Created,Updated")] Ticket ticket, bool onPage)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                //Add back in: Created, SubmitterId
                //Set: DeveloperId to null, IsArchived and IsResolved will be false
                ticket.TicketStatusId = db.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
                ticket.Created = DateTime.Now;
                ticket.SubmitterId = userId; 
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }

            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            //var model = new TicketEditVM();
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View();
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,SubmitterId,DeveloperId,Issue,IssueDescription,Created,Updated,IsResolved,IsArchived")]Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //Go out and get an unedited copy of the Ticket from the DB
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //Somehow compare my old Ticket with the New Ticket to make any number of decisions that might be required
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticketHelper.EditedTicket(oldTicket, newTicket);
                await notificationHelper.ManageNotifications(oldTicket, newTicket);
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
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