using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrdersService.Domain.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("status")]
        public int Status { get; set; }

        [BsonElement("destination")]
        public string Destination { get; set; }

        [BsonElement("address")]
        public string Address {  get; set; }

        [BsonElement("clientid")]
        public string ClientId { get; set; }

        [BsonElement("driverid")]
        public string DriverId { get; set; }

        [BsonElement("drivermark")]
        public int DriverMark { get; set; }

        [BsonElement("clientmark")]
        public int ClientMark { get; set; }
    }
}
