using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayListEditor.Repositories.Test
{
    [TestFixture]
    public class M3URepositoryTest
    {
        private IRepository _fixture;
        private Mock<IPlaylist> _mockedPlaylist;
        private Mock<IFileSystem> _mockedFileSystem;
        

        [SetUp]
        public void Init()
        {

            _mockedFileSystem = new Mock<IFileSystem>();
            _fixture = new M3URepository(_mockedFileSystem.Object);

            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("Artist 1");
            mockedItem1.Setup(x => x.Title).Returns("Demo Song 1");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(123));
            mockedItem1.Setup(x => x.Path).Returns(@"c:\testlied1.mp3");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("Artist 2");
            mockedItem2.Setup(x => x.Title).Returns("Demo Song 2");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(456));
            mockedItem2.Setup(x => x.Path).Returns(@"c:\testlied2.mp3");

            IPlaylistItem[] myMockedItems = new[] { mockedItem1.Object, mockedItem2.Object };

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

            _fixture = new M3URepository(_mockedFileSystem.Object);

            string referenceContent = "#EXTM3U\r\n#EXTART:Artist 1\r\n#EXTINF:123,Demo Song 1\r\nc:\\testlied1.mp3\r\n#EXTART:Artist 2\r\n#EXTINF:456,Demo Song 2\r\nc:\\testlied2.mp3";



        //act
        _fixture.Save(_mockedPlaylist.Object, @"C:\temp\liste.m3u");

        //asssert
        Assert.That(contentToWrite, Is.EqualTo(referenceContent));

        }

        [Test]
        public void LoadTest()
        {
            //arrange
            IPlaylist resultPlayList;
            //act
            resultPlayList = _fixture.Load("C:/temp/liste_Load.m3u");
            //assert
            Assert.That(resultPlayList.ItemList.Count, Is.EqualTo(2));
        }
    }
}
