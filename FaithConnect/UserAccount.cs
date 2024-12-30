//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaithConnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAccount
    {
        public UserAccount()
        {
            this.Event = new HashSet<Event>();
            this.EventAttendance = new HashSet<EventAttendance>();
            this.Feedback = new HashSet<Feedback>();
            this.Forum = new HashSet<Forum>();
            this.ForumComments = new HashSet<ForumComments>();
            this.Groups = new HashSet<Groups>();
            this.Notification = new HashSet<Notification>();
            this.Post = new HashSet<Post>();
            this.PostComments = new HashSet<PostComments>();
            this.UserInformation = new HashSet<UserInformation>();
        }
    
        public int id { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> role { get; set; }
        public string vcode { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public Nullable<System.DateTime> date_modified { get; set; }
    
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventAttendance> EventAttendance { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
        public virtual ICollection<Forum> Forum { get; set; }
        public virtual ICollection<ForumComments> ForumComments { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<PostComments> PostComments { get; set; }
        public virtual Role Role1 { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
    }
}
