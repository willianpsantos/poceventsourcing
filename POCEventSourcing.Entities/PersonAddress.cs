using POCEventSourcing.Core;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace POCEventSourcing.Entities
{
    public class PersonAddress : Entity
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        [ForeignKey("Person")]
        public long PersonId { get; set; }

        [BsonIgnore]
        [TrackerIgnore]
        [JsonIgnore]
        public Person Person { get; set; }
    }
}
