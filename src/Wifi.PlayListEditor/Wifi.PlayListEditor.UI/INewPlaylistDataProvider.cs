using System.Windows.Forms;

namespace Wifi.PlayListEditor.UI
{
    public interface INewPlaylistDataProvider
    {
        string Title { get; }
        string Author { get; }
        DialogResult StartDialog();

        
    }
}
