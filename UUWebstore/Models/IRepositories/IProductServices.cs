using System;
using System.Collections.Generic;

namespace UUWebstore.Models.IRepositories
{
    public interface IProductServices
    {
        List<productSubCategory> getProductSubcategoriesByProductCategoryId(int productCategoryId);
        List<getProductList_sp_client_Result> SearchProductsReult(int productOptions_All_featured, int productCategoryId, int productSubCategoryId, int UserId,int webReference, int pageNumber, int pageSize);
        bool MakeProductAsFeatured(Int64 productId, bool chk);
        List<getAllProductClient_sp_Result> GetClientProductCategories(int ddl_filter);
        int SelectProduct(Int64 productId, bool chk);
        sp_ProductForClient_GetById_Result ProductForClient_GetById(Int64 id);
    }
}
