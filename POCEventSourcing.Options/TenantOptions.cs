namespace POCEventSourcing.Options
{
    public class TenantOptions
    {
        public ServiceBusOptions? ServiceBusOptions { get; set; }
        public WritableDatabaseOptions? WritableDatabaseOptions { get; set; }
        public CacheOptions? CacheOptions { get; set; }
        public ReadableDatabaseOptions? ReadableDatabaseOptions { get; set; }
        public AuditLogTableStorageOptions? AuditLogTableStorageOptions { get; set; }
    }
}
