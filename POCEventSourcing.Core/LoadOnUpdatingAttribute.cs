namespace POCEventSourcing.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LoadOnUpdatingAttribute : Attribute
    {

    }
}
