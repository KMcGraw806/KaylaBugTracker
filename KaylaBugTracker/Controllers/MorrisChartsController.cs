using KaylaBugTracker.Models;
using KaylaBugTracker.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaylaBugTracker.Controllers
{
	
    public class MorrisChartsController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

		//ThisLiteral is a List of MorrisChartData
		//data: [
		//		{ priority: 'Immediate', value: 20 },
		//		{ priority: 'High', value: 10 },
		//		{ priority: 'Medium', value: 5 },
		//		{ priority: 'Low', value: 5 },
		//		{ priority: 'None', value: 20 }
		//	],
		public JsonResult GetAllTicketPriorityData()
        {
			var tickets = db.Tickets.ToList();
			var data = new List<MorrisChartData>();
			foreach(var priority in db.TicketPriorities.ToList())
            {
				data.Add(new MorrisChartData()
				{
					Label = priority.Name,
					Value = tickets.Where(t => t.TicketPriorityId == priority.Id).Count()
				}) ;
            }

			return Json(data);
        }
		public JsonResult GetAllTicketStatusData()
        {
			var tickets = db.Tickets.ToList();
			var data = new List<MorrisChartData>();
			foreach(var status in db.TicketStatuses.ToList())
            {
				data.Add(new MorrisChartData()
				{
					Label = status.Name,
					Value = tickets.Where(t => t.TicketStatusId == status.Id).Count()
				}) ;
            }

			return Json(data);
        }
		public JsonResult GetAllTicketTypeData()
		{
			var tickets = db.Tickets.ToList();
			var data = new List<MorrisChartData>();
			foreach (var type in db.TicketTypes.ToList())
			{
				data.Add(new MorrisChartData()
				{
					Label = type.Name,
					Value = tickets.Where(t => t.TicketTypeId == type.Id).Count()
				});
			}

			return Json(data);
		}
	}
}