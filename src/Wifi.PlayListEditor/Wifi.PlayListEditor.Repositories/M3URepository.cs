using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
using Wifi.PlayListEditor.Items;
using m3uParser;
using System.Web;
using System.Runtime.CompilerServices;
using System.IO;


namespace Wifi.PlayListEditor.Repositories
{
    public class M3URepository : IRepository
    {
        private string _extension = ".m3u";
        private string _description;
        

        public M3URepository(string description) 
        {
            _description = description;
        }

        public string Extension
        {
            get { return _extension; }
            //set { _extension = value; }
        }

        public string Decription
        {
            get { return _description; }
            set { _description = value; }
        }

        public IPlaylist Load(string playlistFilePath)
        {
            //todo: abfrage auf .extension
            if (!playlistFilePath.EndsWith(".m3u") || playlistFilePath == null)
            {
                return null;
            }

            var returnList = new Playlist("FromFile", "unknown");
            var m3u = M3U.ParseFromFile(playlistFilePath);
            foreach (var media in m3u.Medias)
            {
                var playlistItem = new Mp3Item(media.MediaFile);
                returnList.Add(playlistItem);
            }

            return returnList;
        }

        public void Save(IPlaylist playlist, string playlistFilePath)
        {
            string saveString = "#EXTM3U\n";
            using (StreamWriter sw = new StreamWriter(playlistFilePath))
            {
                foreach (var item in playlist.ItemList)
                {
                    saveString += $"#EXTINF:{item.Duration.TotalSeconds.ToString()},{item.Artist} - {item.Title}\n";
                    saveString += $"{item.Path}\n";
                }
                sw.WriteLine(saveString);
            }
        }
    }
}
