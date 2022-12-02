﻿using Microsoft.Extensions.Options;
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

        [OneTimeTearDown]
        public void Clear()
        {
            _fixture.PlaylistCollection.DeleteMany(x => true, default);
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

            //assert
            var count = _fixture.PlaylistCollection.CountDocuments(x => true, default);
            Assert.That(count, Is.EqualTo(1));


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