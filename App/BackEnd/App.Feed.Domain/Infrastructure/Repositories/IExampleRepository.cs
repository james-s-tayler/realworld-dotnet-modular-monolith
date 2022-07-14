using App.Core.DataAccess;
using App.Feed.Domain.Entities;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal interface IExampleRepository : ICrudRepository<ExampleEntity, int>
    {
        
    }
}