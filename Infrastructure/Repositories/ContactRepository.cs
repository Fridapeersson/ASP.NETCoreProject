using Infrastructure.Contexts;
using Infrastructure.Entities.Contact;

namespace Infrastructure.Repositories;

public class ContactRepository : BaseRepo<ContactUsEntity>
{
    public ContactRepository(DataContext context) : base(context)
    {
    }
}
