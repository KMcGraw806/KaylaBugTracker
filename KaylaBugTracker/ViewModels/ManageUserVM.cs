using KaylaBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaylaBugTracker.ViewModels
{
    public class ManageUserVM
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string Role { get; set; }

        public IEnumerable<Project> AssignedProjects { get; set; }
        //public IEnumerable<SelectListItem> AssignedProject { get; set; }
        public MultiSelectList ProjectList { get; set; }
    }
}