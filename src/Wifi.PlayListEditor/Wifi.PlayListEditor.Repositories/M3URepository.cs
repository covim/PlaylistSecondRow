using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
using Wifi.PlayListEditor.Items;
using System.Web;
using System.Runtime.CompilerServices;
using System.IO;
using PlaylistsNET.Models;
using PlaylistsNET.Content;
using System.IO.Abstractions;
using System.Runtime.InteropServices.ComTypes;
using System.Globalization;

namespace Wifi.PlayListEditor.Repositories
{
    public class M3URepository : IRepository
    {
        private readonly string _extension;
        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IPlaylistFactory _playlistFactory;

        public M3URepository(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
            _extension = ".m3u";
        }


        public M3URepository(IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory) : this(new FileSystem(), playlistItemFactory, playlistFactory)
        {
            _extension = ".m3u";
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
        }

        public string Description => "M3U Playlist file";


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
            DateTime dateFromFile = new DateTime(2022,11,15);
            var paramsSeparator = ':';


            //https://learn.microsoft.com/en-in/dotnet/csharp/language-reference/operators/null-coalescing-operator
            //https://learn.microsoft.com/en-in/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-
            //https://youtu.be/n3s_apjZzBw

            nameFromFile = fileLines.FirstOrDefault(x => x.Contains("#NAME"))?.Split(paramsSeparator)[1] ?? "No Name";
            authorFromFile = fileLines.FirstOrDefault(x => x.Contains("#AUTHOR"))?.Split(paramsSeparator)[1] ?? "No Author";
            dateFromFile = DateTime.ParseExact(fileLines.FirstOrDefault(x => x.Contains("#CREATEAT:"))?.Split(paramsSeparator)[1] ?? DateTime.Today.ToString("yyyy-MM-dd"), "yyyy-MM-dd",CultureInfo.InvariantCulture);

            

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


            M3uPlaylist m3uPlaylist = new M3uPlaylist();
            m3uPlaylist.IsExtended = true;


            foreach (var item in playlist.ItemList)
            {
                m3uPlaylist.PlaylistEntries.Add(new M3uPlaylistEntry()
                {
                    Duration = item.Duration,
                    Path = item.Path,
                    Title = item.Title
                });
            }


            M3uContent content = new M3uContent();
            string text = content.ToText(m3uPlaylist);

            text += $"\r\n#AUTHOR:{playlist.Author}" +
                    $"\r\n#NAME:{playlist.Name}" +
                    $"\r\n#CREATEAT:{playlist.CreateAt.ToString("yyyy-MM-dd")}";

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
