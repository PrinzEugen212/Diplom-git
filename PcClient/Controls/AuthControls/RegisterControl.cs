using PcClient.DataSources;
using PcClient.Models;

namespace PcClient.Controls.AuthControls
{
    public partial class RegisterControl : UserControl
    {
        private IDataSource _datasource;

        public EventHandler BackToAuthHandler;

        public RegisterControl(IDataSource dataSource)
        {
            _datasource = dataSource;
            InitializeComponent();
        }

        private async void bRegister_Click(object sender, EventArgs e)
        {

            if (tbPassword.Text != tbRepeatPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            if (await _datasource.LoginExists(tbLogin.Text))
            {
                MessageBox.Show("Данный логин уже существует");
                return;
            }

            if (await _datasource.EmailExists(tbEmail.Text))
            {
                MessageBox.Show("Данная почта уже существует");
                return;
            }

            User user = new User()
            {
                Email = tbEmail.Text,
                Login = tbLogin.Text,
                Password = tbPassword.Text,
            };
            if (await _datasource.Register(user))
            {
                MessageBox.Show("Пользователь создан");
                BackToAuthHandler.Invoke(this, EventArgs.Empty);
            }
        }

        private void bAuthorize_Click(object sender, EventArgs e)
        {
            BackToAuthHandler.Invoke(this, EventArgs.Empty);
        }
    }
}
