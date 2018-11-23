using System.Collections.Generic;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class StateServices : BaseClass.BaseClass, IStateServices
    {

        private readonly UnitOfWork uow;

        public StateServices()
        {
            uow = new UnitOfWork();
        }
        public List<state> getAllStatesByCountryId(int countryID)
        {
            if(countryID==0)
                return uow.state_.GetAll().ToList();

            return uow.state_.Find(e=>e.countryId==countryID).ToList();
        }
        public List<city> getAllCityByStateId(int stateId)
        {
            if (stateId == 0)
                return uow.city_.GetAll().ToList();

            return uow.city_.Find(e => e.stateId == stateId).ToList();
        }
    }
}