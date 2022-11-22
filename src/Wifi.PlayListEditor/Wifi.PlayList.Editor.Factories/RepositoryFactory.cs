using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Wifi.PlayListEditor.Repositories;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayList.Editor.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {

        private readonly IFileSystem _fileSystem;
        private readonly IPlaylistItemFactory _playlistItemFactory;
        private readonly IPlaylistFactory _playlistFactory;
        private List<IRepository> _availableTypes;

        public RepositoryFactory(IPlaylistItemFactory playlistItemFactory) : this(new FileSystem(), playlistItemFactory)
        {
        }

        public RepositoryFactory(IFileSystem fileSystem, IPlaylistItemFactory playlistItemFactory, )
        {
            _fileSystem = fileSystem;
            _playlistItemFactory = playlistItemFactory;
            _availableTypes = new List<IRepository>()
            {
                new M3URepository(_fileSystem , _playlistItemFactory, _playlistFactory),
                //new PlsRepository(_fileSystem , _playlistItemFactory)
            };


        }
        public IEnumerable<IFileDescription> AvailableTypes => _availableTypes;

        public IRepository Create(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath) || _fileSystem.File.Exists(itemPath))
            {
                return null;
            }

            var extension = Path.GetExtension(itemPath);
            var repository = _availableTypes.FirstOrDefault(x => x.Extension == extension);

            return repository;

        }
    }
}

