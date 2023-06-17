using PcClient.DataSources;
using Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcClient.Forms
{
    public partial class PublicLinkForm : Form
    {
        private PublicLink _publicLink;
        private int _contentID;
        private bool _isFile;
        private string _downloadCount;
        private IDataSource _dataSource;

        public PublicLinkForm(IDataSource dataSource, PublicLink publicLink)
        {
            _dataSource = dataSource;
            _publicLink = publicLink;
            _contentID = publicLink.ContentID;
            _isFile = publicLink.IsFile;
            InitializeComponent();
            if (publicLink != null)
            {
                printInformation();
            }
        }

        public PublicLinkForm(IDataSource dataSource, int contendID, bool isFile)
        {
            _dataSource = dataSource;
            _contentID = contendID;
            _isFile = isFile;
            InitializeComponent();
        }

        private void printInformation()
        {
            tbLink.Text = $"{Constants.PublicUrl}/{_publicLink.ID}";
            tbCount.Text = _publicLink.DownloadCount.ToString();
            tbCount.Enabled = false;
            bCreate.Visible = false;
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbLink.Text);
        }

        private async void bCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbCount.Text))
            {
                _downloadCount = tbCount.Text;
            }

            PublicLink link =  await _dataSource.CreateLink(_contentID.ToString(), _isFile, _downloadCount);
            _publicLink = link;
            printInformation();
        }

        private async void bDelete_Click(object sender, EventArgs e)
        {
            bool isSuccess = await _dataSource.DeleteLink(_contentID.ToString(), _isFile);
            if (isSuccess)
            {
                MessageBox.Show("Ссылка удалена");
                Close();
            }
        }
    }
}
