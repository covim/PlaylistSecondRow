using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Wifi.PlaylistEditor.Types
{
    public interface IPlaylistItem : IFileDescription
    {
        string Title { get; set; }
        string Artist { get; set; }
        TimeSpan Duration { get; }
        string Path { get; }
        byte[] Thumbnail { get; set; }


    }
}
