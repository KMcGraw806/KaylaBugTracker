using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.ViewModels
{
    public class MyTicketVM
    {
        public int TicketPriorityId { get; set; }

        public int TicketStatusId { get; set; }

        public int TicketTypeId { get; set; }
        [Required]
        public string Issue { get; set; }
        [Required]
        public string IssueDescription { get; set; }

        public bool IsResolved { get; set; }

        public bool IsArchived { get; set; }

        public string DeveloperId { get; set; }
    }
}