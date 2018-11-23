using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUWebstore.Models;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IsettingServices _IsettingServices;
        private readonly IProductServices _IProductServices;
        private readonly IStateServices _IStateServices;
        private readonly ICountryServices _ICountryServices;
        private readonly IaccountServices _IAccountServices;
        private readonly IProductOrderRepository _productOrderRepository;
        private IContactUsServices _IContactUsServices;
        public HomeController()
        {
            _IsettingServices = new settingServices();
            _IProductServices = new ProductServices();
            _IStateServices = new StateServices();
            _ICountryServices = new CountryServices();
            _IAccountServices = new accountServices();
            _productOrderRepository = new ProductOrderRepository();
            _IContactUsServices = new ContactUsServices();

        }
        public ActionResult Index()
        {
            var webSetting = _IsettingServices.getClientWebsiteReference();
            ViewBag.banners = webSetting.Where(e => e.Type == "3").ToList();
            ViewBag.featuredProduct = webSetting.Where(e => e.Type == "2").ToList();
            ViewBag.ProductCategory = webSetting.Where(e => e.Type == "1").ToList();
            ViewBag.BlockPramotionAdmin = (webSetting.Where(e => e.Type == "BlockPramotionAdmin").Select(e => new { e.productTitle }).FirstOrDefault()).productTitle;
            ViewBag.youTubePramotionAdmin =( webSetting.Where(e => e.Type == "youTubePramotionAdmin").Select(e=>new { e.imgWebAddress}).FirstOrDefault()).imgWebAddress;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.AboutUsHtmlConten = _IsettingServices.GetAboutUsHtmlContent();
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.ContactUsHml = _IsettingServices.GetContactUsHml();
            return View();
        }
        public ActionResult Products()
        {
            var result = _IsettingServices.getClientWebsiteReference();
            ViewBag.categories = result.Where(e=>e.Type=="1").ToList();
            ViewBag.ProductReference = (result.Where(e => e.Type == "featuredReferrenceValue").Select(e => new { e.imgWebAddress }).FirstOrDefault()).imgWebAddress;
            return View();
        }
        public ActionResult Product(int category)
        {
            TempData["category"] = category;
            return RedirectToAction("Products");
        }
        public ActionResult partial_Products(int productCategoryId, int webReference, int pageNumber, int pageSize)
        {
            var Products = _IProductServices.SearchProductsReult(0, productCategoryId, 0, Convert.ToInt32(BaseUtil.GetWebConfigValue("ClientID")), webReference, pageNumber, pageSize);
            return PartialView("partial_Products", Products);
        }
       
        public ActionResult ProductsDetails( string title, Int64 productId)
        {           
            return View(_IProductServices.ProductForClient_GetById(productId));
        }
        [Route("product-order/{title}-{productId}")]
        public ActionResult OrderProduct(string title, Int64 productId)
        {
            ViewBag.title = title;
            ViewBag.productId = productId;            
            return View();
        }
        public ActionResult partial_CustomerLogOn()
        {
            _fillDropDownList();
            return PartialView("partial_CustomerLogOn");
        }

        public ActionResult partial_CustomerLogIn()
        {
           
            return PartialView("partial_CustomerLogIn");
        }
        [HttpPost]
        public ActionResult partial_CustomerLogOn(user user,FormCollection frm)
        {         
            _fillDropDownList(user);
            var productId = frm["hdn_title_partial_logon"].ToString();
            var productTitle = frm["hdn_productId_partial_logon"].ToString();
            bool result = false;         
            if (!_IAccountServices.boolCheckExiststance(user.userName, "uname"))
            {
                ViewBag.message = "User name already exists in database.";
            }
            else if (!_IAccountServices.boolCheckExiststance(user.mobile, "mobile"))
            {
                ViewBag.message = "Mobile number already exists in database.";
            }
            else if (!_IAccountServices.boolCheckExiststance(user.emailAddress, "email"))
            {
                ViewBag.message = "Email address already exists in database.";
            }
            else
            {
                result = _IAccountServices.create_update_userINformations_sp(user);

                if (result)
                {
                    var Loginresult = _IAccountServices.UserLogin(user.emailAddress, user.password);
                    if (Loginresult != null)
                    {
                        BaseUtil.SetSessionValue(AdminInfo.LoginID.ToString(), Loginresult.userId.ToString());
                        BaseUtil.SetSessionValue(AdminInfo.FullName.ToString(), Loginresult.fullName.ToString());
                        BaseUtil.SetSessionValue(AdminInfo.userType.ToString(), Loginresult.roleID.ToString());
                        return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 2 });
                    }
                    else {
                        ViewData["message"] = Loginresult.message;
                        return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 1 });
                    }
                }
                else
                {
                    ViewData["message"] = "User not updated.";
                }
                return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 1 });
            }
            return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 1 });
        }
        [HttpPost]
        public ActionResult partial_CustomerLogIn(LoginIdViewModel LoginModel, FormCollection frm)
        {
            var productId = frm["hdn_productId_partial_login"].ToString();
            var productTitle = frm["hdn_title_partial_login"].ToString();
            var Loginresult = _IAccountServices.UserLogin(LoginModel.Email, LoginModel.Password);
            if (Loginresult != null)
            {
                BaseUtil.SetSessionValue(AdminInfo.LoginID.ToString(), Loginresult.userId.ToString());
                BaseUtil.SetSessionValue(AdminInfo.FullName.ToString(), Loginresult.fullName.ToString());
                BaseUtil.SetSessionValue(AdminInfo.userType.ToString(), Loginresult.roleID.ToString());
                return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 2 });
            }
            else
            {
                ViewData["message"] = Loginresult.message;
                return RedirectToAction("OrderProduct", new { title = productTitle, productId = productId, page = 1 });
            }
        }
        private void _fillDropDownList(user ouser = null)
        {
            if (ouser == null)
            {               
                ViewBag.countryId = _ICountryServices.getAllCountries().Select(e => new { e.countryId, e.name });
                ViewBag.stateId = _IStateServices.getAllStatesByCountryId(1).Select(e => new { e.stateId, e.name });
                ViewBag.cityId = _IStateServices.getAllCityByStateId(1).Select(e => new { e.cityId, e.CityName });
            }
            else
            {             
                ViewBag.countryId = new SelectList(_ICountryServices.getAllCountries().Select(e => new { e.countryId, e.name }), "countryId", "name", ouser.countryId);
                ViewBag.stateId = new SelectList(_IStateServices.getAllStatesByCountryId(0).Select(e => new { e.stateId, e.name }), "stateId", "name", ouser.stateId);
                ViewBag.cityId = new SelectList(_IStateServices.getAllCityByStateId(0).Select(e => new { e.cityId, e.CityName }), "cityId", "CityName", ouser.cityId);
            }
        }

        public ActionResult Partial_ProductOrderInforamtion()
        {
            _fillDropDownList();
            return PartialView("Partial_ProductOrderInforamtion");
        }
        [HttpPost]
        public ActionResult Partial_ProductOrderInforamtion(ProductOrder productOrder, HttpPostedFileBase files, FormCollection frm)
        {
            var productId = frm["hdn_productId"].ToString();
            var hdn_slug = frm["hdn_slug"].ToString();
            _fillDropDownList();

            if (files != null)
            {
                var oBaseClass = new BaseClass();
                productOrder.Attachment1 = oBaseClass.UploadFile(files, "/images/orderImage");
            }
            Console.Write("Iam in controller");
                 productOrder.createdDate = BaseUtil.GetCurrentDateTime();
                 productOrder.orderStatusId = 1;
                 productOrder.productId =Convert.ToInt64( productId);
                 productOrder.orderToWebClient = Convert.ToInt32(BaseUtil.GetWebConfigValue("ClientID"));
                if (!_productOrderRepository.CreateProductOrder(productOrder))
                {
                Console.Write("Order not submitted, try again later");
                   TempData["message"] = "Order not submitted, try again later";
                    return RedirectToAction("ProductsDetails", new { title = hdn_slug, productId = productId});
                }
                else
            {
                Console.Write("Thank  submitted, try again later");
                TempData["message"] = "Thank you for providing us your order";
                    return RedirectToAction("Index");
                }
           
        }

        public ActionResult Partial_Contact()
        {
            return PartialView("Partial_Contact");
        }
        [HttpPost]
        public ActionResult Partial_Contact(ContactU contact)
        {
           TempData["result"] = _IContactUsServices.SaveContactUs(contact);
            return RedirectToAction("Contact");
        }
        //private void  getCookie()
        //{
        //    HttpCookie cookie = Request.Cookies["WebStoreSiteCookie"];
        //    if (cookie == null)
        //    {
        //        var webSetting = _IsettingServices.getClientWebsiteReference();
        //        string myObjectJson = new JavaScriptSerializer().Serialize(webSetting);
        //        cookie = new HttpCookie("WebStoreSiteCookie", myObjectJson)
        //        {
        //            Expires = DateTime.Now.AddMinutes(10)
        //        };
        //        HttpContext.Response.Cookies.Add(cookie);
        //        Response.Cookies.Add(cookie);               
        //    }
        //    else {
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        var webSetting = serializer.Deserialize<List<getClientWebsiteReference_sp_Result>>(cookie.Value);

        //        var banners = webSetting.Where(e => e.Type == "3").ToList();
        //        var featuredProduct = webSetting.Where(e => e.Type == "1").ToList();
        //        var featuredProductCategory = webSetting.Where(e => e.Type == "1").ToList();
        //    }
        //}.

        public ActionResult NotFound404()
        {
            Response.StatusCode = 404;
            ViewBag.Title = "Error 404 - File not Found";
            return View();
        }
    }
}