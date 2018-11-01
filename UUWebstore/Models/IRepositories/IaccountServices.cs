using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UUWebstore.Models;

namespace UUWebstore.Models.IRepositories
{
   public interface IaccountServices
    {
        sp_LoginUser_Result UserLogin(String userID, string password);
        int resetPassword(resetPassword oresetPassword);
        
        //create new website Client or Supplier via website
        bool create_update_userINformations_sp(user objUser);
        bool boolCheckExiststance(string  uname, string caseToCheck);
        List<getFilteredUsers_Result> getFilteredUsers(search search);
        user getUserById(Int64 userId);
        string getUserByEmailAddress(string emailId);
        int resetPasswordFromForget(forgotPassword forgotPassword);
    }
}
