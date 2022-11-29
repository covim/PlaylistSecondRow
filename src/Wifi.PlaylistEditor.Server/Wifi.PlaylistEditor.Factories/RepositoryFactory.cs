﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Wifi.PlaylistEditor.Repositories;
using Wifi.PlaylistEditor.Types;

namespace Wifi.PlaylistEditor.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {

        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IPlaylistFactory _playlistFactory;
        private List<IRepository> _availableTypes;

        public RepositoryFactory(IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory) : this(new FileSystem(), playlistItemFactory, playlistFactory)
        {
        }

        public RepositoryFactory(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory, IPlaylistFactory playlistFactory)
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _playlistFactory = playlistFactory;
            _availableTypes = new List<IRepository>()
            {
                new M3URepository(_fileSystem , _playlistItemFactory, _playlistFactory),
                new PlsRepository(_fileSystem , _playlistItemFactory, _playlistFactory),
                new JsonRepository(_fileSystem , _playlistItemFactory, _playlistFactory),
            };


        }
        public IEnumerable<IFileDescription> AvailableTypes => _availableTypes;

        public IRepository Create(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath))
            {
                return null;
            }

            var extension = Path.GetExtension(itemPath);
            var repository = _availableTypes.FirstOrDefault(x => x.Extension == extension);

            return repository;

        }
    }
}
