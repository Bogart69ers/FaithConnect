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
        [Authorize]
        public ActionResult Index()
        {

            IsUserLoggedSession();
            var username = User.Identity.Name;

            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }

        [HttpPost]
        public ActionResult Index(UserInformation userInf)
        {
            if (_AccManager.UpdateUserInformation(userInf, ref ErrorMessage) == ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(userInf);

            }
            return RedirectToAction("Index");

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

                if (user.status != (Int32)Status.Active)
                {
                    TempData["username"] = username;
                    return RedirectToAction("Verify");
                }

                var info = _AccManager.GetUserInfoByUsername(username);
                if (info == null)
                {
                    ViewBag.Error = "User information not found.";
                    return View();
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
                        return RedirectToAction("Index", "Home");
                    case Constant.Role_User:
                        return RedirectToAction("Index", "Home");
                    default:
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Error = ErrorMessage;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
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
        public ActionResult Verify(string code, string username)
        {
            if (String.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            TempData["username"] = username;

            var user = _AccManager.GetUserByUsername(username);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return View();
            }

            if (!user.vcode.Equals(code))
            {
                TempData["error"] = "Incorrect Code";
                return View();
            }

            user.status = (int)Status.Active;
            _AccManager.UpdateUser(user, ref ErrorMessage);

            // Now add the user information to the UserInformation table
            var ui = new UserInformation
            {
                userId = user.userId,
                email = user.email,
                first_name = TempData["firstname"]?.ToString(),
                last_name = TempData["lastname"]?.ToString(),
                phone = TempData["phone"]?.ToString()
            };

            if (_AccManager.CreateUserInformation(ui, ref ErrorMessage) != ErrorCode.Success)
            {
                TempData["error"] = ErrorMessage;
                return View();
            }

            TempData["SuccessMessage"] = "Account verified successfully.";
            return RedirectToAction("Login");
        }


        [Authorize]
        public ActionResult MyProfile()
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Error retrieving user information.";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult MyProfile(UserInformation userInf, HttpPostedFileBase profilePicture)
        {
            var user = _AccManager.GetUserByUserId(userInf.userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Error updating profile: User not found.";
                return RedirectToAction("Index");
            }

            if (profilePicture != null && profilePicture.ContentLength > 0)
            {
                var uploadsFolderPath = Server.MapPath("~/UploadedFiles/");

                // Check if the directory exists, and create it if it does not
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Save the profile picture to the server
                var fileName = Path.GetFileName(profilePicture.FileName);
                var serverSavePath = Path.Combine(uploadsFolderPath, fileName);
                profilePicture.SaveAs(serverSavePath);

                // Update the user's profile with the new image file name
                userInf.imageFile = fileName;
            }

            userInf.userId = user.userId;  // Ensure userId is correctly set
            if (_AccManager.UpdateUserInformation(userInf, ref ErrorMessage) == ErrorCode.Error)
            {
                TempData["ErrorMessage"] = $"Error updating profile: {ErrorMessage}";
                return View(userInf);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index");
        }



        [AllowAnonymous]
        public ActionResult Signup()
        {
            ViewBag.Role = Utilities.ListRole;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserAccount ua, string ConfirmPass, string firstname, string lastname, string phone)
        {
            try
            {
                if (!ua.password.Equals(ConfirmPass))
                {
                    ModelState.AddModelError(string.Empty, "Password not match");
                    var roleList = Utilities.ListRole;
                    roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                    ViewBag.Role = roleList;
                    return View(ua);
                }

                var ui = new UserInformation
                {
                    first_name = firstname,
                    last_name = lastname,
                    phone = phone
                };

                if (!ModelState.IsValid)
                {
                    var roleList = Utilities.ListRole;
                    roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                    ViewBag.Role = roleList;
                    return View(ua);
                }

                if (_AccManager.SignUp(ua, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
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
                    ModelState.AddModelError(string.Empty, errorMessage);
                    ViewBag.Role = Utilities.ListRole;
                    return View(ua);
                }

                TempData["username"] = ua.username;
                TempData["firstname"] = firstname;
                TempData["lastname"] = lastname;
                TempData["phone"] = phone;

                TempData["SuccessMessage"] = "Account created successfully. Please enter the OTP sent to your email.";
                return RedirectToAction("Verify");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var roleList = Utilities.ListRole;
                roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                ViewBag.Role = roleList;
                return View(ua);
            }
        }





    }
}