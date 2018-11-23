using System;
using UUWebstore.Models.IRepositories;

namespace UUWebstore.Models.Repositories
{
    public class ProductOrderRepository : BaseClass.BaseClass, IProductOrderRepository
    {
        private readonly UnitOfWork uow;

        public ProductOrderRepository()
        {
            uow = new UnitOfWork();
        }
        public bool CreateProductOrder(ProductOrder oProductOrder)
        {
            Console.Write("Iam in controller");
            uow.productOrder.Add(oProductOrder);
            return true;
        }
    }
}