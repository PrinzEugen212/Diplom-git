using PcClient.DataSources;
using PcClient.Forms;

namespace PcClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            IDataSource dataSource = new HttpDataSource();
            Application.Run(new AuthForm(dataSource));
        }
    }
}