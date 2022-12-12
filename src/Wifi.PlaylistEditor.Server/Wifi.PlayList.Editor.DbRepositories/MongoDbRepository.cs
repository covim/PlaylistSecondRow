using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;

namespace Wifi.PlayList.Editor.DbRepositories
{
    public class MongoDbRepository : IDatabaseRepository<PlaylistEntity, PlaylistItemEntity>
    {
        private IMongoCollection<PlaylistEntity> _playlistsCollection;
        private IMongoCollection<PlaylistItemEntity> _itemsCollection;

        public MongoDbRepository(IOptions<PlaylistDbSettings> playlistDbSettings)
        {
            if (playlistDbSettings == null)
            {
                return;
            }

            var mongoClient = new MongoClient(playlistDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(playlistDbSettings.Value.DatabaseName);

            _playlistsCollection = mongoDatabase.GetCollection<PlaylistEntity>(playlistDbSettings.Value.PlaylistCollectionName);
            _itemsCollection = mongoDatabase.GetCollection<PlaylistItemEntity>(playlistDbSettings.Value.ItemsCollectionName);
        }

        public async Task CreateAsync(PlaylistEntity newPlaylist)
        {
            if (newPlaylist == null)
            {
                return;
            }
            await _playlistsCollection.InsertOneAsync(newPlaylist);
        }

        public async Task<List<PlaylistEntity>> GetAsync()
        {
            return await _playlistsCollection.Find(x => true).ToListAsync();
        }

        public async Task<PlaylistEntity> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return await _playlistsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task RemovePlaylistAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            await _playlistsCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(string id, PlaylistEntity updatedPlaylist)
        {
            if (string.IsNullOrEmpty(id) || updatedPlaylist == null)
            {
                return;
            }
            await _playlistsCollection.ReplaceOneAsync(x => x.Id == id, updatedPlaylist);
        }


        //items
        public async Task<List<PlaylistItemEntity>> GetItemsAsync()
        {
            return await _itemsCollection.Find(x => true).ToListAsync();
        }
        public async Task CreateItemAsync(PlaylistItemEntity newPlaylistItem)
        {
            if (newPlaylistItem == null)
            {
                return;
            }

            await _itemsCollection.InsertOneAsync(newPlaylistItem);
        }
        public async Task RemoveItemAsync(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }

            await _itemsCollection.DeleteOneAsync(x => x.Id == itemId);
        }
    }
}