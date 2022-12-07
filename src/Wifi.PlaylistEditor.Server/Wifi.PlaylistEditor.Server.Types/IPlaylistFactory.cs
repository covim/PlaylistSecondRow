using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlaylistEditor.Types
{
    public interface IPlaylistFactory
    {

        IEnumerable<IStorageDescription> AvailableTypes { get; }

        IPlaylist Create(string author, string name, DateTime createAt);

        IPlaylist Create(Guid id, string name, string author, DateTime createAt);
    }
}
