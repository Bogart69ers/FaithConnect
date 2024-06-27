using FaithConnect.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Models;

namespace FaithConnect.Controllers
{
    public class BaseController : Controller
    {
        public String ErrorMessage;
        public BaseRepository<UserAccount> _UserAcc;
        public AccountManager _AccManager;

        public String Username { get { return User.Identity.Name; } }
        public String UserId { get { return _AccManager.GetUserByUsername(Username).userId; } }
        // GET: Base
        public BaseController()
        {
            _UserAcc = new BaseRepository<UserAccount>();
            _AccManager = new AccountManager();
        }

        public void IsUserLoggedSession()
        {
            UserLogged userLogged = new UserLogged();
            if (User != null)
            {
                if (User != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        userLogged.UserAccount = _AccManager.GetUserByUsername(User.Identity.Name);
                        userLogged.UserInformation = _AccManager.CreateOrRetrieve(userLogged.UserAccount.username, ref ErrorMessage);
                    }
                }
                Session["User"] = userLogged;
            }
            
        }
    }
}