using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Core.DataAccess
{
    public interface ICrudRepository<TEntity, TId>
    {
        Task<bool> Exists(TId id);
        Task<TEntity> GetById(TId id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TId> Create(TEntity user);
        Task Update(TEntity user);
        Task Delete(TId id);
        Task<int> DeleteAll();
    }
}