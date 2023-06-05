using POCEventSourcing.Core;

namespace POCEventSourcing.Interfaces.Services
{
    public interface IService<TEntity> where TEntity : Entity
    {
        Task<long> InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(long id);
    }
}
