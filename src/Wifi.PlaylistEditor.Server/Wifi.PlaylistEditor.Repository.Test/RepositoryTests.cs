﻿using Moq;
using NUnit.Framework;
using System.IO.Abstractions;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Wifi.PlaylistEditor.Types;
using Wifi.PlaylistEditor.Repositories;
using NUnit.Framework.Constraints;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;



namespace Wifi.PlaylistEditor.Repository.Test
{
    [TestFixture(typeof(M3URepository), ".m3u", "M3U Playlist file", "#EXTM3U\r\n#EXTINF:101,Demo Song 1\r\nc:\\testlied1.mp3\r\n#EXTINF:202,Demo Song 2\r\nc:\\testlied2.mp3\r\n#AUTHOR:dj Joe\r\n#NAME:Superliste\r\n#CREATEAT:2022-11-15")]
    [TestFixture(typeof(JsonRepository), ".json", "json Playlist file", "{\"title\":\"Superliste\",\"author\":\"dj Joe\",\"createdAt\":\"2022-11-15\",\"items\":[{\"path\":\"c:\\\\testlied1.mp3\"},{\"path\":\"c:\\\\testlied2.mp3\"}]}")]
    [TestFixture(typeof(PlsRepository), ".pls", "pls Playlist file", "[playlist]\r\n\r\nFile1=c:\\testlied1.mp3\r\nTitle1=Demo Song 1\r\nLength1=101\r\n\r\nFile2=c:\\testlied2.mp3\r\nTitle2=Demo Song 2\r\nLength2=202\r\n\r\nNumberOfEntries=2\r\n\r\nVersion=2")]

    public class RepositoryTests<T> where T : IRepository
    {
        private IRepository _fixture;
        private Mock<IPlaylist> _mockedPlaylist;
        private Mock<IFileSystem> _mockedFileSystem;
        private Mock<IPlaylistItemFactory> _mockedPlaylistItemFactory;
        private Mock<IPlaylistFactory> _mockedPlaylistFactory;
        private readonly string _refExtension;
        private readonly string _refDescription;
        private readonly string _refContent;

        public RepositoryTests(string refExtension, string refDescription, string refContent)
        {
            _refExtension = refExtension;
            _refDescription = refDescription;
            _refContent = refContent;
        }

