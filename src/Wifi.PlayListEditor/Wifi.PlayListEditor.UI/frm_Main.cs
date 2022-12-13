using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wifi.PlayList.Editor.Factories;
using Wifi.PlayListEditor.Types;
using Wifi.PlayListEditor.UI.RestModels;

namespace Wifi.PlayListEditor.UI
{
    public partial class frm_Main : Form
    {
        private INewPlaylistDataProvider _newPlaylistDataProvider;
        private IPlaylist _playlist;

        private IPlaylistFactory _playlistFactory;
        private IPlaylistItemFactory _playlistItemFactory;
        private IRepositoryFactory _repositoryFactory;

        //für den Container im Programm.cs müssen die benötigten typen im Construktor sein
        public frm_Main(INewPlaylistDataProvider newPlaylistDataProvider,
                     IPlaylistFactory playlistFactory,
                     IPlaylistItemFactory playlistItemFactory,
                     IRepositoryFactory repositoryFactory)
        {
           
            _newPlaylistDataProvider = newPlaylistDataProvider;
            _playlistFactory = playlistFactory;
            _playlistItemFactory = playlistItemFactory;
            _repositoryFactory = repositoryFactory;

            InitializeComponent();

        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            lbl_itemDetailinfo.Text = string.Empty;
            lbl_playlistInfo.Text = string.Empty;

            EnableEditMenuItems(false);
        }

        private void UpdatePlaylistItemView()
        {
            int index = 0;

            lst_itemView.Items.Clear();
            imageList1.Images.Clear();

            foreach (var playlistItem in _playlist.ItemList)
            {
                var listViewItem = new ListViewItem(playlistItem.Title);
                listViewItem.Tag = playlistItem; //ablage für die Referenz auf das original
                listViewItem.ImageIndex = index;

                lst_itemView.Items.Add(listViewItem);

                var image = playlistItem.Thumbnail == null ? Resource1.noImageAvailable : playlistItem.Thumbnail;

                imageList1.Images.Add(image);



                index++;

            }

            lst_itemView.LargeImageList = imageList1;
        }

        private void UpdatePlaylistItemView(PlaylistList playlistList)
        {
            int index = 0;

            lst_itemView.Items.Clear();
            imageList1.Images.Clear();

            foreach (var playlistItem in playlistList.Playlists)
            {
                var listViewItem = new ListViewItem(playlistItem.Name);
                listViewItem.Tag = playlistItem; //ablage für die Referenz auf das original
                listViewItem.ImageIndex = index;

                lst_itemView.Items.Add(listViewItem);

                var image =  Resource1.noImageAvailable;
                imageList1.Images.Add(image);

                index++;

            }

            lst_itemView.LargeImageList = imageList1;
        }

        private void UpdatePlaylistInfoView()
        {
            lbl_playlistInfo.Text = $"{_playlist.Name} [{_playlist.Duration.ToString(@"hh\:mm\:ss")}] @ {_playlist.Author}";
        }

        private void InitFileDialog(FileDialog fileDialog, IEnumerable<IFileDescription> availableTypes, string title, string dafaultFileName)
        {
            fileDialog.Title = title;
            fileDialog.FileName = dafaultFileName;
            fileDialog.Filter = CreateFilterText(availableTypes);
        }

        private string CreateFilterText(IEnumerable<IFileDescription> availableTypes)
        {
            string filter = string.Empty;

            string allTypesFilter = "All Types | ";
            foreach (var type in availableTypes)
            {
                filter += $"{type.Description}|*{type.Extension}|";
                allTypesFilter += $"*{type.Extension};";


            }
            filter += allTypesFilter;
            filter = filter.Substring(0, filter.Length - 1);

            return filter;
        }

        private void UpdateItemDetailsView(IPlaylistItem playlistitem)
        {
            //Artist: Peter Coolplayer
            //Path: c:\myMusic\firstSong.mp3
            //Duration: 00:03:25
            if (playlistitem != null)
            {
                lbl_itemDetailinfo.Text = $"Artist: {playlistitem.Artist} - {playlistitem.Title}\n" +
                                $"Path: {playlistitem.Path}\n" +
                                $"Duration: {playlistitem.Duration.ToString(@"hh\:mm\:ss")}";
            }
            else
            {
                lbl_itemDetailinfo.Text = string.Empty ;
            }
            
        }

