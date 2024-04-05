using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class SubscribersRepository : BaseRepo<SubscribersEntity>
{
    public SubscribersRepository(DataContext context) : base(context)
    {
    }

}
