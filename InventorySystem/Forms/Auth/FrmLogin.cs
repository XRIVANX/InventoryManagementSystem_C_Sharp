using System;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

            // Wire up the hover glow events for btnLogin and btnExit
            btnLogin.MouseEnter += Button_MouseEnter;
            btnLogin.MouseLeave += Button_MouseLeave;

            btnExit.MouseEnter += Button_MouseEnter;
            btnExit.MouseLeave += Button_MouseLeave;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            // Clear any default error provider icons
            ep.Clear();

            // Position error icons INSIDE the right edge of the textboxes
            ep.SetIconAlignment(txtUsername, ErrorIconAlignment.MiddleRight);
            ep.SetIconPadding(txtUsername, -22);

            ep.SetIconAlignment(txtPassword, ErrorIconAlignment.MiddleRight);
            ep.SetIconPadding(txtPassword, -22);

            // 1. Center the card panel on load
            CenterCardPanel();

            // 2. Test DB Connection & Ensure Admin
            string msg;
            if (!DbHelper.TestConnection(out msg))
            {
                MessageBox.Show("Cannot connect to the database.\n\n" + msg,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = false;
                return;
            }

            AuthService.EnsureDefaultAdmin();
            txtUsername.Focus();
        }

        private void FrmLogin_Resize(object sender, EventArgs e)
        {
            // Keep the card centered on window resize
            CenterCardPanel();
        }

        private void CenterCardPanel()
        {
            if (LgnCardPanel != null)
            {
                LgnCardPanel.Left = (this.ClientSize.Width - LgnCardPanel.Width) / 2;
                LgnCardPanel.Top = (this.ClientSize.Height - LgnCardPanel.Height) / 2;
            }
        }

        // --- HOVER GLOW EVENT HANDLERS ---
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                // Glowing bright green background with white text when hovered
                btn.BackColor = System.Drawing.Color.FromArgb(32, 201, 103);
                btn.ForeColor = System.Drawing.Color.White;
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                // Reverts to transparent/no color with dark text when unhovered
                btn.BackColor = System.Drawing.Color.Transparent;
                btn.ForeColor = System.Drawing.Color.Black;
            }
        }
        

        private void chkShow_CheckedChanged(object sender, EventArgs e)
            => txtPassword.PasswordChar = chkShow.Checked ? '\0' : '•';

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool ok = Validator.Required(txtUsername, ep, "Username");
            ok &= Validator.Required(txtPassword, ep, "Password");
            if (!ok) return;

            Cursor = Cursors.WaitCursor;
            btnLogin.Enabled = false;

            try
            {
                var result = AuthService.Login(txtUsername.Text.Trim(), txtPassword.Text);
                if (result.Success)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Firebrick;
                    lblStatus.Text = result.Message;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Login");
            }
            finally
            {
                Cursor = Cursors.Default;
                btnLogin.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();
    }
}