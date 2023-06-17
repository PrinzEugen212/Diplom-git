using PcClient.DataSources;
using PcClient.Models;

namespace PcClient.Controls
{
    public partial class ProfileChangeControl : UserControl
    {
        private IDataSource _dataSource;
        private User _user;

        public event EventHandler UserChanged;

        public ProfileChangeControl(IDataSource dataSource, User user)
        {
            _dataSource = dataSource;
            _user = user;
            InitializeComponent();
            initializeLayout();
        }

        private void initializeLayout()
        {
            tbLogin.Text = _user.Login;
            tbEmail.Text = _user.Email;
        }

        private async void bChange_Click(object sender, EventArgs e)
        {
            User _previous = new User()
            {
                Id = _user.Id,
                Email = _user.Email,
                Login = _user.Login,
                Password = _user.Password,
            };

            if (!string.IsNullOrEmpty(tbLogin.Text))
            {
                _user.Login = tbLogin.Text;
            }


            if (!string.IsNullOrEmpty(tbEmail.Text))
            {
                _user.Email = tbEmail.Text;
            }

            if (!string.IsNullOrEmpty(tbPassword.Text))
            {
                if (tbPassword.Text == tbRepeatPassword.Text)
                {
                    _user.Password = tbPassword.Text;
                }
            }

            if (!_previous.Equals(_user))
            {
                if (!await _dataSource.ChangeUser(_user))
                {
                    MessageBox.Show("Ошибка");
                    return;
                }

                UserChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
