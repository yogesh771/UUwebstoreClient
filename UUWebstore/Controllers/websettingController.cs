using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUWebstore.Models;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Controllers
{
    public class websettingController : BaseClass
    {
        private readonly IsettingServices _IsettingServices;
        public websettingController()
        {
            _IsettingServices = new settingServices();
        }
        public ActionResult availablebanners()
        {
            ViewBag.cateoryDDlValue = new SelectList(_IsettingServices.getFeaturedCategoryReferrence(3).Select(e => new { e.cateoryDDlValue, e.categoryCode }), "cateoryDDlValue", "categoryCode");
            return View();
        }
        public ActionResult Partial_SearchBannerReult(int ddl_filter_AutoAll)
        {
            return PartialView("Partial_SearchBannerReult",_IsettingServices.bannersCreaTedByUserId(ddl_filter_AutoAll));
        }
        public ActionResult uploadbanner()
        {
            return View();
        }
        [HttpPost]
        public ActionResult uploadbanner(banner obanner, HttpPostedFileBase file)
        {
            if (file != null)
            {
                obanner.bannerImgURL = UploadFile(file, "/bannersImg");
            }
            else
            {
                TempData["result"] = "Please upload banner image";
                return View();
            }

            if (_IsettingServices.uploadBanner(obanner) == 1)
            {
                TempData["result"] = 1;
                return RedirectToAction("availablebanners");
            }          
            return View();
        }
       
        public ActionResult Edit(int id)
        {          
            return View(_IsettingServices.getBannerById(id));
        }
        [HttpPost]
        public ActionResult Edit(banner obanner, HttpPostedFileBase file)
        {
            if (file != null)
            {
                obanner.bannerImgURL = UploadFile(file, "/bannersImg");
            }
            
            if (_IsettingServices.uploadBanner(obanner) == 1)
            {
                TempData["result"] = "Banner updated. ";
            }
            return View(obanner);
        }
        public ActionResult AllProductCategories()
        {
            var categoryType = 1; //1 for Product category 2 for product 
            ViewBag.cateoryDDlValue = new SelectList(_IsettingServices.getFeaturedCategoryReferrence(categoryType).Select(e => new { e.cateoryDDlValue, e.categoryCode }), "cateoryDDlValue", "categoryCode");
            return View();
        }
        public ActionResult Partial_SearchProductCategorieReult(int ddl_filter,int ddl_filter_AutoAll)
        {
            return PartialView("Partial_SearchProductCategorieReult", _IsettingServices.GetAllProductCategories(ddl_filter, ddl_filter_AutoAll));
        }
        public int makeFeaturedCategory(bool chk, int productCategoryId, Int64 proCategoryClientId)
        {
            return  _IsettingServices.MakeProductCategoriesAsFeatured(productCategoryId, chk);
        }
        public bool SaveAdminFeaturedReferrence(int ddl_filter_AutoAll, int categoryType)
        {
             //1 for Product category 2 for product 
            return _IsettingServices.SaveAdminFeaturedReferrence(categoryType, ddl_filter_AutoAll);           
        }
        
        public int SelectProductCategories(int productCategoryId, bool chk)
        {
            return _IsettingServices.SelectProductCategories(productCategoryId, chk);
        }
        public ActionResult ContactUs()
        {
            var ContactUsContent = new ContactUsContent();
            ContactUsContent.ContactUsContentHtml = _IsettingServices.GetContactUsHml();
            return View(ContactUsContent);
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUsContent contactUsContent)
        {
           TempData["result"] = _IsettingServices.UpdateContactUsHml(contactUsContent.ContactUsContentHtml);                     
            return View();
        }
        public ActionResult AboutUs()
        {
            var AboutUsHtmlContent = new AboutUsHtml();
            AboutUsHtmlContent.AboutUsHtmlContent = _IsettingServices.GetAboutUsHtmlContent();
            return View(AboutUsHtmlContent);
        }
        [HttpPost]
        public ActionResult AboutUs(AboutUsHtml contactInformation)
        {
            TempData["result"] = _IsettingServices.UpdateAboutUsHtmlContent(contactInformation.AboutUsHtmlContent);
            return View();
        }
        public ActionResult HomePage()
        {
            var AboutUsHtmlContent = new AboutUsHtml();
            AboutUsHtmlContent.AboutUsHtmlContent = _IsettingServices.GetAboutUsHtmlContent();
            return View(AboutUsHtmlContent);
        }
        [HttpPost]
        public ActionResult HomePage(AboutUsHtml contactInformation)
        {
            TempData["result"] = _IsettingServices.UpdateAboutUsHtmlContent(contactInformation.AboutUsHtmlContent);
            return View();
        }

        public ActionResult SocailMedia()
        {
            var oSocailMedia = new SocailMedia();
            var ClientWebsite = _IsettingServices.getClientWebsiteReference();
            oSocailMedia.Facebook= ClientWebsite.Where(e => e.Type == "Facebook").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.GPlus = ClientWebsite.Where(e => e.Type == "GPlus").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.Twiter = ClientWebsite.Where(e => e.Type == "Twiter").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.YouTube = ClientWebsite.Where(e => e.Type == "YouTube").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.Instagram = ClientWebsite.Where(e => e.Type == "Instagram").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.pinterest = ClientWebsite.Where(e => e.Type == "pinterest").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            oSocailMedia.LinkedIn = ClientWebsite.Where(e => e.Type == "LinkedIn").Select(e => new { e.imgWebAddress }).FirstOrDefault().imgWebAddress;
            return View(oSocailMedia);
        }
        [HttpPost]
        public ActionResult SocailMedia(SocailMedia oSocailMedia)
        {
            if (_IsettingServices.UpdateSocialMedia(oSocailMedia) > 0)
            {
                ViewBag.message = "ok";
            }
            else{
                ViewBag.message = "no";
            }
            return View(oSocailMedia);
        }
    }
}