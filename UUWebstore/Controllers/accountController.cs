using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UUWebstore.Models;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;


namespace UUWebstore.Controllers
{
    public class accountController : BaseClass
    {
        private readonly IaccountServices _IAccountServices;
        private readonly IRoleUserServices _IRoleUserServicess;
        private readonly IStateServices _IStateServices;
        private readonly ICountryServices _ICountryServices;
        public accountController()
        {
            _IAccountServices = new accountServices();
            _IRoleUserServicess = new RoleUserSerives();
            _IStateServices = new StateServices();
            _ICountryServices = new CountryServices();
        }

        // GET: account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.message = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginIdViewModel oLoginIdViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _IAccountServices.UserLogin(oLoginIdViewModel.Email, oLoginIdViewModel.Password);
                if (result.message == "")
                {
                    BaseUtil.SetSessionValue(AdminInfo.LoginID.ToString(), result.userId.ToString());
                    BaseUtil.SetSessionValue(AdminInfo.FullName.ToString(), result.fullName.ToString());
                    BaseUtil.SetSessionValue(AdminInfo.userType.ToString(), result.roleID.ToString());
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.message = result.message;
                    return View(oLoginIdViewModel);
                }

            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                CaptureErrorValues(errors);
            }
            return View(oLoginIdViewModel);
        }

        public ActionResult reset_password()
        {
            return View();
        }
        [HttpPost]
        public ActionResult reset_password(resetPassword oresetPassowrd)
        {
            if (ModelState.IsValid)
            {
                var result = _IAccountServices.resetPassword(oresetPassowrd);
                ViewBag.message = result;
                return View();
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                CaptureErrorValues(errors);
                TempData["error"] = errors;
                return View("Error");
            }
            return View();
        }
        public ActionResult forgot_password()
        {
            return View();
        }
        private void fillddl(user ouser=null)
        {
            
            if (ouser != null)
            {
                ViewBag.roleID = new SelectList(_IRoleUserServicess.getAllRoles().Select(e => new { e.roleID, e.name }), "roleID", "name", ouser.roleID);
                ViewBag.countryId = new SelectList(_ICountryServices.getAllCountries().Select(e => new { e.countryId, e.name }), "countryId", "name", ouser.countryId);
                ViewBag.stateId = new SelectList(_IStateServices.getAllStatesByCountryId(0).Select(e => new { e.stateId, e.name }), "stateId", "name", ouser.stateId);
                ViewBag.cityId = new SelectList(_IStateServices.getAllCityByStateId(0).Select(e => new { e.cityId, e.CityName }), "cityId", "CityName", ouser.cityId);
            }
            else {
                ViewBag.roleID = new SelectList(_IRoleUserServicess.getAllRoles().Select(e => new { e.roleID, e.name }), "roleID", "name");
                ViewBag.countryId = new SelectList(_ICountryServices.getAllCountries().Select(e => new { e.countryId, e.name }), "countryId", "name");
                ViewBag.stateId = new SelectList(_IStateServices.getAllStatesByCountryId(0).Select(e => new { e.stateId, e.name }), "stateId", "name");
                ViewBag.cityId = new SelectList(_IStateServices.getAllCityByStateId(0).Select(e => new { e.cityId, e.CityName }), "cityId", "CityName");
            }
        }
        public ActionResult profile(Int64 userID)
        {
            fillddl();
            var user = _IAccountServices.getUserById(userID);
           
            return View(user);
        }
        [HttpPost]
        public ActionResult profile(user user)
        {
            fillddl(user);
           
            bool result = _IAccountServices.create_update_userINformations_sp(user);
            ViewBag.message = result == true ? "Record saved." : "Record not saved.";
            return View(user);
        }
        [HttpPost]
        public string sendLinkToMail(string EmailAddress)
        {
            return _IAccountServices.getUserByEmailAddress(EmailAddress);
        }
        [HttpGet]      
        public ActionResult Reset__password(Int64 id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult Reset__password(forgotPassword oforgotPassword)
        {
            ViewBag.message= _IAccountServices.resetPasswordFromForget(oforgotPassword);
            return View();
        }
        public ActionResult fillState(Int32 countryID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(_IStateServices.getAllStatesByCountryId(countryID).Select(e => new { e.stateId, e.name }));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillCity(Int32 stateId)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(_IStateServices.getAllCityByStateId(stateId).Select(e => new { e.cityId, e.CityName }));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}