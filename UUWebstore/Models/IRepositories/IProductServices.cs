using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UUWebstore.Models.IRepositories
{
   public interface IProductServices
    {
        List<productSubCategory> getProductSubcategoriesByProductCategoryId(int productCategoryId);
        List<getProductList_sp_Result> SearchProductsReult(int productOptions_All_featured, int productCategoryId, int productSubCategoryId, int UserId);
        bool MakeProductAsFeatured(Int64 productId, bool chk);
        List<getAllProductClient_sp_Result> GetClientProductCategories(int ddl_filter);
        int SelectProduct(Int64 productId, bool chk);
    }
}
