using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaithConnect.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }
        public ActionResult ManageAccount()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }

        public ActionResult Approval()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }
        public ActionResult Reports()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }


        public ActionResult CreateGroup()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        
    }
}