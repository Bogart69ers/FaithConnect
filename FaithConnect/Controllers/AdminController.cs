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
            return View();
        }
        public ActionResult ManageAccount()
        {
            return View();
        }

        public ActionResult Approval()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }
    }
}