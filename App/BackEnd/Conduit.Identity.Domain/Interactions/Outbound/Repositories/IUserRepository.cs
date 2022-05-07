using Conduit.Core.DataAccess;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Interactions.Outbound.Repositories
{
    public interface IUserRepository : ICrudRepository<User, int>
    {
        
    }
}