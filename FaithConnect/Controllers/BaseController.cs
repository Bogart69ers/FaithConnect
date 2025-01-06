using FaithConnect.Repository;
using System;
using System.Web.Mvc;
using FaithConnect.Models;
using FaithConnect.ViewModel;
using System.Linq;
using System.Collections.Generic;



namespace FaithConnect.Controllers
{
    public class BaseController : Controller
    {
        public String ErrorMessage;
        public BaseRepository<UserAccount> _UserAcc;
        public AccountManager _AccManager;
        public ImageManager _imgMgr;
        public GroupManager _groupManager;
        public EventManager _eventManager;
        public PostManager _postManager;
        public ForumManager _forumManager;
        public TagsManager _tagsManager;
        public CommentManager _commentManager;
        public NotificationManager _NotificationManager;
        public PostMediaManager _postMediaManager; 
        public EventMediaManager _eventMediaManager; 

        public String Username { get { return User.Identity.Name; } }
        public String UserId { get { return _AccManager.GetUserByUsername(Username).userId; } }
        // GET: Base
        public BaseController()
        {
            _UserAcc = new BaseRepository<UserAccount>();
            _AccManager = new AccountManager();
            _imgMgr = new ImageManager();
            _groupManager = new GroupManager();
            _postManager = new PostManager();
            _eventManager = new EventManager();
            _forumManager = new ForumManager();
            _tagsManager = new TagsManager();
            _commentManager = new CommentManager();
            _NotificationManager = new NotificationManager();
            _postMediaManager = new PostMediaManager(); 
            _eventMediaManager = new EventMediaManager();
        }
        private string GetAvatarUrl(int id)
        {
            // 1) If userId = 0 or invalid, return a default image or handle as needed
            if (id == 0)
                return Url.Content("~/UploadedFiles/default.png");

            // 2) Use your image manager to get user’s image
            var userImage = _imgMgr.ListImgAttachByUserId(id).FirstOrDefault();
            if (userImage == null || string.IsNullOrEmpty(userImage.imageFile))
            {
                // fallback to default
                return Url.Content("~/UploadedFiles/default.png");
            }
            // 3) Otherwise return the actual path
            return Url.Content("~/UploadedFiles/" + userImage.imageFile);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var userAccount = _AccManager.GetUserByUsername(username);
                var userInfo = _AccManager.GetUserInfoByUsername(username);

                if (userAccount != null)
                {
                    // 1) Fetch raw notifications
                    var notifications = _NotificationManager.GetNotificationsByUserId(userAccount.id);

                    // 2) Convert them to notificationViewModel
                    var notificationViewModels = notifications
                        .Select(n => new notificationViewModel
                        {
                            Notification = n,
                            AvatarUrl = GetAvatarUrl((int)n.userIdfrom)
                        })
                        .ToList();

                    // 3) Store in ViewBag
                    ViewBag.Notifications = notificationViewModels;
                }
            }
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