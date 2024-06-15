using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class AccountManager
    {
        private BaseRepository<UserAccount> _userAcc;
        private BaseRepository<UserInformation> _userInfo;


        public AccountManager()
        {
            _userAcc = new BaseRepository<UserAccount>();
            _userInfo = new BaseRepository<UserInformation>();

        }

        public UserAccount GetUserById(int Id)
        {
            return _userAcc.Get(Id);
        }
        public UserAccount GetUserByUserId(String userId)
        {
            return _userAcc._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserAccount GetUserByUsername(String username)
        {
            return _userAcc._table.Where(m => m.username == username).FirstOrDefault();
        }
        public UserAccount GetUserByEmail(String email)
        {
            return _userAcc._table.Where(m => m.email == email).FirstOrDefault();
        }

        public ErrorCode SignIn(String username, String password, ref String errMsg)
        {
            var userSignIn = GetUserByUsername(username);
            if (userSignIn == null)
            {
                errMsg = "User not exist!";
                return ErrorCode.Error;
            }

            if (!userSignIn.password.Equals(password))
            {
                errMsg = "Password is Incorrect";
                return ErrorCode.Error;
            }

            // user exist
            errMsg = "Login Successful";
            return ErrorCode.Success;
        }

        public ErrorCode SignUp(UserAccount userac, ref String errMsg)
        {
            userac.userId = Utilities.gUid;
            userac.vcode = Utilities.code.ToString();
            userac.date_created = DateTime.Now;
            userac.status = (Int32)Status.InActive;

            // if the user already exist this will execute
            if (GetUserByUsername(userac.username) != null)
            {
                errMsg = "Username Already Exist";
                return ErrorCode.Error;
            }
            // if the email already exist this will execute
            if (GetUserByEmail(userac.email) != null)
            {
                errMsg = "Email Already Exist";
                return ErrorCode.Error;
            }

            if (_userAcc.Create(userac, out errMsg) != ErrorCode.Success)
            {
                return ErrorCode.Error;
            }

            // use the generated code for OTP "ua.code"
            // send email or sms here...........

            return ErrorCode.Success;
        }

        public ErrorCode UpdateUser(UserAccount userac, ref String errMsg)
        {
            return _userAcc.Update(userac.id, userac, out errMsg);
        }
        public ErrorCode UpdateUserInformation(UserInformation userinf, ref String errMsg)
        {
            return _userInfo.Update(userinf.id, userinf, out errMsg);
        }       
        public UserInformation GetUserInfoByUsername(String username)
        {
            var userAcc = GetUserByUsername(username);
            return _userInfo._table.Where(m => m.userId == userAcc.userId).FirstOrDefault();
        }
        public UserInformation GetUserInfoByUserId(String userId)
        {
            return _userInfo._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserInformation CreateOrRetrieve(String username, ref String err)
        {
            var User = GetUserByUsername(username);
            var UserInfo = GetUserInfoByUserId(User.userId);
            if (UserInfo != null)
                return UserInfo;

            UserInfo = new UserInformation();
            UserInfo.userId = User.userId;
            UserInfo.email = User.email;
            UserInfo.Status = (Int32)Status.Active;

            var userEmail = User.email;
            if (userEmail != null)
            {
                UserInfo.email = userEmail;
            }
            _userInfo.Create(UserInfo, out err);

            return GetUserInfoByUserId(User.userId);
        }

        public UserAccount Retrieve(int id, ref String err)
        {
            var User = GetUserById(id);

            return GetUserByUserId(User.userId);
        }
    }
}