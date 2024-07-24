using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupViewModel
    {
        public UserAccount UserAccount { get; set; }
        public UserInformation UserInformation { get; set; }
        public List<Groups> Groups { get; set; }
        public List<Groups> JoinedGroups { get; set; }
        public List<Groups> PendingGroups { get; set; }
        public List<UserInformation> UserInformations { get; set; }
        public virtual ICollection<Image> Image { get; set; }
        public virtual UserInformation UserInformation1 { get; set; }

    }
    public class GroupDetails
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int MemberCount { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string Description { get; set; }
    }
}
