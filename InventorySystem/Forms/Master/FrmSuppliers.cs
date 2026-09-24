using System;
using System.Data;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Models;
using InventorySystem.Services;

namespace InventorySystem.Forms.Master
{
    public partial class FrmSuppliers : Form
    {
        private int _currentId = 0;

        public FrmSuppliers()
        {
            InitializeComponent();
        }

        private void FrmSuppliers_Load(object sender, EventArgs e)
        {
            LoadGrid();
            SetMode(false);
        }

        private void LoadGrid()
        {
            try
            {
                dgvSuppliers.DataSource = SupplierService.Search(txtSearch.Text.Trim());
                FormatGrid();
                lblCount.Text = dgvSuppliers.Rows.Count + " record(s) found";
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Load suppliers"); }
        }

        private void FormatGrid()
        {
            if (dgvSuppliers.Columns.Count == 0) return;
            dgvSuppliers.Columns["SupplierID"].Visible = false;
            dgvSuppliers.Columns["SupplierCode"].HeaderText = "Code";
            dgvSuppliers.Columns["SupplierName"].HeaderText = "Supplier Name";
            dgvSuppliers.Columns["ContactPerson"].HeaderText = "Contact";
            dgvSuppliers.Columns["Phone"].HeaderText = "Phone";
            dgvSuppliers.Columns["Email"].HeaderText = "Email";
            dgvSuppliers.Columns["Address"].HeaderText = "Address";
            dgvSuppliers.Columns["IsActive"].HeaderText = "Active";
            dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

        private void dgvSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null) return;
            var r = dgvSuppliers.CurrentRow;
            _currentId = Convert.ToInt32(r.Cells["SupplierID"].Value);
            txtSupplierCode.Text = r.Cells["SupplierCode"].Value?.ToString();
            txtSupplierName.Text = r.Cells["SupplierName"].Value?.ToString();
            txtContactPerson.Text = r.Cells["ContactPerson"].Value?.ToString();
            txtPhone.Text = r.Cells["Phone"].Value?.ToString();
            txtEmail.Text = r.Cells["Email"].Value?.ToString();
            txtAddress.Text = r.Cells["Address"].Value?.ToString();
            chkActive.Checked = Convert.ToBoolean(r.Cells["IsActive"].Value);
            SetMode(false);
        }

        private bool ValidateForm()
        {
            bool ok = Validator.Required(txtSupplierCode, ep, "Supplier code");
            ok &= Validator.Required(txtSupplierName, ep, "Supplier name");
            ok &= Validator.Email(txtEmail, ep, true);

            if (ok)
            {
                int dup = Convert.ToInt32(DbHelper.Scalar(
                    "SELECT COUNT(*) FROM Suppliers WHERE SupplierCode=@c AND SupplierID<>@id",
                    CommandType.Text, DbHelper.P("@c", txtSupplierCode.Text.Trim()), DbHelper.P("@id", _currentId)));
                if (dup > 0) { ep.SetError(txtSupplierCode, "This supplier code already exists."); ok = false; }
            }
            return ok;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                var s = new Supplier
                {
                    SupplierID = _currentId,
                    SupplierCode = txtSupplierCode.Text.Trim().ToUpper(),
                    SupplierName = txtSupplierName.Text.Trim(),
                    ContactPerson = txtContactPerson.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    IsActive = chkActive.Checked
                };
                if (_currentId == 0) SupplierService.Insert(s); else SupplierService.Update(s);

                MessageBox.Show("Supplier saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm(); SetMode(false);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save supplier"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) { MessageBox.Show("Select a supplier first."); return; }
            if (MessageBox.Show("Delete this supplier?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                string msg = SupplierService.Delete(_currentId);
                MessageBox.Show(msg, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Delete supplier"); }
        }

        private void SetMode(bool editing)
        {
            pnlEntry.Enabled = editing || _currentId == 0;
            btnSave.Enabled = editing;
            btnDelete.Enabled = !editing && _currentId > 0;
        }

        private void btnNew_Click(object sender, EventArgs e) { ClearForm(); SetMode(true); txtSupplierCode.Focus(); }
        private void btnCancel_Click(object sender, EventArgs e) { ClearForm(); SetMode(false); }

        private void ClearForm()
        {
            _currentId = 0;
            txtSupplierCode.Clear();
            txtSupplierName.Clear();
            txtContactPerson.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            chkActive.Checked = true;
            ep.Clear();
        }
    }
}