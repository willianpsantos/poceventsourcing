using POCEventSourcing.Core;

namespace POCEventSourcing.Interfaces.Cache
{
    public interface ICacheListManager
    {
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Entity;
        void Add<TEntity>(TEntity entity) where TEntity : Entity;
        void Update<TEntity>(TEntity entity) where TEntity : Entity;
        bool Remove<TEntity>(TEntity entity) where TEntity : Entity;
        IEnumerable<TEntity> Find<TEntity>(Func<TEntity, bool> expression) where TEntity : Entity;
    }
}
