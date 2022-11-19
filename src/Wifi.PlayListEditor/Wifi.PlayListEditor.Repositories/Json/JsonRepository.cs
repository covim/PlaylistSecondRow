using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
using static System.Net.Mime.MediaTypeNames;

namespace Wifi.PlayListEditor.Repositories.Json
{
    public class JsonRepository : IRepository
    {
        private readonly string _extension;
        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;

        public JsonRepository(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _extension = ".json";
        }
        public JsonRepository(IPlaylistItemFactory playlistItemFactory) : this(new FileSystem(), playlistItemFactory)
        {
            _extension = ".json";
            _playlistItemFactory = playlistItemFactory;
        }
        public string Description => "json Playlist file";
        public string Extension
        {
            get { return _extension; }
            //set { _extension = value; }
        }


        public IPlaylist Load(string playlistFilePath)
        {
            throw new NotImplementedException();
            //todo string einlesen und deserialisiern
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
