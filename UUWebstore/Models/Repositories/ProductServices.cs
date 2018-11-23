using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class ProductServices : BaseClass.BaseClass, IProductServices
    {
        private readonly UnitOfWork uow;

        public ProductServices()
        {
            uow = new UnitOfWork();
        }
        public List<productSubCategory> getProductSubcategoriesByProductCategoryId(int productCategoryId)
        {
            return uow.ProductSubCategory.Find(e => e.productCategoryId == productCategoryId).ToList();
        }
        public List<getProductList_sp_client_Result>SearchProductsReult(int productOptions_All_featured, int productCategoryId, int productSubCategoryId, int UserId,int webReference, int pageNumber, int pageSize)
        {

            var productOptions_All_featured_ = new SqlParameter("@productOptions_All_featured", productOptions_All_featured );
          
            var productCategoryId_ = new SqlParameter("@productCategoryId", SqlString.Null);
            if (productCategoryId!=0)
                productCategoryId_ = new SqlParameter("@productCategoryId", productCategoryId);


            var productSubCategoryId_ = new SqlParameter("@productSubCategoryId", SqlString.Null);
            if (productSubCategoryId != 0)
                productSubCategoryId_ = new SqlParameter("@productSubCategoryId", productCategoryId);

            var UserId_ = new SqlParameter("@UserId", UserId);
            var webReference_ = new SqlParameter("@webReference", webReference); 
            int SkipProducts = 0;
            SkipProducts=pageNumber == 0 ? SkipProducts : pageNumber * pageSize;

            var result = uow.sp_LoginUser_Result_.SQLQuery<getProductList_sp_client_Result>("getProductList_sp_client @productOptions_All_featured,@productCategoryId,@productSubCategoryId,@UserId,@webReference",
                                                                                   productOptions_All_featured_, productCategoryId_, productSubCategoryId_, UserId_, webReference_).Skip(SkipProducts).Take(pageSize).ToList();

            return result;
        }

        public bool MakeProductAsFeatured(Int64 productId, bool chk)
        {
            var UserId = Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()));
            var product = uow.ProductForClient.SingleOrDefault(e=>e.productId== productId && e.modifiedBy== UserId);
            if(product==null)
            {
                var createdBy = new SqlParameter("@createdBy", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
                var productId_ = new SqlParameter("@productId", productId);
                var chk_ = new SqlParameter("@chk", chk);
                var isFeatured = new SqlParameter("@isFeatured", true);
                
                return uow.sp_LoginUser_Result_.SQLQuery<int>("SelectProduct_sp @createdBy,@productId,@chk,@isFeatured", createdBy, productId_, chk_, isFeatured).FirstOrDefault() == 1 ? true : false;               
            }
            product.modifiedBy = UserId;
            product.modifiedDate = BaseUtil.GetCurrentDateTime();
            product.isFeaturedProduct = chk;
            uow.ProductForClient.Update(product);
            return true;
        }
        public List<getAllProductClient_sp_Result> GetClientProductCategories(int ddl_filter)
        {
            var userid = new SqlParameter("@userid", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()).ToString()));
            var isFeatured = new SqlParameter("@isFeatured", ddl_filter);
            return uow.sp_LoginUser_Result_.SQLQuery<getAllProductClient_sp_Result>("getAllProductClient_sp @userId,@isFeatured", userid, isFeatured).ToList();

        }
        public int SelectProduct(Int64 productId, bool chk)
        {
            var createdBy = new SqlParameter("@createdBy", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
            var productId_ = new SqlParameter("@productId", productId);
            var chk_ = new SqlParameter("@chk", chk);
            var isFeatured = new SqlParameter("@isFeatured", SqlString.Null);
            var result = uow.sp_LoginUser_Result_.SQLQuery<int>("SelectProduct_sp @createdBy,@productId,@chk,@isFeatured",
                                                                                         createdBy, productId_, chk_, isFeatured).FirstOrDefault();
            return result;
        }

        public sp_ProductForClient_GetById_Result ProductForClient_GetById(Int64 id)
        {
            var createdBy = new SqlParameter("@userId", Convert.ToInt32(BaseUtil.GetWebConfigValue("ClientID")));
            var productId_ = new SqlParameter("@productId", id);
            var result = uow.sp_LoginUser_Result_.SQLQuery<sp_ProductForClient_GetById_Result>("sp_ProductForClient_GetById @productId,@userId", productId_, createdBy).FirstOrDefault();
            return result;
        }
    }
}