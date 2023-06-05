using Azure.Data.Tables;
using MassTransit;
using Microsoft.Extensions.Options;
using POCEventSourcing.Interfaces.Trackers;
using POCEventSourcing.Options;
using POCEventSourcing.ReplicationJob.Interfaces;
using System.Text;

namespace POCEventSourcing.ReplicationJob.Consumers
{
    internal class ReplicationConsumer : IConsumer<IEntityChangeStoredMessage>
    {
        private readonly AuditLogTableStorageOptions _tableStorageOptions;
        private readonly ILogger<ReplicationConsumer> _logger;
        
        //internal ReplicationConsumer(AzureTableStorageOptions tableStorageOptions)
        public ReplicationConsumer(ILogger<ReplicationConsumer> logger, IOptions<AuditLogTableStorageOptions> tableStorageOptions)
        {
            _tableStorageOptions = tableStorageOptions.Value;
            _logger = logger;
        }

        private string GetTableStorageFilter(string partitionKey, string[] rowKeys)
        {
            var builder = new StringBuilder();            

            builder.Append($" PartitionKey eq '{partitionKey}' ");            

            if (rowKeys.Length == 1)
            {
                builder.Append(" and ");
                builder.Append($" RowKey eq '{rowKeys[0]}'");
            }
            else if(rowKeys.Length > 1)
            {
                var first = rowKeys.First();
                var last = rowKeys.Last();

                builder.Append(" and ");
                builder.Append($" RowKey ge '{first}'");
                builder.Append(" and ");
                builder.Append($" RowKey le '{last}' ");
            }

            return builder.ToString();
        }

        public async Task Consume(ConsumeContext<IEntityChangeStoredMessage> context)
        {
            _logger.LogInformation($"[{DateTime.UtcNow}] Recebendo mensagens do ServiceBus ...");

            var message = context.Message;
            var tableClient = new TableClient(_tableStorageOptions.ConnectionString, _tableStorageOptions.TableName);

            var rowKeys =
                message
                    .Responses
                    .OrderBy(x => x.EventDate)
                    .Select(x => x.RowKey)
                    .ToArray();

            var filter = GetTableStorageFilter(message.PartitionKey, rowKeys);
            var result = tableClient.QueryAsync<TableEntity>(filter);

            await foreach (var page in result.AsPages())
            {
                var list = page.Values.ToList();

                _logger.LogInformation($"[{DateTime.UtcNow}] {list.Count} eventos a serem processados.");

                var entries =
                    list
                        .Select(l => l.ToEntityEventEntry())
                        .OrderBy(l => l.EventDate)
                        .ToArray();

                foreach (var entry in entries)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow}] Processando evento do tipo {nameof(entry.State)} para a entidade do tipo {entry.EntityName} e ID {entry.EntityId} ...");

                    entry.PartitionKey = message.PartitionKey;

                    var processor = EntityProcessorTypeMapper.GetProcessor(entry.EntityName);
                    await processor.ProcessEntryAsync(entry);

                    _logger.LogInformation($"[{DateTime.UtcNow}] Evento processado.");
                }

                _logger.LogInformation($"[{DateTime.UtcNow}] TODOS OS EVENTOS PROCESSADOS.");
            }
        }
    }
}
