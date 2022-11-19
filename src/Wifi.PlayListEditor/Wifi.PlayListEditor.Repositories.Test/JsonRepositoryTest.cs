using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wifi.PlayListEditor.Repositories.Json;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayListEditor.Repositories.Test
{
    [TestFixture]
    public class JsonRepositoryTest
    {
        private IRepository _fixture;
        private Mock<IPlaylist> _mockedPlaylist;
        private Mock<IFileSystem> _mockedFileSystem;
        private Mock<IPlaylistItemFactory> _mockedPlaylistItemFactory;

        [SetUp]
        public void Init()
        {

            _mockedFileSystem = new Mock<IFileSystem>();
            _mockedPlaylistItemFactory = new Mock<IPlaylistItemFactory>();
            _fixture = new JsonRepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);

            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(101));
            mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(202));
            mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

            var mockedItem3 = new Mock<IPlaylistItem>();
            mockedItem3.Setup(x => x.Title).Returns("TestBild");
            mockedItem3.Setup(x => x.Path).Returns(@"c:\testbild1.jpg");

            IPlaylistItem[] myMockedItems = new[] { mockedItem1.Object, mockedItem2.Object, mockedItem3.Object };

            _mockedPlaylist = new Mock<IPlaylist>();
            _mockedPlaylist.Setup(x => x.Author).Returns("dj Joe");
            _mockedPlaylist.Setup(x => x.Name).Returns("Superliste");
            _mockedPlaylist.Setup(x => x.CreateAt).Returns(new DateTime(2022, 11, 15));
            _mockedPlaylist.Setup(x => x.ItemList).Returns(myMockedItems);

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

            _fixture = new JsonRepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);
            //_fixture = new JsonRepository(_mockedPlaylistItemFactory.Object);

            string referenceContent = "{\"title\":\"Superliste\",\"author\":\"dj Joe\",\"createdAt\":\"2022-11-15\",\"items\":[{\"path\":\"c:\\\\testlied1.mp3\"},{\"path\":\"c:\\\\testlied2.mp3\"},{\"path\":\"c:\\\\testbild1.jpg\"}]}";


            //act
            _fixture.Save(_mockedPlaylist.Object, @"C:\temp\liste.json");

            //asssert
            Assert.That(contentToWrite, Is.EqualTo(referenceContent));

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

            _fixture = new JsonRepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);


            //act
            _fixture.Save(null, @"C:\temp\liste.m3u");

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

            _fixture = new JsonRepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);


            //act
            _fixture.Save(_mockedPlaylist.Object, String.Empty);

            //asssert
            _mockedFile.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        //[Test]
        //public void LoadTest()
        //{
        //    //Arrange
        //    string referenceContent = "#EXTM3U\r\n" +
        //        "#EXTINF:101,Demo Song 1\r\nc:\\testlied1.mp3\r\n" +
        //        "#EXTINF:202,Demo Song 2\r\nc:\\testlied2.mp3\r\n";

        //    string[] referenceContentArray = new string[8];
        //    referenceContentArray[0] = "#EXTM3U";
        //    referenceContentArray[1] = "#EXTINF:101,Demo Song 1";
        //    referenceContentArray[2] = "c:\\testlied1.mp3";
        //    referenceContentArray[3] = "#EXTINF:202,Demo Song 2";
        //    referenceContentArray[4] = "c:\\testlied2.mp3";
        //    referenceContentArray[5] = "#AUTHOR:dj Joe";
        //    referenceContentArray[6] = "#NAME:Superliste";
        //    referenceContentArray[7] = "#CREATEAT:2022-11-15";

        //    var mockedFile = new Mock<IFile>();
        //    mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(referenceContent)));
        //    mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(referenceContentArray);

        //    _mockedFileSystem = new Mock<IFileSystem>();
        //    _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

        //    DateTime creatAtSoll = new DateTime(2022,11,15);


        //    //teuscht die anwesenheit von .mp3 files vor
        //    _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

        //    _fixture = new M3URepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);

        //    //act
        //    IPlaylist playlist = _fixture.Load("demoplaylist.m3u");

        //    //assert
        //    Assert.That(playlist.Duration, Is.EqualTo(TimeSpan.FromSeconds(303)));
        //    Assert.That(playlist.ItemList.Count(), Is.EqualTo(2));
        //    Assert.That(playlist.Author, Is.EqualTo("dj Joe"));
        //    Assert.That(playlist.Name, Is.EqualTo("Superliste"));
        //    Assert.That(playlist.CreateAt, Is.EqualTo(creatAtSoll));
        //}

        //[Test]
        //public void LoadTest_IsEmpty()
        //{
        //    //Arrange
        //    string referenceContent = "#EXTM3U\r\n" +
        //        "#EXTINF:101,Demo Song 1\r\nc:\\testlied1.mp3\r\n" +
        //        "#EXTINF:202,Demo Song 2\r\nc:\\testlied2.mp3\r\n";

        //    string[] referenceContentArray = new string[8];
        //    referenceContentArray[0] = "#EXTM3U";
        //    referenceContentArray[1] = "#EXTINF:101,Demo Song 1";
        //    referenceContentArray[2] = "c:\\testlied1.mp3";
        //    referenceContentArray[3] = "#EXTINF:202,Demo Song 2";
        //    referenceContentArray[4] = "c:\\testlied2.mp3";
        //    referenceContentArray[5] = "#AUTHOR:dj Joe";
        //    referenceContentArray[6] = "#NAME:Superliste";
        //    referenceContentArray[7] = "#CREATEAT:2022-11-15";

        //    var mockedFile = new Mock<IFile>();
        //    mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(referenceContent)));
        //    mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(referenceContentArray);

        //    _mockedFileSystem = new Mock<IFileSystem>();
        //    _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

        //    DateTime creatAtSoll = new DateTime(2022, 11, 15);


        //    //teuscht die anwesenheit von .mp3 files vor
        //    _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

        //    _fixture = new M3URepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);

        //    //act
        //    IPlaylist playlist = _fixture.Load(String.Empty);

        //    //assert
        //    Assert.That(playlist, Is.Null);

        //}

        //[Test]
        //public void LoadTest_IsNUll()
        //{
        //    //Arrange
        //    string referenceContent = "#EXTM3U\r\n" +
        //        "#EXTINF:101,Demo Song 1\r\nc:\\testlied1.mp3\r\n" +
        //        "#EXTINF:202,Demo Song 2\r\nc:\\testlied2.mp3\r\n";

        //    string[] referenceContentArray = new string[8];
        //    referenceContentArray[0] = "#EXTM3U";
        //    referenceContentArray[1] = "#EXTINF:101,Demo Song 1";
        //    referenceContentArray[2] = "c:\\testlied1.mp3";
        //    referenceContentArray[3] = "#EXTINF:202,Demo Song 2";
        //    referenceContentArray[4] = "c:\\testlied2.mp3";
        //    referenceContentArray[5] = "#AUTHOR:dj Joe";
        //    referenceContentArray[6] = "#NAME:Superliste";
        //    referenceContentArray[7] = "#CREATEAT:2022-11-15";

        //    var mockedFile = new Mock<IFile>();
        //    mockedFile.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(referenceContent)));
        //    mockedFile.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(referenceContentArray);

        //    _mockedFileSystem = new Mock<IFileSystem>();
        //    _mockedFileSystem.Setup(x => x.File).Returns(mockedFile.Object);

        //    DateTime creatAtSoll = new DateTime(2022, 11, 15);


        //    //teuscht die anwesenheit von .mp3 files vor
        //    _mockedPlaylistItemFactory = CreateMockedPlaylistItemFactory();

        //    _fixture = new M3URepository(_mockedFileSystem.Object, _mockedPlaylistItemFactory.Object);

        //    //act
        //    IPlaylist playlist = _fixture.Load(null);

        //    //assert
        //    Assert.That(playlist, Is.Null);

        //}

        //private Mock<IPlaylistItemFactory> CreateMockedPlaylistItemFactory()
        //{
        //    var mockedPlaylistItemFactory = new Mock<IPlaylistItemFactory>();

        //    var mockedItem1 = new Mock<IPlaylistItem>();
        //    mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
        //    mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(101));
        //    mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

        //    var mockedItem2 = new Mock<IPlaylistItem>();
        //    mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
        //    mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(202));
        //    mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

        //    mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied1.mp3")).Returns(mockedItem1.Object);
        //    mockedPlaylistItemFactory.Setup(x => x.Create(@"c:\testlied2.mp3")).Returns(mockedItem2.Object);

        //    return mockedPlaylistItemFactory;
        //}
    }
}
