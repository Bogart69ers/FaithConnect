using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Utils;
using FaithConnect.Repository;
using FaithConnect.ViewModel;

namespace FaithConnect.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var accounts = _AccManager.GetAllUsers();
            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts
            };

            return View(model);
        }


        public ActionResult Approval()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var accounts = _AccManager.GetAllUsers();
            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts
            };

            return View(model);
        }
        public ActionResult Reports()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var accounts = _AccManager.GetAllUsers();
            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts
            };

            return View(model);
        }


        public ActionResult ManageGroups()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var groups = _groupManager.GetAllGroups();

            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                Groups = groups,
                Group = new Groups()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateGroup(Groups group)
        {
            try
            {
                var currentUser = _AccManager.GetUserByUsername(User.Identity.Name);
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    var model = PrepareManageAccountViewModel();
                    return View("ManageGroups", model);
                }

                group.groupId = Utilities.gUid;
                group.date_created = DateTime.Now;

                if (_groupManager.CreateGroup(group, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageGroups", model);
                }

                TempData["SuccessMessage"] = "Group created successfully.";
                return RedirectToAction("ManageGroups");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("ManageGroups", model);
            }
        }

        [HttpPost]
        public ActionResult DeleteGroup(int id)
        {
            var result = _groupManager.DeleteGroup(id, ref ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return RedirectToAction("ManageGroups");
            }

            ModelState.AddModelError("", ErrorMessage);
            var model = PrepareManageAccountViewModel();
            return View("ManageGroups", model);
        }

        
        public ActionResult ManageAccount()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var accounts = _AccManager.GetAllUsers();
            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts,
                UserAccount = new UserAccount()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserAccount ua, string ConfirmPass, string firstname, string lastname, string phone, string address)
        {
            try
            {
                if (!ua.password.Equals(ConfirmPass))
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    var model = PrepareManageAccountViewModel();
                    return View("ManageAccount", model);
                }

                if (!ModelState.IsValid)
                {
                    var model = PrepareManageAccountViewModel();
                    return View("ManageAccount", model);
                }

                ua.userId = Utilities.gUid;
                ua.vcode = Utilities.code.ToString();
                ua.date_created = DateTime.Now;
                ua.status = (int)Status.InActive;

                if (_AccManager.SignUp(ua, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageAccount", model);
                }

                var ui = new UserInformation
                {
                    userId = ua.userId,
                    first_name = firstname,
                    last_name = lastname,
                    phone = phone,
                    address = address,
                    email = ua.email,
                    status = (int)Status.Active,
                    date_created = DateTime.Now
                };

                if (_AccManager.CreateUserInformation(ui, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageAccount", model);
                }

                string emailBody = $"Your verification code is: {ua.vcode}";
                string errorMessage = "";

                var mailManager = new MailManager();
                bool emailSent = mailManager.SendEmail(ua.email, "Verification Code", emailBody, ref errorMessage);

                if (!emailSent)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageAccount", model);
                }

                TempData["SuccessMessage"] = "Account created successfully. Please enter the OTP sent to your email.";
                return RedirectToAction("ManageAccount");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("ManageAccount", model);
            }
        }




        public ActionResult Edit(int id)
        {
            var user = _AccManager.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserAccount userAccount, string ConfirmPass)
        {
            if (string.IsNullOrEmpty(userAccount.password) && string.IsNullOrEmpty(ConfirmPass))
            {
                // Password is not provided, no need to check for password matching
                ModelState.Remove("password");
                ModelState.Remove("ConfirmPass");
            }
            else if (!userAccount.password.Equals(ConfirmPass))
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Json(new { success = false, message = "Passwords do not match." });
            }

            if (ModelState.IsValid)
            {
                var existingUser = _AccManager.GetUserById(userAccount.id);

                if (existingUser != null)
                {
                    existingUser.username = userAccount.username;
                    existingUser.email = userAccount.email;
                    existingUser.role = userAccount.role;
                    existingUser.status = userAccount.status;

                    if (!string.IsNullOrEmpty(userAccount.password))
                    {
                        existingUser.password = userAccount.password; // Only update password if provided
                    }

                    var result = _AccManager.UpdateUser(existingUser, ref ErrorMessage);
                    if (result == ErrorCode.Success)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, message = ErrorMessage });
                }
            }

            return Json(new { success = false, message = "Invalid model state." });
        }




        [HttpGet]
        public JsonResult GetUser(int id)
        {
            var user = _AccManager.GetUserById(id);
            if (user != null)
            {
                return Json(new
                {
                    id = user.id,
                    username = user.username,
                    email = user.email,
                    role = user.role,
                    status = user.status
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _AccManager.DeleteUser(id, ref ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return RedirectToAction("ManageAccount");
            }
            // Handle error
            ModelState.AddModelError("", ErrorMessage);
            var model = PrepareManageAccountViewModel();
            return View("ManageAccount", model);
        }
        private ManageAccountViewModel PrepareManageAccountViewModel()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var accounts = _AccManager.GetAllUsers();
            var groups = _groupManager.GetAllGroups();
            return new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts,
                UserAccount = new UserAccount(),
                Groups = groups,
                Group = new Groups()
            };
        }
        

    }
}
