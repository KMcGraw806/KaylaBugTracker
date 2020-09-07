using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.ViewModels
{
    public class DashboardViewModel
    {
        public int TicketCount { get; set; }
        public int HighPriorityTicketCount { get; set; }
        public int NewTicketCount { get; set; }
        public int TotalComments { get; set; }

        public List<Ticket> AllTickets { get; set; }

        public ProjectViewModel ProjectVM { get; set; }

        public DashboardViewModel()
        {
            AllTickets = new List<Ticket>();
            ProjectVM = new ProjectViewModel();
        }
    }
}