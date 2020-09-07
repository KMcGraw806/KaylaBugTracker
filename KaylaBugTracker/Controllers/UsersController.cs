using KaylaBugTracker.Helpers;
using KaylaBugTracker.Models;
using KaylaBugTracker.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace KaylaBugTracker.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult ManageUserRoles(string id)
        {
            var userRole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);
            return View(db.Users.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRoles(string id, string roleName)
        {
            foreach(var role in roleHelper.ListUserRoles(id))
            {
                roleHelper.RemoveUserFromRole(id, role);
            }
            if(!string.IsNullOrEmpty(roleName))
            {
                roleHelper.AddUserToRole(id, roleName);
            }

            return RedirectToAction("ManageUserRoles", id);
        }

        public ActionResult Manage(string userId)
        {
            var manageUserVM = new ManageUserVM();
            var user = db.Users.Find(userId);

            manageUserVM.FirstName = user.FirstName;
            manageUserVM.LastName = user.LastName;
            manageUserVM.Email = user.Email;
            manageUserVM.AvatarPath = user.AvatarPath;
            manageUserVM.AssignedProjects = projectHelper.ListUserProjects(userId);
            manageUserVM.ProjectList = new MultiSelectList(db.Projects, "Id", "Name", manageUserVM.AssignedProjects.Select(p => p.Id));
            manageUserVM.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();

            ViewBag.RoleList = new SelectList(db.Roles, "Name", "Name", manageUserVM.Role);

            return View(manageUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Submitter")]
        public ActionResult Manage([Bind (Include = "Id,ProjectId,TicketPriorityId,TicketTypeId,Title,Description")] Ticket ticket)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}