using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;

namespace POCEventSourcing.Interfaces.Repositories
{
    public interface IPersonAddressRepository : IRepository<PersonAddress>, IEntityEventEntriesManager
    {
    }
}
