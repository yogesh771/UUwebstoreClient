using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UUWebstore.Models.IRepositories
{
   public interface IStateServices
    {
        List<state> getAllStatesByCountryId(int getAllStatesByCountryId);
        List<city> getAllCityByStateId(int stateId);
    }
}
