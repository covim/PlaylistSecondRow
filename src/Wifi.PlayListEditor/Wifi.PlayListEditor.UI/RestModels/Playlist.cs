using System;
using System.Collections.Generic;


namespace Wifi.PlayListEditor.UI.RestModels
{

    public partial class Playlist

    {

        public string Id { get; set; }

        public string Name { get; set; }

        public long? Duration { get; set; }

        public string Autor { get; set; }

        public DateTime DateOfCreation { get; set; }

        public List<PlaylistItem> Items { get; set; }

    }
}
