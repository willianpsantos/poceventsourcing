using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Interfaces.Trackers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCEventSourcing.Services
{
    public abstract class Service
    {
        protected readonly IEntityChangeTrackerEventStorage _tableTracker;
        protected readonly IPostStoredEntityChangeTracking _postTracker;

        protected Service
        (
            IEntityChangeTrackerEventStorage tableTracker,
            IPostStoredEntityChangeTracking postTracker
        )
        {
            _tableTracker = tableTracker;
            _postTracker = postTracker;
        }

        protected virtual async Task SendTrackedChangesAsync<TEntity>(IEntityEventEntriesManager entriesManager) where TEntity : Entity
        {
            var entries = entriesManager.GetEntityEventEntries<TEntity>();

            var responses = await _tableTracker.StoreChangesAsync<TEntity>(entries);

            await _postTracker.SendResultsAsync(responses);                 

            entriesManager.ClearEntityEventEntries();
        }
    }
}
