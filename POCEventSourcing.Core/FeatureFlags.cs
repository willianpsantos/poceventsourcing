namespace POCEventSourcing.Core
{
    public static class FeatureFlags
    {
        public const string AzureEventSourceType_TableStorage = "Table";
        public const string AzureEventSourceType_EventBus = "EventBus";
        public const string AzureEventSourceType_ServiceBus = "ServiceBus";
        public const string AzureEventSourceType_EventGrid = "EventGrid";
    }
}
