using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayListEditor.UI
{
    public class DummyItem : IPlaylistItem
    {
        public DummyItem(TimeSpan duration, string path)
        {
            Duration = duration;
            Path = path;
        }

        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; }
        public string Path { get; }
        public Image Thumbnail { get; set; }
        public string Extension => "REST";
        public string Description => "Contetn from Rest";


    }
}
