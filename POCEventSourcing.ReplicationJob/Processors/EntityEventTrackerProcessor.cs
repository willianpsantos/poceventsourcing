using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POCEventSourcing.Core;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Entities;
using POCEventSourcing.ReplicationJob.Interfaces;

namespace POCEventSourcing.ReplicationJob.Processors
{
    internal class EntityEventTrackerProcessor : BaseEntityEventTrackerProcessor, IEntityEventTrackerProcessor
    {
        public EntityEventTrackerProcessor(POCReplicationDbContext context) : base(context)
        {

        }

        public async Task ProcessEntryAsync(EntityChangesTrackerEventEntry entry)
        {
            var entityType = EntityNameTypeMapper.GetEntityType(entry.EntityName);
            object? data = null;
            ReplicationEntity? replicationEntity = null;

            switch (entry.State)
            {
                case Enums.EntityEventState.Added:
                    data = JsonConvert.DeserializeObject(entry.Original, entityType);

                    if (data is not null)
                    {
                        replicationEntity = data as ReplicationEntity;
                        SetReplicationProperties(entry, ref replicationEntity);
                        _context.Entry(replicationEntity).State = EntityState.Added;
                    }
                    break;

                case Enums.EntityEventState.Modified:
                    data = JsonConvert.DeserializeObject(entry.Updated, entityType);

                    if (data is not null)
                    {
                        replicationEntity = data as ReplicationEntity;
                        SetReplicationProperties(entry, ref replicationEntity);
                        _context.Entry(replicationEntity).State = EntityState.Modified;
                    }
                    break;

                case Enums.EntityEventState.Deleted:
                    data = JsonConvert.DeserializeObject(entry.Original, entityType);

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
