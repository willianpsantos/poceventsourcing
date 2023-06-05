using POCEventSourcing.Enums;

namespace POCEventSourcing.Core
{
    public class EntityEventEntry<TEntity> where TEntity : Entity
    {
        public TEntity Original { get; set; }
        public TEntity? Updated { get; set; }
        public EntityEventState State { get; set; }
        public DateTime EventDate { get; set; }
    }
}
