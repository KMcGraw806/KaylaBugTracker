using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.Helpers
{
    public class TicketEditVM
    {
        public int Id { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string DeveloperId { get; set; }
        public string Issue { get; set; }
        public string IssueDescription { get; set; }
        public bool IsResolved { get; set; }
        public bool IsArchived { get; set; }

        public TicketEditVM(Ticket ticket)
        {
            Id = ticket.Id;
            PriorityId = ticket.TicketPriorityId;
            StatusId = ticket.TicketStatusId;
            TypeId = ticket.TicketTypeId;
            DeveloperId = ticket.DeveloperId;
            Issue = ticket.Issue;
            IssueDescription = ticket.IssueDescription;
            IsResolved = ticket.IsResolved;
            IsArchived = ticket.IsArchived;
        }
    }
}