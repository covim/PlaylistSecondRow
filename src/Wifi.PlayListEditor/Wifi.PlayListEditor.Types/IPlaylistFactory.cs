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

        IPlaylist Create(string name, string author, DateTime createAt);
    }
}
