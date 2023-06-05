namespace POCEventSourcing.Interfaces.Services
{
    public  interface ISenderTrackedChangesService
    {
        Task SendTrackedChangesAsync();
    }
}
