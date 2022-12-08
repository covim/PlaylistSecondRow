using System.Globalization;
using Wifi.PlayList.Editor.DbRepositories;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;
using Wifi.PlaylistEditor.Types;
using Wifi.PlayListEditor.Service.Mappings;

namespace Wifi.PlayListEditor.Service.Domain
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistFactory _playlistFactory;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IDatabaseRepository<PlaylistEntity> _databaseRepository;

        public PlaylistService(IPlaylistFactory playlistFactory,
                                IPlaylistItemFactory playlistItemFactory,
                                IDatabaseRepository<PlaylistEntity> databaseRepository)
        {
            _playlistFactory = playlistFactory;
            _playlistItemFactory = playlistItemFactory;
            _databaseRepository = databaseRepository;
        }
        public async Task<IEnumerable<IPlaylist>> GetAllPlaylists()
        {
            var playlistEntities = await _databaseRepository.GetAsync();
            if(playlistEntities == null || playlistEntities.Count == 0) 
            {
                return new List<IPlaylist>();
            }

            return playlistEntities.ToDomain(_playlistFactory, _playlistItemFactory);
        }

        public async Task<IPlaylist> GetPlaylist(string id)
        {
            var playlistEntity = await _databaseRepository.GetAsync(id);
            if (playlistEntity == null)
            {
                return null;
            }

            return playlistEntity.ToDomain(_playlistFactory, _playlistItemFactory);
        }

        public async Task<Object> DeletePlaylist(string id)
        {
            await _databaseRepository.RemoveAsync(id);
            
            return null;
        }


    }
}
