using POCEventSourcing.Core;

namespace POCEventSourcing.Entities
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }

        [LoadOnUpdating]
        public virtual ICollection<PersonAddress> Addresses { get; set; }
    }
}
