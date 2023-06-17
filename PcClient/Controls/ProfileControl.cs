using PcClient.Utils;
using PcClient.Models;

namespace PcClient.Controls
{
    public partial class ProfileControl : UserControl
    {
        private User _user;
        private long _userFilesSize;

        public event EventHandler ProfileControlClick;

        public ProfileControl(User user, long userFilesSize)
        {
            _user = user;
            _userFilesSize = userFilesSize;
            InitializeComponent();
            DefaultFunctions df = new DefaultFunctions();
            df.Implement(this);
            initializeLayout();
        }

        public void ChangeUser(User user)
        {
            _user = user;
            this.lUserName.Text = _user.Login;
            this.lEmail.Text = _user.Email;
        }

        private void initializeLayout()
        {
            this.lUserName.Text = _user.Login;
            this.lEmail.Text = _user.Email;
            this.lFreeSize.Text = Utils.GeneralUtils.FileSizeToString(Constants.MaxFileSize - _userFilesSize);
            this.lFilesSize.Text = Utils.GeneralUtils.FileSizeToString(_userFilesSize);
            this.pbSizeComparsion.Maximum = 100;
            this.pbSizeComparsion.Value = (int)((double)_userFilesSize / Constants.MaxFileSize * 100);
            foreach (var c in this.Controls)
            {
                ((Control)c).Click += ProfileControl_Click;
            }
        }

        private void ProfileControl_Click(object sender, EventArgs e)
        {
            ProfileControlClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
