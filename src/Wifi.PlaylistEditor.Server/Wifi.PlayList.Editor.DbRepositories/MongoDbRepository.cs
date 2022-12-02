using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;

namespace Wifi.PlayList.Editor.DbRepositories
{
    public class MongoDbRepository : IDatabaseRepository<PlaylistEntity>
    {
        public IMongoCollection<PlaylistEntity> _playlistsCollection;

        public MongoDbRepository(IOptions<PlaylistDbSettings> playlistDbSettings)
        {
            if (playlistDbSettings == null)
            {
                return;
            }

            var mongoClient = new MongoClient(playlistDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(playlistDbSettings.Value.DatabaseName);

            _playlistsCollection = mongoDatabase.GetCollection<PlaylistEntity>(playlistDbSettings.Value.CollectionName);
        }

        public IMongoCollection<PlaylistEntity> PlaylistCollection
        {
            get { return _playlistsCollection; }
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

        public async Task RemoveAsync(string id)
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
    }
}