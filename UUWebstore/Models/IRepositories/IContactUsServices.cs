using System.Collections.Generic;

namespace UUWebstore.Models.IRepositories
{
    public interface IContactUsServices
    {
        int SaveContactUs(ContactU oContact);
        List<ContactU> GetAllContacts();
    }
}
