using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Utils;
using System.Web.Security;
using System.IO;
using System.Data.Entity;
using FaithConnect.Repository;
using FaithConnect.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace FaithConnect.Controllers
{
    [Authorize(Roles = "Admin, User, Spiritual Leader")]
    public class HomeController : BaseController
    {

        [HttpPost]
        public JsonResult RepostPost(int postId)
        {
            string errorMsg = string.Empty;

            try
            {
                var username = User.Identity.Name;
                var userAccount = _AccManager.GetUserByUsername(username);
                var userId = userAccount.id;

                var repost = new Repost
                {
                    postId = postId,
                    userId = userId,
                    dateReposted = DateTime.Now
                };

                // Use the result of AddRepost
                var result = _repostManager.AddRepost(repost, ref errorMsg);

                if (result == ErrorCode.Success)
                {
                    return Json(new { success = true, message = "Repost successful!" });
                }

                return Json(new { success = false, error = errorMsg ?? "Unknown error occurred." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }




        [HttpPost]
        public JsonResult ToggleLike(int postId)
        {
            var username = User.Identity.Name;
            var userAccount = _AccManager.GetUserByUsername(username);
            var userId = userAccount.id;
            var reactionManager = new ReactionManager();

            string errorMsg = string.Empty;

            if (reactionManager.UserHasLiked(postId, userId))
            {
                // Remove the like
                reactionManager.RemoveReaction(postId, userId, ref errorMsg);
            }
            else
            {
                // Add or update the like
                reactionManager.AddOrUpdateReaction(postId, userId, ref errorMsg);
            }

            var likeCount = reactionManager.GetReactionCount(postId);

            return Json(new { success = true, likeCount });
        }



        public ActionResult GenerateEventReport(int groupId)
        {
            try
            {
                var group = _groupManager.GetGroupById(groupId);
                var groupName = group?.groupName ?? "Unknown Group";

                var events = _eventManager.GetEventsByGroupId(groupId);
                var attendanceCounts = events
                    .ToDictionary(e => e.id, e => _eventManager.GetEventAttendanceCount(e.id));

                using (var ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    var titleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
                    document.Add(new Paragraph($"Event Report for Group: {groupName}", titleFont));
                    document.Add(new Paragraph($"Generated on: {DateTime.Now}", new Font(Font.FontFamily.HELVETICA, 12)));
                    document.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 15, 40, 25, 20, 20 });

                    var headerFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                    table.AddCell(new PdfPCell(new Phrase("Event ID", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Event Title", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Event Date", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Attendees", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Event Status", headerFont)));

                    foreach (var ev in events)
                    {
                        table.AddCell(ev.id.ToString());
                        table.AddCell(ev.title ?? "N/A");
                        table.AddCell(ev.event_date?.ToString("yyyy-MM-dd hh:mm tt") ?? "N/A");
                        table.AddCell(attendanceCounts.ContainsKey(ev.id) ? attendanceCounts[ev.id].ToString() : "0");
                        string statusText = ev.status == 1 ? "Approved" : "Pending";
                        table.AddCell(statusText);
                    }

                    document.Add(table);
                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "inline; filename=EventReport.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                    Response.OutputStream.Flush();
                    Response.End();

                    return new EmptyResult(); // Avoid returning a view
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "events" });
            }
        }

        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var userInformation = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var userAccount = _AccManager.GetUserByUsername(username);
            var posts = _postManager.GetAllPosts();
            var events = _eventManager.GetAllEventAttendance();

            ViewBag.CurrentUserId = userAccount.id;

            var groupPhoto = events
            .Select(e => e.Groups.GroupImage.FirstOrDefault()?.coverPhoto)
            .FirstOrDefault();

            ViewBag.GroupPhoto = groupPhoto != null
            ? Url.Content("~/UploadedFiles/" + groupPhoto)
            : Url.Content("~/UploadedFiles/default-cover.png");

            var eventAttendanceCounts = events
               .GroupBy(e => e.eventId)
               .ToDictionary(
                   g => g.Key,
                   g => g.Count(a => a.status == 1) // Count only those marked as "Going"
               );

            ViewBag.EventAttendanceCounts = eventAttendanceCounts;

            var allUsers = _AccManager.GetAllUserInfo();  // Change this method to retrieve UserInformation
   
            var model = new GroupViewModel
            {
                Events = events,
                UserAcc = userAccount,
                UserInformation = userInformation,
                Posts = posts,
                UserInformations = allUsers  // Add the list of all users

            };

            return View(model);
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
        [HttpGet]
        public JsonResult GetUnreadNotificationCount()
        {
            var username = User.Identity.Name;
            var userAccount = _AccManager.GetUserByUsername(username);
            var notifications = _NotificationManager.GetNotificationsByUserId(userAccount.id);

            var unreadCount = notifications.Count(n => n.isRead == false);

            return Json(new { unreadCount }, JsonRequestBehavior.AllowGet);
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

                if (user.status != (Int32)Status.Active)
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

            FormsAuthentication.SetAuthCookie(username, false);

            TempData["SuccessMessage"] = "Account verified successfully.";
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult MyProfile()
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var userInfo = _AccManager.GetUserInfoByUsername(username);
            var userAcc = _AccManager.GetUserByUsername(username);

            var posts = _postManager.GetAllPosts();
            var repostedPosts = _repostManager.GetRepostedPostsByUser(userAcc.id); // Fetch reposted posts for the user

            var allUsers = _AccManager.GetAllUserInfo();  // Change this method to retrieve UserInformation

            ViewBag.CurrentUserId = userAcc.id;

            var model = new GroupViewModel
            {
                UserInformation = userInfo,
                UserInformations = allUsers,
                Posts = posts,
                RepostedPosts = repostedPosts // Include reposted posts in the model

            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult MyProfile(UserInformation userInf, string religion, string address, string lastname, string phone, string email, string firstname, string bio, HttpPostedFileBase profilePicture, HttpPostedFileBase coverPhoto)
        {
            IsUserLoggedSession();
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var userInfo = _AccManager.GetUserInfoByUsername(username);


                var user = _AccManager.GetUserByUserId((Int32)userInfo.userId);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Error updating profile: User not found.";               
                    return View(user);

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

                    var existingImage = _imgMgr.ListImgAttachByUserId(userInfo.id).FirstOrDefault();
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
                            userId = userInfo.id
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

                    var existingImage = _imgMgr.ListImgAttachByUserId(userInfo.id).FirstOrDefault();
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
                            userId = userInfo.id
                        };

                        if (_imgMgr.CreateImg(img, ref ErrorMessage) == ErrorCode.Error)
                        {
                            ModelState.AddModelError(String.Empty, ErrorMessage);
                            return View(userInf);
                        }
                    }
                }

                UserInformation inf = new UserInformation
                {                     
                    id = userInfo.id,
                    userId = userInfo.userId,
                    date_created = userInfo.date_created,
                    status = userInfo.status,
                    religion = userInfo.religion,
                    first_name = firstname,
                    last_name = lastname,
                    email = email,
                    phone = phone,
                    address = address,
                    bio = bio
                };
                if (_AccManager.UpdateUserInformation(inf, ref ErrorMessage) == ErrorCode.Error)
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
            ViewBag.Role = Utils.Utilities.ListRole;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserInformation ui, UserAccount ua, string ConfirmPass, string firstname, string lastname, string phone, string address, string religion)
        {
            try
            {
                if (!ua.password.Equals(ConfirmPass))
                {
                    ModelState.AddModelError(string.Empty, "Password not match");
                    var roleList = Utils.Utilities.ListRole;
                    roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                    ViewBag.Role = roleList;
                    return View(ua);
                }

                if (!ModelState.IsValid)
                {
                    var roleList = Utils.Utilities.ListRole;
                    roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                    ViewBag.Role = roleList;
                    return View(ua);
                }

                if (_AccManager.SignUp(ua, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    ViewBag.Role = Utils.Utilities.ListRole;
                    return View(ua);
                }

                var userInfo = _AccManager.UserInfoSignup(ua.username,firstname,lastname,phone,address,religion, ref ErrorMessage);


                var user = _AccManager.GetUserByEmail(ua.email);
                string verificationCode = ua.vcode;

                string emailBody = $"Your verification code is: {verificationCode}";
                string errorMessage = "";

                var mailManager = new MailManager();
                bool emailSent = mailManager.SendEmail(ua.email, "Verification Code", emailBody, ref errorMessage);

                if (!emailSent)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                    ViewBag.Role = Utils.Utilities.ListRole;
                    return View(ua);
                }
                TempData["username"] = ua.username;

                TempData["SuccessMessage"] = "Account created successfully. Please enter the OTP sent to your email.";
                return RedirectToAction("Verify");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var roleList = Utils.Utilities.ListRole;
                roleList.FirstOrDefault(x => x.Value == "User").Selected = true;
                ViewBag.Role = roleList;
                return View(ua);
            }
        }

        [HttpPost]
        public ActionResult DeleteEvent(int id, int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var result = _eventManager.DeleteEvent(id, ref ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "events" });
            }
            // Handle error

            return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "events" });
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
                AllGroups = discoverGroups,
                Group = groups
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult CheckIfGoing(int eventId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var userId = user.userId;

            var existingAttendance = _eventManager.CheckAttendance(eventId, userId);

            return Json(new { isGoing = existingAttendance != null });
        }

        [HttpPost]
        public ActionResult MarkAsGoing(int eventId, int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var userId = user.userId;
            // Check if user already marked "Going" for this event

            var existingAttendance = _eventManager.CheckAttendance(eventId, userId);

            if (existingAttendance != null && existingAttendance.status == 1)
            {
                TempData["WarningMessage"] = "You have already marked yourself as 'Going' for this event.";
                return Redirect(Request.UrlReferrer?.ToString() ?? Url.Action("Index", "Home"));
            }

            var eventAttendance = new EventAttendance
            {
                eventId = eventId,
                groupId = groupId,
                userId = userId,
                status = 1,
            };
            var result = _eventManager.MarkEventAsGoing(eventAttendance, ref ErrorMessage);

            if (result == ErrorCode.Success)
            {
                TempData["SuccessMessage"] = "You have successfully marked yourself as 'Going' for this event.";
            }
            else
            {
                TempData["ErrorMessage"] = ErrorMessage;
            }

            // Redirect to the referring page
            return Redirect(Request.UrlReferrer?.ToString() ?? Url.Action("Index", "Home"));

        }

        [HttpPost]
        public ActionResult CancelJoinRequest(int groupId, string returnUrl)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return Redirect(returnUrl ?? Url.Action("Group"));
            }

            if (_groupManager.CancelJoinRequest(groupId, user.id, ref ErrorMessage) != ErrorCode.Success)
            {
                TempData["ErrorMessage"] = ErrorMessage;
                return Redirect(returnUrl ?? Url.Action("Group"));
            }

            TempData["SuccessMessage"] = "Join request canceled successfully.";
            return Redirect(returnUrl ?? Url.Action("Group"));
        }


        [HttpPost]
        public ActionResult JoinGroup(int groupId, string returnUrl)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return Redirect(returnUrl ?? Url.Action("Group"));
            }

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
                return Redirect(returnUrl ?? Url.Action("Group"));
            }

            var group = _groupManager.GetGroupById(groupId);
            if (group == null)
            {
                TempData["ErrorMessage"] = "Group not found.";
                return Redirect(returnUrl ?? Url.Action("Group"));
            }

            var adminUserId = group.groupAdmin;

            string requestMessage = $"{user.first_name} has requested to join your group '{group.groupName}'.";

            // 5) Create & save notification for the group's admin
            var newNotification = new Notification
            {
                userId = adminUserId,  // recipient is the admin
                message = requestMessage,
                isRead = false,
                date = DateTime.Now, // or CreatedAt, etc.
                userIdfrom = user.id
            };

            string notifErrorMsg;
            var notifResult = _NotificationManager.CreateNotification(newNotification, out notifErrorMsg);
            if (notifResult == ErrorCode.Error)
            {
                // handle error if needed
                TempData["ErrorMessage"] = notifErrorMsg;
            }

            // 6) Provide user feedback
            TempData["SuccessMessage"] = "Join request sent successfully.";
            return Redirect(returnUrl ?? Url.Action("Group"));
        }
        [HttpGet]
        public JsonResult GetEventAttendanceCount(int eventId)
        {
            var count = _eventManager.GetEventAttendanceCount(eventId);
            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GroupDetail(int groupId)
        {
            IsUserLoggedSession();
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var useracc = _AccManager.GetUserByUsername(username);
            var userinfo = _AccManager.GetUserInfoByUsername(username);
            var allGroups = _groupManager.GetAllGroups();
            var post = _postManager.GetPostsByGroupId(groupId);

            ViewBag.CurrentUserId = useracc.id;
            ViewBag.CurrentUserInfoId = userinfo.id;

            var group = _groupManager.GetGroupById(groupId);
            if (group == null)
            {
                TempData["ErrorMessage"] = "Group not found.";
                return RedirectToAction("Group");
            }

            ViewBag.Currentgroup = group.id;
            
            // Retrieve and join memberships with user information for full details
            var membership = _groupManager.GetMembershipsByGroup(groupId) ?? new List<GroupMembership>();
            var membrships = _groupManager.GetAllGroupMemberships();

            var memberships = _groupManager.GetAllGroupMemberships()
                .Where(m => m.groupId == groupId)
                .ToList();
            var pendingMemberships = _groupManager.GetMembershipsByGroup(groupId) ?? new List<GroupMembership>();

            var posts = _postManager.GetPostByGroup(groupId) ?? new List<Post>();
            var forum = _forumManager.GetForumByGroup(groupId) ?? new List<Forum>();
            var events = _eventManager.GetEventsByGroupIdWithMedia(groupId).ToList();

            var groupImage = group?.GroupImage?.FirstOrDefault();
            var groupName = group.groupName;

            ViewBag.GroupName = groupName; 
            ViewBag.GroupImage = groupImage?.coverPhoto ?? "default.png";

            var allUserInfos = _AccManager.GetAllUserInfo();
            var groupMemberships = _groupManager.GetAllGroupMemberships(); // Fetch all group memberships
            var userGroupMemberships = groupMemberships.Where(m => m.userId == userinfo.id).ToList();
            var joinedOrPendingGroupIds = userGroupMemberships.Select(m => m.groupId).ToList();
            var discoverGroups = allGroups.Where(g => !joinedOrPendingGroupIds.Contains(g.id)).ToList();

            var eventAttendanceCounts = events
                .Select(e => new
                {
                    EventId = e.id,
                    GoingCount = _eventManager.GetEventAttendanceCount(e.id)
                })
                .ToDictionary(e => e.EventId, e => e.GoingCount);


            ViewBag.EventAttendanceCounts = eventAttendanceCounts;

            var userMembershipDetails = memberships
                .Join(allUserInfos,
                      m => m.userId,
                      u => u.userId,
                      (m, u) => new UserMembershipDetail
                      {
                          Membership = m,
                          User = u
                      }).ToList();

            var userMember = membership
                .Join(allUserInfos,
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
                UserInformation = user,
                Events = events,
                Posts = posts,
                Forums = _forumManager.GetForumsByGroupId(groupId) ?? new List<Forum>(),
                GroupMemberships = memberships, // Use combined list here
                GrpMembership = membrships,
                MemberManagements = pendingMemberships,
                PostManage = posts,
                ForumManage = forum,
                AllGroupMembers = discoverGroups,
                AllGroups = _groupManager.GetAllGroups() ?? new List<Groups>(),
                UserMembership = memberships.FirstOrDefault(m => m.userId == userinfo.id),
                GroupMembership = userMembershipDetails,
                MemberManagement = userMember,
                UserInformations = allUserInfos // Ensure UserInformations is populated
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeGroupPhoto(int groupId, string groupName, string description, HttpPostedFileBase coverPhoto)
        {
            IsUserLoggedSession();

            if (ModelState.IsValid)
            {
                var group = _groupManager.GetGroupById(groupId);
                if (group == null)
                {
                    TempData["ErrorMessage"] = "Error updating profile: User not found.";
                    return RedirectToAction("GroupDetail", new { groupId });
                }

                group.groupName = groupName;
                group.description = description;


                // Handle cover photo upload
                if (coverPhoto != null && coverPhoto.ContentLength > 0)
                {
                    var uploadsFolderPath = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(uploadsFolderPath))
                        Directory.CreateDirectory(uploadsFolderPath);

                    var coverFileName = Path.GetFileName(coverPhoto.FileName);
                    var coverSavePath = Path.Combine(uploadsFolderPath, coverFileName);
                    coverPhoto.SaveAs(coverSavePath);

                    var existingImage = _imgMgr.ListImgAttachByGroupId(groupId).FirstOrDefault();
                    if (existingImage != null)
                    {
                        existingImage.coverPhoto = coverFileName;
                        if (_imgMgr.UpdateGroupImg(existingImage, ref ErrorMessage) == ErrorCode.Error)
                        {
                            TempData["ErrorMessage"] = ErrorMessage;
                            return RedirectToAction("GroupDetail", new { groupId });
                        }
                    }
                    else
                    {
                        GroupImage img = new GroupImage
                        {
                            coverPhoto = coverFileName,
                            groupId = groupId
                        };

                        if (_imgMgr.CreateGroupImg(img, ref ErrorMessage) == ErrorCode.Error)
                        {
                            TempData["ErrorMessage"] = ErrorMessage;
                            return RedirectToAction("GroupDetail", new { groupId });
                        }
                    }
                }

                if (_groupManager.UpdateGroup(group, ref ErrorMessage) == ErrorCode.Error)
                {
                    TempData["ErrorMessage"] = ErrorMessage;
                    return RedirectToAction("GroupDetail", new { groupId });
                }

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("GroupDetail", new { groupId });
            }

            TempData["ErrorMessage"] = "Validation failed. Please check your input.";
            return RedirectToAction("GroupDetail", new { groupId });
        }

        [HttpPost]
        public ActionResult RemoveMember(int userId, int id, int status, int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(groupId);
            var result = _groupManager.RemoveGroupMembership(id, ref ErrorMessage); 
            var userInfo = _AccManager.GetUserInfoById(userId);

            if (result == ErrorCode.Success)
            {
                string requestMessage = $"Admin has Removed you From the Group '{group.groupName}'.";

                // 5) Create & save notification for the group's admin
                var newNotification = new Notification
                {
                    userId = userInfo.userId,  // recipient is the admin
                    message = requestMessage,
                    isRead = false,
                    date = DateTime.Now, // or CreatedAt, etc.
                    userIdfrom = user.id
                };

                string notifErrorMsg;
                var notifResult = _NotificationManager.CreateNotification(newNotification, out notifErrorMsg);
                if (notifResult == ErrorCode.Error)
                {
                    // handle error if needed
                    TempData["ErrorMessage"] = notifErrorMsg;
                }

                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "manage" });
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult UpdateMembershipStatus(int userId,int id, int status, int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(groupId);
            var result = _groupManager.UpdateMembershipStatus(id, groupId, status, ref ErrorMessage);
            var userInfo = _AccManager.GetUserInfoById(userId);

            if (result == ErrorCode.Success)
            {
                string requestMessage = $"Admin has Approved your request to join '{group.groupName}'.";

                // 5) Create & save notification for the group's admin
                var newNotification = new Notification
                {
                    userId = userInfo.userId,  // recipient is the admin
                    message = requestMessage,
                    isRead = false,
                    date = DateTime.Now, // or CreatedAt, etc.
                    userIdfrom = user.id
                };

                string notifErrorMsg;
                var notifResult = _NotificationManager.CreateNotification(newNotification, out notifErrorMsg);
                if (notifResult == ErrorCode.Error)
                {
                    // handle error if needed
                    TempData["ErrorMessage"] = notifErrorMsg;
                }

                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "manage" });
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }

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
            var userInformation = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var userAccount = _AccManager.GetUserByUsername(username);
            var posts = _postManager.GetAllPosts();
            var events = _eventManager.GetAllEventAttendance();

            ViewBag.CurrentUserId = userAccount.id;

            var allUsers = _AccManager.GetAllUserInfo();  // Change this method to retrieve UserInformation

           
            var groupPhoto = events
            .Select(e => e.Groups.GroupImage.FirstOrDefault()?.coverPhoto)
            .FirstOrDefault();

            ViewBag.GroupPhoto = groupPhoto != null
            ? Url.Content("~/UploadedFiles/" + groupPhoto)
            : Url.Content("~/UploadedFiles/default-cover.png");

            var eventAttendanceCounts = events
                .GroupBy(e => e.eventId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count(a => a.status == 1) // Count only those marked as "Going"
                );

            ViewBag.EventAttendanceCounts = eventAttendanceCounts;

            var model = new GroupViewModel
            {
                Events = events,
                UserAcc = userAccount,
                UserInformation = userInformation,
                Posts = posts,
                UserInformations = allUsers  // Add the list of all users

            };

            return View(model);
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
        public ActionResult Post(Post post, int groupId, int createdBy, String content, String tags, IEnumerable<HttpPostedFileBase> mediaFiles)
        {
            try
            {
                var currentUser = _AccManager.GetUserByUsername(User.Identity.Name);
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    var model = PrepareManageAccountViewModel();
                    return View("GroupDetail", model);
                }

                post.content = content;
                post.createdBy = createdBy;
                post.groupId = groupId;
                post.title = content;

                if (_postManager.AddPost(post, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageGroups", model);
                }

                if (mediaFiles != null && mediaFiles.Any())
                {
                    foreach (var file in mediaFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Validate file (optional)
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".avi", ".mov" };
                            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                            if (!allowedExtensions.Contains(extension))
                            {
                                ModelState.AddModelError("", "Unsupported file type.");
                                continue; // Skip this file
                            }

                            // Limit file size (e.g., 5MB)
                            if (file.ContentLength > (5 * 1024 * 1024))
                            {
                                ModelState.AddModelError("", "File size exceeds the limit of 5MB.");
                                continue; // Skip this file
                            }

                            // Generate a unique file name
                            var fileName = Guid.NewGuid().ToString() + extension;
                            var uploadPath = Server.MapPath("~/Uploads/PostMedia/");
                            var filePath = System.IO.Path.Combine(uploadPath, fileName);

                            // Ensure the upload directory exists
                            if (!System.IO.Directory.Exists(uploadPath))
                            {
                                System.IO.Directory.CreateDirectory(uploadPath);
                            }

                            // Save the file
                            file.SaveAs(filePath);

                            // Determine media type
                            string mediaType = "";
                            if (file.ContentType.StartsWith("image"))
                                mediaType = "image";
                            else if (file.ContentType.StartsWith("video"))
                                mediaType = "video";
                            else
                                mediaType = "unknown"; // Handle accordingly

                            // Create PostMedia entry
                            PostMedia media = new PostMedia
                            {
                                postId = post.id, // Assuming post.id is set after creation
                                mediaType = mediaType,
                                mediaFile = "/Uploads/PostMedia/" + fileName,
                                uploadedAt = DateTime.Now,
                                uploadedBy = currentUser.id
                            };

                            string mediaError = "";
                            var mediaResult = _postMediaManager.CreatePostMedia(media, ref mediaError);
                            if (mediaResult != ErrorCode.Success)
                            {
                                // Log the error or handle accordingly
                                ModelState.AddModelError("", "Failed to upload media: " + mediaError);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(tags))
                {
                    var tagManager = new TagsManager();
                    var tagList = tags.Split(',')
                                      .Select(t => t.Trim())
                                      .Where(t => !string.IsNullOrEmpty(t))
                                      .ToList();
                    tagManager.AssociateTagsWithPost(post.id, tagList);
                }
                TempData["SuccessMessage"] = "Post Uploaded.";
                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "contents" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("GroupDetail", model);
            }

        }
        [HttpPost]
        public JsonResult ForumComment(int forumId, int userId, int groupId, string comment)
        {
            try
            {
                if (string.IsNullOrEmpty(comment))
                {
                    return Json(new { success = false, message = "Comment cannot be empty." });
                }

                var comm = new ForumComments
                {
                    forumId = forumId,
                    userId = userId,
                    groupId = groupId,
                    comment = comment,
                    date_created = DateTime.Now
                };

                string errorMsg = "";
                var result = _commentManager.AddForumComment(comm, ref errorMsg);

                if (result == ErrorCode.Error)
                {
                    return Json(new { success = false, message = "Error adding comment: " + errorMsg });
                }

                var commentUser = _AccManager.GetUserInfoByUserId(userId);

                return Json(new
                {
                    success = true,
                    message = "Comment added successfully!",
                    comment = new
                    {
                        comm.comment,
                        comm.date_created,
                        user = commentUser.first_name + " " + commentUser.last_name,
                        profileImage = string.IsNullOrEmpty(commentUser.Image?.FirstOrDefault()?.imageFile)
                            ? "/UploadedFiles/default.png"
                            : "/UploadedFiles/" + commentUser.Image.FirstOrDefault()?.imageFile
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., to a file or database)
                return Json(new { success = false, message = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddComment(int postId, int userId, int groupId, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                return Json(new { success = false, message = "Comment cannot be empty." });
            }

            var comm = new PostComments
            {
                postId = postId,
                userId = userId,
                groupId = groupId,
                comment = comment,
                date_created = DateTime.Now
            };

            string errorMsg = "";
            var result = _commentManager.AddComment(comm, ref errorMsg);

            if (result == ErrorCode.Error)
            {
                return Json(new { success = false, message = "Error adding comment: " + errorMsg });
            }

            var commentUser = _AccManager.GetUserInfoByUserId(userId);

            return Json(new
            {
                success = true,
                message = "Comment added successfully!",
                comment = new
                {
                    comm.comment,
                    comm.date_created,
                    user = commentUser.first_name + " " + commentUser.last_name,
                    profileImage = string.IsNullOrEmpty(commentUser.Image?.FirstOrDefault()?.imageFile)
                        ? "/UploadedFiles/default.png"
                        : "/UploadedFiles/" + commentUser.Image.FirstOrDefault()?.imageFile
                }
            });
        }


        // Optional: Action to delete a comment
        public ActionResult DeleteComment(int commentId, int postId)
        {
            string errorMsg = "";
            var result = _commentManager.DeleteComment(commentId, ref errorMsg);

            if (result == ErrorCode.Error)
            {
                ViewBag.ErrorMessage = "Error deleting comment: " + errorMsg;
            }
            else
            {
                ViewBag.SuccessMessage = "Comment deleted successfully!";
            }

            return RedirectToAction("PostDetails", new { postId });
        }

        [HttpPost]
        public ActionResult Forum(Forum fo, int groupId, int createdBy, String content)
        {
            try
            {
                var currentUser = _AccManager.GetUserByUsername(User.Identity.Name);
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    var model = PrepareManageAccountViewModel();
                    return View("GroupDetail", model);
                }
                
                fo.content = content;
                fo.createdBy = createdBy;
                fo.groupId = groupId;
                fo.title = content;

                if (_forumManager.AddForum(fo, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("ManageGroups", model);
                }      
                TempData["SuccessMessage"] = "Post Uploaded.";
                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "forums" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("GroupDetail", model);
            }

        }

        [HttpPost]
        public ActionResult CreateEvent(Event ev, string title, string description, int groupId, string event_date, string event_time, HttpPostedFileBase mediaFiles)
        {
            try
            {
                var user = _AccManager.GetUserByUsername(User.Identity.Name);
                var userInfo = _AccManager.GetUserInfoByUsername(user.username);
                var group = _groupManager.GetGroupById(groupId);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    var model = PrepareManageAccountViewModel();
                    return View("GroupDetail", new { groupId = groupId, activeTab = "events" });
                }

                // Combine event_date and event_time into a DateTime
                DateTime parsedDate = DateTime.ParseExact(event_date, "yyyy-MM-dd", null);
                TimeSpan parsedTime = TimeSpan.Parse(event_time);

                DateTime eventDateTime = parsedDate.Date.Add(parsedTime);

                // Set additional event properties
                ev.createdBy = user.id;
                ev.groupId = groupId;
                ev.description = description;
                ev.title = title;
                ev.event_date = eventDateTime;

                // Save the event using the manager
                if (_eventManager.CreateEvent(ev, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    var model = PrepareManageAccountViewModel();
                    return View("GroupDetail", model);
                }

                if (mediaFiles != null && mediaFiles.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".avi", ".mov" };
                    var extension = System.IO.Path.GetExtension(mediaFiles.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Unsupported file type.");
                    }

                    if (mediaFiles.ContentLength > (5 * 1024 * 1024))
                    {
                        ModelState.AddModelError("", "File size exceeds the limit of 5MB.");
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;

                    var uploadsFolderPath = Server.MapPath("~/Uploads/EventMedia");
                    if (!Directory.Exists(uploadsFolderPath))
                        Directory.CreateDirectory(uploadsFolderPath);

                    var filePath = System.IO.Path.Combine(uploadsFolderPath, fileName);

                    mediaFiles.SaveAs(filePath);

                    string mediaType = "";
                    if (mediaFiles.ContentType.StartsWith("image"))
                        mediaType = "image";
                    else if (mediaFiles.ContentType.StartsWith("video"))
                        mediaType = "video";
                    else
                        mediaType = "unknown";

                    EventMedia media = new EventMedia
                    {
                        eventId = ev.id, // Assuming ev.id is set after creation
                        mediaType = mediaType,
                        mediaFile = "/Uploads/EventMedia/" + fileName,
                        uploadedAt = DateTime.Now,
                        uploadedBy = user.id
                    };

                    string mediaError = "";
                    var mediaResult = _eventMediaManager.CreateEventMedia(media, ref mediaError);
                    if (mediaResult != ErrorCode.Success)
                    {
                        // Log the error or handle accordingly
                        ModelState.AddModelError("", "Failed to upload media: " + mediaError);
                    }

                }                                  
                

                var superAdmins = _AccManager.GetAllSuperAdmins();


                if (superAdmins != null && superAdmins.Any())
                {
                    foreach (var superAdmin in superAdmins)
                    {
                        string requestMessage = $"Group Page {group.groupName} Requested for an Event Approval title:'{title}'.";

                        // Create a new notification object
                        var superAdminNotification = new Notification
                        {
                            userId = superAdmin.id,          // Recipient: Super Admin
                            message = requestMessage,     // Notification message
                            isRead = false,                  // Mark as unread
                            userIdfrom = userInfo.id       // Sender: Current User
                        };

                        // Attempt to save the notification
                        string superAdminNotifErrorMsg;
                        var superAdminNotifResult = _NotificationManager.CreateNotification(superAdminNotification, out superAdminNotifErrorMsg);

                        if (superAdminNotifResult == ErrorCode.Error)
                        {
                            ModelState.AddModelError(string.Empty, ErrorMessage);
                            var model = PrepareManageAccountViewModel();
                            return View("GroupDetail", model);
                        }
                    }
                }

                return RedirectToAction("GroupDetail", new { groupId = groupId, activeTab = "events" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                var model = PrepareManageAccountViewModel();
                return View("GroupDetail", model);
            }
        }
       
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
                group.status = (int)Status.InActive;
                group.groupName = groupName;
                group.description = description;
                group.groupId = Utils.Utilities.gUid;
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

                var superAdmins = _AccManager.GetAllSuperAdmins();

                if (superAdmins != null && superAdmins.Any())
                {
                    foreach (var superAdmin in superAdmins)
                    {
                        string superAdminMessage = $"{userInfo.first_name} {userInfo.last_name} has requested to create the group '{groupName}'.";

                        // Create a new notification object
                        var superAdminNotification = new Notification
                        {
                            userId = superAdmin.id,          // Recipient: Super Admin
                            message = superAdminMessage,     // Notification message
                            isRead = false,                  // Mark as unread
                            userIdfrom = userInfo.id       // Sender: Current User
                        };

                        // Attempt to save the notification
                        string superAdminNotifErrorMsg;
                        var superAdminNotifResult = _NotificationManager.CreateNotification(superAdminNotification, out superAdminNotifErrorMsg);

                        if (superAdminNotifResult == ErrorCode.Error)
                        {
                            ModelState.AddModelError(string.Empty, ErrorMessage);
                            var model = PrepareManageAccountViewModel();
                            return View("Group", model);
                        }
                    }
                }

                // Inform the user of the successful group creation
                TempData["SuccessMessage"] = "Group created successfully. Awaiting admin approval.";
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