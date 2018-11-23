using System.Web.Mvc;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Controllers
{
    public class ContactUsController : BaseClass
    {
        // GET: ContactUs
        private IContactUsServices _IContactUsServices;
        public ContactUsController()
        {
            _IContactUsServices = new ContactUsServices();
        }
        
        public ActionResult Contacts()
        {
            return View(_IContactUsServices.GetAllContacts());
        }
     
    }
}