using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class SupplierServices : BaseClass.BaseClass, ISupplierServices
    {
        private readonly UnitOfWork uow;

        public SupplierServices()
        {
            uow = new UnitOfWork();
        }
        public List<get_filterSuppliers_Result> Get_filterSuppliers(string supplierName, int cityId, int stateId, int countryId)
        {
            var supplierName_ = new SqlParameter("@supplierName", SqlString.Null);
            if (supplierName!="")
                supplierName_ = new SqlParameter("@supplierName", supplierName);

            var cityId_ = new SqlParameter("@cityId", SqlString.Null);
            if (cityId != 0)
                cityId_ = new SqlParameter("@cityId", cityId);


            var stateId_ = new SqlParameter("@stateId", SqlString.Null);
            if (stateId != 0)
                stateId_ = new SqlParameter("@stateId", stateId);

            var countryId_ = new SqlParameter("@countryId", SqlString.Null);
            if (countryId != 0)
                countryId_ = new SqlParameter("@countryId", countryId);
            var userId = new SqlParameter("@userId", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
            var result = uow.sp_LoginUser_Result_.SQLQuery<get_filterSuppliers_Result>("get_filterSuppliers @supplierName,@cityId,@stateId,@countryId, @userId",
                                                                                         supplierName_, cityId_, stateId_, countryId_, userId).ToList();
            return result;

        }

        public int SelectSupplier(Int64 UserId, bool chk)
        {
             var createdBy = new SqlParameter("@createdBy", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
             var supplierId_UserID = new SqlParameter("@supplierId_UserID", UserId);            
             var chk_ = new SqlParameter("@chk", chk);

            var result = uow.sp_LoginUser_Result_.SQLQuery<int>("SelectSupplier_sp @createdBy,@supplierId_UserID,@chk",
                                                                                         createdBy, supplierId_UserID, chk_).FirstOrDefault();
            return result;
        }
    }
}