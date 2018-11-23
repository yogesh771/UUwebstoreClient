using System;
using System.Collections.Generic;

namespace UUWebstore.Models.IRepositories
{
    public interface ISupplierServices
    {
        List<get_filterSuppliers_Result> Get_filterSuppliers(string supplierName, int cityId, int stateId, int countryId);
        int SelectSupplier(Int64 UserId, bool chk);
    }
}
