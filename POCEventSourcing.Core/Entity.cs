using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POCEventSourcing.Core
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual string GetRowKey(DateTime? date = null)
        {
            var id = Id.ToString().PadLeft(10, '0');
            var timestamp = (date ?? DateTime.UtcNow).Ticks.ToString().PadLeft(25, '0');

            return $"{timestamp}-{id}";
        }

        public virtual string GetCacheKey()
        {
            return this.GetType().Name + $"-{Id}";
        }
        public override int GetHashCode()
        {
            return Convert.ToInt32(Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            var parsed = obj as Entity;

            if (parsed == null)
                return false;

            return parsed.Id == Id;
        }

        public bool Equals(Entity obj)
        {
            return Equals(obj);
        }
    }
}
