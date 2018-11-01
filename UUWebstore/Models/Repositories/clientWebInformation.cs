using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class clientWebInformation : BaseClass.BaseClass, IclientWebInformation
    {
        private readonly UnitOfWork uow;

        public clientWebInformation()
        {
            uow = new UnitOfWork();
        }

        public bool addDefaultthemeToClientWebsite(Models.clientWebInformation oclientWebInformation)
        {
            uow.clientWebInformation_.Add(oclientWebInformation);
            return true;
        }
       
    }
 }