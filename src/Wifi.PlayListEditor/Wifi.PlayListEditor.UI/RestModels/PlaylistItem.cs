namespace Wifi.PlayListEditor.UI.RestModels
{

    public class PlaylistItem

    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public long? Duration { get; set; }

        public byte[] Thumbnail { get; set; }

        public string Extension { get; set; }

        public string Path { get; set; }

    }
}
