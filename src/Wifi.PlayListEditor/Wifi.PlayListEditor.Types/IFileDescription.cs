using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IFileDescription
    {
        /// <summary>
        /// Die Dateiextension die verwendet werden soll für das jeweilige Playlist Format 
        /// z.B.: *.m3u  (siehe Wikipedia) oder *.mp3 oder oder oder
        /// </summary>
        string Extension { get; }
        string Description { get; }
    }
}
