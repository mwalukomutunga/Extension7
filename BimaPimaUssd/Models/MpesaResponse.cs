using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace BimaPimaUssd.Models
{

    public class MpesaCallback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        public Body MpesaBody { get; set; }
    }

    public class Body
    {
        
        [JsonPropertyName("stkCallback")]
        public stkCallback stkCallback { get; set; }
    }

    public class stkCallback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [JsonPropertyName("MerchantRequestID")]
        public string MerchantRequestID { get; set; }
        [JsonPropertyName("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; }
        [JsonPropertyName("ResultCode")]
        public int ResultCode { get; set; }
        [JsonPropertyName("ResultDesc")]
        public string ResultDesc { get; set; }
        [JsonPropertyName("CallbackMetadata")]
        public Callbackmetadata CallbackMetadata { get; set; }
    }

    public class Callbackmetadata
    {
        public Item[] Item { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

}
