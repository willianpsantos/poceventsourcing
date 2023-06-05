using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.Repositories;
using POCEventSourcing.Interfaces.Services;
using POCEventSourcing.Interfaces.Trackers;

namespace POCEventSourcing.Services
{
    public class PersonService : Service, IPersonService
    {
        private readonly IPersonRepository _personRepository;       

        public PersonService
        (
            IPersonRepository repository, 
            IEntityChangeTrackerEventStorage tableTracker,
            IPostStoredEntityChangeTracking postTracker
        )
        : base(tableTracker, postTracker)
        {
            _personRepository = repository;

        }

        public async Task SendTrackedChangesAsync()
        {
            await SendTrackedChangesAsync<Person>(_personRepository);
        }

        public async Task DeleteAsync(long id)
        {
            await _personRepository.DeleteAsync(id);
        }

        public async Task<long> InsertAsync(Person entity)
        {
            var id = await _personRepository.InsertAsync(entity);

            return id;
        }        

        public async Task UpdateAsync(Person entity)
        {
            await _personRepository.UpdateAsync(entity);
        }
    }
}