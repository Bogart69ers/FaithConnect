using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.Repository
{
    public class ManageAccountViewModel
    { 
        public UserInformation UserInformation { get; set; }
        public List<UserInformation> UserInformations { get; set; }
        public List<UserAccount> UserAccounts { get; set; }
        public UserAccount UserAccount { get; set; }
        public Groups Group { get; set; }
        public IEnumerable<Groups> Groups { get; set; }
        public IEnumerable<UserAccount> UserAccountIE { get; set; }



    }
}