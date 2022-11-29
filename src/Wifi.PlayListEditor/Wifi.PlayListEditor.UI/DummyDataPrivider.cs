using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wifi.PlayListEditor.UI
{
    public class DummyDataPrivider : INewPlaylistDataProvider
    {
        public string Title => "SuperSongs 2022";

        public string Author => "DJ Millenium";

        public DialogResult StartDialog()
        {
            return DialogResult.OK;
        }
    }
}
