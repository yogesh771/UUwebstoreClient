using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Models.BaseClass
{
    public class BaseClass : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper();
            String ActionName = filterContext.ActionDescriptor.ActionName.ToUpper();
            String Action = string.Format("{0}Controller{1}", ControllerName, ActionName).ToUpper();

            if (BaseUtil.ListControllerExcluded().Contains(ControllerName))
            {
                if (ControllerName == "ACCOUNT" || ControllerName == "HOME" || (ControllerName == "marketings" && ActionName == "INDEX") )
                {
                    return;
                }
            }
            if (BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) == "")
            {
                filterContext.Result = null;
                filterContext.Result = new RedirectResult(BaseUtil.GetApplicationPath()+"/Account/login");
                return;
            }
            return;

        }
        protected override void OnException(ExceptionContext exceptionContext)
        {
             sbv_uuwebstoreEntities db = new sbv_uuwebstoreEntities();
            if (!exceptionContext.ExceptionHandled)
            {
                var uow = new UnitOfWork();
                var AppErrorLog = new AppErrorLog();
                AppErrorLog.ErrorMsg = exceptionContext.Exception.Message;
                AppErrorLog.datelog = BaseUtil.GetCurrentDateTime();
                if (exceptionContext.Exception.InnerException != null)
                {
                    AppErrorLog.innerException = exceptionContext.Exception.InnerException.Message;
                    AppErrorLog.stackTrace = exceptionContext.Exception.StackTrace;
                }
                uow.AppErrorLog_.Add(AppErrorLog);
                TempData["error"] = exceptionContext.Exception.Message;
                TempData["innererror"]= exceptionContext.Exception.InnerException;
                exceptionContext.ExceptionHandled = true;
                exceptionContext.Result= new RedirectResult("~/Views/Shared/Error.cshtml");
                return;
            }
        }
        public void CaptureErrorValues(string error)
        {
            var uow = new UnitOfWork();
            var AppErrorLog = new AppErrorLog();
            AppErrorLog.ErrorMsg = error;
            AppErrorLog.datelog = BaseUtil.GetCurrentDateTime();
            uow.AppErrorLog_.Add(AppErrorLog);

        }
        public string UploadFile(HttpPostedFileBase file, string path)
        {
            String fileName = "";
            fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
            var savedToPath = Path.Combine(HostingEnvironment.MapPath(@"~" + path), fileName);
            file.SaveAs(savedToPath);
            return BaseUtil.GetApplicationPath()+ path + "/" + fileName;
        }
    }
  
}