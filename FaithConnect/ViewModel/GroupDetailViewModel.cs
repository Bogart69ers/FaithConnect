﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupDetailViewModel
    {
        public List<UserInformation> UserInformations { get; set; } = new List<UserInformation>();
        public UserInformation UserInformation { get; set; }
        public UserAccount UserAcc { get; set; }
        public List<GroupMembership> GrpMembership { get; set; } = new List<GroupMembership>(); // Initialize to avoid null

        public Groups Group { get; set; }
        public IEnumerable<Event> Events { get; set; } = new List<Event>(); // Initialize to avoid null
        public IEnumerable<Forum> Forums { get; set; } = new List<Forum>(); // Initialize to avoid null
        public IEnumerable<Post> Posts { get; set; } = new List<Post>(); // Initialize to avoid null
        public List<Groups> AllGroups { get; set; } = new List<Groups>(); // Initialize to avoid null
        public List<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>(); // Initialize to avoid null
        public List<Post> PostManage { get; set; } = new List<Post>(); // Initialize to avoid null
        public List<Forum> ForumManage { get; set; } = new List<Forum>();
        public List<GroupMembership> MemberManagements { get; set; } = new List<GroupMembership>(); // Initialize to avoid null

        public GroupMembership UserMembership { get; set; }
        public List<UserMembershipDetail> GroupMembership { get; set; } = new List<UserMembershipDetail>();
        public List<UserMembershipDetail> MemberManagement { get; set; } = new List<UserMembershipDetail>(); // Modified to use UserMembershipDetail
        public List<Groups> AllGroupMembers { get; set; }

        public virtual ICollection<Image> Image { get; set; }

    }
    public class UserMembershipDetail
    {
        public GroupMembership Membership { get; set; }
        public UserInformation User { get; set; }
    }
}
