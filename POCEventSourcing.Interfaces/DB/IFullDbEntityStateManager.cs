namespace POCEventSourcing.Interfaces.DB
{
    public interface IFullDbEntityStateManager : IDbEntityStateManager, IEntityEventEntriesManager
    {
        IFullDbEntityStateManager WriteOnReadableDatabase(bool write);
        IFullDbEntityStateManager WriteOnCacheDatabase(bool write);
    }
}
