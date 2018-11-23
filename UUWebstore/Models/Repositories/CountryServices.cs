using System.Collections.Generic;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class CountryServices : BaseClass.BaseClass, ICountryServices
    {

        private readonly UnitOfWork uow;

        public CountryServices()
        {
            uow = new UnitOfWork();
        }
        public List<country> getAllCountries()
        {
            return uow.country_.GetAll().ToList();
        }
    }
}