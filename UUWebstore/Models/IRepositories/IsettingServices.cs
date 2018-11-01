using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UUWebstore.Models.IRepositories
{
   public interface IsettingServices
    {
        int uploadBanner(banner obanner);       
        banner getBannerById(int bannerId);
        List<getAllbannersCreatedBy_sp_Result> bannersCreaTedByUserId(int ddl_filter_AutoAll);
        List<getAllProductCategories_sp_Result> GetAllProductCategories(int ddl_filter , int ddl_filter_AutoAll);

        int MakeProductCategoriesAsFeatured(int productCategoryId, bool chk);
        List<featuredCategoryReferrence> getFeaturedCategoryReferrence(int categoryType);
        bool SaveAdminFeaturedReferrence(int categoryType, int ddl_filter_AutoAll);
        int SelectProductCategories(Int64 productCategoryId, bool chk);
        string GetContactUsHml();
        int UpdateContactUsHml(string Content);
        int UpdateAboutUsHtmlContent(string Content);
        string GetAboutUsHtmlContent();
        string GetHomePageHtmlContent();
        int UpdateHomePageHtmlContent(string Content);
    }
}
