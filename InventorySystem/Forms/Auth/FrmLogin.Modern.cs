using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Helpers;

namespace InventorySystem.Forms.Auth
{
    public partial class FrmLogin
    {
        private void InitializeModernLogin()
        {
            SuspendLayout();
            BackgroundImage = null; BackColor = ModernTheme.Canvas;
            Text = "InventorySystem | Sign in"; StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 640); ClientSize = new Size(1040, 690);
            Font = ModernTheme.Font(10);
            LgnCardPanel.Size = new Size(820, 530); LgnCardPanel.BackColor = Color.White;
            LgnCardPanel.BackgroundImage = null; LgnCardPanel.BorderStyle = BorderStyle.None;
            var brand = new Panel { Dock = DockStyle.Left, Width = 335, BackColor = ModernTheme.Navy };
            var name = ModernTheme.Label("INVENTORY\nSYSTEM", 25, Color.White, true); name.Location = new Point(32, 44);
            var badge = ModernTheme.Label("OPERATIONS WORKSPACE", 8, Color.FromArgb(102, 211, 194), true); badge.Location = new Point(34, 144);
            var headline = ModernTheme.Label("Everything in stock.\nEverything in view.", 20, Color.White, true); headline.Location = new Point(32, 238);
            var description = ModernTheme.Label("One place to manage your products,\nstock movements, and daily operations.", 10, Color.FromArgb(178, 196, 216)); description.Location = new Point(34, 332);
            var footer = ModernTheme.Label("Clarity for every working day.", 9, Color.FromArgb(102, 211, 194)); footer.Location = new Point(34, 456);
            brand.Controls.AddRange(new Control[] { name, badge, headline, description, footer });
            LgnCardPanel.Controls.Add(brand);
            label3.Text = "Welcome back"; label3.AutoSize = true; label3.Font = ModernTheme.Font(24, true); label3.ForeColor = ModernTheme.Ink; label3.Location = new Point(377, 44);
            var intro = ModernTheme.Label("Sign in to your inventory workspace.", 10, ModernTheme.Muted); intro.Location = new Point(379, 94); LgnCardPanel.Controls.Add(intro);
            label1.Text = "Username"; label1.Font = ModernTheme.Font(9, true); label1.ForeColor = ModernTheme.Ink; label1.Location = new Point(380, 150);
            label2.Text = "Password"; label2.Font = ModernTheme.Font(9, true); label2.ForeColor = ModernTheme.Ink; label2.Location = new Point(380, 230);
            txtUsername.Multiline = false; txtUsername.Font = ModernTheme.Font(13); txtUsername.SetBounds(380, 177, 394, 34); txtUsername.TabIndex = 0;
            txtPassword.Multiline = false; txtPassword.Font = ModernTheme.Font(13); txtPassword.SetBounds(380, 257, 394, 34); txtPassword.TabIndex = 1; txtPassword.PasswordChar = '•';
            chkShow.Font = ModernTheme.Font(9); chkShow.ForeColor = ModernTheme.Muted; chkShow.Location = new Point(380, 307); chkShow.TabIndex = 2;
            lblStatus.AutoSize = false; lblStatus.SetBounds(380, 338, 394, 44); lblStatus.Font = ModernTheme.Font(9); lblStatus.ForeColor = Color.Firebrick;
            btnLogin.Text = "Sign in  →"; btnLogin.SetBounds(380, 393, 394, 46); btnLogin.TabIndex = 3; ModernTheme.Button(btnLogin, true);
            btnExit.Text = "Close application"; btnExit.SetBounds(380, 449, 394, 37); btnExit.TabIndex = 4; ModernTheme.Button(btnExit);
            AcceptButton = btnLogin; CancelButton = btnExit;
            CenterCardPanel(); ResumeLayout(true);
        }
    }
}
