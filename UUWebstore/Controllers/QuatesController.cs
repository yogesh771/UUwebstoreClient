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
    public class QuatesController : BaseClass
    {
        private readonly IQuatesRepository _IQuatesRepository;
        public QuatesController()
        {
            _IQuatesRepository = new QuatesRepository();
        }
        public ActionResult Index()
        {
           return View( _IQuatesRepository.RecievedQuate(null));
        }
        public ActionResult QuateDetails(Int64 orderId)
        {
            return View(_IQuatesRepository.RecievedQuate(orderId));
        }
    }
}