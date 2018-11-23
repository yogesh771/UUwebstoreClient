using System.Collections.Generic;

namespace UUWebstore.Models.IRepositories
{
    public interface IStateServices
    {
        List<state> getAllStatesByCountryId(int getAllStatesByCountryId);
        List<city> getAllCityByStateId(int stateId);
    }
}
