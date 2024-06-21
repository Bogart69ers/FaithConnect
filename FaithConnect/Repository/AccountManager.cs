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

        public UserAccount GetUserByUserId(string userId)
        {
            return _userAcc._table.FirstOrDefault(m => m.userId == userId);
        }

        public UserAccount GetUserByUsername(string username)
        {
            return _userAcc._table.FirstOrDefault(m => m.username == username);
        }

        public UserAccount GetUserByEmail(string email)
        {
            return _userAcc._table.FirstOrDefault(m => m.email == email);
        }

        public ErrorCode SignIn(string username, string password, ref string errMsg)
        {
            var userSignIn = GetUserByUsername(username);
            if (userSignIn == null)
            {
                errMsg = "User does not exist!";
                return ErrorCode.Error;
            }

            if (!userSignIn.password.Equals(password))
            {
                errMsg = "Password is incorrect";
                return ErrorCode.Error;
            }

            errMsg = "Login successful";
            return ErrorCode.Success;
        }

        public ErrorCode SignUp(UserAccount userac, ref string errMsg)
        {
            userac.userId = Utilities.gUid;
            userac.vcode = Utilities.code.ToString();
            userac.date_created = DateTime.Now;
            userac.status = (int)Status.InActive;

            if (GetUserByUsername(userac.username) != null)
            {
                errMsg = "Username already exists";
                return ErrorCode.Error;
            }

            if (GetUserByEmail(userac.email) != null)
            {
                errMsg = "Email already exists";
                return ErrorCode.Error;
            }

            if (_userAcc.Create(userac, out errMsg) != ErrorCode.Success)
            {
                return ErrorCode.Error;
            }

            return ErrorCode.Success;
        }

        public ErrorCode UpdateUser(UserAccount userac, ref string errMsg)
        {
            return _userAcc.Update(userac.id, userac, out errMsg);
        }

        public ErrorCode UpdateUserInformation(UserInformation userinf, ref string errMsg)
        {
            return _userInfo.Update(userinf.id, userinf, out errMsg);
        }

        public UserInformation GetUserInfoByUsername(string username)
        {
            var userAcc = GetUserByUsername(username);
            if (userAcc == null)
            {
                return null;
            }
            return _userInfo._table.FirstOrDefault(m => m.userId == userAcc.userId);
        }

        public UserInformation GetUserInfoByUserId(string userId)
        {
            return _userInfo._table.FirstOrDefault(m => m.userId == userId);
        }

        public UserInformation CreateOrRetrieve(string username, ref string err)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                err = "User does not exist";
                return null;
            }

            var userInfo = GetUserInfoByUserId(user.userId);
            if (userInfo != null)
            {
                return userInfo;
            }

            userInfo = new UserInformation
            {
                userId = user.userId,
                email = user.email,
                Status = (int)Status.Active
            };

            _userInfo.Create(userInfo, out err);

            return GetUserInfoByUserId(user.userId);
        }

        public UserAccount Retrieve(int id, ref string err)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                err = "User not found";
                return null;
            }

            return GetUserByUserId(user.userId);
        }
    }
}