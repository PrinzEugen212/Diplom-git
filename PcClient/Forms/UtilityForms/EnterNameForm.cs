namespace PcClient.Forms
{
    public partial class EnterNameForm : Form
    {
        public Action<string> NameChosen;

        public EnterNameForm()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (tbName.Text.Length == 0)
            {
                MessageBox.Show("Заполните имя");
                return;
            }

            NameChosen?.Invoke(tbName.Text);
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
