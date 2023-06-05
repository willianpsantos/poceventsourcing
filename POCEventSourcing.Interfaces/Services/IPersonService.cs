using POCEventSourcing.Entities;

namespace POCEventSourcing.Interfaces.Services
{
    public interface IPersonService : IService<Person>, ISenderTrackedChangesService
    {
    }
}
