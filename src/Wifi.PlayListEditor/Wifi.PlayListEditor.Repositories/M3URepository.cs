using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.PlayListEditor.Types;
using m3uParser;
using System.Web;
using System.Runtime.CompilerServices;
using System.IO;
//using M3U;

namespace Wifi.PlayListEditor.Repositories
{
    public class M3URepository : IRepository
    {
        private string _extension = ".m3u";
        private string _description;
        private m3uParser.Model.Extm3u _m3uFileContent;

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
            throw new NotImplementedException();
        }

        public void Save(IPlaylist playlist, string playlistFilePath)
        {
            string saveString = "#EXTM3U\n";
            using (StreamWriter sw = new StreamWriter(playlistFilePath))
            {
                foreach (var item in playlist.ItemList)
                {
                    saveString += $"#EXINF:{item.Duration.TotalSeconds.ToString()},{item.Artist} - {item.Title}\n";
                    saveString += $"{item.Path}\n";
                }
                sw.WriteLine(saveString);
            }
        }
    }
}
