﻿using System.Windows.Forms;

namespace Wifi.PlayListEditor.UI
{
    internal interface INewPlaylistDataProvider
    {
        string Title { get; }
        string Author { get; }
        DialogResult StartDialog();

        
    }
}
