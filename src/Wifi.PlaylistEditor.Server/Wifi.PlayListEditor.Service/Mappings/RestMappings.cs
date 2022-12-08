using Wifi.PlaylistEditor.Types;
using Wifi.PlayListEditor.Service.Models;

namespace Wifi.PlayListEditor.Service.Mappings
{
    public static class RestMappings
    {
        public static PlaylistList ToEntity(this IEnumerable<IPlaylist> domainObjects)
        {
            var PlaylistInfo = domainObjects.Select(x => new PlaylistInfo { Id = x.Id.ToString(), Name = x.Name });

            return new PlaylistList { Playlists = PlaylistInfo.ToList() };
        }

        public static Models.Playlist ToEntity(this IPlaylist domainObject)
        {
            return new Models.Playlist { Id = domainObject.Id.ToString(), Name = domainObject.Name };
        }
    }
}
