using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class QuatesRepository : BaseClass.BaseClass, IQuatesRepository
    {

        private readonly UnitOfWork uow;

        public QuatesRepository()
        {
            uow = new UnitOfWork();
        }
        public List<sp_productOrder_GetAll_Result> RecievedQuate(Int64? productId)
        {
            var orderTOWebClient = new SqlParameter("@orderTOWebClient", Convert.ToInt64(BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString())));
            var productId_ = new SqlParameter("@OrderID", SqlString.Null);
            if(productId!=null) productId_ = new SqlParameter("@OrderID", productId);

            var RecievedQuate = uow.productOrder.SQLQuery<sp_productOrder_GetAll_Result>("sp_productOrder_GetAll @OrderID, @orderTOWebClient", orderTOWebClient, productId_).ToList();
            return RecievedQuate;
        }
    }
}