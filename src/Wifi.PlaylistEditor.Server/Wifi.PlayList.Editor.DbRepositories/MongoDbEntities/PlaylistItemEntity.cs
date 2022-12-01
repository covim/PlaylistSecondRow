using MongoDB.Bson.Serialization.Attributes;

namespace Wifi.PlayList.Editor.DbRepositories.MongoDbEntities
{
    public class PlaylistItemEntity
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("path")]
        public string Path { get; set; }
    }
}