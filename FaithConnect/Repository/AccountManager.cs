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

        public UserAccount GetUserByUserId(int userId)
        {
            return _userAcc._table.FirstOrDefault(m => m.id == userId);
        }

        public UserAccount GetUserByUsername(string username)
        {
            return _userAcc._table.FirstOrDefault(m => m.username == username);
        }

        public UserAccount GetUserByEmail(string email)
        {
            return _userAcc._table.FirstOrDefault(m => m.email == email);
        }
        public ErrorCode DeleteUser(int id, ref string errMsg)
        {
            try
            {
                var user = GetUserById(id);
                if (user == null)
                {
                    errMsg = "User not found";
                    return ErrorCode.Error;
                }
                return _userAcc.Delete(id, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"An error occurred: {ex.Message}";
                return ErrorCode.Error;
            }
        }

        public List<UserAccount> GetAllUsers()
        {
            return _userAcc.GetAll();
        }
        public List<UserInformation> GetAllUserInfo()
        {
            return _userInfo.GetAll();
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
            try
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
            catch (Exception ex)
            {
                errMsg = $"An error occurred: {ex.Message}";
                return ErrorCode.Error;
            }
        }

        public ErrorCode CreateUserInformation(UserInformation userinf, ref string errMsg)
        {
            return _userInfo.Create(userinf, out errMsg);
        }

        public ErrorCode UpdateUser(UserAccount userac, ref string errMsg)
        {
            return _userAcc.Update(userac.id, userac, out errMsg);
        }

        public ErrorCode UpdateUserInformation(UserInformation userinf, ref string errMsg)
        {
            return _userInfo.Update(userinf.id, userinf, out errMsg);
        }

        public UserInformation UserInfoSignup(string username,string firstname, string lastname, string phone, string address, string religion, ref String err)
        {
            var user = GetUserByUsername(username);

            var userInfo = GetUserInfoByUserId(user.id);
            if (userInfo != null)
            {
                return userInfo;
            }
            userInfo = new UserInformation();
            userInfo.userId = user.id;
            userInfo.email = user.email;
            userInfo.status = (int)Status.InActive;
            userInfo.date_created = DateTime.Now;
            userInfo.first_name = firstname;
            userInfo.last_name = lastname;
            userInfo.address = address;
            userInfo.phone = phone;
            userInfo.religion = religion;


            var userEmail = user.email;
            if (userEmail != null)
            {
                userInfo.email = userEmail;
            }

            _userInfo.Create(userInfo, out err);

            return GetUserInfoByUserId(user.id);
        }
        public UserInformation GetUserInfoByUsername(string username)
        {
            var userAcc = GetUserByUsername(username);
            if (userAcc == null)
            {
                return null;
            }
            return _userInfo._table.FirstOrDefault(m => m.userId == userAcc.id);
        }

        public UserInformation GetUserInfoByUserId(int userId)
        {
            return _userInfo._table.FirstOrDefault(m => m.userId == userId);
        }

        public UserInformation GetUserInfoById(int Id)
        {
            return _userInfo.Get(Id);
        }
        public UserInformation CreateOrRetrieve(String username, ref String err)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                err = "User does not exist";
                return null;
            }

            var userInfo = GetUserInfoByUserId(user.id);
            if (userInfo != null)
            {
                return userInfo;
            }

            userInfo = new UserInformation();
            userInfo.userId = user.id;
            userInfo.email = user.email;
            userInfo.status = (int)Status.Active;
            userInfo.date_created = DateTime.Now;          


            var userEmail = user.email;
            if (userEmail != null)
            {
                userInfo.email = userEmail;
            }
            _userInfo.Create(userInfo, out err);

            return GetUserInfoByUserId(user.id);
        }


        public UserAccount Retrieve(int id, ref string err)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                err = "User not found";
                return null;
            }

            return GetUserByUserId(user.id);
        }

        public string GetUserNameById(int? userId)
        {
            if (!userId.HasValue)
            {
                return "Unknown User"; // Or handle as needed
            }
            var user = _userInfo._table.FirstOrDefault(u => u.userId == userId.Value);
            return user != null ? $"{user.first_name} {user.last_name}" : "Unknown User";
        }
    }
}
