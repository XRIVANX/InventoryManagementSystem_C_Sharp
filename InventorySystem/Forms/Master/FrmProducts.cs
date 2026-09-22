using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Services;
using InventorySystem.Models;

namespace InventorySystem.Forms.Master
{
    public partial class FrmProducts : Form
    {
        private int _currentId = 0;

        public FrmProducts()
        {
            InitializeComponent();
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            LoadCombos();
            LoadGrid();
            SetMode(false);
        }

        private void LoadCombos()
        {
            BindCombo(cboCategory, "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive=1 ORDER BY CategoryName",
                      "CategoryName", "CategoryID");
            BindCombo(cboUom, "SELECT UomID, UomCode + ' - ' + UomName AS Display FROM UnitOfMeasure ORDER BY UomCode",
                      "Display", "UomID");
            BindCombo(cboSupplier, "SELECT SupplierID, SupplierName FROM Suppliers WHERE IsActive=1 ORDER BY SupplierName",
                      "SupplierName", "SupplierID");

            var dt = DbHelper.GetData("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName");
            var row = dt.NewRow(); row["CategoryID"] = DBNull.Value; row["CategoryName"] = "-- All Categories --";
            dt.Rows.InsertAt(row, 0);
            cboCategoryFilter.DataSource = dt;
            cboCategoryFilter.DisplayMember = "CategoryName";
            cboCategoryFilter.ValueMember = "CategoryID";

            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.AddRange(new object[] { "All", "Active only", "Inactive only" });
            cboStatusFilter.SelectedIndex = 1;
        }

        private void BindCombo(ComboBox cb, string sql, string display, string value)
        {
            cb.DataSource = DbHelper.GetData(sql);
            cb.DisplayMember = display;
            cb.ValueMember = value;
            cb.SelectedIndex = -1;
        }