        [SetUp]
        public void Init()
        {

            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedPlaylistItemFactory = new Mock<IPlaylistItemFactory>();
            _mockedPlaylistFactory = new Mock<IPlaylistFactory>();

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });
            //_fixture = new M3URepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);

            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(101));
            mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(202));
            mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

            IPlaylistItem[] myMockedItems = new[] { mockedItem1.Object, mockedItem2.Object };

            _mockedPlaylist = new Mock<IPlaylist>();
            _mockedPlaylist.Setup(x => x.Author).Returns("dj Joe");
            _mockedPlaylist.Setup(x => x.Name).Returns("Superliste");
            _mockedPlaylist.Setup(x => x.CreateAt).Returns(new DateTime(2022, 11, 15));
            _mockedPlaylist.Setup(x => x.ItemList).Returns(myMockedItems);

        }

        [Test]
        public void Extension_get()
        {
            var extension = _fixture.Extension;

            Assert.That(extension, Is.EqualTo(_refExtension));
        }

        [Test]
        public void Description_get()
        {
            var description = _fixture.Description;

            Assert.That(description, Is.EqualTo(_refDescription));
        }

        [Test]
        public void SaveTest()
        {
            //arrang
            string contentToWrite = string.Empty;

            var _mockedFile = new Mock<IFile>();
            _mockedFile.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<string, string>((path, content) =>
                       {
                           contentToWrite = content;
                       });

            var _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(_mockedFile.Object);

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });


            //act
            _fixture.Save(_mockedPlaylist.Object, @"C:\temp\liste" + _refExtension);

            //asssert
            Assert.That(contentToWrite, Is.EqualTo(_refContent));

        }

        [Test]
        public void SaveTest_playListIsNull()
        {
            //arrang
            string contentToWrite = string.Empty;

            var _mockedFile = new Mock<IFile>();
            _mockedFile.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<string, string>((path, content) =>
                       {
                           contentToWrite = content;
                       });

            var _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(_mockedFile.Object);

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });


            //act
            _fixture.Save(null, @"C:\temp\liste" + _refExtension);

            //asssert
            Assert.That(contentToWrite, Is.EqualTo(String.Empty));
            _mockedFile.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [Test]
        public void SaveTest_badSaveString()
        {
            //arrang
            string contentToWrite = string.Empty;

            var _mockedFile = new Mock<IFile>();
            _mockedFile.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<string, string>((path, content) =>
                       {
                           contentToWrite = content;
                       });

            var _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(_mockedFile.Object);

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });


            //act
            _fixture.Save(_mockedPlaylist.Object, String.Empty);

            //asssert
            _mockedFile.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [Test]
        public void LoadTest()
        {
            //Arrange


            string[] seperator = { "\r\n", };

            var mockedFile = new Mock<IFile>();
            mockedFile.Setup(x => x.OpenRead("demoplaylist" + _refExtension)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(_refContent)));
            mockedFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(_refContent.Split(seperator, StringSplitOptions.None));

            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

            DateTime creatAtSoll = new DateTime(2022, 11, 15);


            //teuscht die anwesenheit von .mp3 files vor
            _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();
            //_mockedPlaylistFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(_mockedPlaylist.Object);
            _mockedPlaylistFactory = CreateMockedPlayListFactory();

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });

            //act
            IPlaylist playlist = _fixture.Load("demoplaylist" + _refExtension);
            var duration = playlist.Duration;

            //assert
            Assert.That(playlist.Duration, Is.EqualTo(TimeSpan.FromSeconds(300)));
            Assert.That(playlist.ItemList.Count(), Is.EqualTo(2));
            Assert.That(playlist.Author, Is.EqualTo("dj Joe"));
            Assert.That(playlist.Name, Is.EqualTo("Superliste"));
            Assert.That(playlist.CreateAt, Is.EqualTo(creatAtSoll));
        }

        [Test]
        public void LoadTest_IsEmpty()
        {
            //Arrange
            string[] seperator = { "\r\n", };


            var mockedFile = new Mock<IFile>();
            mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(_refContent)));
            mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(_refContent.Split(seperator, StringSplitOptions.None));

            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

            DateTime creatAtSoll = new DateTime(2022, 11, 15);


            //teuscht die anwesenheit von .mp3 files vor
            _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });

            //act
            IPlaylist playlist = _fixture.Load(String.Empty);

            //assert
            Assert.That(playlist, Is.Null);

        }

        [Test]
        [TestCaseSource(nameof(LoadTestCases))]
        public void LoadTest_WithTestData(int testNr, string data)
        {
            //Arrange
            string[] seperator = { "\r\n", };

            var mockedFile = new Mock<IFile>();
            mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(_refContent)));
            mockedFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(_refContent.Split(seperator, StringSplitOptions.None));

            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

            DateTime creatAtSoll = new DateTime(2022, 11, 15);


            //teuscht die anwesenheit von .mp3 files vor
            _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });

            //act
            IPlaylist playlist = _fixture.Load(data);

            //assert
            Assert.That(playlist, Is.Null);

        }

        public static IEnumerable<object> LoadTestCases()
        {
            return new object[]
            {
                new object[] { 1, string.Empty },
                new object[] { 2, null },
                new object[] { 3, @"C:\123123\2341243" },
            };
        }


        [Test]
        public void LoadTest_IsNUll()
        {
            //Arrange
            var mockedFile = new Mock<IFile>();
            mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(_refContent)));


            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

            DateTime creatAtSoll = new DateTime(2022, 11, 15);


            //teuscht die anwesenheit von .mp3 files vor
            _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

            _fixture = (T)Activator.CreateInstance(typeof(T), new object[] { _mockedFileSystem.Object, _mockedPlaylistItemFactory.Object, _mockedPlaylistFactory.Object });

            //act
            IPlaylist playlist = _fixture.Load(null);

            //assert
            Assert.That(playlist, Is.Null);

        }






        private Mock<IPlaylistItemFactory> CreateMockedPlaylistItemFactory()
        {
            var mockedPlaylistItemFactory = new Mock<IPlaylistItemFactory>();

            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(100));
            mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(200));
            mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

            mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied1.mp3")).Returns(mockedItem1.Object);
            mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied2.mp3")).Returns(mockedItem2.Object);

            return mockedPlaylistItemFactory;
        }


        private Mock<IPlaylistFactory> CreateMockedPlayListFactory()
        {
            var mockedPlaylistItemFactory = new Mock<IPlaylistItemFactory>();
            var mockedPlaylistFactory = new Mock<IPlaylistFactory>();

            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(100));
            mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(200));
            mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

            mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied1.mp3")).Returns(mockedItem1.Object);
            mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied2.mp3")).Returns(mockedItem2.Object);


            IPlaylistItem[] myMockedItems = new[] { mockedItem1.Object, mockedItem2.Object };

            var mockedPlaylist = new Mock<IPlaylist>();
            mockedPlaylist.Setup(x => x.Author).Returns("dj Joe");
            mockedPlaylist.Setup(x => x.Name).Returns("Superliste");
            mockedPlaylist.Setup(x => x.Duration).Returns(new TimeSpan(0, 0, 300));
            mockedPlaylist.Setup(x => x.CreateAt).Returns(new DateTime(2022, 11, 15));
            mockedPlaylist.Setup(x => x.ItemList).Returns(myMockedItems);

            mockedPlaylistFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(mockedPlaylist.Object);

            return mockedPlaylistFactory;
        }
    }
}