        private void EnableEditMenuItems(bool isEnable)
        {
            saveToolStripMenuItem.Enabled = isEnable;
            itemsToolStripMenuItem.Enabled = isEnable;
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_newPlaylistDataProvider.StartDialog() == DialogResult.Cancel) { return; }

            _playlist = _playlistFactory.Create(_newPlaylistDataProvider.Title, _newPlaylistDataProvider.Author, DateTime.Now);

            EnableEditMenuItems(true);
            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();


        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            InitFileDialog(openFileDialog1, _playlistItemFactory.AvailableTypes, "select new item(s)", "");
           

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }

            foreach (var itemPath in openFileDialog1.FileNames)
            {
                var item = _playlistItemFactory.Create(itemPath);

                if (item == null)
                {
                    continue;
                }

                _playlist.Add(item);
            }



            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _playlist.Clear();

            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitFileDialog(saveFileDialog1, _repositoryFactory.AvailableTypes, "Playlist speichern", _playlist.Name);
            

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }



            var repository = _repositoryFactory.Create(saveFileDialog1.FileName);

            if (repository == null)
            {
                MessageBox.Show("Fileformat kann nicht gespeichtert werden!!");

                return;
            }

            repository.Save(_playlist, saveFileDialog1.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitFileDialog(openFileDialog1, _repositoryFactory.AvailableTypes, "Select Playlist", "");
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }

            try
            {
                
                var repository = _repositoryFactory.Create(openFileDialog1.FileName);
                if (repository != null)
                {
                    _playlist = repository.Load(openFileDialog1.FileName);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Da ist was schief gelaufen");
            }
            

            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
            EnableEditMenuItems(true);


        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if( lst_itemView.SelectedItems == null || lst_itemView.SelectedItems.Count == 0) { return; }

            foreach (ListViewItem item in lst_itemView.SelectedItems)
            {
                if (item.Tag is IPlaylistItem playlistItem)
                {
                    _playlist.Remove(playlistItem);
                }                
            }

            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
        }

        private void lst_itemView_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void lst_itemView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Tag is IPlaylistItem playlistitem)
            {
                UpdateItemDetailsView(e.IsSelected ? playlistitem : null);
            }
        }

        private HttpClient _client;
        private HttpClient InitClient()
        {            
            if (_client == null)
            {
                HttpClient client= new HttpClient();
                client.BaseAddress = new Uri("http://localhost:49153/playlistapi/v1/"); // gehört in eine Settings Datei
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                return client;
            }
            return _client;
        }

        private async void getAllPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _client = InitClient();

            HttpResponseMessage response = await _client.GetAsync("playlists");
            if (response.IsSuccessStatusCode)
            {
                var playlistsJson = await response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<PlaylistList>(playlistsJson);

                UpdatePlaylistItemView(playlists);
            }



        }

        private async void lst_itemView_DoubleClick(object sender, EventArgs e)
        {
            if(lst_itemView.SelectedItems.Count != 1) 
            {
                return;
            }

            if (lst_itemView.SelectedItems[0].Tag is PlaylistInfo playlistInfo)
            {
                _playlist = await GetPlaylistFromServer(playlistInfo.Id);
                
                UpdatePlaylistItemView();
                UpdatePlaylistInfoView();
            }
        }

        private async Task<IPlaylist> GetPlaylistFromServer(string id)
        {
            _client = InitClient();

            HttpResponseMessage response = await _client.GetAsync($"playlists/{id}");
            if (response.IsSuccessStatusCode)
            {
                var playlistJson = await response.Content.ReadAsStringAsync();
                var playlistEntity = JsonConvert.DeserializeObject<RestModels.Playlist>(playlistJson);

                IPlaylist playlist = ConvertToDomain(playlistEntity);

                return playlist;
            }
            return null;
        }

        private IPlaylist ConvertToDomain(RestModels.Playlist playlistEntity)
        {
            if (playlistEntity == null) { return null; }

            var playlist = _playlistFactory.Create(playlistEntity.Name, playlistEntity.Autor, playlistEntity.DateOfCreation);
            
            foreach (var item in playlistEntity.Items)
            {
                var domainItem = new DummyItem(TimeSpan.FromSeconds((double)item.Duration), item.Path)
                {
                    Artist = item.Artist,
                    Title = item.Title,
                    Thumbnail = item.Thumbnail != null ? Image.FromStream(new MemoryStream(item.Thumbnail)) : Resource1.noImageAvailable,
                };

                playlist.Add(domainItem);
            }
            return playlist;
        }
    }
}
