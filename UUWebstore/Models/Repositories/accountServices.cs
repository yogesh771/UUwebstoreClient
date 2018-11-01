
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models;
using System.IO;

namespace UUWebstore.Models.Repositories
{
    public class accountServices : BaseClass.BaseClass, IaccountServices
    {

        private readonly UnitOfWork uow;

        public accountServices()
        {
            uow = new UnitOfWork();
        }

        public sp_LoginUser_Result UserLogin(String userID, string password)
        {
            var param_userID = new SqlParameter("@userID", userID);
            var param_password = new SqlParameter("@password", password);
            var result = uow.sp_LoginUser_Result_.SQLQuery<sp_LoginUser_Result>("sp_LoginUser @password,@userID", param_password, param_userID).FirstOrDefault();
            return result;
        }
        public int resetPassword(resetPassword oresetPassword)
        {
            var userID = new SqlParameter("@userID", BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString());
            var old_password = new SqlParameter("@oldPassword", oresetPassword.oldPassword);
            var new_password = new SqlParameter("@newPassword", oresetPassword.newPassword);
            var result = uow.sp_LoginUser_Result_.SQLQuery<int>("resetPassword_sp @userId,@oldPassword,@newPassword", userID, old_password, new_password).FirstOrDefault();
            return result;
        }

        public int resetPasswordFromForget(forgotPassword forgotPassword)
        {
            var userID = new SqlParameter("@userID", forgotPassword.userID);       
            var new_password = new SqlParameter("@newPassword", forgotPassword.Password);
           var result = uow.sp_LoginUser_Result_.SQLQuery<int>("resetPasswordfromForget_sp @userId,@newPassword", userID, new_password).FirstOrDefault();
            return result;
        }

        public bool create_update_userINformations_sp(user objUser)
        {
            var result = false;
            objUser.password = "";
            objUser.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            objUser.modifiedDate = BaseUtil.GetCurrentDateTime();
            if (objUser.userId == 0)
            {
                objUser.isActive = true;
                objUser.isDelete = false;
                objUser.isBlocked = false;
                objUser.createdBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                objUser.createdDate = BaseUtil.GetCurrentDateTime();
                uow.user_.Add(objUser);
                if (objUser.roleID == 2)
                {
                    var oclientWebInformation = new Models.clientWebInformation();
                    oclientWebInformation.themeId = 1;
                    oclientWebInformation.userId = objUser.userId;
                    oclientWebInformation.createdBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    oclientWebInformation.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    oclientWebInformation.modifiedDate = BaseUtil.GetCurrentDateTime();
                    oclientWebInformation.createdDate = BaseUtil.GetCurrentDateTime();
                    oclientWebInformation.isDelete = false;
                    oclientWebInformation.isActive = true;

                    var oserviceClientWebInfo =new clientWebInformation();
                    oserviceClientWebInfo.addDefaultthemeToClientWebsite(oclientWebInformation);

                }
                result = true;
            }
            else
            {
                uow.user_.Update(objUser);
                result = true;
            }
            return result;
        }
        public bool boolCheckExiststance(string uname, string caseToCheck)
        {
            var user = (dynamic)null;
            switch (caseToCheck)
            {
                case "email":
                   user = uow.user_.Find(u => u.emailAddress == uname).FirstOrDefault();
                 break;
                case "mobile":
                    user = uow.user_.Find(u => u.mobile == uname).FirstOrDefault();
                 break;
                case "uname":
                    user = uow.user_.Find(u => u.userName == uname).FirstOrDefault();
                 break;
            }
            if (user == null)
                return true;

            return false;
        }

        public List<getFilteredUsers_Result> getFilteredUsers(search search)
        {
            var role_ =    new SqlParameter("@RoleID",  search.roleID);
            var username =  new SqlParameter("@userName", search.userName =="" ? SqlString.Null : search.userName);
            var fullname =  new SqlParameter("@fullName", search.fullName == "" ? SqlString.Null : search.fullName);
            var zipCode = new SqlParameter("@zipcode", search.zipcode == "" ? SqlString.Null : search.zipcode);

            var cityid = new SqlParameter("@cityid", SqlString.Null);
            if (search.cityId != 0)
            cityid =    new SqlParameter("@cityid",  search.cityId);

            var stateid = new SqlParameter("@stateid", SqlString.Null);
            if (search.stateId != 0)
                stateid = new SqlParameter("@stateid", search.stateId);

            var countryid = new SqlParameter("@countryid", SqlString.Null);
            if (search.countryId != 0)
                countryid = new SqlParameter("@countryid", search.countryId);  

            var result = uow.sp_LoginUser_Result_.SQLQuery<getFilteredUsers_Result>("getFilteredUsers @RoleID  ,@userName,@fullName,@cityid ,@stateid,@countryid ,@zipcode",
                                                                                                     role_, username, fullname, cityid, stateid, countryid, zipCode).ToList();
            return result;
        }

        public user getUserById(Int64 userId)
        {
            return uow.user_.Find(u => u.userId == userId).FirstOrDefault();
        }
        public string  getUserByEmailAddress(string  emailId)
        {
            string result = "no";
            var user= uow.user_.Find(u => u.emailAddress == emailId).FirstOrDefault();
            if (user != null)
            {
                StreamReader sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Emailer/ForgotPassword.html"));
                string HTML_Body = sr.ReadToEnd();
                string final_Html_Body = HTML_Body.Replace("#name", user.fullName).Replace("#userID", user.userId.ToString());
                sr.Close();
                string To = user.emailAddress;
                result= BaseUtil.sendEmailer(user.emailAddress, "Reset your password, UU WebStore.", final_Html_Body);               
            }
            return result;
        }
    }
}