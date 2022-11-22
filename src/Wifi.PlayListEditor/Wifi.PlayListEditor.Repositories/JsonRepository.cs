using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Wifi.PlayListEditor.Repositories.Json;
using Wifi.PlayListEditor.Types;
using static System.Net.Mime.MediaTypeNames;

namespace Wifi.PlayListEditor.Repositories
{
    public class JsonRepository : IRepository
    {
        private readonly string _extension;
        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IPlaylistFactory _playlistFactory;

        public JsonRepository(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
            _extension = ".json";
        }
        public JsonRepository(IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory) : this(new FileSystem(), playlistItemFactory, playlistFactory)
        {
            _extension = ".json";
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
        }
        public string Description => "json Playlist file";
        public string Extension
        {
            get { return _extension; }
            //set { _extension = value; }
        }


        public IPlaylist Load(string playlistFilePath)
        {
            //throw new NotImplementedException();
            //todo string einlesen und deserialisiern

            if (string.IsNullOrEmpty(playlistFilePath))
            {
                return null;
            }

            string json = String.Empty;
            var jsonStream = _fileSystem.File.OpenRead(playlistFilePath);
            using (var sr = new StreamReader(jsonStream))
            {
                json = sr.ReadToEnd();
            }

            PlaylistEntity domain = JsonConvert.DeserializeObject<PlaylistEntity>(json);


            //var myPlaylist = new Playlist(domain.title, domain.author, DateTime.ParseExact(domain.createdAt,"yyyy-MM-dd", CultureInfo.InvariantCulture));
            var myPlaylist = _playlistFactory.Create(domain.title, domain.author, DateTime.ParseExact(domain.createdAt, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            //add items
            foreach (var item in domain.items)
            {
                var newItem = _playlistItemFactory.Create(item.path);
                if (item.path != null)
                {
                    myPlaylist.Add(newItem);
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
            var entity = playlist.ToEntity();

            var settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

            string json = JsonConvert.SerializeObject(entity);


            try
            {
                _fileSystem.File.WriteAllText(playlistFilePath, json);
            }
            catch (Exception)
            {
                return;
            }

        }


    }
}
