using Wifi.PlaylistEditor.Types;

namespace Wifi.PlayListEditor.Service.Domain
{
    public interface IPlaylistService
    {
        Task<IEnumerable<IPlaylist>> GetAllPlaylists();
        Task <IPlaylist> GetPlaylist(string id);
        Task<Object> DeletePlaylist(string id);

    }
}