using Application.Core.DataAccess;
using Application.Feed.Domain.Entities;

namespace Application.Feed.Domain.Infrastructure.Repositories
{
    internal interface IExampleRepository : ICrudRepository<ExampleEntity, int>
    {
        
    }
}