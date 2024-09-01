using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBDemo
{
    public class Book
    {
        [BsonId] // Marks this property as the document's primary key
        [BsonRepresentation(BsonType.ObjectId)] // Represents as ObjectId in MongoDB
        public string Id { get; set; }

        [BsonElement("Title")] // Specifies the field name in MongoDB
        public string Title { get; set; }

        [BsonElement("Author")]
        public string Author { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("PublishedDate")]
        public DateTime PublishedDate { get; set; }
    }
}
