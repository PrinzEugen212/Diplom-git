using PcClient.DataSources;
using PcClient.Events;
using PcClient.Models;

namespace PcClient.Controls.AuthControls
{
    public partial class AuthControl : UserControl
    {
        private IDataSource _datasource;

        public event Action<AuthEventArgs> SuccessAuthEvent;
        public EventHandler RegisterEvent;

        public AuthControl(IDataSource dataSource)
        {
            InitializeComponent();
            _datasource = dataSource;
        }

        private async void bAuthorize_Click(object sender, EventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPasswod.Text;
            User user = await _datasource.Authorize(login, password);
            if (user is not null)
            {
                SuccessAuthEvent?.Invoke(new AuthEventArgs() { User = user });
            }
        }

        private void bRegister_Click(object sender, EventArgs e)
        {
            RegisterEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
