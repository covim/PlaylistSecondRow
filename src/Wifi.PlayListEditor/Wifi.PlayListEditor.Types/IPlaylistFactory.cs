using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IPlaylistFactory
    {
       
        IEnumerable<IStorageDescription> AvailableTypes { get; }

        IPlaylist Create(string author, string name, DateTime createAt);
    }
}
