using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlaylistEditor.Types
{
    public interface IRepository : IFileDescription
    {

        IPlaylist Load(string playlistFilePath);
        void Save(IPlaylist playlist, string playlistFilePath);

    }
}
