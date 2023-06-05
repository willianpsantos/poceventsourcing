namespace POCEventSourcing.ReplicationJob.Entities
{
    internal class Person : ReplicationEntity
    {
        public string Name { get; set; }
        public string Document { get; set; }

        public ICollection<PersonAddress> Addresses { get; set; }
    }
}
