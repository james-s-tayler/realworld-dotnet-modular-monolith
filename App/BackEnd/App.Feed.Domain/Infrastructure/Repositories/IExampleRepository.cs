using System.Collections.Generic;
using System.Threading.Tasks;
using App.Feed.Domain.Entities;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal interface IExampleRepository
    {
        Task<bool> Exists(int id);
        Task<ExampleEntity> GetById(int id);
        Task<IEnumerable<ExampleEntity>> GetAll();
        Task<int> Create(ExampleEntity exampleEntity);
        Task Update(ExampleEntity exampleEntity);
        Task Delete(int id);
        Task<int> DeleteAll();
    }
}