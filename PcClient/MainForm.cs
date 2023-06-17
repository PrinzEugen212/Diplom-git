using PcClient.DataSources;
using PcClient.Events;
using PcClient.Forms;
using PcClient.Models;
using User = PcClient.Models.User;

namespace PcClient
{
    public partial class MainForm : Form
    {
        private IDataSource _dataSource;
        private User _user;

        public MainForm(IDataSource dataSource, User user)
        {
            _dataSource = dataSource;
            _user = user;
            InitializeComponent();
            initializeLayout();
        }

        private async void initializeLayout()
        {
            tlpMain.Controls.Clear();
            FileForm fileForm = new FileForm(_dataSource, _user.Id.ToString());
            openFileForm(fileForm);

            long filesSize = await fileForm.FillFormAsRoot();
            ProfileForm profileForm = new ProfileForm(_dataSource, _user, filesSize);
            openProfileForm(profileForm);
        }

        private void openFileForm(FileForm childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            tlpMain.Controls.Add(childForm, 1, 0);
            childForm.BringToFront();
            childForm.Show();
        }

        private void openProfileForm(ProfileForm form)
        {
            form.UserChanged += UserChangedHandler;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tlpMain.Controls.Add(form, 0, 0);
            form.BringToFront();
            form.Show();
        }

        private void UserChangedHandler(UserEventArgs e)
        {
            _user = e.User;
            initializeLayout();
        }
    }
}
