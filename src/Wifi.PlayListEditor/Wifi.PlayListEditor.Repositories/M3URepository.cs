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

namespace Wifi.PlayListEditor.Repositories
{
    public class M3URepository : IRepository
    {
        private readonly string _extension;
        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;

        public M3URepository(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _extension = ".m3u";
        }


        public M3URepository(IPlaylistItemFactory playlistItemFactory) : this(new FileSystem(), playlistItemFactory)
        {
            _extension = ".m3u";
            _playlistItemFactory = playlistItemFactory;
        }

        public string Description => "M3U Playlist file";


        public string Extension
        {
            get { return _extension; }
            //set { _extension = value; }
        }



        public IPlaylist Load(string playlistFilePath)
        {
            //todo: abfrage auf .extension
            //if (!playlistFilePath.EndsWith(".m3u") || playlistFilePath == null)
            //{
            //    return null;
            //}

            //var returnList = new Playlist("FromFile", "unknown");
            //var m3u = M3U.ParseFromFile(playlistFilePath);
            //foreach (var media in m3u.Medias)
            //{
            //    var playlistItem = new Mp3Item(media.MediaFile);
            //    returnList.Add(playlistItem);
            //}
            //return returnList;

            var stream = _fileSystem.File.OpenRead(playlistFilePath);

            var parser = PlaylistParserFactory.GetPlaylistParser(_extension);
            IBasePlaylist playlist = parser.GetFromStream(stream);

            var myPlaylist = new Playlist("M3UPlaylist", "WifiPlayListEditor");


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
                    AlbumArtist = item.Artist,
                    Duration = item.Duration,
                    Path = item.Path,
                    Title = item.Title
                });
            }


            M3uContent content = new M3uContent();
            string text = content.ToText(m3uPlaylist);

            _fileSystem.File.WriteAllText(playlistFilePath, text);

        }
    }
}
