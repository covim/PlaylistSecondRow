using SharpCompress.Common;
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
                                                                DateTime.ParseExact(x.CreatedAt, "yyyyMMdd", CultureInfo.InvariantCulture)));
        }

        public static IPlaylist ToDomain(this PlaylistEntity entity,
                                         IPlaylistFactory playlistFactory,
                                         IPlaylistItemFactory playlistItemFactory)
        {
            return playlistFactory.Create(Guid.Parse(entity.Id),
                                          entity.Author,
                                          entity.Title,
                                          DateTime.ParseExact(entity.CreatedAt, "yyyyMMdd", CultureInfo.InvariantCulture));
        }


    }
}
