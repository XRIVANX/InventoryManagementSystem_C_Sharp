using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;

namespace InventorySystem.Forms.Trans
{
    public partial class FrmAdjustment : Form
    {
        private DataTable _lines;
        private readonly DraftChanges _draftChanges = new DraftChanges();
        private int _txnId = 0;
        private string _status = "NEW";

        public FrmAdjustment()
        {
            InitializeComponent();
            _draftChanges.Watch(pnlHeader);
            InventorySystem.Helpers.ModernTheme.Apply(this);
        }

        private void FrmAdjustment_Load(object sender, EventArgs e)
        {
            if (!Session.IsManager)
            {
                MessageBox.Show("Only a Manager or Administrator can create stock adjustments.",
                    "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
                return;
            }
            InitLinesTable();
            LoadCombos();
            NewDocument();
        }

        private void InitLinesTable()
        {
            _lines = new DataTable();
            _lines.Columns.Add("ProductID", typeof(int));
            _lines.Columns.Add("SKU", typeof(string));
            _lines.Columns.Add("ProductName", typeof(string));
            _lines.Columns.Add("BatchID", typeof(object));
            _lines.Columns.Add("BatchNo", typeof(string));
            _lines.Columns.Add("Quantity", typeof(decimal));   // signed: + increase, - decrease
            _lines.Columns.Add("UnitCost", typeof(decimal));
            _lines.Columns.Add("LineTotal", typeof(decimal), "Quantity * UnitCost");
            _draftChanges.Watch(_lines);
            dgvLines.DataSource = _lines;
        }

        private void LoadCombos()
        {
            cboAdjustType.Items.Clear();
            cboAdjustType.Items.AddRange(new object[] { "Increase", "Decrease" });
            cboAdjustType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAdjustType.SelectedIndex = -1;

            cboReason.Items.Clear();
            cboReason.Items.AddRange(new object[] { "Damaged", "Expired", "Lost", "Found", "Data-entry correction", "Sample" });
            cboReason.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReason.SelectedIndex = -1;

            cboWarehouse.DataSource = DbHelper.GetData(
                "SELECT WarehouseID, WarehouseName FROM Warehouses WHERE IsActive=1 ORDER BY WarehouseName");
            cboWarehouse.DisplayMember = "WarehouseName";
            cboWarehouse.ValueMember = "WarehouseID";
            cboWarehouse.SelectedIndex = -1;

            cboProduct.DataSource = DbHelper.GetData(
                "SELECT ProductID, SKU + ' - ' + ProductName AS Display FROM Products WHERE IsActive=1 ORDER BY ProductName");
            cboProduct.DisplayMember = "Display";
            cboProduct.ValueMember = "ProductID";
            cboProduct.SelectedIndex = -1;
        }

        private void NewDocument()
        {
            _txnId = 0; _status = "NEW";
            _lines.Rows.Clear();
            lblTxnNo.Text = "(auto-generated on save)";
            dtpDate.Value = DateTime.Now;
            txtReferenceNo.Clear(); txtRemarks.Clear();
            cboWarehouse.SelectedIndex = -1;
            cboReason.SelectedIndex = -1;
            ClearLineEntry();
            UpdateTotals();
            UpdateButtons();
            ep.Clear();
        }

        // ---------- helpers ----------

        private static int? ToNullableInt(object v)
            => (v == null || v == DBNull.Value) ? (int?)null : Convert.ToInt32(v);

        private static bool SameBatch(object a, object b) => ToNullableInt(a) == ToNullableInt(b);

        private decimal GetOnHand(int productId, object batchId, int warehouseId)
        {
            var v = DbHelper.Scalar(
                @"SELECT ISNULL(SUM(QtyOnHand),0) FROM StockBalance
                  WHERE ProductID=@p AND WarehouseID=@w AND (@b IS NULL OR BatchID=@b)",
                CommandType.Text,
                DbHelper.P("@p", productId),
                DbHelper.P("@w", warehouseId),
                DbHelper.P("@b", batchId));
            return Convert.ToDecimal(v);
        }

        private decimal GetCartNetChange(int productId, object batchId)
        {
            decimal total = 0;
            foreach (DataRow r in _lines.Rows)
                if ((int)r["ProductID"] == productId && SameBatch(r["BatchID"], batchId))
                    total += (decimal)r["Quantity"];   // already signed
            return total;
        }

        private object CurrentBatchId()
            => (cboBatch.Enabled && cboBatch.SelectedValue is int) ? (object)(int)cboBatch.SelectedValue : null;

        private void RefreshOnHand()
        {
            if (!(cboProduct.SelectedValue is int pid) || !(cboWarehouse.SelectedValue is int wh))
            {
                lblCurrentOnHand.Text = "On hand: -";
                return;
            }
            object bid = CurrentBatchId();
            decimal current = GetOnHand(pid, bid, wh) + GetCartNetChange(pid, bid);
            lblCurrentOnHand.Text = "On hand: " + current.ToString("N2");
        }

        // ---------- line entry ----------

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cboProduct.SelectedValue is int pid))
            {
                cboBatch.DataSource = null;
                cboBatch.Enabled = false;
                RefreshOnHand();
                return;
            }

