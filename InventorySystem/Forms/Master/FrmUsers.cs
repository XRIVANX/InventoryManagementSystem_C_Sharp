using System;
using System.Data;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Models;
using InventorySystem.Services;

namespace InventorySystem.Forms.Master
{
    public partial class FrmUsers : Form
    {
        private int _currentId = 0;

        public FrmUsers()
        {
            InitializeComponent();
        }

        private void FrmUsers_Load(object sender, EventArgs e)
        {
            if (!Session.IsAdmin)
            {
                MessageBox.Show("Only an Administrator can manage users.", "Access denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
                return;
            }
            LoadRoles();
            LoadGrid();
            SetMode(false);
        }

        private void LoadRoles()
        {
            cboRole.DataSource = DbHelper.GetData("SELECT RoleID, RoleName FROM Roles ORDER BY RoleName");
            cboRole.DisplayMember = "RoleName";
            cboRole.ValueMember = "RoleID";
            cboRole.SelectedIndex = -1;
        }

        private void LoadGrid()
        {
            try
            {
                dgvUsers.DataSource = UserService.Search(txtSearch.Text.Trim());
                FormatGrid();
                lblCount.Text = dgvUsers.Rows.Count + " record(s) found";
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Load users"); }
        }

        private void FormatGrid()
        {
            if (dgvUsers.Columns.Count == 0) return;
            dgvUsers.Columns["UserID"].Visible = false;
            dgvUsers.Columns["RoleID"].Visible = false;
            dgvUsers.Columns["Username"].HeaderText = "Username";
            dgvUsers.Columns["FullName"].HeaderText = "Full Name";
            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["RoleName"].HeaderText = "Role";
            dgvUsers.Columns["IsActive"].HeaderText = "Active";
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var r = dgvUsers.CurrentRow;
            if (r.Cells["UserID"].Value == DBNull.Value) return;
            _currentId = Convert.ToInt32(r.Cells["UserID"].Value);
            txtUsername.Text = r.Cells["Username"].Value?.ToString();
            txtFullName.Text = r.Cells["FullName"].Value?.ToString();
            txtEmail.Text = r.Cells["Email"].Value?.ToString();
            cboRole.SelectedValue = r.Cells["RoleID"].Value;
            chkActive.Checked = Convert.ToBoolean(r.Cells["IsActive"].Value);
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtUsername.ReadOnly = true;   // username can't change once created
            SetMode(false);
        }

        private bool ValidateForm()
        {
            bool ok = Validator.Required(txtUsername, ep, "Username");
            ok &= Validator.Required(txtFullName, ep, "Full name");
            ok &= Validator.ComboSelected(cboRole, ep, "role");
            ok &= Validator.Email(txtEmail, ep, true);

            bool isNewUser = _currentId == 0;

            if (isNewUser)
            {
                ok &= Validator.Required(txtPassword, ep, "Password");
                ok &= Validator.Required(txtConfirmPassword, ep, "Confirm password");
            }

            if (txtPassword.Text.Length > 0 || txtConfirmPassword.Text.Length > 0)
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ep.SetError(txtConfirmPassword, "Passwords do not match.");
                    ok = false;
                }
                else if (txtPassword.Text.Length > 0 && txtPassword.Text.Length < 6)
                {
                    ep.SetError(txtPassword, "Password must be at least 6 characters.");
                    ok = false;
                }
            }

            if (ok && isNewUser)
            {
                int dup = Convert.ToInt32(DbHelper.Scalar(
                    "SELECT COUNT(*) FROM Users WHERE Username=@u",
                    CommandType.Text, DbHelper.P("@u", txtUsername.Text.Trim())));
                if (dup > 0) { ep.SetError(txtUsername, "This username is already taken."); ok = false; }
            }

            // safety: can't deactivate your own account
            if (ok && _currentId == Session.UserID && !chkActive.Checked)
            {
                ep.SetError(chkActive, "You cannot deactivate your own account.");
                ok = false;
            }

            return ok;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                if (_currentId == 0)
                {
                    AuthService.CreateUser(
                        txtUsername.Text.Trim(),
                        txtPassword.Text,
                        txtFullName.Text.Trim(),
                        txtEmail.Text.Trim(),
                        (int)cboRole.SelectedValue);
                }
                else
                {
                    var u = new User
                    {
                        UserID = _currentId,
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        RoleID = (int)cboRole.SelectedValue,
                        IsActive = chkActive.Checked
                    };
                    UserService.Update(u);

                    if (txtPassword.Text.Length > 0)
                        UserService.ResetPassword(_currentId, txtPassword.Text);
                }

                MessageBox.Show("User saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm(); SetMode(false);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save user"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) { MessageBox.Show("Select a user first."); return; }
            if (_currentId == Session.UserID)
            {
                MessageBox.Show("You cannot deactivate your own account while logged in.",
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Deactivate this user? They will no longer be able to log in.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                string msg = UserService.Deactivate(_currentId);
                MessageBox.Show(msg, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Deactivate user"); }
        }

        private void SetMode(bool editing)
        {
            pnlEntry.Enabled = editing || _currentId == 0;
            btnSave.Enabled = editing;
            btnDelete.Enabled = !editing && _currentId > 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtUsername.ReadOnly = false;
            SetMode(true);
            txtUsername.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e) { ClearForm(); SetMode(false); }

        private void ClearForm()
        {
            _currentId = 0;
            txtUsername.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            cboRole.SelectedIndex = -1;
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            chkActive.Checked = true;
            txtUsername.ReadOnly = false;
            ep.Clear();
        }
    }
}