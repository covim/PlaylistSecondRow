using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlaylistEditor.Types;
using Wifi.PlaylistEditor.Items;
using System.Drawing;
using System.IO;
using File = TagLib.File;

namespace Wifi.PlaylistEditor.Items
{
    public class Mp3Item : IPlaylistItem
    {
        private string _title;
        private string _artist;
        private TimeSpan _duration;
        private string _path;
        private byte[] _thumbnail;
        private Guid _guid;

        /// <summary>
        /// Create a dummy Mp3Item instance with no tag information
        /// </summary>
        public Mp3Item() { }
        public Mp3Item(string filePath)
        {
            _guid = Guid.NewGuid();
            _path = filePath;
            ReadIdTags();
        }
        public string Title
        {
            get => _title;
            set => _title = value;
        }
        public string Artist
        {
            get => _artist;
            set => _artist = value;
        }
        public TimeSpan Duration => _duration;
        public string Path => _path;
        public byte[] Thumbnail
        {
            get => _thumbnail;
            set => _thumbnail = value;
        }
        public string Extension => ".mp3";
        public string Description => "mp3 Music File";
        public Guid Id => _guid;

        private void ReadIdTags()
        {
            if (string.IsNullOrEmpty(_path))
            {
                return;
            }

            var tfile = TagLib.File.Create(_path);

            _title = tfile.Tag.Title;
            _artist = tfile.Tag.FirstPerformer;
            _duration = tfile.Properties.Duration;

            if (tfile.Tag.Pictures != null && tfile.Tag.Pictures.Length > 0)
            {
                _thumbnail = tfile.Tag.Pictures[0].Data.Data;
            }
            else
            {
                _thumbnail = null;
            }
        }
    }
}
