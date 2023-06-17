using PcClient.Controls.AuthControls;
using PcClient.DataSources;
using PcClient.Events;

namespace PcClient.Forms
{
    public partial class AuthForm : Form
    {
        private IDataSource _datasource;

        public AuthForm(IDataSource dataSource)
        {
            _datasource = dataSource;
            InitializeComponent();
            setAuthControl();
        }

        private void RegisterHandler(object source, EventArgs e)
        {
            RegisterControl registerControl = new RegisterControl(_datasource);
            registerControl.BackToAuthHandler += AuthHandler;
            setControl(registerControl);
        }

        private void AuthHandler(object source, EventArgs e)
        {
            setAuthControl();
        }

        private void SuccessAuthHandler(AuthEventArgs e)
        {
            MainForm mainForm = new MainForm(_datasource, e.User);
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
            this.Hide();
        }

        private void setAuthControl()
        {
            AuthControl authControl = new AuthControl(_datasource);
            authControl.SuccessAuthEvent += SuccessAuthHandler;
            authControl.RegisterEvent += RegisterHandler;
            setControl(authControl);
        }

        private void setControl(Control control)
        {
            control.Dock = DockStyle.Fill;
            pAuth.Controls.Clear();
            pAuth.Controls.Add(control);
            control.Show();
        }
    }
}
