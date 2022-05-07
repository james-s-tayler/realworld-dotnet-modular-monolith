using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conduit.Core.DataAccess
{
    public interface ICrudRepository<TEntity, TId>
    {
        Task<bool> Exists(TId id);
        Task<TEntity> GetById(TId id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TId> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TId id);
    }
}