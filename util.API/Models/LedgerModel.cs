using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace util.API.Models
{
    public class LedgerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string LedgerDate { get; set; } = null!;
        public List<LedgerDetail> LedgerDetails { get; set; } = null!;
    }

    public class LedgerDetail
    {
        public string LedgerType { get; set; } = null!;
        public string LedgerNote { get; set; } = null!;
        public int LedgerValue { get; set; }
    }
}
