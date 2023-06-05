namespace POCEventSourcing.Options
{
    public class AuditLogTableStorageOptions
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string PartitionKey { get; set; }
    }
}
