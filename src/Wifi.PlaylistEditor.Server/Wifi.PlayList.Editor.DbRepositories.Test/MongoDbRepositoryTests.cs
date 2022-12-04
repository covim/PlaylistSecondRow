using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
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

        //[OneTimeTearDown] //wird nur einmal nach allen tests ausgeführt
        [TearDown] //wird nach jedem test ausgeführt
        public void Clear()
        {
            var playlists = _fixture.GetAsync().Result;
            foreach (PlaylistEntity playlist in playlists)
            {
                _fixture.RemoveAsync(playlist.Id).Wait();
                for (int i = 0; i < 20; i++)
                {

                    _fixture.GetAsync().Wait();

                }
            }
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
            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();

            //assert           
            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(1));


        }

        [Test]
        public async Task CreateAsync_null()
        {
            //arrange
            
            PlaylistEntity entity = null;
            
            //act
            await _fixture.CreateAsync(entity);
            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();

            //assert
            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(0));


        }

        [Test]
        public async Task GetAsync()
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

            await _fixture.CreateAsync(entity);
            
            //act
            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();

            //assert

            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAsync_EmptyDb()
        {
            //arrange
            

            //act
            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();

            //assert

            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAsyncMitId()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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
            var entity2 = new PlaylistEntity
            {
                Author = "DJ Rudl",
                Title = "Perfecthits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong2.mp3"
                    }
                }
            };

            await _fixture.CreateAsync(entity1);
            await _fixture.CreateAsync(entity2);

            //act
            var playlistItemFromDb = new PlaylistEntity();
            playlistItemFromDb = await _fixture.GetAsync(entity1.Id);
            var playlistItemFromDb2 = new PlaylistEntity();
            playlistItemFromDb2 = await _fixture.GetAsync(entity2.Id);


            //assert

            Assert.That(playlistItemFromDb.Author, Is.EqualTo(entity1.Author));
            Assert.That(playlistItemFromDb2.Author, Is.EqualTo(entity2.Author));

        }

        [Test]
        public async Task GetAsync_idNull()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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

            await _fixture.CreateAsync(entity1);

            string fakeid = null;

            //act
            var playlistItemFromDb = new PlaylistEntity();
            playlistItemFromDb = await _fixture.GetAsync(fakeid);


            //assert
            Assert.That(playlistItemFromDb, Is.EqualTo(null));

        }

        [Test]
        public async Task GetAsync_idEmpty()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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

            await _fixture.CreateAsync(entity1);

            string fakeid = string.Empty;

            //act
            var playlistItemFromDb = new PlaylistEntity();
            playlistItemFromDb = await _fixture.GetAsync(fakeid);


            //assert
            Assert.That(playlistItemFromDb, Is.EqualTo(null));

        }

        [Test]
        public async Task RemoveAsync()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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
            var entity2 = new PlaylistEntity
            {
                Author = "DJ Rudl",
                Title = "Perfecthits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong2.mp3"
                    }
                }
            };

            await _fixture.CreateAsync(entity1);
            await _fixture.CreateAsync(entity2);

            //act
            
            await _fixture.RemoveAsync(entity2.Id);

            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();


            //assert
            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(1));

        }

        [Test]
        public async Task RemoveAsync_idNull()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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
            var entity2 = new PlaylistEntity
            {
                Author = "DJ Rudl",
                Title = "Perfecthits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong2.mp3"
                    }
                }
            };

            await _fixture.CreateAsync(entity1);
            await _fixture.CreateAsync(entity2);

            string fakeid = null;

            //act

            await _fixture.RemoveAsync(fakeid);

            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();


            //assert
            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(2));

        }

        [Test]
        public async Task RemoveAsync_idEmpty()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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
            var entity2 = new PlaylistEntity
            {
                Author = "DJ Rudl",
                Title = "Perfecthits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong2.mp3"
                    }
                }
            };

            await _fixture.CreateAsync(entity1);
            await _fixture.CreateAsync(entity2);

            string fakeid = string.Empty;

            //act

            await _fixture.RemoveAsync(fakeid);

            var playlistItemsFromDb = new List<PlaylistEntity>();
            playlistItemsFromDb = await _fixture.GetAsync();


            //assert
            Assert.That(playlistItemsFromDb.Count, Is.EqualTo(2));

        }

        [Test]
        public async Task UpdateAsync()
        {
            //arrange
            
            var entity1 = new PlaylistEntity
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
            var entity2 = new PlaylistEntity
            {
                Author = "DJ Rudl",
                Title = "Perfecthits",
                CreatedAt = "20221201",
                Id = Guid.NewGuid().ToString(),
                Items = new List<PlaylistItemEntity>
                {
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong1.mp3"
                    },
                    new PlaylistItemEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Path = @"/app/uploads/PerfectSong2.mp3"
                    }
                }
            };
    
            await _fixture.CreateAsync(entity1);
            await _fixture.CreateAsync(entity2);

            //act
            entity1.Author = "DJ Murmeltier";
            await _fixture.UpdateAsync(entity1.Id, entity1);

            var playlistItemFromDb = new PlaylistEntity();
            playlistItemFromDb = await _fixture.GetAsync(entity1.Id);
            var playlistItemFromDb2 = new PlaylistEntity();
            playlistItemFromDb2 = await _fixture.GetAsync(entity2.Id);


            //assert
            Assert.That(playlistItemFromDb.Author, Is.EqualTo("DJ Murmeltier"));

        }


    }
}