using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Items;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayList.Editor.Factories
{
    internal class PlaylistItemFactory : IPlaylistItemFactory
    {
        private IFileSystem _filesystem;

        public PlaylistItemFactory() : this(new FileSystem())
        {
        }
        public PlaylistItemFactory(IFileSystem fileSystem)
        {
            _filesystem = fileSystem;
        }
        public IEnumerable<IFileDescription> AvailableTypes => new IFileDescription[] 
        {
            new Mp3Item(),
            new ImageItem(),
        };

        public IPlaylistItem Create(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath) || _filesystem.File.Exists(itemPath))
            {
                return null;
            }

            var extension = Path.GetExtension(itemPath);
            switch (extension)
            {
                case ".mp3":
                    return new Mp3Item(itemPath);

                case ".jpg":
                    return new ImageItem(itemPath);

                default:
                    return null;

            }

        }
    }
}
