using MongoDB.Driver;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;

namespace Wifi.PlayList.Editor.DbRepositories
{
    public interface IDatabaseRepository<T>
    {
        /// <summary>
        /// Defines CRUD (Create, Read, Update, Delete) methods for a Database repository
        /// </summary>
        
        Task<List<T>> GetAsync();

        Task<T> GetAsync(string id);

        Task CreateAsync(T newPlaylist);

        Task UpdateAsync(string id, T updatedPlayist);

        Task RemoveAsync(string id);



    }
}
