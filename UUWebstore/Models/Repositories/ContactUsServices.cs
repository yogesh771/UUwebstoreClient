using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class ContactUsServices : BaseClass.BaseClass, IContactUsServices
    {

        private readonly UnitOfWork uow;

        public ContactUsServices()
        {
            uow = new UnitOfWork();
        }
        public int SaveContactUs(ContactU oContact)
        {
            oContact.createdate = BaseUtil.GetCurrentDateTime();
            oContact.isRead = false;
            oContact.userId =Convert.ToInt64( BaseUtil.GetWebConfigValue("ClientID"));
            uow.ContactU_.Add(oContact);

          
                StreamReader sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Emailer/ForgotPassword.html"));
                string HTML_Body = sr.ReadToEnd();
                string final_Html_Body = HTML_Body.Replace("#name", oContact.ContactPerson);
                sr.Close();
                string To = oContact.ContactEmail;
                BaseUtil.sendEmailer(oContact.ContactEmail, "Thank you for contacting us,UU WebStore.", final_Html_Body);         


            return 1;
        }

        public List<ContactU> GetAllContacts()
        {
            var userId = Convert.ToInt64(BaseUtil.GetWebConfigValue("ClientID"));
            return uow.ContactU_.GetAll().Where(e => e.userId == userId).OrderByDescending(e => e.createdate).ToList();
        }
    }
}