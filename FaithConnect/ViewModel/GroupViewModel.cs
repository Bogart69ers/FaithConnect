using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupViewModel
    {
        public UserInformation UserInformation { get; set; }
        public List<GroupMembership> GroupMemberships { get; set; }
        public List<Groups> AllGroups { get; set; }


        public virtual ICollection<Image> Image { get; set; }

    }
}
