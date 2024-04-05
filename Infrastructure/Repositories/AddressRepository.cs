using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class AddressRepository : BaseRepo<AddressEntity>
{
    public AddressRepository(DataContext context) : base(context)
    {
    }
}
