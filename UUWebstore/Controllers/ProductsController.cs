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
    public class ProductsController : BaseClass
    {
        private readonly IsettingServices _IsettingServices;
        private readonly IProductServices _IProductServices;
        private readonly IaccountServices _IaccountServices;
        public ProductsController()
        {
            _IaccountServices = new accountServices();
            _IsettingServices = new settingServices();
            _IProductServices = new ProductServices();

        }
        public ActionResult Index()
        {
            search osearch = new search();
            osearch.roleID = 2;osearch.cityId = 0; osearch.countryId = 0; osearch.stateId = 0;
            ViewBag.userId = new SelectList(_IaccountServices.getFilteredUsers(osearch).Select(e => new { e.userId, e.fullName }), "userId", "fullName");
            ViewBag.productCategoryId = new SelectList(_IProductServices.GetClientProductCategories(1).Select(e => new { e.proCategoryId, e.name }), "proCategoryId", "name");
            ViewBag.productSubCategoryId = new SelectList(_IProductServices.getProductSubcategoriesByProductCategoryId(1).Select(e => new { e.productSubCategoryId, e.name }), "productSubCategoryId", "name");
            var categoryType = 2; //1 for Product category 2 for product 
            ViewBag.cateoryDDlValue = new SelectList(_IsettingServices.getFeaturedCategoryReferrence(categoryType).Select(e => new { e.cateoryDDlValue, e.categoryCode }), "cateoryDDlValue", "categoryCode");
            return View();
        }
        public ActionResult fillProductSubcategories(Int32 productCategoryId)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var result = javaScriptSerializer.Serialize(_IProductServices.getProductSubcategoriesByProductCategoryId(productCategoryId).Select(e => new { e.productSubCategoryId, e.name }));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillProductCategories(Int32 ddl_filter)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var result = javaScriptSerializer.Serialize(_IProductServices.GetClientProductCategories(ddl_filter).Select(e => new { e.proCategoryId, e.name }));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partial_SearchProductsReult(int productOptions_All_featured, int productCategoryId, int productSubCategoryId, Int16 cateoryDDlValue,int webReference,int pageNumber, int pageSize)
        {
            var userID =  Convert.ToInt32(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) != "" ? BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) : BaseUtil.GetWebConfigValue("ClientID"));
            return PartialView("Partial_SearchProductsReult", _IProductServices.SearchProductsReult(productOptions_All_featured, productCategoryId,  productSubCategoryId, userID, webReference, pageNumber, pageSize));
        }
        public bool makeFeaturedProduct(Int64 productId, bool chk)
        {
            return _IProductServices.MakeProductAsFeatured(productId, chk);
        }

        public int SelectProduct(int productId, bool chk)
        {
            return _IProductServices.SelectProduct(productId, chk);
        }
    }
}