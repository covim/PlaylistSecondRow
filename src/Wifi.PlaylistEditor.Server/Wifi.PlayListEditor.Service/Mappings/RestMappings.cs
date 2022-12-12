using Wifi.PlaylistEditor.Types;
using Wifi.PlayListEditor.Service.Models;

namespace Wifi.PlayListEditor.Service.Mappings
{
    public static class RestMappings
    {
        public static Models.PlaylistItem ToRestEntity(this IPlaylistItem playlistItem)
        {
            return new PlaylistItem
            {
                Id = playlistItem.Id.ToString(),
                Artist = playlistItem.Artist,
                Title = playlistItem.Title,
                Duration = (long)playlistItem.Duration.TotalSeconds,
                Extension = playlistItem.Extension,
                Path = playlistItem.Path,
                Thumbnail = playlistItem.Thumbnail
            };
        }
        public static ItemList ToRestEntity(this IEnumerable<IPlaylistItem> domainItems)
        {
            var entityList = new ItemList();
            entityList.Items = domainItems.Where(x => x != null)
                                          .Select(x => new PlaylistItem
                                          {
                                              Artist = x.Artist,
                                              Duration = (long)x.Duration.TotalSeconds,
                                              Extension = x.Extension,
                                              Path = x.Path,
                                              Thumbnail = x.Thumbnail,
                                              Title = x.Title,
                                              Id = x.Id.ToString()
                                          }).ToList();

            return entityList;
        }
        public static PlaylistList ToRestEntity(this IEnumerable<IPlaylist> domainObjects)
        {
            var PlaylistInfo = domainObjects.Select(x => new PlaylistInfo { Id = x.Id.ToString(), Name = x.Name });

            return new PlaylistList { Playlists = PlaylistInfo.ToList() };
        }

        public static Models.Playlist ToRestEntity(this IPlaylist domainObject)
        {
            return new Models.Playlist { Id = domainObject.Id.ToString(), Name = domainObject.Name };
        }

        public static IPlaylist ToDomain(this PlaylistUpdate updatedPlaylist, IPlaylistFactory playlistFactory)
        {
            return playlistFactory.Create(updatedPlaylist.Title, updatedPlaylist.Author, DateTime.Now);
        }

        public static IPlaylist ToDomain(this PlaylistPost entity, IPlaylistFactory playlistFactory)
        {
            var playlist = playlistFactory.Create(entity.Autor, entity.Name, DateTime.Now);

            return playlist;
        }
    }
}
