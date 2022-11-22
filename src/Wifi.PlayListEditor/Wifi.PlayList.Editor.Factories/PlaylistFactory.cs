using PlaylistsNET.Models;
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
    public class PlaylistFactory : IPlaylistFactory
    {
        public PlaylistFactory()
        {
        }

        public IEnumerable<IStorageDescription> AvailableTypes => new IStorageDescription[]
        {
            new Playlist(),

        };


        public IPlaylist Create(string name, string author, DateTime createAt)
        {
            return new Playlist(name, author, createAt);
        }
    }
}
