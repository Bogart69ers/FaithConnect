using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Utils;
using FaithConnect.Repository;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FaithConnect.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        // GET: Admin

        public ActionResult GenerateAdminReport()
        {
            try
            {
                // Retrieve data for the report
                var activeGroupsCount = _groupManager.GetAllGroups().Count(g => g.status == 1);
                var pendingGroupsCount = _groupManager.GetAllGroups().Count(g => g.status == 0);
                var activeUsersCount = _AccManager.GetAllUsers().Count(u => u.status == 1);
                var totalPostsCount = _postManager.GetAllPosts().Count();
                var totalEventsCount = _eventManager.GetAllEvents().Count();
                var pendingEventsCount = _eventManager.GetAllEvents().Count(e => e.status == 0);
                var approvedEventsCount = _eventManager.GetAllEvents().Count(e => e.status == 1);

                // Create a memory stream for the PDF
                using (var ms = new MemoryStream())
                {
                    // Initialize PDF document
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    // Load logos
                    string userLogoPath = Server.MapPath("~/Assets/Admin/img/Logo.png");
                    string schoolLogoPath = Server.MapPath("~/Assets/Admin/img/UC_logo.png");

                    var userLogo = iTextSharp.text.Image.GetInstance(userLogoPath);
                    var schoolLogo = iTextSharp.text.Image.GetInstance(schoolLogoPath);

                    // Resize logos
                    userLogo.ScaleAbsolute(50, 50); // Adjust as needed
                    schoolLogo.ScaleAbsolute(50, 25);

                    // Create a table for the header (1 column, everything centered)
                    PdfPTable headerTable = new PdfPTable(1);
                    headerTable.WidthPercentage = 100;

                    // Add logos and title in a vertical layout
                    PdfPTable logoAndTitleTable = new PdfPTable(3);
                    logoAndTitleTable.WidthPercentage = 100;
                    logoAndTitleTable.SetWidths(new float[] { 1, 4, 1 }); // Adjust column widths for spacing

                    // Left logo (school)
                    PdfPCell schoolLogoCell = new PdfPCell(schoolLogo)
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10
                    };
                    logoAndTitleTable.AddCell(schoolLogoCell);

                    // Title and address
                    // Title and subtitle (centered)
                    PdfPCell titleCell = new PdfPCell
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };

                    var titleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
                    var subtitleFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);

                    // Add title
                    Paragraph title = new Paragraph("UNIVERSITY OF CEBU LAPU-LAPU AND MANDAUE", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    titleCell.AddElement(title);

                    // Add address
                    Paragraph address = new Paragraph("A. C. Cortes Ave., Looc Mandaue City", subtitleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    titleCell.AddElement(address);

                    // Add Faith Connect subtitle
                    Paragraph faithConnect = new Paragraph("Faith Connect", subtitleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    titleCell.AddElement(faithConnect);

                    logoAndTitleTable.AddCell(titleCell);


                    // Right logo (Faith Connect)
                    PdfPCell userLogoCell = new PdfPCell(userLogo)
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    logoAndTitleTable.AddCell(userLogoCell);

                    // Add the logo-and-title table to the header table
                    headerTable.AddCell(new PdfPCell(logoAndTitleTable)
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    // Add the header table to the document
                    document.Add(headerTable);

                    // Add some spacing after the header
                    document.Add(new Paragraph("\n"));

                    // Add report title
                    var reportTitleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
                    Paragraph reportTitle = new Paragraph("Admin Dashboard Report", reportTitleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(reportTitle);

                    document.Add(new Paragraph("\n")); // Add spacing

                    // Create a table with metrics
                    PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 70, 30 }); // Adjust column widths

                    var headerFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                    var cellFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);

                    // Table headers
                    table.AddCell(new PdfPCell(new Phrase("Metric", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Count", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                    // Table data
                    table.AddCell(new PdfPCell(new Phrase("Total Active Groups", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(activeGroupsCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Pending Groups", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(pendingGroupsCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Active Users", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(activeUsersCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Total Posts", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(totalPostsCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Total Events", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(totalEventsCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Pending Events", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(pendingEventsCount.ToString(), cellFont)));

                    table.AddCell(new PdfPCell(new Phrase("Approved Events", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(approvedEventsCount.ToString(), cellFont)));

                    // Add table to document
                    document.Add(table);
                    document.Close();

                    // Return the PDF as a response
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "inline; filename=AdminDashboardReport.pdf");
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
                TempData["ErrorMessage"] = $"An error occurred while generating the report: {ex.Message}";
                return RedirectToAction("AdminDashboard");
            }
        }


        public ActionResult AdminDashboard()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var groups = _groupManager.GetAllGroups()
                            .Where(e => e.status == 1)
                               .ToList();

            var pendingGroups = _groupManager.GetAllGroups()
                               .Where(e => e.status == 0)
                               .ToList();

            var users = _AccManager.GetAllUsers();

            var posts = _postManager.GetAllPosts();


            var accounts = _AccManager.GetAllUsers()
                            .Where(e => e.status == 1)
                                            .ToList();

            var events = _eventManager.GetAllEvents();

            var pendingEvents = _eventManager.GetAllEvents()
                                    .Where(e => e.status == 0)
                                    .ToList();
            var approvedEvents = _eventManager.GetAllEvents()
                                    .Where(e => e.status == 1)
                                    .ToList();

            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                UserAccounts = accounts,
                Group = new Groups(),
                Groups = groups,
                Posts = posts,
                Events = events,
                PendingEvents = pendingEvents,
                ApprovedEvents = approvedEvents,
                PendingGroups = pendingGroups
            };

            return View(model);
        }



        public ActionResult Approval()
        {
            var username = User.Identity.Name;
            var userInfo = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var events = _eventManager.GetAllEvent().ToList();


            var accounts = _AccManager.GetAllUsers();
            var model = new ManageAccountViewModel
            {
                Events = events,
                UserInformation = userInfo,
                UserAccounts = accounts
            };

            return View(model);
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
            var accounts = _AccManager.GetAllUsers();

            var model = new ManageAccountViewModel
            {
                UserInformation = userInfo,
                Groups = groups,
                Group = new Groups(),
                UserAccounts = accounts,           
                UserAccount = new UserAccount()

            };

            return View(model);
        }



        [HttpPost]
        public JsonResult MarkAsRead(int notificationId)
        {
            var result = _NotificationManager.MarkAsRead(notificationId, out ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = ErrorMessage });
        }



        [HttpPost]
        public ActionResult CreateGroup(Groups group, String priv ,String groupName,String description, int groupAdmin )
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
                group.privacy = priv;
                group.groupAdmin = groupAdmin;
                group.status = (int)Status.Active;
                group.groupName = groupName;
                group.description = description;
                group.groupId = Utils.Utilities.gUid;
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
        public ActionResult Create(UserAccount ua, string ConfirmPass, string firstname, string lastname, string phone, string address, string religion)
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

                ua.userId = Utils.Utilities.gUid;
                ua.vcode = Utils.Utilities.code.ToString();
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
                    userId = ua.id,
                    first_name = firstname,
                    last_name = lastname,
                    phone = phone,
                    address = address,
                    email = ua.email,
                    status = (int)Status.Active,
                    date_created = DateTime.Now,
                    religion = religion
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
        public JsonResult DeleteGroup(int id)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(id);

            var result = _groupManager.DeleteGroup(id, ref ErrorMessage);

            if (result == ErrorCode.Success)
            {
                return Json(new { success = true, message = "Group deleted successfully." });
            }

            return Json(new { success = false, message = ErrorMessage });
        }


        [HttpPost]
        public ActionResult RejectGroup(int id)
        {

            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(id);

            string requestMessage = $"Admin Rejected your group page '{group.groupName}'.";

            var newNotification = new Notification
            {
                userId = group.groupAdmin,  // recipient is the admin
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

            var result = _groupManager.DeleteGroup(id, ref ErrorMessage);

            if (result == ErrorCode.Success)
            {
                return RedirectToAction("ManageGroups");
            }

            ModelState.AddModelError("", ErrorMessage);
            var model = PrepareManageAccountViewModel();
            return View("ManageGroups", model);
        }


        [HttpPost]
        public ActionResult UpdateUpdateGroupStatus(int id)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(id);
            var result = _groupManager.UpdateGroupStatus(id, ref ErrorMessage);

            string requestMessage = $"Admin Approved your group page '{group.groupName}'.";

            if (result == ErrorCode.Success)
            {

                var newNotification = new Notification
                {
                    userId = group.groupAdmin,  // recipient is the admin
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

                return RedirectToAction("ManageGroups");
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }
        }



        [HttpPost]
        public ActionResult UpdateEventStatus(int id, string title, int groupId)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(groupId);
            var result = _eventManager.UpdateEventStatus(id, ref ErrorMessage);

            string requestMessage = $"Admin Approved Event '{title}' from your group page {group.groupName}.";


            if (result == ErrorCode.Success)
            {
                var newNotification = new Notification
                {
                    userId = group.groupAdmin,  // recipient is the admin
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

                return RedirectToAction("Approval");
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }
        }



        [HttpPost]
        public ActionResult Delete(int id, string title, int groupId, string message)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);
            var group = _groupManager.GetGroupById(groupId);

            string requestMessage = $"Admin Declined Event '{title}' due to '{message}' from your group page {group.groupName}.";

            var newNotification = new Notification
            {
                userId = group.groupAdmin,  // recipient is the admin
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

            var result = _eventManager.DeleteEvent(id, ref ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return RedirectToAction("Approval");
            }
            // Handle error
            ModelState.AddModelError("", ErrorMessage);
            var model = PrepareManageAccountViewModel();
            return View("Approval", model);
        }

        [HttpPost]
        public JsonResult DeleteAccount(int id)
        {
            var username = User.Identity.Name;
            var user = _AccManager.CreateOrRetrieve(username, ref ErrorMessage);

            var result = _AccManager.DeleteUser(id, ref ErrorMessage);
            if (result == ErrorCode.Success)
            {
                return Json(new { success = true, message = "Account deleted successfully." });
            }

            return Json(new { success = false, message = ErrorMessage });
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
