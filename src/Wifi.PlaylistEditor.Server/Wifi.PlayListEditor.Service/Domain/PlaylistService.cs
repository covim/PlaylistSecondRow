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
        private readonly IDatabaseRepository<PlaylistEntity, PlaylistItemEntity> _databaseRepository;

        public PlaylistService(IPlaylistFactory playlistFactory,
                                IPlaylistItemFactory playlistItemFactory,
                                IDatabaseRepository<PlaylistEntity, PlaylistItemEntity> databaseRepository)
        {
            _playlistFactory = playlistFactory;
            _playlistItemFactory = playlistItemFactory;
            _databaseRepository = databaseRepository;
        }
        
        public async Task AddNewPlaylist(IPlaylist newPlaylist)
        {
            if (newPlaylist == null)
            {
                return;
            }

            var entity = new PlaylistEntity
            {
                Id = newPlaylist.Id.ToString(),
                Author = newPlaylist.Author,
                Title = newPlaylist.Name,
                CreatedAt = newPlaylist.CreateAt.ToString("yyyy-MM-dd"),
                Items = newPlaylist.ItemList.Select(x => x.ToEntity())
            };

            await _databaseRepository.CreateAsync(entity);
        }
        public async Task AddItem(IPlaylistItem newItem)
        {
            var entity = newItem.ToEntity();
            await _databaseRepository.CreateItemAsync(entity);
        }
        public async Task<IEnumerable<IPlaylistItem>> GetAllItems()
        {
            var domainItems = new List<IPlaylistItem>();

            var items = await _databaseRepository.GetItemsAsync();

            if (items == null)
            {
                return Enumerable.Empty<IPlaylistItem>();
            }

            foreach (var item in items)
            {
                var domainItem = _playlistItemFactory.Create(Guid.Parse(item.Id), item.Path);
                if (domainItem == null)
                {
                    await DeleteItem(item.Id);
                }
                else
                {
                    domainItems.Add(domainItem);
                }
            }

            return domainItems;
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
        public async Task<IPlaylistItem> GetItemById(string id)
        {
            var items = await _databaseRepository.GetItemsAsync();
            if (items == null)
            {
                return null;
            }

            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return null;
            }

            var playlistItem = _playlistItemFactory.Create(Guid.Parse(item.Id), item.Path);

            return playlistItem;
        }
        public async Task<IPlaylist> GetPlaylistById(string playlistId)
        {
            var playlistEntity = await _databaseRepository.GetAsync(playlistId);
            if (playlistEntity == null)
            {
                return null;
            }

            var playlist = playlistEntity.ToDomain(_playlistFactory, _playlistItemFactory);
            foreach (var item in playlistEntity.Items)
            {
                var playlistitem = _playlistItemFactory.Create(Guid.Parse(item.Id), item.Path);
                if (playlistitem != null)
                {
                    playlist.Add(playlistitem);
                }
            }
            return playlist;    
        }
        public async Task DeletePlaylist(string playlistId)
        {
            await _databaseRepository.RemovePlaylistAsync(playlistId);
        }
        public async Task DeleteItem(string itemId)
        {
            await _databaseRepository.RemoveItemAsync(itemId);
        }
        public async Task UpdatePlaylist(IPlaylist existingPlaylist, IPlaylist updatedPlaylist)
        {
            existingPlaylist.Name = updatedPlaylist.Name;
            existingPlaylist.Author = updatedPlaylist.Author;
            existingPlaylist.Clear();

            foreach (var item in updatedPlaylist.ItemList)
            {
                existingPlaylist.Add(item);
            }

            await _databaseRepository.UpdateAsync(existingPlaylist.Id.ToString(), existingPlaylist.ToEntity());
        }



    }
}
