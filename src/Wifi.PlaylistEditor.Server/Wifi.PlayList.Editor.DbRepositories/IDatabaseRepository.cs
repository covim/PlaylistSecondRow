using MongoDB.Driver;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;

namespace Wifi.PlayList.Editor.DbRepositories
{
    public interface IDatabaseRepository<T, TItem>
    {
        /// <summary>
        /// Defines CRUD (Create, Read, Update, Delete) methods for a Database repository
        /// </summary>
        
        Task<List<T>> GetAsync();

        Task<T> GetAsync(string id);

        Task CreateAsync(T newPlaylist);

        Task UpdateAsync(string id, T updatedPlayist);

        Task CreateItemAsync(TItem newPlaylistItem);

        Task RemovePlaylistAsync(string id);

        Task<List<PlaylistItemEntity>> GetItemsAsync();

        Task RemoveItemAsync(string itemId);

    }
}
