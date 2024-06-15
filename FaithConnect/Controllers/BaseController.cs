using FaithConnect.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaithConnect.Controllers
{
    public class BaseController : Controller
    {
        public String ErrorMessage;
        public BaseRepository<UserAccount> _UserAcc;

        // GET: Base
        public BaseController()
        {
            _UserAcc = new BaseRepository<UserAccount>();
        }

        public void IsUserLoggedSession()
        {
            
            
        }
    }
}