using System;
using System.Data;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Models;
using InventorySystem.Services;

namespace InventorySystem.Forms.Master
{
    public partial class FrmWarehouses : Form
    {
        private int _currentId = 0;

        public FrmWarehouses()
        {
            InitializeComponent();
        }

        private void FrmWarehouses_Load(object sender, EventArgs e)
        {
            LoadGrid();
            SetMode(false);
        }

        private void LoadGrid()
        {
            try
            {
                dgvWarehouses.DataSource = WarehouseService.Search(txtSearch.Text.Trim());
                FormatGrid();
                lblCount.Text = dgvWarehouses.Rows.Count + " record(s) found";
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Load warehouses"); }
        }

        private void FormatGrid()
        {
            if (dgvWarehouses.Columns.Count == 0) return;
            dgvWarehouses.Columns["WarehouseID"].Visible = false;
            dgvWarehouses.Columns["WarehouseCode"].HeaderText = "Code";
            dgvWarehouses.Columns["WarehouseName"].HeaderText = "Warehouse Name";
            dgvWarehouses.Columns["Location"].HeaderText = "Location";
            dgvWarehouses.Columns["IsActive"].HeaderText = "Active";
            dgvWarehouses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

        private void dgvWarehouses_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWarehouses.CurrentRow == null) return;
            var r = dgvWarehouses.CurrentRow;
            _currentId = Convert.ToInt32(r.Cells["WarehouseID"].Value);
            txtWarehouseCode.Text = r.Cells["WarehouseCode"].Value?.ToString();
            txtWarehouseName.Text = r.Cells["WarehouseName"].Value?.ToString();
            txtLocation.Text = r.Cells["Location"].Value?.ToString();
            chkActive.Checked = Convert.ToBoolean(r.Cells["IsActive"].Value);
            SetMode(false);
        }

        private bool ValidateForm()
        {
            bool ok = Validator.Required(txtWarehouseCode, ep, "Warehouse code");
            ok &= Validator.Required(txtWarehouseName, ep, "Warehouse name");

            if (ok)
            {
                int dup = Convert.ToInt32(DbHelper.Scalar(
                    "SELECT COUNT(*) FROM Warehouses WHERE WarehouseCode=@c AND WarehouseID<>@id",
                    CommandType.Text, DbHelper.P("@c", txtWarehouseCode.Text.Trim()), DbHelper.P("@id", _currentId)));
                if (dup > 0) { ep.SetError(txtWarehouseCode, "This warehouse code already exists."); ok = false; }
            }
            return ok;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                var w = new Warehouse
                {
                    WarehouseID = _currentId,
                    WarehouseCode = txtWarehouseCode.Text.Trim().ToUpper(),
                    WarehouseName = txtWarehouseName.Text.Trim(),
                    Location = txtLocation.Text.Trim(),
                    IsActive = chkActive.Checked
                };
                if (_currentId == 0) WarehouseService.Insert(w); else WarehouseService.Update(w);

                MessageBox.Show("Warehouse saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm(); SetMode(false);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save warehouse"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) { MessageBox.Show("Select a warehouse first."); return; }
            if (MessageBox.Show("Delete this warehouse?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                string msg = WarehouseService.Delete(_currentId);
                MessageBox.Show(msg, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Delete warehouse"); }
        }

        private void SetMode(bool editing)
        {
            pnlEntry.Enabled = editing || _currentId == 0;
            btnSave.Enabled = editing;
            btnDelete.Enabled = !editing && _currentId > 0;
        }

        private void btnNew_Click(object sender, EventArgs e) { ClearForm(); SetMode(true); txtWarehouseCode.Focus(); }
        private void btnCancel_Click(object sender, EventArgs e) { ClearForm(); SetMode(false); }

        private void ClearForm()
        {
            _currentId = 0;
            txtWarehouseCode.Clear();
            txtWarehouseName.Clear();
            txtLocation.Clear();
            chkActive.Checked = true;
            ep.Clear();
        }
    }
}