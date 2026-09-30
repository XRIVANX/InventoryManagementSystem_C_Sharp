using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;

namespace InventorySystem.Forms.Trans
{
    public partial class FrmPhysicalCount : Form
    {
        private int _countId = 0;
        private string _status = "NONE";   // NONE (not created yet), OPEN, POSTED

        public FrmPhysicalCount()
        {
            InitializeComponent();
        }

        private void FrmPhysicalCount_Load(object sender, EventArgs e)
        {
            LoadCombos();
            NewDocument();
        }

        private void LoadCombos()
        {
            cboWarehouse.DataSource = DbHelper.GetData(
                "SELECT WarehouseID, WarehouseName FROM Warehouses WHERE IsActive=1 ORDER BY WarehouseName");
            cboWarehouse.DisplayMember = "WarehouseName";
            cboWarehouse.ValueMember = "WarehouseID";
            cboWarehouse.SelectedIndex = -1;
        }

        private void NewDocument()
        {
            _countId = 0;
            _status = "NONE";
            lblCountNo.Text = "(auto-generated on save)";
            dtpCountDate.Value = DateTime.Now;
            txtRemarks.Clear();
            cboWarehouse.SelectedIndex = -1;
            dgvCount.DataSource = null;
            lblVariance.Text = "0 line(s) with variance";
            UpdateButtons();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cboWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Select a warehouse first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DbHelper.RunInTransaction((cn, tx) =>
                {
                    string docNo;
                    using (var cmd = new SqlCommand("sp_NextDocNo", cn, tx)
                    { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@SeriesCode", "PC");
                        var outp = cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 30);
                        outp.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        docNo = outp.Value.ToString();
                    }

                    using (var cmd = new SqlCommand(
                        @"INSERT INTO PhysicalCount (CountNo, WarehouseID, CountDate, Remarks, CreatedBy)
                          VALUES (@no,@wh,@dt,@rem,@u);
                          SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@no", docNo);
                        cmd.Parameters.AddWithValue("@wh", cboWarehouse.SelectedValue);
                        cmd.Parameters.AddWithValue("@dt", dtpCountDate.Value);
                        cmd.Parameters.AddWithValue("@rem", txtRemarks.Text.Trim());
                        cmd.Parameters.AddWithValue("@u", Session.UserID);
                        _countId = (int)cmd.ExecuteScalar();
                    }
                    lblCountNo.Text = docNo;
                });

                DbHelper.Execute("sp_GenerateCountSheet", CommandType.StoredProcedure,
                    DbHelper.P("@CountID", _countId));

                _status = "OPEN";
                LoadLines();
                UpdateButtons();
                MessageBox.Show("Count sheet " + lblCountNo.Text +
                    " generated with the current system quantities.\n\n" +
                    "Encode the counted quantities, then Save Counts, then Post.",
                    "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Generate count sheet"); }
        }

        private void LoadLines()
        {
            dgvCount.DataSource = DbHelper.GetData(
                @"SELECT l.CountLineID, p.SKU, p.ProductName, ISNULL(pb.BatchNo,'-') AS BatchNo,
                         l.SystemQty, l.CountedQty, l.Variance, l.Remarks
                  FROM PhysicalCountLine l
                  JOIN Products p ON p.ProductID = l.ProductID
                  LEFT JOIN ProductBatches pb ON pb.BatchID = l.BatchID
                  WHERE l.CountID = @id
                  ORDER BY p.ProductName;",
                CommandType.Text, DbHelper.P("@id", _countId));

            if (dgvCount.Columns.Count == 0) return;

            foreach (DataGridViewColumn c in dgvCount.Columns) c.ReadOnly = true;
            dgvCount.Columns["CountedQty"].ReadOnly = false;
            dgvCount.Columns["CountLineID"].Visible = false;
            dgvCount.Columns["CountedQty"].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
            dgvCount.Columns["SKU"].HeaderText = "SKU";
            dgvCount.Columns["ProductName"].HeaderText = "Product Name";
            dgvCount.Columns["BatchNo"].HeaderText = "Batch";
            dgvCount.Columns["SystemQty"].HeaderText = "System Qty";
            dgvCount.Columns["CountedQty"].HeaderText = "Counted Qty";
            dgvCount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            HighlightVariances();
        }

