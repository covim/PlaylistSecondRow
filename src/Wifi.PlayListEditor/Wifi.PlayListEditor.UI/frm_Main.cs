using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wifi.PlayList.Editor.Factories;
using Wifi.PlayListEditor.Types;
using Wifi.PlayListEditor.UI.Properties;

namespace Wifi.PlayListEditor.UI
{
    public partial class frm_Main : Form
    {
        private INewPlaylistDataProvider _newPlaylistDataProvider;
        private IPlaylist _playlist;

        private IPlaylistFactory _playlistFactory;
        private IPlaylistItemFactory _playlistItemFactory;
        private IRepositoryFactory _repositoryFactory;
        public frm_Main()
        {
            InitializeComponent();
            _newPlaylistDataProvider = new frm_NewPlaylist();
            _playlistFactory = new PlaylistFactory();
            _playlistItemFactory = new PlaylistItemFactory();
            _repositoryFactory = new RepositoryFactory(_playlistItemFactory, _playlistFactory);
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            lbl_itemDetailinfo.Text = string.Empty;
            lbl_playlistInfo.Text = string.Empty;

            EnableEditMenuItems(false);
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

            _playlist = _playlistFactory.Create(_newPlaylistDataProvider.Author, _newPlaylistDataProvider.Title, DateTime.Now);

            EnableEditMenuItems(true);
            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();


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

        private void UpdatePlaylistInfoView()
        {
            lbl_playlistInfo.Text = $"{_playlist.Name} [{_playlist.Duration.ToString(@"hh\:mm\:ss")}] @ {_playlist.Author}";
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = CreateFilterText(_playlistItemFactory.AvailableTypes);

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

        private string CreateFilterText(IEnumerable<IFileDescription> availableTypes)
        {
            string filter = string.Empty;
            foreach (var type in availableTypes)
            {
                filter += $"{type.Description}|*{type.Extension}|";


            }
            filter = filter.Substring(0, filter.Length - 1);

            return filter;
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _playlist.Clear();

            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = CreateFilterText(_repositoryFactory.AvailableTypes);

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
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }

            try
            {
                var fileName = openFileDialog1.FileName;
                var repository = _repositoryFactory.Create(fileName);
                if (_playlist != null) { _playlist.Clear(); }
                _playlist = repository.Load(fileName);
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
           if( lst_itemView.SelectedItems == null) { return; }

            foreach (ListViewItem item in lst_itemView.SelectedItems)
            {
                var playlistItem = item.Tag;
                _playlist.Remove((IPlaylistItem)playlistItem);

            }

            UpdatePlaylistInfoView();
            UpdatePlaylistItemView();
        }
    }
}
