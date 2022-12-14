using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IPlaylist
    {
        string Name { get; set; }
        string Author { get; set; }
        DateTime CreateAt { get; }
        TimeSpan Duration { get; }
        bool AllowDuplicates { get; set; }
        

        IEnumerable<IPlaylistItem> ItemList { get; }

        void Add(IPlaylistItem itemToAdd);
        void Remove(IPlaylistItem itemToremove);
        void Clear();
    }
}
