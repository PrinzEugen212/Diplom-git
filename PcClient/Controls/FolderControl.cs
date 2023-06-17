using PcClient.DataSources;
using PcClient.Events;
using PcClient.Forms;
using PcClient.Utils;
using PcClient.Models;
using Server.Models;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.LinkLabel;

namespace PcClient.Controls
{
    public partial class FolderControl : UserControl
    {
        private IDataSource _dataSource;
        private Folder _folder;
        private PublicLink _link;
        private string _folderName;

        public event Action<FileNavigationEventArgs> FolderClick;
        public event EventHandler DeleteMe;
        public event EventHandler RefreshLinks;

        public FolderControl(IDataSource dataSource, Folder folder)
        {
            _dataSource = dataSource;
            _folder = folder;
            InitializeComponent();
            DefaultFunctions df = new DefaultFunctions();
            df.Implement(this);
            initializeLayout();
            initializeContextMenu();
        }

        public Folder GetFolder()
        {
            return _folder;
        }

        private void initializeLayout()
        {
            this.tbName.Text = _folder.Name;
            this.lDate.Text = _folder.Modified;
            tbName.Enabled = false;
            foreach (var c in this.Controls)
            {
                if (c == tbName)
                {
                    continue;
                }

                ((Control)c).Click += FolderControl_Click;
            }

            isLinkExists();
        }

        private async void isLinkExists()
        {
            PublicLink link = await _dataSource.LinkExists(_folder.Id.ToString(), false);
            if (link is not null)
            {
                _link = link;
                pbPublic.Image = Properties.Resources.Green;
                return;
            }

            pbPublic.Image = Properties.Resources.Red;
        }

        private void initializeContextMenu()
        {
            ContextMenuStrip contextMenuStrip = GeneralUtils.GetContextMenu(FolderControl_CreatePublicLink);
            contextMenuStrip.Items.Add("Переименовать", null, tbName_Click);
            contextMenuStrip.Items.Add("Удалить", null, bDelete_Click);
            this.ContextMenuStrip = contextMenuStrip;
        }

        private async void FolderControl_CreatePublicLink(object sender, EventArgs e)
        {
            PublicLinkForm publicLinkForm;
            if (_link != null)
            {
                publicLinkForm = new PublicLinkForm(_dataSource, _link);
            }
            else
            {
                publicLinkForm = new PublicLinkForm(_dataSource, _folder.Id, false);
            }
            publicLinkForm.StartPosition = FormStartPosition.Manual;
            publicLinkForm.Location = Cursor.Position;
            publicLinkForm.ShowDialog();
            RefreshLinks?.Invoke(this, EventArgs.Empty);
        }

        private void FolderControl_Click(object sender, EventArgs e)
        {
            FolderClick.Invoke(new FileNavigationEventArgs { Folder = _folder });
        }

        private void tbName_Click(object sender, EventArgs e)
        {
            tbName.Enabled = true;
            tbName.Focus();
        }

        private async void bDelete_Click(object sender, EventArgs e)
        {
            if (await _dataSource.DeleteFolder(_folder.Id.ToString()))
            {
                DeleteMe?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
        }

        private async void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                return;
            }

            if (tbName.Text != _folderName)
            {
                await _dataSource.ChangeFolder(_folder.Id.ToString(), tbName.Text);
            }

            _folder.Name = tbName.Text;
            tbName.Enabled = false;
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                tbName.Enabled = false;
                return;
            }
        }
    }
}
