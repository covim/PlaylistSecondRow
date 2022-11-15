using Moq;
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



            _fixture = new Playlist("Superliste", "Rudi", new DateTime(2022, 4, 15));




        }

        [Test]
        public void Name_Get()
        {
            //arrange

            //act
            var result = _fixture.Name;
            //assert
            Assert.That(result, Is.EqualTo("Superliste"));
        }

        [Test]
        public void Name_Set()
        {
            //arrange
            var newName = "Neue Superliste";
            _fixture.Name = newName;
            //act
            var result = _fixture.Name;
            //assert
            Assert.That(result, Is.EqualTo(newName));
        }

        [Test]
        public void Author_Get()
        {
            //arrange

            //act
            var result = _fixture.Author;
            //assert
            Assert.That(result, Is.EqualTo("Rudi"));
        }

        [Test]
        public void Author_Set()
        {
            //arrange
            var newAuthor = "Neuer Rudi";
            _fixture.Author = newAuthor;
            //act
            var result = _fixture.Author;
            //assert
            Assert.That(result, Is.EqualTo(newAuthor));
        }
        [Test]
        public void AllowDuplicates_Get()
        {
            //arrange

            //act
            var result = _fixture.AllowDuplicates;
            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void AllowDuplicates_Set()
        {
            //arrange
            _fixture.AllowDuplicates = false;
            //act
            var result = _fixture.AllowDuplicates;
            //assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CrateAt_Get()
        {
            //arrange
            //act
            var result = _fixture.CreateAt.Date;
            //assert
            Assert.That(result, Is.EqualTo(new DateTime(2022, 4, 15).Date));

        }

        [Test]
        public void Duration_Get()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(125));
            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Duration).Returns(TimeSpan.FromSeconds(90));

            _fixture.Add(mockedItem1.Object);
            _fixture.Add(mockedItem2.Object);
            //act
            var duration = _fixture.Duration;

            //assert
            Assert.That(duration, Is.EqualTo(TimeSpan.FromSeconds(215)));
        }

        [Test]
        public void ItemList_Get()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            var mockedItem2 = new Mock<IPlaylistItem>();


            _fixture.Add(mockedItem1.Object);
            _fixture.Add(mockedItem2.Object);
            //act
            var items = _fixture.ItemList;

            //assert
            Assert.That(items.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Add()
        {
            //arrange
            var mockedItem = new Mock<IPlaylistItem>();
            var currentCount = _fixture.ItemList.Count();

            //act
            _fixture.Add(mockedItem.Object);

            //assert
            var item = _fixture.ItemList.Last();
            Assert.That(_fixture.ItemList.Count(), Is.EqualTo(currentCount +1));
            Assert.That(item, Is.EqualTo(mockedItem.Object));

        }

        [Test]
        public void Add_AddDuplicates()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("David");
            mockedItem1.Setup(x => x.Title).Returns("Pete");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David");
            mockedItem2.Setup(x => x.Title).Returns("Pete");

            _fixture.Add(mockedItem1.Object);
            var currentCount = _fixture.ItemList.Count();

            //act
            _fixture.Add(mockedItem2.Object);

            //assert
            var item = _fixture.ItemList.Last();
            Assert.That(_fixture.ItemList.Count(), Is.EqualTo(currentCount + 1));
        }

        [Test]
        public void Add_NoDuplicates_DoNotAddItem()
        {
            //arrange
            _fixture.AllowDuplicates = false;
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("David");
            mockedItem1.Setup(x => x.Title).Returns("Pete");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David");
            mockedItem2.Setup(x => x.Title).Returns("Pete");

            

            _fixture.Add(mockedItem1.Object);
            var currentCount = _fixture.ItemList.Count();

            //act
            _fixture.Add(mockedItem2.Object);
           

            //assert
            var item = _fixture.ItemList.Last();
            Assert.That(_fixture.ItemList.Count(), Is.EqualTo(currentCount));
            
        }

        [Test]
        public void Add_NoDuplicates_AddItem()
        {
            //arrange
            _fixture.AllowDuplicates = false;
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("David");
            mockedItem1.Setup(x => x.Title).Returns("Pete");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David1");
            mockedItem2.Setup(x => x.Title).Returns("Pete");



            _fixture.Add(mockedItem1.Object);
            var currentCount = _fixture.ItemList.Count();

            //act
            _fixture.Add(mockedItem2.Object);


            //assert
            Assert.That(_fixture.ItemList.Count(), Is.EqualTo(currentCount + 1));

        }

        [Test]
        public void Add_ItemIsNull()
        {
            //arrange
            IPlaylistItem item = null;
            var currentCount = _fixture.ItemList.Count();

            //act
            _fixture.Add(item);


            //assert
            Assert.That(_fixture.ItemList.Count(), Is.EqualTo(currentCount));

        }

        [Test]
        public void Clear()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            var mockedItem2 = new Mock<IPlaylistItem>();

            _fixture.Add(mockedItem1.Object);
            _fixture.Add(mockedItem2.Object);

            //act
            var items = _fixture.ItemList;
            _fixture.Clear();

            //assert
            Assert.That(items.Count(), Is.Zero);
        }

        [Test]
        public void Remove()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            mockedItem1.Setup(x => x.Artist).Returns("David");
            mockedItem1.Setup(x => x.Title).Returns("Pete");

            var mockedItem2 = new Mock<IPlaylistItem>();
            mockedItem2.Setup(x => x.Artist).Returns("David1");
            mockedItem2.Setup(x => x.Title).Returns("Pete");


            _fixture.Add(mockedItem1.Object);
            _fixture.Add(mockedItem2.Object);
            //act
            var items = _fixture.ItemList;
            _fixture.Remove(mockedItem1.Object);
            

            //assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items.Last(), Is.EqualTo(mockedItem2.Object));
        }

        [Test]
        public void Remove_NullItem()
        {
            //arrange
            var mockedItem1 = new Mock<IPlaylistItem>();
            var mockedItem2 = new Mock<IPlaylistItem>();
            IPlaylistItem nullItem = null;


            _fixture.Add(mockedItem1.Object);
            _fixture.Add(mockedItem2.Object);
            //act
            var items = _fixture.ItemList;
            _fixture.Remove(mockedItem1.Object);
            _fixture.Remove(nullItem);

            //assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items.Last(), Is.EqualTo(mockedItem2.Object));
        }

        [Test]
        public void ctor_Test()
        {
            //arrange
            //act
            _fixture = new Playlist("Liederliste", "Hans");

            //assert
            Assert.That(_fixture.Author, Is.EqualTo("Hans"));
            Assert.That(_fixture.Name, Is.EqualTo("Liederliste"));
            Assert.That(_fixture.CreateAt.Date, Is.EqualTo(DateTime.Now.Date));
        }

        //[Test]
        //public void AdditionOfIntValues()
        //{
        //    //arrange
        //    int zahl1 = 5;
        //    int zahl2 = 6;
        //    int ergSoll = 11;

        //    //act
        //    var result = zahl1 + zahl2;

        //    //assert
        //    Assert.That(result, Is.EqualTo(ergSoll));
        //}
    }
}
