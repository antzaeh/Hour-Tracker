using System;
using System.Windows.Forms;
using TuntiporttiUser;

namespace TuntiPorttiUser
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new loginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    string userId = loginForm.Tag as string;  // Retrieve user ID from Tag
                    if (!string.IsNullOrEmpty(userId))
                    {
                        Application.Run(new MainForm(userId));
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}