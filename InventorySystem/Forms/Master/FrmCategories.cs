using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Models;
using InventorySystem.Services;
using System;
using System.Windows.Forms;


namespace InventorySystem.Forms.Master
{
    public partial class FrmCategories : Form
    {
        private int _currentId = 0;

        public FrmCategories()
        {
            InitializeComponent();
        }

        private void FrmCategories_Load(object sender, EventArgs e)
        {
            LoadGrid();
            SetMode(false);
        }

        private void LoadGrid()
        {
            try
            {
                dgvCategories.DataSource = CategoryService.Search(txtSearch.Text.Trim());
                FormatGrid();
                lblCount.Text = dgvCategories.Rows.Count + " record(s) found";
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Load categories"); }
        }

        private void FormatGrid()
        {
            if (dgvCategories.Columns.Count == 0) return;
            dgvCategories.Columns["CategoryID"].Visible = false;
            dgvCategories.Columns["CategoryName"].HeaderText = "Category Name";
            dgvCategories.Columns["IsActive"].HeaderText = "Active";
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

        private void dgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow == null) return;
            var r = dgvCategories.CurrentRow;
            _currentId = Convert.ToInt32(r.Cells["CategoryID"].Value);
            txtCategoryName.Text = r.Cells["CategoryName"].Value.ToString();
            chkActive.Checked = Convert.ToBoolean(r.Cells["IsActive"].Value);
            SetMode(false);
        }

        private bool ValidateForm()
        {
            bool ok = Validator.Required(txtCategoryName, ep, "Category name");
            if (ok)
            {
                int dup = Convert.ToInt32(DbHelper.Scalar(
                    "SELECT COUNT(*) FROM Categories WHERE CategoryName=@n AND CategoryID<>@id",
                    System.Data.CommandType.Text,
                    DbHelper.P("@n", txtCategoryName.Text.Trim()),
                    DbHelper.P("@id", _currentId)));
                if (dup > 0) { ep.SetError(txtCategoryName, "This category name already exists."); ok = false; }
            }
            return ok;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                var c = new Category
                {
                    CategoryID = _currentId,
                    CategoryName = txtCategoryName.Text.Trim(),
                    IsActive = chkActive.Checked
                };
                if (_currentId == 0) CategoryService.Insert(c); else CategoryService.Update(c);

                MessageBox.Show("Category saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm(); SetMode(false);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save category"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) { MessageBox.Show("Select a category first."); return; }
            if (MessageBox.Show("Delete this category?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                string msg = CategoryService.Delete(_currentId);
                MessageBox.Show(msg, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Delete category"); }
        }

        private void SetMode(bool editing)
        {
            pnlEntry.Enabled = editing || _currentId == 0;
            btnSave.Enabled = editing;
            btnDelete.Enabled = !editing && _currentId > 0;
        }

        private void btnNew_Click(object sender, EventArgs e) { ClearForm(); SetMode(true); txtCategoryName.Focus(); }
        private void btnCancel_Click(object sender, EventArgs e) { ClearForm(); SetMode(false); }

        private void ClearForm()
        {
            _currentId = 0;
            txtCategoryName.Clear();
            chkActive.Checked = true;
            ep.Clear();
        }
    }
}

