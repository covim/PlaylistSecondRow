using System.Globalization;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;
using Wifi.PlaylistEditor.Types;

namespace Wifi.PlayListEditor.Service.Mappings
{
    public static class MongoDbMappings
    {
        public static IEnumerable<IPlaylist> ToDomain(this IEnumerable<PlaylistEntity> entities,
                                                        IPlaylistFactory playlistFactory,
                                                        IPlaylistItemFactory playlistItemFactory)
        {
            return entities.Select(x => playlistFactory.Create(Guid.Parse(x.Id), 
                                                                x.Author, 
                                                                x.Title, 
                                                                DateTime.ParseExact(x.CreatedAt, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
        }
    }
}
