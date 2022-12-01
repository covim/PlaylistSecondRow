using Microsoft.Extensions.Options;
using NUnit.Framework;
using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;


namespace Wifi.PlayList.Editor.DbRepositories.Test
{
    [TestFixture]
    public class MongoDbRepositoryTests
    {
        private IDatabaseRepository<PlaylistEntity> _fixture;

        [SetUp]
        public void Setup()
        {
            var options = Options.Create(new PlaylistDbSettings
            {
                ConnectionString = "mongodb://admin:password@localhost:27017",
                DatabaseName = "playlistDb",
                CollectionName = "playlists"
            });

            _fixture = new MongoDbRepository(options);
        }

        [Test]
        public async Task CreateAsync()
        {
            //arrange
            var entity = new PlaylistEntity
            {
                Author = "DJ Gustl",
                Title = "Superhits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/superSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/superSong2.mp3"
                    }
                }
            };

            //act
            await _fixture.CreateAsync(entity);
        }
    }
}