        private void LoadGrid()
        {
            try
            {
                int? cat = cboCategoryFilter.SelectedValue as int?;
                bool? act = cboStatusFilter.SelectedIndex == 1 ? true
                          : cboStatusFilter.SelectedIndex == 2 ? false : (bool?)null;

                dgvProducts.DataSource = ProductService.Search(txtSearch.Text.Trim(), cat, act);
                FormatGrid();
                lblCount.Text = dgvProducts.Rows.Count + " record(s) found";
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Load products"); }
        }

        private void FormatGrid()
        {
            if (dgvProducts.Columns.Count == 0) return;

            foreach (DataGridViewColumn c in dgvProducts.Columns) c.Visible = false;
            string[] show = { "SKU", "ProductName", "CategoryName", "UomCode", "OnHand",
                              "ReorderLevel", "SellingPrice", "IsActive" };
            string[] head = { "SKU", "Product Name", "Category", "UOM", "On Hand",
                              "Reorder Lvl", "Price", "Active" };
            for (int i = 0; i < show.Length; i++)
            {
                if (!dgvProducts.Columns.Contains(show[i])) continue;
                dgvProducts.Columns[show[i]].Visible = true;
                dgvProducts.Columns[show[i]].HeaderText = head[i];
                dgvProducts.Columns[show[i]].DisplayIndex = i;
            }
            dgvProducts.Columns["SellingPrice"].DefaultCellStyle.Format = "N2";
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewRow r in dgvProducts.Rows)
            {
                decimal onHand = Convert.ToDecimal(r.Cells["OnHand"].Value);
                decimal level = Convert.ToDecimal(r.Cells["ReorderLevel"].Value);
                if (onHand <= level) r.DefaultCellStyle.BackColor = Color.MistyRose;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();
        private void cboCategoryFilter_SelectedIndexChanged(object sender, EventArgs e) => LoadGrid();
        private void cboStatusFilter_SelectedIndexChanged(object sender, EventArgs e) => LoadGrid();

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            var r = dgvProducts.CurrentRow;
            _currentId = Convert.ToInt32(r.Cells["ProductID"].Value);
            txtSKU.Text = r.Cells["SKU"].Value.ToString();
            txtBarcode.Text = r.Cells["Barcode"].Value?.ToString();
            txtName.Text = r.Cells["ProductName"].Value.ToString();
            txtDescription.Text = r.Cells["Description"].Value?.ToString();
            cboCategory.SelectedValue = r.Cells["CategoryID"].Value;
            cboUom.SelectedValue = r.Cells["UomID"].Value;
            cboSupplier.SelectedValue = r.Cells["SupplierID"].Value ?? DBNull.Value;
            txtReorderLevel.Text = r.Cells["ReorderLevel"].Value.ToString();
            txtReorderQty.Text = r.Cells["ReorderQty"].Value.ToString();
            txtSellingPrice.Text = r.Cells["SellingPrice"].Value.ToString();
            chkBatchTracked.Checked = Convert.ToBoolean(r.Cells["IsBatchTracked"].Value);
            chkActive.Checked = Convert.ToBoolean(r.Cells["IsActive"].Value);
            SetMode(false);
        }

        private bool ValidateForm()
        {
            decimal rl, rq, price;
            bool ok = Validator.Required(txtSKU, ep, "SKU");
            ok &= Validator.Required(txtName, ep, "Product name");
            ok &= Validator.ComboSelected(cboCategory, ep, "category");
            ok &= Validator.ComboSelected(cboUom, ep, "unit of measure");
            ok &= Validator.PositiveNumber(txtReorderLevel, ep, "Reorder level", out rl, true);
            ok &= Validator.PositiveNumber(txtReorderQty, ep, "Reorder quantity", out rq, true);
            ok &= Validator.PositiveNumber(txtSellingPrice, ep, "Selling price", out price, true);

            if (ok)
            {
                int dup = Convert.ToInt32(DbHelper.Scalar(
                    "SELECT COUNT(*) FROM Products WHERE SKU=@s AND ProductID<>@id",
                    CommandType.Text, DbHelper.P("@s", txtSKU.Text.Trim()), DbHelper.P("@id", _currentId)));
                if (dup > 0) { ep.SetError(txtSKU, "This SKU is already used."); ok = false; }
            }
            return ok;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                var p = new Product
                {
                    ProductID = _currentId,
                    SKU = txtSKU.Text.Trim().ToUpper(),
                    Barcode = txtBarcode.Text.Trim(),
                    ProductName = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    CategoryID = (int)cboCategory.SelectedValue,
                    UomID = (int)cboUom.SelectedValue,
                    SupplierID = cboSupplier.SelectedValue as int?,
                    ReorderLevel = decimal.Parse(txtReorderLevel.Text),
                    ReorderQty = decimal.Parse(txtReorderQty.Text),
                    SellingPrice = decimal.Parse(txtSellingPrice.Text),
                    IsBatchTracked = chkBatchTracked.Checked,
                    IsActive = chkActive.Checked
                };

                if (_currentId == 0) ProductService.Insert(p); else ProductService.Update(p);

                MessageBox.Show("Product saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm(); SetMode(false);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save product"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) { MessageBox.Show("Select a product first."); return; }
            if (MessageBox.Show("Delete this product?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                string msg = ProductService.Delete(_currentId);
                MessageBox.Show(msg, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(); ClearForm();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Delete product"); }
        }

        private void SetMode(bool editing)
        {
            pnlEntry.Enabled = editing || _currentId == 0;
            btnSave.Enabled = editing;
            btnDelete.Enabled = !editing && _currentId > 0;
        }

        private void btnNew_Click(object sender, EventArgs e) { ClearForm(); SetMode(true); txtSKU.Focus(); }
        private void btnCancel_Click(object sender, EventArgs e) { ClearForm(); SetMode(false); }

        private void ClearForm()
        {
            _currentId = 0;
            foreach (Control c in pnlEntry.Controls)
            {
                if (c is TextBox t) t.Clear();
                if (c is ComboBox cb) cb.SelectedIndex = -1;
            }
            chkActive.Checked = true;
            chkBatchTracked.Checked = false;
            ep.Clear();
        }
    }
}