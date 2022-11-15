using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
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

        [SetUp]
        public void Init()
        {
            _fixture = new M3URepository("SuperListezumSpeichern");
        }

        [Test]
        public void SaveTest()
        {
            //arrang
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("David");
            mockedItem1.Setup(x => x.Title).Returns("Pete");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(123));
            mockedItem1.Setup(x => x.Path).Returns("c:\\testlied1.mp3");
            string sollString = "#EXTM3U\n#EXTINF:123,David - Pete\nc:\\testlied1.mp3\n#EXTINF:456,David - Pete\nc:\\testlied2.mp3\n\r\n";


            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David");
            mockedItem2.Setup(x => x.Title).Returns("Pete");
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(456));
            mockedItem2.Setup(x => x.Path).Returns("c:\\testlied2.mp3");

            var newPlaylist = new Playlist("SuperHits", "MoorMann");
            newPlaylist.Add(mockedItem1.Object);
            newPlaylist.Add(mockedItem2.Object);

            //act
            _fixture.Save(newPlaylist, "C:/temp/liste.m3u");

            //asssert
            Assert.That(File.ReadAllText("C:/temp/liste.m3u"), Is.EqualTo(sollString));

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
