﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
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


        // unit Test würde in dieser Klasse nur den Konstruktor ausführen und die Daten mit der externen Datei abgleichen
        // "C:\Users\User\Music\DemoMusik\001 - Bruno Mars - Grenade.mp3"
        public Mp3Item(string filePath)
        {
            _path = filePath;
            ReadIdTags();
        }

        private void ReadIdTags()
        {
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



    }
}
