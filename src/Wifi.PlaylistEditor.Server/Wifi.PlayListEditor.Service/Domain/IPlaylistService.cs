using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;
using Wifi.PlaylistEditor.Types;

namespace Wifi.PlayListEditor.Service.Domain
{
    public interface IPlaylistService
    {
        Task AddNewPlaylist(IPlaylist newPlaylist);
        Task<IEnumerable<IPlaylist>> GetAllPlaylists();
        Task <IPlaylist> GetPlaylistById(string playlistId);
        Task DeletePlaylist(string playlistId);
        Task UpdatePlaylist(IPlaylist existingPlaylist, IPlaylist updatedPlaylist);


        Task AddItem(IPlaylistItem newItem);
        Task<IPlaylistItem> GetItemById(string id);
        Task<IEnumerable<IPlaylistItem>> GetAllItems();
        Task DeleteItem(string itemId);
    }
}