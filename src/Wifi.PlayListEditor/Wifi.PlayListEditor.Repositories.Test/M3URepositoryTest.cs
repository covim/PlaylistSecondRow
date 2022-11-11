using System;
using System.Collections.Generic;
using System.Linq;
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
            mockedItem1.Setup(x => x.Path).Returns("c:/testlied1");


            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David");
            mockedItem2.Setup(x => x.Title).Returns("Pete");
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(456));
            mockedItem1.Setup(x => x.Path).Returns("c:/testlied2");

            Playlist newPlaylist = new Playlist("SuperHits", "MoorMann");
            newPlaylist.Add(mockedItem1.Object);
            newPlaylist.Add(mockedItem2.Object);

            //act
            _fixture.Save(newPlaylist, "C:\\temp");

            //asssert
            Assert.That(1, Is.EqualTo(1)); //macht keinen sinn will nur mal sehen ob der Test läuft

        }
    }
}
