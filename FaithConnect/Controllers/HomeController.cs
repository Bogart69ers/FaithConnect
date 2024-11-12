using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Utils;
using System.Web.Security;
using System.IO;
using FaithConnect.Repository;
using FaithConnect.ViewModel;

namespace FaithConnect.Controllers
{
    [Authorize(Roles = "Admin, User, Spiritual Leader")]
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            IsUserLoggedSession();
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
                        return RedirectToAction("AdminDashboard", "Admin");
                    case Constant.Role_User:
                        return RedirectToAction("Index", "Home");
                    case Constant.Role_Spiritual:
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
            var userinfo = _AccManager.GetUserInfoByUsername(username);
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
            userinfo.status = (int)Status.Active;
            _AccManager.UpdateUser(user, ref ErrorMessage);
            _AccManager.UpdateUserInformation(userinfo, ref ErrorMessage);


            TempData["SuccessMessage"] = "Account verified successfully.";
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult MyProfile()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var userInfo = _AccManager.GetUserInfoByUsername(username);
            if (userInfo == null)
            {
                TempData["ErrorMessage"] = "Error retrieving user information.";
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }

        [HttpPost]
        public ActionResult MyProfile(UserInformation userInf, HttpPostedFileBase profilePicture, HttpPostedFileBase coverPhoto)
        {
            IsUserLoggedSession();
            if (ModelState.IsValid)
            {
                var user = _AccManager.GetUserByUserId((Int32)userInf.userId);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Error updating profile: User not found.";
                    return View(userInf);
                }

                // Handle profile picture upload
                if (profilePicture != null && profilePicture.ContentLength > 0)
                {
                    var uploadsFolderPath = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(uploadsFolderPath))
                        Directory.CreateDirectory(uploadsFolderPath);

                    var profileFileName = Path.GetFileName(profilePicture.FileName);
                    var profileSavePath = Path.Combine(uploadsFolderPath, profileFileName);
                    profilePicture.SaveAs(profileSavePath);

                    var existingImage = _imgMgr.ListImgAttachByUserId(userInf.id).FirstOrDefault();
                    if (existingImage != null)
                    {
                        existingImage.imageFile = profileFileName;
                        if (_imgMgr.UpdateImg(existingImage, ref ErrorMessage) == ErrorCode.Error)
                        {
                            ModelState.AddModelError(String.Empty, ErrorMessage);
                            return View(userInf);
                        }
                    }
                    else
                    {
                        Image img = new Image
                        {
                            imageFile = profileFileName,
                            userId = userInf.id
                        };

                        if (_imgMgr.CreateImg(img, ref ErrorMessage) == ErrorCode.Error)
                        {
                            ModelState.AddModelError(String.Empty, ErrorMessage);
                            return View(userInf);
                        }
                    }
                }

                // Handle cover photo upload
                if (coverPhoto != null && coverPhoto.ContentLength > 0)
                {
                    var uploadsFolderPath = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(uploadsFolderPath))
                        Directory.CreateDirectory(uploadsFolderPath);

                    var coverFileName = Path.GetFileName(coverPhoto.FileName);
                    var coverSavePath = Path.Combine(uploadsFolderPath, coverFileName);
                    coverPhoto.SaveAs(coverSavePath);

