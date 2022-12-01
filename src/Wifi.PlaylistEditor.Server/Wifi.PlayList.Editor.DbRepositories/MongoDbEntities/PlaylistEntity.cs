using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Wifi.PlayList.Editor.DbRepositories.MongoDbEntities
{
    public class PlaylistEntity
    {
        [BsonId]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("createdDate")]
        public string CreatedAt { get; set; }

        [BsonElement("items")]
        public List<PlaylistItemEntity> Items { get; set; }

    }
}
