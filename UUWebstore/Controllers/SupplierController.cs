using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUWebstore.Models.BaseClass;
using UUWebstore.Models.IRepositories;
using UUWebstore.Models.Repositories;

namespace UUWebstore.Controllers
{
    public class SupplierController : BaseClass
    {
        private readonly ISupplierServices _ISupplierServices;
        private readonly IStateServices _IStateServices;
        private readonly ICountryServices _ICountryServices;
        public SupplierController()
        {
            _ISupplierServices = new SupplierServices();
            _IStateServices = new StateServices();
            _ICountryServices = new CountryServices();
        }

        public ActionResult Index()
        {
            ViewBag.countryId = new SelectList(_ICountryServices.getAllCountries().Select(e => new { e.countryId, e.name }), "countryId", "name");
            ViewBag.stateId = new SelectList(_IStateServices.getAllStatesByCountryId(1).Select(e => new { e.stateId, e.name }), "stateId", "name");
            ViewBag.cityId = new SelectList(_IStateServices.getAllCityByStateId(1).Select(e => new { e.cityId, e.CityName }), "cityId", "CityName");
            return View();
        }
        public ActionResult Partial_GetFilteredSupplier(string supplierName, int cityId, int stateId, int countryId)
        {            
            return PartialView("Partial_GetFilteredSupplier", _ISupplierServices.Get_filterSuppliers(supplierName, cityId, stateId, countryId));
        }

        public int SelectSupplier(Int64 UserId, bool chk)
        {
            return _ISupplierServices.SelectSupplier(UserId, chk);
        }
    }
}