using Conduit.Core.DataAccess;
using Conduit.Social.Domain.Entities;

namespace Conduit.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<User, int> {}
}