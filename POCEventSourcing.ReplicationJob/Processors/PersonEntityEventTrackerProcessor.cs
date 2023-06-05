using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POCEventSourcing.Core;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Entities;
using POCEventSourcing.ReplicationJob.Interfaces;

namespace POCEventSourcing.ReplicationJob.Processors
{
    internal class PersonEntityEventTrackerProcessor : BaseEntityEventTrackerProcessor, IEntityEventTrackerProcessor
    {        
        public PersonEntityEventTrackerProcessor(POCReplicationDbContext context) : base(context)
        {

        }

        protected override void SetReplicationProperties<TEntity>(EntityChangesTrackerEventEntry entry, ref TEntity data)
        {
            base.SetReplicationProperties(entry, ref data);
            var person = data as Person;

            if(person is not null && person.Addresses is not null && person.Addresses.Count > 0)
            {
                foreach(var address in person.Addresses)
                {
                    address.LastPartitionKeyApplied = entry.PartitionKey;
                    address.LastRowKeyApplied = entry.RowKey;
                    address.ReplicatedAt = DateTime.UtcNow;                             
                }
            }
        }

        public async Task ProcessEntryAsync(EntityChangesTrackerEventEntry entry)
        {
            Person? data = null;

            switch (entry.State)
            {
                case Enums.EntityEventState.Added:
                    data = JsonConvert.DeserializeObject<Person>(entry.Original);
                    
                    if (data is not null)
                    {
                        SetReplicationProperties<Person>(entry, ref data);
                        await _context.Person.AddAsync(data);
                    }
                    break;

                case Enums.EntityEventState.Modified:
                    data = JsonConvert.DeserializeObject<Person>(entry.Updated);

                    if (data is not null)
                    {                        
                        var original = 
                            await _context
                                .Person
                                .Include(p => p.Addresses)
                                .Where(p => p.Id == data.Id)
                                .FirstOrDefaultAsync();

                        original.Name = data.Name;
                        original.Document = data.Document;
                        original.CreatedAt = data.CreatedAt;
                        original.CreatedBy = data.CreatedBy;
                        original.UpdatedBy = data.UpdatedBy;
                        original.UpdatedAt = data.UpdatedAt;
                        original.Addresses = data.Addresses;

                        SetReplicationProperties<Person>(entry, ref original);
                    }
                    break;

                case Enums.EntityEventState.Deleted:
                    data = JsonConvert.DeserializeObject<Person>(entry.Original);

                    if (data is not null)
                    {
                        _context.Entry(data).State = EntityState.Deleted;
                    }
                    break;
            }

            if(data is null)
            {
                return;
            }

            await _context.SaveChangesAsync();
        }
    }
}
