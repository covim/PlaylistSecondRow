﻿using Newtonsoft.Json;
using System;
using System.IO.Abstractions;
using Wifi.PlaylistEditor.Types;
using Wifi.PlaylistEditor.Repositories.Json;


namespace Wifi.PlaylistEditor.Repositories
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

            if (string.IsNullOrEmpty(playlistFilePath) || !_fileSystem.File.Exists(playlistFilePath))
            {
                return null;
            }

            string json = String.Empty;
            var jsonStream = _fileSystem.File.OpenRead(playlistFilePath);
            using (var sr = new StreamReader(jsonStream))
            {
                json = sr.ReadToEnd();
            }

            PlaylistEntity entity = JsonConvert.DeserializeObject<PlaylistEntity>(json);


            var myPlaylist = entity.ToDomain(_playlistItemFactory, _playlistFactory);

            //add items
            foreach (var item in entity.items)
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

