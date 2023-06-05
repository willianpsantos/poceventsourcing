using POCEventSourcing.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POCEventSourcing.ReplicationJob.Entities
{
    internal class ReplicationEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new long Id { get; set; }

        public DateTime? ReplicatedAt { get; set; }
        public string? LastPartitionKeyApplied { get; set; }
        public string? LastRowKeyApplied { get; set; }
    }
}
