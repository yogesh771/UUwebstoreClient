using System;
using System.Collections.Generic;

namespace UUWebstore.Models.IRepositories
{
    public interface IQuatesRepository
    {
        List<sp_productOrder_GetAll_Result> RecievedQuate(Int64? productId);
    }
}