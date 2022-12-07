using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlaylistEditor.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wifi.PlaylistEditor.Types
{
    public class Playlist : IPlaylist
    {
        private string _name;
        private string _author;
        private DateTime _createAt;
        private List<IPlaylistItem> _itemList;
        private bool _allowDuplicates;
        private string _storageLocation = "local";
        private string _description = "local saved Playlist";
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public Playlist() { }
        public Playlist(string name, string author) : this(name, author, DateTime.Now)
        {
        }
        public Playlist(string name, string author, DateTime createAt)
        {
            _name = name;
            _author = author;
            _createAt = createAt;
            _allowDuplicates = true;
            _itemList = new List<IPlaylistItem>();
            _id = Guid.NewGuid();
        }

        public Playlist(Guid id, string name, string author, DateTime createAt)
        {
            _name = name;
            _author = author;
            _createAt = createAt;
            _allowDuplicates = true;
            _itemList = new List<IPlaylistItem>();
            _id = id;
        }


        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Author
        {
            get => _author;
            set => _author = value;
        }
        public IEnumerable<IPlaylistItem> ItemList
        {
            get { return _itemList; }
        }
        public DateTime CreateAt => _createAt;
        public TimeSpan Duration
        {
            get
            {
                TimeSpan duration = TimeSpan.Zero;
                _itemList.ForEach(x => duration = duration.Add(x.Duration));

                return duration;
            }
        }


        public bool AllowDuplicates
        {
            get => _allowDuplicates;
            set => _allowDuplicates = value;
        }





        public string StorageLocation
        {
            get => _storageLocation;
            //set => _storageLocation = value; 
        }
        public string Description
        {
            get => _description;
            //set => _description = value; 
        }

        public void Add(IPlaylistItem itemToAdd)
        {

            bool addNewItem = true;

            if (itemToAdd == null)
            {
                return;
            }


            if (!_allowDuplicates)
            {
                addNewItem = !_itemList.Any(x => x.Artist == itemToAdd.Artist && x.Title == itemToAdd.Title);
            }
            if (addNewItem)
            {
                _itemList.Add(itemToAdd);
            }

        }
        public void Clear()
        {
            _itemList.Clear();
        }
        public void Remove(IPlaylistItem itemToremove)
        {
            _itemList.Remove(itemToremove);
        }
    }
}

