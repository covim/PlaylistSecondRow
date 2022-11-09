using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types.Test
{
    [TestFixture]
    public class PlaylistTests
    {
        private IPlaylist _fixture;

        [SetUp]
        public void Init()
        {
            _fixture = new Playlist("Superliste", "Rudi", new DateTime(2022, 15, 4));

        }


        [TearDown]
        public void Cleanup()
        {
            
        }

        [Test]
        public void AdditionOfIntValues()
        {
            //arrange
            int zahl1 = 5;
            int zahl2 = 6;
            int ergSoll = 11;

            //act
            var result = zahl1 + zahl2;

            //assert
            Assert.That(result, Is.EqualTo(ergSoll));
        }
    }
}
