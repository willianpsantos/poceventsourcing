using POCEventSourcing.Core;

namespace POCEventSourcing.Interfaces.DB
{
    public interface IDbEntityStateManager
    {
        Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity;
    }
}
