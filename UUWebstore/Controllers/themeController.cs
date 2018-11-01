using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Controllers
{
    public class themeController : BaseClass
    {
        private readonly IthemeServices _IthemeServices;       
        public themeController()
        {
            _IthemeServices = new themeServices();         
        }

        // GET: theme
        public ActionResult Index()
        {
            return View(_IthemeServices.getallTheme());
        }
        public int applytheme(int themeId)
        {
           return  _IthemeServices.ApplyTheme(themeId);
        }
        public ActionResult browseLogo()
        {
           ViewBag.img= _IthemeServices.SelectLogo();
            return View();
        }
        [HttpPost]
        public ActionResult browseLogo(HttpPostedFileBase file)
        {
            String fileName = "";
            int result = 0;
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), fileName);
                file.SaveAs(path);
                result=_IthemeServices.uploadLogo("/images/" + fileName);
            }
             ViewBag.result = result;
             return RedirectToAction("browseLogo");
        }
        public ActionResult browseFavicon()
        {
            ViewBag.img = _IthemeServices.SelectFavicon();
            return View();
        }
        [HttpPost]
        public ActionResult browseFavicon(HttpPostedFileBase file)
        {
            String fileName = "";
            int result = 0;
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), fileName);
                file.SaveAs(path);
                result = _IthemeServices.uploadFavicon("/images/" + fileName);
            }
          
            ViewBag.result = result;
            return RedirectToAction("browseFavicon");
        }
       

    }
}