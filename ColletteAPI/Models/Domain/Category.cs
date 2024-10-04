using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColletteAPI.Models.Domain
{
    public class Category
    {
        [BsonId]  // Marks this as the MongoDB ID field
        [BsonRepresentation(BsonType.ObjectId)]  // Ensures MongoDB can convert between ObjectId and string
        public string Id { get; set; }  // MongoDB's ObjectId

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
