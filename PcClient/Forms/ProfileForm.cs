using PcClient.Controls;
using PcClient.DataSources;
using PcClient.Events;
using User = PcClient.Models.User;

namespace PcClient.Forms
{
    public partial class ProfileForm : Form
    {
        private IDataSource _dataSource;
        private User _user;
        private bool _isChangeFormDisplayed;
        private long _userFileSize;

        public event Action<UserEventArgs> UserChanged;

        public ProfileForm(IDataSource dataSource, User user, long userFileSize)
        {
            _dataSource = dataSource;
            _user = user;
            _userFileSize = userFileSize;
            InitializeComponent();
            initializeLayout();
        }

        private void initializeLayout()
        {
            ProfileControl profileControl = new ProfileControl(_user, _userFileSize);
            addProfileControl(profileControl);
        }

        private void addProfileControl(ProfileControl control)
        {
            control.Dock = DockStyle.Fill;
            control.ProfileControlClick += profileClickHandler;
            tlpMain.Controls.Add(control, 0, 0);
            control.Show();
        }

        private void addProfileChangeControl(ProfileChangeControl control)
        {
            control.Dock = DockStyle.Fill;
            control.UserChanged += userChangedHandler;
            tlpMain.Controls.Add(control, 0, 1);
            control.Show();
        }

        private void profileClickHandler(object sender, EventArgs e)
        {
            if (_isChangeFormDisplayed)
            {
                tlpMain.Controls.RemoveAt(1);
                _isChangeFormDisplayed = false;
                return;
            }

            _isChangeFormDisplayed = true;
            ProfileChangeControl profileControl = new ProfileChangeControl(_dataSource, _user);
            addProfileChangeControl(profileControl);
        }

        private async void userChangedHandler(object sender, EventArgs e)
        {
            _user = await _dataSource.GetUser(_user.Id.ToString());
            UserChanged?.Invoke(new UserEventArgs() { User = _user });
        }
    }
}
