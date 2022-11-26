using PlaylistsNET.Content;
using PlaylistsNET.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlaylistEditor.Types;


namespace Wifi.PlaylistEditor.Repositories
{
    public class PlsRepository : IRepository
    {
        private readonly string _extension;
        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IPlaylistFactory _playlistFactory;

        public PlsRepository(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
            _extension = ".pls";
        }


        public PlsRepository(IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory) : this(new FileSystem(), playlistItemFactory, playlistFactory)
        {
            _extension = ".pls";
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
        }

        public string Description => "pls Playlist file";


        public string Extension
        {
            get { return _extension; }
            //set { _extension = value; }
        }



        public IPlaylist Load(string playlistFilePath)
        {
            if (string.IsNullOrEmpty(playlistFilePath) || !_fileSystem.File.Exists(playlistFilePath))
            {
                return null;
            }

            var stream = _fileSystem.File.OpenRead(playlistFilePath);
            var parser = PlaylistParserFactory.GetPlaylistParser(_extension);
            IBasePlaylist playlist = parser.GetFromStream(stream);

            List<string> fileLines = _fileSystem.File.ReadAllLines(playlistFilePath).ToList();


            string nameFromFile = String.Empty;
            string authorFromFile = String.Empty;
            DateTime dateFromFile = new DateTime(2022, 11, 15);
            var paramsSeparator = ':';


            nameFromFile = "No Name";
            authorFromFile = playlistFilePath.Split('\\').Last();
            dateFromFile = DateTime.Today;

            var myPlaylist = _playlistFactory.Create(nameFromFile, authorFromFile, dateFromFile);


            //add items
            var paths = playlist.GetTracksPaths();
            foreach (var itemPath in paths)
            {
                var item = _playlistItemFactory.Create(itemPath);
                if (item != null)
                {
                    myPlaylist.Add(item);
                }
            }

            return myPlaylist;
        }

        public void Save(IPlaylist playlist, string playlistFilePath)
        {
            if (playlist == null || string.IsNullOrEmpty(playlistFilePath))
            {
                return;
            }


            PlsPlaylist plsPlaylist = new PlsPlaylist();



            foreach (var item in playlist.ItemList)
            {
                plsPlaylist.PlaylistEntries.Add(new PlsPlaylistEntry()
                {
                    Length = item.Duration,
                    Path = item.Path,
                    Title = item.Title
                });
            }


            PlsContent content = new PlsContent();
            string text = content.ToText(plsPlaylist);


            try
            {
                _fileSystem.File.WriteAllText(playlistFilePath, text);
            }
            catch (Exception)
            {
                return;
            }


        }
    }
}

