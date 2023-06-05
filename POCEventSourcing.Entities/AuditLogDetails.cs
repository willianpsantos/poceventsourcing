using POCEventSourcing.Core;

namespace POCEventSourcing.Entities
{
    public class AuditLogDetails : Entity
    {    	
        public long AuditLogId { get; set; }
        
        public string? PropertyName { get; set; }
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
        public string? PropertyReferenceName { get; set; }
    
        public virtual AuditLog? AuditLogs { get; set; }
    }
}
