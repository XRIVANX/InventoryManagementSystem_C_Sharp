using InventorySystem.Forms;
using InventorySystem.Forms.Auth;
using InventorySystem.Helpers;
using System;
using System.Windows.Forms;

namespace InventorySystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += (s, e) => ErrorHandler.Handle(e.Exception, "UI thread");
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                ErrorHandler.Handle((Exception)e.ExceptionObject, "Background thread");

            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                    Application.Run(new FrmDashboard());
            }
        }
    }
}