        private void HighlightVariances()
        {
            decimal totalVar = 0;
            int varLines = 0;
            foreach (DataGridViewRow r in dgvCount.Rows)
            {
                if (r.Cells["Variance"].Value == DBNull.Value || r.Cells["Variance"].Value == null) continue;
                decimal v = Convert.ToDecimal(r.Cells["Variance"].Value);
                if (v > 0) { r.DefaultCellStyle.BackColor = System.Drawing.Color.Honeydew; varLines++; }
                else if (v < 0) { r.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose; varLines++; }
                else r.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                totalVar += v;
            }
            lblVariance.Text = $"{varLines} line(s) with variance   |   Net variance: {totalVar:N2}";
        }

        private void dgvCount_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCount.Columns[e.ColumnIndex].Name != "CountedQty") return;
            string v = Convert.ToString(e.FormattedValue);
            if (string.IsNullOrWhiteSpace(v)) return;

            decimal d;
            if (!decimal.TryParse(v, out d) || d < 0)
            {
                dgvCount.Rows[e.RowIndex].ErrorText = "Counted quantity must be zero or a positive number.";
                e.Cancel = true;
            }
            else dgvCount.Rows[e.RowIndex].ErrorText = "";
        }

        private void btnSaveCounts_Click(object sender, EventArgs e)
        {
            if (_countId == 0) return;
            dgvCount.EndEdit();

            try
            {
                DbHelper.RunInTransaction((cn, tx) =>
                {
                    foreach (DataGridViewRow r in dgvCount.Rows)
                    {
                        if (r.IsNewRow) continue;
                        using (var cmd = new SqlCommand(
                            "UPDATE PhysicalCountLine SET CountedQty=@q WHERE CountLineID=@id;", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@q", r.Cells["CountedQty"].Value ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@id", r.Cells["CountLineID"].Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });
                LoadLines();
                MessageBox.Show("Counted quantities saved.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save counts"); }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (!Session.IsManager)
            {
                MessageBox.Show("Only a Manager or Administrator can post a physical count.",
                    "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (_countId == 0) return;

            foreach (DataGridViewRow r in dgvCount.Rows)
            {
                if (r.Cells["CountedQty"].Value == DBNull.Value || r.Cells["CountedQty"].Value == null)
                {
                    MessageBox.Show("Every line must have a counted quantity before posting.\n\n" +
                        "Fill in every row (or 0 if there is truly none), then Save Counts first.",
                        "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (MessageBox.Show(
                "Posting will adjust inventory to match the counted quantities.\nThis cannot be undone.\n\nContinue?",
                "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                DbHelper.Execute("sp_PostPhysicalCount", CommandType.StoredProcedure,
                    DbHelper.P("@CountID", _countId),
                    DbHelper.P("@UserID", Session.UserID));

                _status = "POSTED";
                LoadLines();
                UpdateButtons();
                MessageBox.Show("Physical count posted. Stock now matches the counted quantities.",
                    "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Post physical count"); }
        }

        private void btnNew_Click(object sender, EventArgs e) => NewDocument();

        private void UpdateButtons()
        {
            bool hasSheet = _countId > 0;
            bool editable = _status == "OPEN";
            pnlHeader.Enabled = _status == "NONE";
            btnGenerate.Enabled = _status == "NONE";
            btnSaveCounts.Enabled = editable;
            btnPost.Enabled = editable;
            dgvCount.Enabled = hasSheet;
            lblStatusBadge.Text = _status == "NONE" ? "NOT STARTED" : _status;
        }
    }
}