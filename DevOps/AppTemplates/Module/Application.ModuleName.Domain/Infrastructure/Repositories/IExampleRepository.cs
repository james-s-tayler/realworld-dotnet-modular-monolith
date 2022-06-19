using Application.Core.DataAccess;
using Application.ModuleName.Domain.Entities;

namespace Application.ModuleName.Domain.Infrastructure.Repositories
{
    internal interface IExampleRepository : ICrudRepository<Example, int>
    {
        
    }
}