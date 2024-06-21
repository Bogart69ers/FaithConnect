using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.Models
{
    public class UserLogged
    {
        public UserAccount UserAccount { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}