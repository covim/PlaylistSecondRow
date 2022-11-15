using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;

namespace Wifi.PlayList.Editor.Factories
{
    public interface IPlaylistItemFactory
    {
        IPlaylistItem Create(string itemPath);
    }
}
