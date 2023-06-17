namespace PcClient.Controls
{
    public class DefaultFunctions
    {
        private Control _control;

        public void Implement(Control control)
        {
            _control = control;
            control.MouseEnter += DefaultControl_MouseEnter;
            control.MouseLeave += DefaultControl_MouseLeave;
            foreach (var c in control.Controls)
            {
                ((Control)c).MouseEnter += DefaultControl_MouseEnter;
                ((Control)c).MouseLeave += DefaultControl_MouseLeave;
            }
        }

        private void DefaultControl_MouseEnter(object sender, EventArgs e)
        {
            _control.Cursor = Cursors.Hand;
            _control.BackColor = SystemColors.Control;
        }

        private void DefaultControl_MouseLeave(object sender, EventArgs e)
        {
            _control.Cursor = Cursors.Default;
            _control.BackColor = SystemColors.Window;
        }
    }
}
