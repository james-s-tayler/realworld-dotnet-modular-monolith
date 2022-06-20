using Application.Core.DataAccess;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal interface IExampleRepository : ICrudRepository<Example, int>
    {
    }
}