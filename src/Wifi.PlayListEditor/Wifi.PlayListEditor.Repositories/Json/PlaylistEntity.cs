using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Repositories.Json
{
    internal class PlaylistEntity
    {
        public string title { get; set; }
        public string author { get; set; }
        public string createdAt { get; set; }
        public IEnumerable<ItemEntity> items { get; set; }
    }
}
