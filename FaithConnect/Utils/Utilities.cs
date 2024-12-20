using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaithConnect.Repository;



namespace FaithConnect.Utils
{

    public enum MembershipStatus
    {       
        Pending,
        Joined,
        Rejected
    }
    
    public enum PostStatus
    {
        Pending,
        Valid,
        Rejected
    }

    public enum ErrorCode
    {
        Success,
        Error,
        NotFound,
        AlreadyExists // For duplicate "Going" requests

    }

    public enum RoleType
    {
        User,
        Admin,
        SpiritualLeader
    }
    
    public enum Status
    {
        InActive,
        Active
    }
    public class Constant
    {
        public const string Role_Admin = "Admin";
        public const string Role_User = "User";
        public const string Role_Spiritual = "Spiritual Leader";

        public const int ERROR = 1;
        public const int SUCCESS = 0;
    }
   public class Utilities
    {
        public static String gUid
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        public static int code
        {
            get
            {
                Random r = new Random();
                return r.Next(100000, 999999);
            }
        }

        public static List<SelectListItem> ListRole
        {
            get
            {
                BaseRepository<Role> role = new BaseRepository<Role>();
                var list = new List<SelectListItem>();
                foreach (var item in role.GetAll())
                {
                    var r = new SelectListItem
                    {
                        Text = item.roleName,
                        Value = item.roleId.ToString()
                    };

                    list.Add(r);
                }

                return list;
            }
        }
    }
}