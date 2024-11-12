using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupDetailViewModel
    {
        public List<UserInformation> UserInformations { get; set; } = new List<UserInformation>();
        public UserInformation UserInformation { get; set; }

        public Groups Group { get; set; }
        public IEnumerable<Event> Events { get; set; } = new List<Event>(); // Initialize to avoid null
        public IEnumerable<Forum> Forums { get; set; } = new List<Forum>(); // Initialize to avoid null
        public IEnumerable<Post> Posts { get; set; } = new List<Post>(); // Initialize to avoid null
        public List<Groups> AllGroups { get; set; } = new List<Groups>(); // Initialize to avoid null
        public List<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>(); // Initialize to avoid null
        public GroupMembership UserMembership { get; set; }
        public List<UserMembershipDetail> GroupMembership { get; set; } = new List<UserMembershipDetail>(); // Modified to use UserMembershipDetail

        public virtual ICollection<Image> Images { get; set; }

    }
    public class UserMembershipDetail
    {
        public GroupMembership Membership { get; set; }
        public UserInformation User { get; set; }
    }
}
