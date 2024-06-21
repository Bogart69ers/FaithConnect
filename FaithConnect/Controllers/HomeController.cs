using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Utils;
using System.Web.Security;
using System.IO;
using FaithConnect.Repository;

namespace FaithConnect.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            IsUserLoggedSession();
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = string.Empty;
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (_AccManager == null)
            {
                ViewBag.Error = "Account manager is not initialized.";
                return View();
            }

            if (_AccManager.SignIn(username, password, ref ErrorMessage) == ErrorCode.Success)
            {
                var user = _AccManager.GetUserByUsername(username);
                if (user == null)
                {
                    ViewBag.Error = "User not found.";
                    return View();
                }

                var info = _AccManager.GetUserInfoByUsername(username);
                if (info == null)
                {
                    ViewBag.Error = "User information not found.";
                    return View();
                }

                if (user.status != (int)Status.Active)
                {
                    TempData["username"] = username;
                    return RedirectToAction("Verify");
                }

                FormsAuthentication.SetAuthCookie(username, false);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (user.Role1 == null)
                {
                    ViewBag.Error = "User role is not defined.";
                    return View();
                }

                switch (user.Role1.roleName)
                {
                    case Constant.Role_Admin:
                        return RedirectToAction("Index", "Admin");
                    case Constant.Role_User:
                        return RedirectToAction("Index", "User");
                    default:
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Error = ErrorMessage;

            return View();
        }



        [AllowAnonymous]
        public ActionResult Verify()
        {
            if (String.IsNullOrEmpty(TempData["username"] as String))
                return RedirectToAction("Login");

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Verify(String code, String username)
        {
            if (String.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            TempData["username"] = username;

            var user = _AccManager.GetUserByUsername(username);

            if (!user.vcode.Equals(code))
            {
                TempData["error"] = "Incorrect Code";
                return View();
            }

            user.status = (Int32)Status.Active;
            _AccManager.UpdateUser(user, ref ErrorMessage);

            return RedirectToAction("MyProfile");
        }
        [AllowAnonymous]
        public ActionResult MyProfile()
        {
            //IsUserLoggedSession();

            //var username = User.Identity.Name;

            //var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            //var userEmail = _AccManager.GetUserByEmail(user.email);

            //ViewBag.userEmail = userEmail.email;

            return View();
        }

        //[HttpPost]
        //public ActionResult MyProfile(UserInformation userInf, HttpPostedFileBase profilePicture)
        //{
        //    var userEmail = _AccManager.GetUserByEmail(userInf.email); // Assuming User.Identity.Name contains the user's email

        //    ViewBag.userEmail = userEmail.email;

        //    if (profilePicture != null && profilePicture.ContentLength > 0)
        //    {
        //        var uploadsFolderPath = Server.MapPath("~/UploadedFiles/");

        //        // Check if the directory exists, and create it if it does not
        //        if (!Directory.Exists(uploadsFolderPath))
        //        {
        //            Directory.CreateDirectory(uploadsFolderPath);
        //        }

        //        // Save the profile picture to the server
        //        var fileName = Path.GetFileName(profilePicture.FileName);
        //        var serverSavePath = Path.Combine(uploadsFolderPath, fileName);
        //        profilePicture.SaveAs(serverSavePath);

        //        // Update the user's profile with the new image file name
        //        userInf.imageFile = fileName;

        //        // Update user information in the database
        //        if (_AccManager.UpdateUserInformation(userInf, ref ErrorMessage) == ErrorCode.Error)
        //        {
        //            ModelState.AddModelError(string.Empty, ErrorMessage);
        //            return View(userInf);
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Please select a valid image file.");
        //    }

        //    TempData["Message"] = ModelState.IsValid ? "Profile picture updated successfully." : "Failed to update profile picture.";
        //    return RedirectToAction("MyProfile"); // Redirect to the MyProfile action to refresh the view
        //}

        [AllowAnonymous]
        public ActionResult Signup()
        {
            ViewBag.Role = Utilities.ListRole;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserAccount ua, string ConfirmPass)
        {
            if (!ua.password.Equals(ConfirmPass))
            {
                ModelState.AddModelError(String.Empty, "Password not match");
                var roleList = Utilities.ListRole;
                roleList.FirstOrDefault(x => x.Value == "user").Selected = true;
                ViewBag.Role = roleList;
                return View(ua);
            }

            if (_AccManager.SignUp(ua, ref ErrorMessage) != ErrorCode.Success)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);

                ViewBag.Role = Utilities.ListRole;
                return View(ua);
            }

            var user = _AccManager.GetUserByEmail(ua.email);
            string verificationCode = ua.vcode;

            string emailBody = $"Your verification code is: {verificationCode}";
            string errorMessage = "";

            var mailManager = new MailManager();
            bool emailSent = mailManager.SendEmail(ua.email, "Verification Code", emailBody, ref errorMessage);

            if (!emailSent)
            {
                ModelState.AddModelError(String.Empty, errorMessage);
                ViewBag.Role = Utilities.ListRole;
                return View(ua);
            }
            TempData["username"] = ua.username;
            return RedirectToAction("Verify");

        }

    }
}