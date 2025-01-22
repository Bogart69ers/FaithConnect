using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupViewModel
    {
        public UserAccount UserAcc { get; set; }
        public UserInformation UserInformation { get; set; }
        public Groups group { get; set; }

        public List<UserInformation> UserInformations { get; set; }
        public IEnumerable<EventAttendance> Events { get; set; } = new List<EventAttendance>(); // Initialize to avoid null

        public IEnumerable<Post> RecommendedPosts { get; set; } = new List<Post>();

        public IEnumerable<Post> FilteredRandomPosts { get; set; } = new List<Post>();
        public List<Groups> Group { get; set; }
        public List<Post> RepostedPosts { get; set; } // Include RepostedPosts


        public GroupMembership GroupMembership { get; set; }
        public List<GroupMembership> GroupMemberships { get; set; }
        public List<Groups> AllGroups { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public Dictionary<int, List<string>> TagsByPost { get; set; }


        public virtual ICollection<Image> Image { get; set; }

    }
}
