namespace POCEventSourcing.Interfaces.Trackers
{
    public interface IPostStoredEntityChangeTracking
    {
        Task SendResultsAsync(IEntityChangeStoredMessage message);
    }
}
