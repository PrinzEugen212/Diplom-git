using PcClient.DataSources;
using PcClient.DataSources.DataSourceUtils;
using PcClient.Forms;
using PcClient.Utils;
using Server.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using File = PcClient.Models.File;

namespace PcClient.Controls
{
    public partial class FileControl : UserControl
    {
        private IDataSource _dataSource;
        private File _file;
        private PublicLink _link;
        private string _downloadedFilePath;
        private string _filePath;
        private long _uploadingFileSize;
        private long _uploaded;

        public event EventHandler DeleteMe;
        public event EventHandler RefreshLinks;

        public FileControl(IDataSource dataSource, File file, string filePath, long fileSize = 0)
        {
            _file = file;
            _dataSource = dataSource;
            _uploadingFileSize = fileSize;
            _filePath = filePath;
            InitializeComponent();
            DefaultFunctions df = new DefaultFunctions();
            df.Implement(this);
            initializeLayout();
            initializeContextMenu();
        }

        public File GetFile()
        {
            return _file;
        }

        private void initializeLayout()
        {
            setFileName();
            this.lDate.Text = _file.Modified;
            this.lSize.Text = GeneralUtils.FileSizeToString(_file.Size);
            this.pProgress.BackColor = Color.FromArgb(60, 124, 252, 0);
            changeProgress(0);
            isLinkExists();
            foreach (var c in this.Controls)
            {

                ((Control)c).MouseEnter += FileControl_MouseEnter;
            }

        }

        private async void isLinkExists()
        {
            PublicLink link = await _dataSource.LinkExists(_file.Id.ToString(), true);
            if (link is not null)
            {
                _link = link;
                pbPublic.Image = Properties.Resources.Green;
                return;
            }

            pbPublic.Image = Properties.Resources.Red;
        }

        private void setFileName()
        {
            if (_file.Name.Length > Constants.MaxDisplayNameLength)
            {
                this.lName.Text = $"{_file.Name[..Constants.MaxDisplayNameLength]}... .{_file.Name.Split(".").Last()}";
            }
            else
            {
                this.lName.Text = _file.Name;
            }
        }

        private void initializeContextMenu()
        {
            ContextMenuStrip contextMenuStrip = GeneralUtils.GetContextMenu(FileControl_CreatePublicLink);
            contextMenuStrip.Items.Add("Скачать файл", null, bDownload_Click);
            contextMenuStrip.Items.Add("Изменить файл", null, bChange_Click);
            contextMenuStrip.Items.Add("Удалить файл", null, bDelete_Click);
            this.ContextMenuStrip = contextMenuStrip;
        }

        private void changeProgress(double? progressPercentage)
        {
            this.pProgress.Width = (int)(this.Width * (progressPercentage / 100));
            if (progressPercentage >= 100)
            {
                hideProgress();
                _uploaded = 0;
                _uploadingFileSize = 0;
                if (!string.IsNullOrEmpty(_downloadedFilePath))
                {
                    MessageBox.Show($"Файл сохранён в {_downloadedFilePath}");
                    _downloadedFilePath = string.Empty;
                }
            }
        }

        public void InvokeProgress(long bytesRead)
        {
            _uploaded += bytesRead;
            Invoke(new Action(() =>
            {
                changeProgress((double)_uploaded / _uploadingFileSize * 100);
            }));
        }

        private void hideProgress()
        {
            changeProgress(0);
        }

        private async void FileControl_CreatePublicLink(object sender, EventArgs e)
        {
            PublicLinkForm publicLinkForm;
            if (_link != null)
            {
                publicLinkForm = new PublicLinkForm(_dataSource, _link);
            }
            else
            {
                publicLinkForm = new PublicLinkForm(_dataSource, _file.Id, true);
            }
            publicLinkForm.StartPosition = FormStartPosition.Manual;
            publicLinkForm.Location = Cursor.Position;
            publicLinkForm.ShowDialog();
            RefreshLinks?.Invoke(this, EventArgs.Empty);
        }

        private async void bDownload_Click(object sender, EventArgs e)
        {
            _downloadedFilePath = GeneralUtils.GetUniquePath(GeneralUtils.GetDownloadsFolder(), _file.Name);
            _dataSource.DownloadFile(_file.Id.ToString(), _downloadedFilePath, changeProgress);
        }

        private async void bChange_Click(object sender, EventArgs e)
        {
            string fileName;
            string filePath;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    fileName = openFileDialog.SafeFileName;
                    _uploadingFileSize = new FileInfo(filePath).Length;
                }
                else
                {
                    return;
                }
            }

            if (await _dataSource.ChangeFile(_file.Id.ToString(), fileName, System.IO.File.OpenRead(filePath), InvokeProgress))
            {
                MessageBox.Show("Файл изменён");
                _file.Name = fileName;
                setFileName();
            }
        }

        private async void bDelete_Click(object sender, EventArgs e)
        {
            if (await _dataSource.DeleteFile(_file.Id.ToString()))
            {
                DeleteMe?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
        }

        private void FileControl_Resize(object sender, EventArgs e)
        {
            var newLocation = new Point(lName.Location.X + lName.Width + (int)(this.Width * 0.1), lName.Location.Y);
            bDownload.Location = newLocation;
            bDownload.Width = (int)(this.Width * 0.11);
        }

        private void FileControl_Click(object sender, EventArgs e)
        {
            bDownload.Visible = !bDownload.Visible;
            bDownload.Enabled = !bDownload.Enabled;
        }

        private void FileControl_MouseEnter(object sender, EventArgs e)
        {
            ttFile.Show(_filePath, this);
        }
    }
}