            bool batched = Convert.ToBoolean(DbHelper.Scalar(
                "SELECT IsBatchTracked FROM Products WHERE ProductID=@id",
                CommandType.Text, DbHelper.P("@id", pid)));

            cboBatch.Enabled = batched;
            if (batched) LoadBatches(pid); else cboBatch.DataSource = null;

            var cost = DbHelper.Scalar("SELECT AverageCost FROM Products WHERE ProductID=@id",
                CommandType.Text, DbHelper.P("@id", pid));
            txtUnitCost.Text = Convert.ToDecimal(cost).ToString("0.00");

            RefreshOnHand();
        }

        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue is int pid && cboBatch.Enabled) LoadBatches(pid);
            RefreshOnHand();
        }

        private void cboBatch_SelectedIndexChanged(object sender, EventArgs e) => RefreshOnHand();

        private void LoadBatches(int productId)
        {
            if (!(cboWarehouse.SelectedValue is int wh))
            {
                cboBatch.DataSource = null;
                return;
            }

            // for an adjustment, show every batch that has any recorded balance row (even zero),
            // since "Found" stock might add to a batch that currently sits at zero
            var dt = DbHelper.GetData(
                @"SELECT pb.BatchID,
                         pb.BatchNo + ISNULL('  (exp ' + CONVERT(VARCHAR(10), pb.ExpiryDate, 101) + ')','')
                                    + '  qty ' + CAST(CAST(ISNULL(b.QtyOnHand,0) AS DECIMAL(18,2)) AS VARCHAR(20)) AS Display
                  FROM ProductBatches pb
                  LEFT JOIN StockBalance b ON b.BatchID = pb.BatchID AND b.WarehouseID = @w
                  WHERE pb.ProductID = @p AND pb.IsActive = 1
                  ORDER BY pb.ExpiryDate",
                CommandType.Text, DbHelper.P("@p", productId), DbHelper.P("@w", wh));

            cboBatch.DataSource = dt;
            cboBatch.DisplayMember = "Display";
            cboBatch.ValueMember = "BatchID";
            cboBatch.SelectedIndex = -1;
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            decimal qty, cost;
            bool ok = Validator.ComboSelected(cboWarehouse, ep, "warehouse");
            ok &= Validator.ComboSelected(cboProduct, ep, "product");
            ok &= Validator.ComboSelected(cboAdjustType, ep, "adjustment type");
            ok &= Validator.ComboSelected(cboReason, ep, "reason");
            ok &= Validator.PositiveNumber(txtQty, ep, "Quantity", out qty);
            ok &= Validator.PositiveNumber(txtUnitCost, ep, "Unit cost", out cost, true);
            if (cboBatch.Enabled && !(cboBatch.SelectedValue is int))
            {
                ep.SetError(cboBatch, "Select a batch.");
                ok = false;
            }
            if (!ok) return;

            int pid = (int)cboProduct.SelectedValue;
            int wh = (int)cboWarehouse.SelectedValue;
            object bid = CurrentBatchId();
            bool decrease = cboAdjustType.Text == "Decrease";
            decimal signedQty = decrease ? -qty : qty;

            if (decrease)
            {
                decimal current = GetOnHand(pid, bid, wh) + GetCartNetChange(pid, bid);
                if (qty > current)
                {
                    ep.SetError(txtQty, $"Only {current:N2} currently on hand \u2014 cannot decrease by more than that.");
                    return;
                }
            }
            ep.SetError(txtQty, "");

            foreach (DataRow r in _lines.Rows)
            {
                if ((int)r["ProductID"] == pid && SameBatch(r["BatchID"], bid))
                {
                    r["Quantity"] = (decimal)r["Quantity"] + signedQty;
                    UpdateTotals();
                    ClearLineEntry();
                    return;
                }
            }

            var row = _lines.NewRow();
            row["ProductID"] = pid;
            row["SKU"] = GetProductField(pid, "SKU");
            row["ProductName"] = cboProduct.Text;
            row["BatchID"] = bid ?? (object)DBNull.Value;
            row["BatchNo"] = bid != null ? cboBatch.Text : "-";
            row["Quantity"] = signedQty;
            row["UnitCost"] = cost;
            _lines.Rows.Add(row);

            UpdateTotals();
            ClearLineEntry();
            cboProduct.Focus();
        }

        private string GetProductField(int productId, string field)
        {
            var v = DbHelper.Scalar($"SELECT {field} FROM Products WHERE ProductID=@id",
                CommandType.Text, DbHelper.P("@id", productId));
            return v?.ToString();
        }

        private void ClearLineEntry()
        {
            cboProduct.SelectedIndex = -1;
            cboBatch.DataSource = null;
            cboBatch.Enabled = false;
            cboAdjustType.SelectedIndex = -1;
            txtQty.Clear();
            txtUnitCost.Clear();
            lblCurrentOnHand.Text = "On hand: -";
            ep.Clear();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow == null) return;
            _lines.Rows.RemoveAt(dgvLines.CurrentRow.Index);
            UpdateTotals();
            RefreshOnHand();
        }

        private void UpdateTotals()
        {
            decimal q = 0, v = 0;
            foreach (DataRow r in _lines.Rows)
            {
                q += (decimal)r["Quantity"];
                v += (decimal)r["Quantity"] * (decimal)r["UnitCost"];
            }
            lblTotalQty.Text = q.ToString("N2");
            lblTotalCost.Text = v.ToString("N2");
            lblLineCount.Text = _lines.Rows.Count + " item(s)";

            cboWarehouse.Enabled = _lines.Rows.Count == 0 && _status != "POSTED";
        }

        // ---------- save / post ----------

        private void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (_lines.Rows.Count == 0)
            {
                MessageBox.Show("Add at least one item before saving.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool ok = Validator.ComboSelected(cboWarehouse, ep, "warehouse");
            ok &= Validator.ComboSelected(cboReason, ep, "reason");
            if (!ok) return;

            try
            {
                DbHelper.RunInTransaction((cn, tx) =>
                {
                    if (_txnId == 0)
                    {
                        string docNo;
                        using (var cmd = new SqlCommand("sp_NextDocNo", cn, tx)
                        { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddWithValue("@SeriesCode", "ADJ");
                            var outp = cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 30);
                            outp.Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            docNo = outp.Value.ToString();
                        }

                        using (var cmd = new SqlCommand(
                            @"INSERT INTO StockTransaction
                                (TransactionNo, TransactionType, TransactionDate, ReferenceNo, Reason, Remarks, Status, CreatedBy)
                              VALUES (@no,'ADJUST',@dt,@ref,@rsn,@rem,'DRAFT',@user);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", docNo);
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@ref", txtReferenceNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@rsn", cboReason.Text);
                            cmd.Parameters.AddWithValue("@rem", txtRemarks.Text.Trim());
                            cmd.Parameters.AddWithValue("@user", Session.UserID);
                            _txnId = (int)cmd.ExecuteScalar();
                        }
                        lblTxnNo.Text = docNo;
                    }
                    else
                    {
                        using (var cmd = new SqlCommand(
                            @"UPDATE StockTransaction SET TransactionDate=@dt, ReferenceNo=@ref, Reason=@rsn, Remarks=@rem
                               WHERE TransactionID=@id AND Status='DRAFT';
                              DELETE FROM StockTransactionLine WHERE TransactionID=@id;", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@ref", txtReferenceNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@rsn", cboReason.Text);
                            cmd.Parameters.AddWithValue("@rem", txtRemarks.Text.Trim());
                            cmd.Parameters.AddWithValue("@id", _txnId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    foreach (DataRow r in _lines.Rows)
                    {
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO StockTransactionLine
                                (TransactionID, ProductID, BatchID, ToWarehouseID, Quantity, UnitCost)
                              VALUES (@tid,@pid,@bid,@wh,@qty,@cost);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@tid", _txnId);
                            cmd.Parameters.AddWithValue("@pid", r["ProductID"]);
                            cmd.Parameters.AddWithValue("@bid", r["BatchID"]);
                            cmd.Parameters.AddWithValue("@wh", cboWarehouse.SelectedValue);
                            cmd.Parameters.AddWithValue("@qty", r["Quantity"]);   // signed
                            cmd.Parameters.AddWithValue("@cost", r["UnitCost"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                _draftChanges.MarkSaved();
                _status = "DRAFT";
                UpdateButtons();
                MessageBox.Show("Draft saved as " + lblTxnNo.Text +
                                ".\nClick POST to apply the adjustment.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save adjustment draft"); }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (_txnId == 0) { MessageBox.Show("Save the draft first."); return; }
            if (_draftChanges.IsDirty) { MessageBox.Show("Save Draft before posting your changes.","Unsaved draft",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
            if (MessageBox.Show(
                "Posting will change stock levels and this document can no longer be edited.\n\nContinue?",
                "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DbHelper.Execute("sp_PostTransaction", CommandType.StoredProcedure,
                    DbHelper.P("@TransactionID", _txnId),
                    DbHelper.P("@UserID", Session.UserID));

                _status = "POSTED";
                UpdateButtons();
                UpdateTotals();
                MessageBox.Show("Adjustment " + lblTxnNo.Text + " posted. Inventory updated.",
                    "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Post adjustment"); }
        }

        private void btnNew_Click(object sender, EventArgs e) => NewDocument();

        private void btnPrint_Click(object sender, EventArgs e)
        {
            InventorySystem.Forms.Reports.TransactionReportWindow.Show(this,_txnId);
        }

        private void UpdateButtons()
        {
            bool editable = _status != "POSTED";
            pnlHeader.Enabled = editable;
            pnlLineEntry.Enabled = editable;
            btnAddLine.Enabled = editable;
            btnRemoveLine.Enabled = editable;
            btnSaveDraft.Enabled = editable;
            btnPost.Enabled = editable && _txnId > 0;
            btnPrint.Enabled = _status == "POSTED" && _txnId > 0;
            lblStatusBadge.Text = _status;
        }
    }
}


