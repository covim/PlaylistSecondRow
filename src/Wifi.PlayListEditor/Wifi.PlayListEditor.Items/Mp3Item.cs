using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
using Wifi.PlayListEditor.Items;
using System.Drawing;
using System.IO;

namespace Wifi.PlayListEditor.Items
{
    public class Mp3Item : IPlaylistItem
    {
        private string _title;
        private string _artist;
        private TimeSpan _duration;
        private string _path;
        private Image _thumbnail;

        /// <summary>
        /// Create a dummy Mp3Item instance with no tag information
        /// </summary>
        public Mp3Item(){}
        public Mp3Item(string filePath)
        {
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
        public Image Thumbnail
        {
            get => _thumbnail;
            set => _thumbnail = value;
        }
        public string Extension => ".mp3";
        public string Description => "mp3 Music File";

        private void ReadIdTags()
        {
            if (string.IsNullOrEmpty(_path))
            {
                return;
            }

            var tfile = TagLib.File.Create(_path);

            _title = tfile.Tag.Title;
            _artist = tfile.Tag.FirstAlbumArtist;
            _duration = tfile.Properties.Duration;

            if (tfile.Tag.Pictures != null && tfile.Tag.Pictures.Length > 0)
            {
                _thumbnail = Image.FromStream(new MemoryStream(tfile.Tag.Pictures[0].Data.Data));
            }
            else
            {
                _thumbnail = null;
            }
        }
    }
}
