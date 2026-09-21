using System;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        public FrmLogin() { InitializeComponent(); }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
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
            catch (Exception ex) { ErrorHandler.Handle(ex, "Login"); }
            finally { Cursor = Cursors.Default; btnLogin.Enabled = true; }
        }

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();
    }
}