                    var existingImage = _imgMgr.ListImgAttachByUserId(userInf.id).FirstOrDefault();
                    if (existingImage != null)
                    {
                        existingImage.coverPhoto = coverFileName;
                        if (_imgMgr.UpdateImg(existingImage, ref ErrorMessage) == ErrorCode.Error)
                        {
                            ModelState.AddModelError(String.Empty, ErrorMessage);
                            return View(userInf);
                        }
                    }
                    else
                    {
                        Image img = new Image
                        {
                            coverPhoto = coverFileName,
                            userId = userInf.id
                        };

                        if (_imgMgr.CreateImg(img, ref ErrorMessage) == ErrorCode.Error)
                        {
                            ModelState.AddModelError(String.Empty, ErrorMessage);
                            return View(userInf);
                        }
                    }
                }

                if (_AccManager.UpdateUserInformation(userInf, ref ErrorMessage) == ErrorCode.Error)
                {
                    ModelState.AddModelError(String.Empty, ErrorMessage);
                    return View(userInf);
                }

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("MyProfile");
            }
            return View(userInf);
        }


        [AllowAnonymous]
        public ActionResult Signup()
        {
            ViewBag.Role = Utilities.ListRole;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserInformation ui, UserAccount ua, string ConfirmPass, string firstname, string lastname, string phone, string address)
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

                var userInfo = _AccManager.UserInfoSignup(ua.username,firstname,lastname,phone,address, ref ErrorMessage);


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
        
        [Authorize]
        public ActionResult Group()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var userInformation = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var groups = _groupManager.GetAllGroups();
            var accounts = _AccManager.GetAllUsers();

            if (userInformation == null)
            {
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToAction("Index");
            }

            var groupMemberships = _groupManager.GetAllGroupMemberships(); // Fetch all group memberships
            var allGroups = _groupManager.GetAllGroups();

            // Filter out groups that the user has joined or has pending requests for
            var userGroupMemberships = groupMemberships.Where(m => m.userId == userInformation.id).ToList();
            var joinedOrPendingGroupIds = userGroupMemberships.Select(m => m.groupId).ToList();
            var discoverGroups = allGroups.Where(g => !joinedOrPendingGroupIds.Contains(g.id)).ToList();

            var model = new GroupViewModel
            {
                UserInformation = userInformation,
                GroupMemberships = groupMemberships,
                AllGroups = discoverGroups
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult JoinGroup(int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var membership = new GroupMembership
            {
                groupId = groupId,
                userId = user.id,
                status = 0, // Pending status
                dateJoined = DateTime.Now
            };

            if (_groupManager.AddGroupMembership(membership, ref ErrorMessage) != ErrorCode.Success)
            {
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToAction("Group");
            }

            TempData["SuccessMessage"] = "Join request sent successfully.";
            return RedirectToAction("Group");
        }
        public ActionResult GroupDetail(int groupId)
        {

            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var useracc = _AccManager.GetUserByUsername(username);
            var userinfo = _AccManager.GetUserInfoByUsername(username);

            ViewBag.CurrentUserId = useracc.id;
            ViewBag.CurrentUserInfoId = userinfo.id;

            var group = _groupManager.GetGroupById(groupId);
            if (group == null)
            {
                TempData["ErrorMessage"] = "Group not found.";
                return RedirectToAction("Group");
            }

            ViewBag.Currentgroup = group.id;

            // Assuming GetMembershipsByGroupId is a method in GroupManager
            var memberships = _groupManager.GetMembershipsByGroupId(groupId) ?? new List<GroupMembership>();

            var userMembershipsDetails = memberships
                .Join(_AccManager.GetAllUserInfo(),
                      m => m.userId,
                      u => u.userId,
                      (m, u) => new UserMembershipDetail
                      {
                          Membership = m,
                          User = u
                      }).ToList();

            var model = new GroupDetailViewModel
            {
                Group = group,
                Events = _eventManager.GetEventsByGroupId(groupId) ?? new List<Event>(),
                Posts = _postManager.GetPostsByGroupId(groupId) ?? new List<Post>(),
                Forums = _forumManager.GetForumsByGroupId(groupId) ?? new List<Forum>(),
                GroupMemberships = memberships,
                AllGroups = _groupManager.GetAllGroups() ?? new List<Groups>(),
                UserMembership = memberships.FirstOrDefault(m => m.userId == userinfo.id) // You may not need this if it's in the new list
            };

            return View(model);
        }



        [HttpPost]
        public ActionResult LeaveGroup(int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var groupManager = new GroupManager();
            var membership = groupManager.GetAllGroupMemberships().FirstOrDefault(m => m.groupId == groupId && m.userId == user.id);

            if (membership == null)
            {
                TempData["ErrorMessage"] = "Membership not found.";
                return RedirectToAction("Group");
            }

            if (groupManager.RemoveGroupMembership(membership.id, ref ErrorMessage) != ErrorCode.Success)
            {
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToAction("Group");
            }

            TempData["SuccessMessage"] = "Left group successfully.";
            return RedirectToAction("Group");
        }

        [HttpPost]
        public ActionResult ApproveGroupMembership(int groupId, int userId)
        {
            var membership = _groupManager.GetGroupMembershipByGroupIdAndUserId(groupId, userId);
            if (membership == null)
            {
                TempData["ErrorMessage"] = "Group membership not found.";
                return RedirectToAction("Group");
            }

            membership.status = 1; // Approved status
            if (_groupManager.UpdateGroupMembership(membership, ref ErrorMessage) != ErrorCode.Success)
            {
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToAction("Group");
            }

            TempData["SuccessMessage"] = "Group membership approved successfully.";
            return RedirectToAction("Group");
        }


        [Authorize]
        public ActionResult MyCalendar()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            return View(user);
        }
        [HttpPost]
        public ActionResult MyCalendar(Event ev)
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            return View(user);
        }

        public ActionResult Events()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            return View();
        }
        public ActionResult Guides()
        {
            return View();
        }

        //public ActionResult 

        [HttpPost]
        public ActionResult CreateGroup(Groups group, String privacy, String groupName, String description, int groupAdmin)
        {
            try
            {
                var currentUser = _AccManager.GetUserByUsername(User.Identity.Name);
                var userInfo = _AccManager.GetUserInfoByUsername(currentUser.username);
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    var model = PrepareManageAccountViewModel();
                    return View("Group", model);
                }
                group.privacy = privacy;
                group.groupAdmin = groupAdmin;
                group.status = (int)Status.Active;
                group.groupName = groupName;
                group.description = description;
                group.groupId = Utilities.gUid;
                group.date_created = DateTime.Now;

                if (_groupManager.CreateGroup(group, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("Group", model);
                }

                var groupMembership = new GroupMembership
                {
                    userId = userInfo.id,
                    groupId = group.id,
                    status = 1, // Assuming '1' represents 'joined' status
                    dateJoined = DateTime.Now
                };

                if (_groupManager.AddGroupMembership(groupMembership, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("Group", model);
                }

                TempData["SuccessMessage"] = "Group created successfully.";
                return RedirectToAction("Group");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("Group", model);
            }
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