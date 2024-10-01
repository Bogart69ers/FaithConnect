using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupDetailViewModel
    {
        public UserInformation UserInformation { get; set; }
        public List<UserInformation> UserInformations { get; set; }
        public Groups Group { get; set; }
        public IEnumerable<Event> Events { get; set; } = new List<Event>(); // Initialize to avoid null
        public IEnumerable<Forum> Forums { get; set; } = new List<Forum>(); // Initialize to avoid null
        public IEnumerable<Post> Posts { get; set; } = new List<Post>(); // Initialize to avoid null
        public List<Groups> AllGroups { get; set; } = new List<Groups>(); // Initialize to avoid null
        public List<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>(); // Initialize to avoid null
    }
}
