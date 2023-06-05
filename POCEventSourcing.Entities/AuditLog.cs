using POCEventSourcing.Core;

namespace POCEventSourcing.Entities
{
    public class AuditLog : Entity
    {
        public AuditLog()
        {
            this.AuditLogDetails = new HashSet<AuditLogDetails>();
        }
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public System.DateTime EventDateUTC { get; set; }
        public int EventType { get; set; }
        public string TypeFullName { get; set; }
        public long RecordId { get; set; }
        public Nullable<long> ParentId { get; set; }
        public Nullable<System.DateTime> NotificationSent { get; set; }
        public int MasterID { get; set; }
        public string SourceSystem { get; set; }
        public virtual ICollection<AuditLogDetails> AuditLogDetails { get; set; }
    }
}
