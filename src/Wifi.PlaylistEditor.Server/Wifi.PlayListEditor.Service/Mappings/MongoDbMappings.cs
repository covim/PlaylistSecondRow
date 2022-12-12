using SharpCompress.Common;
using System.Globalization;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;
using Wifi.PlaylistEditor.Types;

namespace Wifi.PlayListEditor.Service.Mappings
{
    public static class MongoDbMappings
    {
        public static PlaylistItemEntity ToEntity(this IPlaylistItem playlistItem)
        {
            return new PlaylistItemEntity
            {
                Id = playlistItem.Id.ToString(),
                Path = playlistItem.Path,
            };
        }
        
        public static IEnumerable<IPlaylist> ToDomain(this IEnumerable<PlaylistEntity> entities,
                                                      IPlaylistFactory playlistFactory,
                                                      IPlaylistItemFactory playlistItemFactory)
        {
            return entities.Select(x => playlistFactory.Create(Guid.Parse(x.Id), 
                                                                x.Author, 
                                                                x.Title, 
                                                                DateTime.ParseExact(x.CreatedAt, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
        }

        public static IPlaylist ToDomain(this PlaylistEntity entity,
                                         IPlaylistFactory playlistFactory,
                                         IPlaylistItemFactory playlistItemFactory)
        {
            return playlistFactory.Create(Guid.Parse(entity.Id),
                                          entity.Author,
                                          entity.Title,
                                          DateTime.ParseExact(entity.CreatedAt, "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        public static PlaylistEntity ToEntity(this IPlaylist playlist)
        {
            return new PlaylistEntity
            {
                Id = playlist.Id.ToString(),
                Author = playlist.Author,
                Title = playlist.Name,
                CreatedAt = playlist.CreateAt.ToString("yyyy-MM-dd"),
                Items = playlist.ItemList.Select(x => x.ToEntity())
            };
        }


    }
}
