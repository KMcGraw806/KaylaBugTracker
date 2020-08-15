using KaylaBugTracker.Helpers;
using KaylaBugTracker.Models;
using System.Linq;
using System.Web.Mvc;

namespace KaylaBugTracker.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

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
    }
}