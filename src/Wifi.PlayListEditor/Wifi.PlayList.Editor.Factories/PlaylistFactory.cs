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
    internal class PlaylistFactory : IPlaylistFactory
    {
        public PlaylistFactory()
        {

        }
        public IPlaylist Create(string name, string author)
        {
            return new Playlist(name, author);
        }
    }
}
