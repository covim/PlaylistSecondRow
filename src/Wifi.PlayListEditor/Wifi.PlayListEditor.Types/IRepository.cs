using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IRepository
    {
        /// <summary>
        /// Die Dateiextension die verwendet werden soll für das jeweilige Playlist Format 
        /// z.B.: *.m3u  (siehe Wikipedia)
        /// </summary>
        string Extension { get; }   
        string Description { get; }
        IPlaylist Load(string playlistFilePath);
        void Save(IPlaylist playlist, string playlistFilePath);

    }
}
