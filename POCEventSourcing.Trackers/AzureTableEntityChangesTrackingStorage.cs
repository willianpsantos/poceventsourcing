using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.Trackers;
using POCEventSourcing.Options;
using POCEventSourcing.Trackers.Responses;
using POCEventSourcing.Trackers.Messages;
using Azure.Core.Serialization;

namespace POCEventSourcing.Trackers
{
    internal class XSerializer : ObjectSerializer
    {
        public override object? Deserialize(Stream stream, Type returnType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<object?> DeserializeAsync(Stream stream, Type returnType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(Stream stream, object? value, Type inputType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override ValueTask SerializeAsync(Stream stream, object? value, Type inputType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class AzureTableEntityChangesTrackingStorage : EntityChangesTrackingStorage, IEntityChangeTrackerEventStorage
    {
        private readonly AuditLogTableStorageOptions _tableOptions;
        private readonly TableClient _tableClient;

        public AzureTableEntityChangesTrackingStorage(IOptions<AuditLogTableStorageOptions> tableOptions)
        {
            _tableOptions = tableOptions.Value;
            _tableClient = new TableClient(_tableOptions.ConnectionString, _tableOptions.TableName);
        }

        public async Task<IEntityChangeStoredMessage> StoreChangesAsync<TEntity>(IEnumerable<EntityEventEntry<TEntity>> entries) where TEntity : Entity
        {
            var eventTracker = GetEntityTrackerEvent(_tableOptions.PartitionKey, entries);
            var tableEntities = GetTableEntities(eventTracker);

            List<TableTransactionAction> tableActions = new List<TableTransactionAction>(tableEntities.Count());

            tableActions.AddRange(
                tableEntities.Select(
                    tableEntity => new TableTransactionAction(
                        TableTransactionActionType.Add, 
                        tableEntity
                    )
                )
            );

            Response<IReadOnlyList<Response>> responses = await _tableClient.SubmitTransactionAsync(tableActions.ToArray());
            HashSet<AzureTableEntityChangeStoredResponse> tableResponses = new HashSet<AzureTableEntityChangeStoredResponse>();
            Response rawResponse = responses.GetRawResponse();

            foreach (var tableEntity in tableEntities)
            {
                var azureResponse = new AzureTableEntityChangeStoredResponse
                {
                    Success = rawResponse.Status >= 200 && rawResponse.Status <= 210,
                    RequestId = rawResponse.ClientRequestId,
                    PartitionKey = tableEntity.PartitionKey,
                    RowKey = tableEntity.RowKey,
                    EventDate = tableEntity.GetDateTime("EventDate")
                };

                tableResponses.Add(azureResponse);
            }

            var message = new AzureServiceBusEntittyChangesStoredMessage
            {
                PartitionKey = _tableOptions.PartitionKey,
                Responses = tableResponses.ToArray()
            };

            return message;
        }

    }
}
