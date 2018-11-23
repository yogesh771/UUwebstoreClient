using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class settingServices : BaseClass.BaseClass, IsettingServices
    {

        private readonly UnitOfWork uow;

        public settingServices()
        {
            uow = new UnitOfWork();
        }
        public int uploadBanner(banner obanner)
        {
            obanner.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString());
            obanner.modifiedDate = BaseUtil.GetCurrentDateTime();
            obanner.userId = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString());
            if (obanner.bannerId == 0)
            {
                obanner.createdBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString());
                obanner.createdDate = BaseUtil.GetCurrentDateTime();
                obanner.isDelete = false;
                uow.banner_.Add(obanner);
                return 1;
            }
            else {              
                uow.banner_.Update(obanner);
                return 1;
            }
        }
       
        public List<getAllbannersCreatedBy_sp_Result> bannersCreaTedByUserId(int ddl_filter_AutoAll)
        {
             var param_userID = new SqlParameter("@userID", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString()));
            var ddl_filter_AutoAll_ = new SqlParameter("@ddl_filter_AutoAll", ddl_filter_AutoAll);
            return uow.sp_LoginUser_Result_.SQLQuery<getAllbannersCreatedBy_sp_Result>("getAllbannersCreatedBy_sp @userId,@ddl_filter_AutoAll", param_userID, ddl_filter_AutoAll_).ToList();
        }
        public banner getBannerById(int bannerId)
        {
            return uow.banner_.GetByID(bannerId);
        }
        public List<getAllProductCategories_sp_Result> GetAllProductCategoriesForUser()
        {
            var param_userid = new SqlParameter("@userid", Convert.ToInt32(BaseUtil.GetWebConfigValue("ClientID")));
            var param_featuredOption = new SqlParameter("@featuredOption", "0");
            var res = uow.sp_LoginUser_Result_.SQLQuery<getAllProductCategories_sp_Result>("getAllProductCategories_sp @userid, @featuredOption", param_userid, param_featuredOption).ToList();
            return res;
        }
        public List<getAllProductCategories_sp_Result> GetAllProductCategories(int ddl_filter, int ddl_filter_AutoAll)
        {
            var param_featuredOption = new SqlParameter("@featuredOption", ddl_filter);
            var param_userid = new SqlParameter("@userid", SqlString.Null);
            if (ddl_filter == 1)
            {
                 param_featuredOption = new SqlParameter("@featuredOption", ddl_filter);               
                 if (ddl_filter_AutoAll == 1)
                {
                    param_userid = new SqlParameter("@userid", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString()));
                    return uow.sp_LoginUser_Result_.SQLQuery<getAllProductCategories_sp_Result>("getAllProductCategories_sp @userid, @featuredOption", param_userid, param_featuredOption).ToList();
                }
                else if (ddl_filter_AutoAll == 2)
                {
                    param_userid = new SqlParameter("@userid", Convert.ToInt32(BaseUtil.GetWebConfigValue("AdminID")));
                    return uow.sp_LoginUser_Result_.SQLQuery<getAllProductCategories_sp_Result>("getAllProductCategories_sp @userid, @featuredOption", param_userid, param_featuredOption).ToList();
                }
            else
                {
                    var userid = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString());
                    return uow.sp_LoginUser_Result_.SQLQuery<getAllProductCategories_sp_Result>("getAllProductCategories_sp @userid, @featuredOption", param_userid, param_featuredOption).Where(e => e.createdBy == 1 || e.createdBy == userid).ToList();
                }
            }
            else
            {
                var res = uow.sp_LoginUser_Result_.SQLQuery<getAllProductCategories_sp_Result>("getAllProductCategories_sp @userid, @featuredOption", param_userid, param_featuredOption).ToList();
                return res;
            }
        }
        
        public int MakeProductCategoriesAsFeatured(int productCategoryId, bool chk)
        {
            var userId = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var productCategory = uow.productCategoryClient_.Find(e => e.proCategoryId == productCategoryId && e.createdBy == userId).FirstOrDefault();
            if (chk)
            {               
                if (productCategory == null)
                {
                    productCategoryClient obj = new productCategoryClient();
                    obj.createdBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    obj.createdDate = BaseUtil.GetCurrentDateTime();
                    obj.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    obj.modifiedDate = BaseUtil.GetCurrentDateTime();
                    obj.isFeaturedCategory = true;
                    obj.proCategoryId = productCategoryId;
                    obj.isActive = true;
                    obj.isDelete = false;
                    uow.productCategoryClient_.Add(obj);
                    return 1;
                }
                else
                {
                    productCategory.isFeaturedCategory = true;
                    productCategory.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    productCategory.modifiedDate = BaseUtil.GetCurrentDateTime();
                    uow.productCategoryClient_.Update(productCategory);
                    return 1;
                }
            }
            else {
                if (productCategory != null)
                {
                    productCategory.isFeaturedCategory = false;
                    productCategory.modifiedBy = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
                    productCategory.modifiedDate = BaseUtil.GetCurrentDateTime();
                    uow.productCategoryClient_.Update(productCategory);
                    return 1;
                }
                return 0;
            }
        }       
        // fill dropdownfeaturedCategory by admin OR by website Admin or by both
        public List<featuredCategoryReferrence> getFeaturedCategoryReferrence(int categoryType)
        {
            return uow.featuredCategoryReferrence_.Find(e => e.categoryType == categoryType).ToList();//.Select(e => new { e.cateoryDDlValue, e.categoryCode })
        }
        public bool SaveAdminFeaturedReferrence(int categoryType, int ddl_filter_AutoAll)
        {
            var userID =Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var ClientFeaturedReferrence = uow.ClientFeaturedReferrence_.Find(e => e.categoryType == categoryType && e.userId == userID).FirstOrDefault();
            if (ClientFeaturedReferrence == null)
            {
                var obj = new ClientFeaturedReferrence();
                obj.categoryType = categoryType;
                obj.featuredReferrenceValue = ddl_filter_AutoAll;
                obj.userId = userID;
                uow.ClientFeaturedReferrence_.Add(obj);
            }
            else
            {
                ClientFeaturedReferrence.featuredReferrenceValue = ddl_filter_AutoAll;
                uow.ClientFeaturedReferrence_.Update(ClientFeaturedReferrence);
            }
            return true;
        }

        public int SelectProductCategories(Int64 productCategoryId, bool chk)
        {
            var createdBy = new SqlParameter("@createdBy", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
            var productCategoryId_ = new SqlParameter("@productCategoryId", productCategoryId);
            var chk_ = new SqlParameter("@chk", chk);

            var result = uow.sp_LoginUser_Result_.SQLQuery<int>("SelectProductCategories_sp @createdBy,@productCategoryId,@chk",
                                                                                         createdBy, productCategoryId_, chk_).FirstOrDefault();
            return result;
        }

        public string  GetContactUsHml()
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) != "" ? BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) : BaseUtil.GetWebConfigValue("ClientID"));
            var contactUs=uow.clientWebInformation_.Find(e => e.userId == userID).Select(e=> new{e.contactUsPageContent }).FirstOrDefault();
            return contactUs.contactUsPageContent;
        }
        public int UpdateContactUsHml(string Content)
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) != "" ? BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) : BaseUtil.GetWebConfigValue("ClientID"));
            var contactUs = uow.clientWebInformation_.SingleOrDefault(e => e.userId == userID);
            contactUs.contactUsPageContent = Content;
            uow.clientWebInformation_.Update(contactUs);
            return 1;
        }
        public string GetAboutUsHtmlContent()
        {
           
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())!=""? BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()): BaseUtil.GetWebConfigValue("ClientID"));
            var aboutUsPageContent = uow.clientWebInformation_.Find(e => e.userId == userID).Select(e => new { e.aboutUsPageContent }).FirstOrDefault();
            return aboutUsPageContent.aboutUsPageContent;
        }
        public int UpdateAboutUsHtmlContent(string Content)
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var aboutUsPageContent = uow.clientWebInformation_.SingleOrDefault(e => e.userId == userID);
            aboutUsPageContent.aboutUsPageContent = Content;
            uow.clientWebInformation_.Update(aboutUsPageContent);
            return 1;
        }
        public int UpdateSocialMedia(SocailMedia oSocailMedia)
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var aboutUsPageContent = uow.clientWebInformation_.SingleOrDefault(e => e.userId == userID);
            aboutUsPageContent.Facebook = oSocailMedia.Facebook;
            aboutUsPageContent.LinkedIn = oSocailMedia.LinkedIn;
            aboutUsPageContent.GPlus = oSocailMedia.GPlus;
            aboutUsPageContent.Instagram = oSocailMedia.Instagram;
            aboutUsPageContent.pinterest = oSocailMedia.pinterest;
            aboutUsPageContent.Twiter = oSocailMedia.Twiter;
            aboutUsPageContent.YouTube = oSocailMedia.YouTube;
            uow.clientWebInformation_.Update(aboutUsPageContent);
            return 1;
        }
        public string GetHomePageHtmlContent()
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) != "" ? BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) : BaseUtil.GetWebConfigValue("ClientID"));
            var homePageContent = uow.clientWebInformation_.Find(e => e.userId == userID).Select(e => new { e.homePageContent }).FirstOrDefault();
            return homePageContent.homePageContent;
        }
        public int UpdateHomePageHtmlContent(string Content)
        {
            var userID = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var homePageContent = uow.clientWebInformation_.SingleOrDefault(e => e.userId == userID);
            homePageContent.homePageContent = Content;
            uow.clientWebInformation_.Update(homePageContent);
            return 1;
        }
        public List<getClientWebsiteReference_sp_Result> getClientWebsiteReference()
        {
            var userId = new SqlParameter("@userId", Convert.ToInt32(BaseUtil.GetWebConfigValue("ClientID")));
            var result = uow.sp_LoginUser_Result_.SQLQuery<getClientWebsiteReference_sp_Result>("getClientWebsiteReference_sp @userId", userId).ToList();
            return result;
        }
    }
}