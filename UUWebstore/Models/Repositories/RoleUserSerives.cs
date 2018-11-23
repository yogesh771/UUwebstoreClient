using System.Collections.Generic;
using System.Linq;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class RoleUserSerives : BaseClass.BaseClass, IRoleUserServices
    {

        private readonly UnitOfWork uow;

        public RoleUserSerives()
        {
            uow = new UnitOfWork();
        }
        public List<roleUser> getAllRoles()
        {
            return uow.roleUser_.GetAll().ToList();
        }
    }
}