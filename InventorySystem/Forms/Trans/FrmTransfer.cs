using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;

namespace InventorySystem.Forms.Trans
{
    public partial class FrmTransfer : Form
    {
        private DataTable _lines;
        private int _txnId = 0;
        private string _status = "NEW";

        public FrmTransfer()
        {
            InitializeComponent();
        }

        private void FrmTransfer_Load(object sender, EventArgs e)
        {
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
            _lines.Columns.Add("Quantity", typeof(decimal));
            _lines.Columns.Add("UnitCost", typeof(decimal));
            _lines.Columns.Add("LineTotal", typeof(decimal), "Quantity * UnitCost");
            dgvLines.DataSource = _lines;
        }

        private void LoadCombos()
        {
            var wh = DbHelper.GetData(
                "SELECT WarehouseID, WarehouseName FROM Warehouses WHERE IsActive=1 ORDER BY WarehouseName");

            cboFromWarehouse.DataSource = wh;
            cboFromWarehouse.DisplayMember = "WarehouseName";
            cboFromWarehouse.ValueMember = "WarehouseID";
            cboFromWarehouse.SelectedIndex = -1;

            cboToWarehouse.DataSource = wh.Copy();
            cboToWarehouse.DisplayMember = "WarehouseName";
            cboToWarehouse.ValueMember = "WarehouseID";
            cboToWarehouse.SelectedIndex = -1;

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
            cboFromWarehouse.SelectedIndex = -1;
            cboToWarehouse.SelectedIndex = -1;
            ClearLineEntry();
            UpdateTotals();
            UpdateButtons();
            ep.Clear();
        }

        // ---------- availability ----------

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

        private decimal GetCartQty(int productId, object batchId)
        {
            decimal total = 0;
            foreach (DataRow r in _lines.Rows)
                if ((int)r["ProductID"] == productId && SameBatch(r["BatchID"], batchId))
                    total += (decimal)r["Quantity"];
            return total;
        }

        private object CurrentBatchId()
            => (cboBatch.Enabled && cboBatch.SelectedValue is int) ? (object)(int)cboBatch.SelectedValue : null;

        private void RefreshAvailable()
        {
            if (!(cboProduct.SelectedValue is int pid) || !(cboFromWarehouse.SelectedValue is int wh))
            {
                lblAvailable.Text = "Available: -";
                return;
            }
            if (cboBatch.Enabled && !(cboBatch.SelectedValue is int))
            {
                lblAvailable.Text = "Available: select a batch";
                return;
            }
            object bid = CurrentBatchId();
            decimal available = GetOnHand(pid, bid, wh) - GetCartQty(pid, bid);
            lblAvailable.Text = "Available: " + available.ToString("N2");
        }

        // ---------- line entry ----------

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cboProduct.SelectedValue is int pid))
            {
                cboBatch.DataSource = null;
                cboBatch.Enabled = false;
                RefreshAvailable();
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

            RefreshAvailable();
        }

        private void cboFromWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue is int pid && cboBatch.Enabled) LoadBatches(pid);
            RefreshAvailable();
        }

        private void cboBatch_SelectedIndexChanged(object sender, EventArgs e) => RefreshAvailable();

        private void LoadBatches(int productId)
        {
            if (!(cboFromWarehouse.SelectedValue is int wh))
            {
                cboBatch.DataSource = null;
                return;
            }

            var dt = DbHelper.GetData(
                @"SELECT pb.BatchID,
                         pb.BatchNo + ISNULL('  (exp ' + CONVERT(VARCHAR(10), pb.ExpiryDate, 101) + ')','')
                                    + '  qty ' + CAST(CAST(b.QtyOnHand AS DECIMAL(18,2)) AS VARCHAR(20)) AS Display
                  FROM ProductBatches pb
                  JOIN StockBalance b ON b.BatchID = pb.BatchID AND b.WarehouseID = @w
                  WHERE pb.ProductID = @p AND b.QtyOnHand > 0
                  ORDER BY pb.ExpiryDate",
                CommandType.Text, DbHelper.P("@p", productId), DbHelper.P("@w", wh));

            cboBatch.DataSource = dt;
            cboBatch.DisplayMember = "Display";
            cboBatch.ValueMember = "BatchID";
            cboBatch.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            decimal qty, cost;
            bool ok = Validator.ComboSelected(cboFromWarehouse, ep, "source warehouse");
            ok &= Validator.ComboSelected(cboToWarehouse, ep, "destination warehouse");
            ok &= Validator.ComboSelected(cboProduct, ep, "product");
            ok &= Validator.PositiveNumber(txtQty, ep, "Quantity", out qty);
            ok &= Validator.PositiveNumber(txtUnitCost, ep, "Unit cost", out cost, true);
            if (cboBatch.Enabled && !(cboBatch.SelectedValue is int))
            {
                ep.SetError(cboBatch, "Select a batch that has stock.");
                ok = false;
            }
            if (ok && cboFromWarehouse.SelectedValue is int fw && cboToWarehouse.SelectedValue is int tw && fw == tw)
            {
                ep.SetError(cboToWarehouse, "Destination must be different from the source warehouse.");
                ok = false;
            }
            else ep.SetError(cboToWarehouse, "");
            if (!ok) return;

            int pid = (int)cboProduct.SelectedValue;
            int wh = (int)cboFromWarehouse.SelectedValue;
            object bid = CurrentBatchId();

            decimal available = GetOnHand(pid, bid, wh) - GetCartQty(pid, bid);
            if (qty > available)
            {
                ep.SetError(txtQty, $"Only {available:N2} available" +
                                    (GetCartQty(pid, bid) > 0 ? " (after what's already on this document)." : "."));
                return;
            }
            ep.SetError(txtQty, "");

            foreach (DataRow r in _lines.Rows)
            {
                if ((int)r["ProductID"] == pid && SameBatch(r["BatchID"], bid))
                {
                    r["Quantity"] = (decimal)r["Quantity"] + qty;
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
            row["Quantity"] = qty;
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
            txtQty.Clear();
            txtUnitCost.Clear();
            lblAvailable.Text = "Available: -";
            ep.Clear();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow == null) return;
            _lines.Rows.RemoveAt(dgvLines.CurrentRow.Index);
            UpdateTotals();
            RefreshAvailable();
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

            bool locked = _lines.Rows.Count > 0 || _status == "POSTED";
            cboFromWarehouse.Enabled = !locked;
            cboToWarehouse.Enabled = !locked;
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
            bool ok = Validator.ComboSelected(cboFromWarehouse, ep, "source warehouse");
            ok &= Validator.ComboSelected(cboToWarehouse, ep, "destination warehouse");
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
                            cmd.Parameters.AddWithValue("@SeriesCode", "TRF");
                            var outp = cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 30);
                            outp.Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            docNo = outp.Value.ToString();
                        }

                        using (var cmd = new SqlCommand(
                            @"INSERT INTO StockTransaction
                                (TransactionNo, TransactionType, TransactionDate, ReferenceNo, Remarks, Status, CreatedBy)
                              VALUES (@no,'TRANSFER',@dt,@ref,@rem,'DRAFT',@user);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", docNo);
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@ref", txtReferenceNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@rem", txtRemarks.Text.Trim());
                            cmd.Parameters.AddWithValue("@user", Session.UserID);
                            _txnId = (int)cmd.ExecuteScalar();
                        }
                        lblTxnNo.Text = docNo;
                    }
                    else
                    {
                        using (var cmd = new SqlCommand(
                            @"UPDATE StockTransaction SET TransactionDate=@dt, ReferenceNo=@ref, Remarks=@rem
                               WHERE TransactionID=@id AND Status='DRAFT';
                              DELETE FROM StockTransactionLine WHERE TransactionID=@id;", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@ref", txtReferenceNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@rem", txtRemarks.Text.Trim());
                            cmd.Parameters.AddWithValue("@id", _txnId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    foreach (DataRow r in _lines.Rows)
                    {
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO StockTransactionLine
                                (TransactionID, ProductID, BatchID, FromWarehouseID, ToWarehouseID, Quantity, UnitCost)
                              VALUES (@tid,@pid,@bid,@fw,@tw,@qty,@cost);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@tid", _txnId);
                            cmd.Parameters.AddWithValue("@pid", r["ProductID"]);
                            cmd.Parameters.AddWithValue("@bid", r["BatchID"]);
                            cmd.Parameters.AddWithValue("@fw", cboFromWarehouse.SelectedValue);
                            cmd.Parameters.AddWithValue("@tw", cboToWarehouse.SelectedValue);
                            cmd.Parameters.AddWithValue("@qty", r["Quantity"]);
                            cmd.Parameters.AddWithValue("@cost", r["UnitCost"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                _status = "DRAFT";
                UpdateButtons();
                MessageBox.Show("Draft saved as " + lblTxnNo.Text +
                                ".\nClick POST to move the stock.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save transfer draft"); }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (_txnId == 0) { MessageBox.Show("Save the draft first."); return; }
            if (MessageBox.Show(
                "Posting will move the stock between warehouses and this document can no longer be edited.\n\nContinue?",
                "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DbHelper.Execute("sp_PostTransaction", CommandType.StoredProcedure,
                    DbHelper.P("@TransactionID", _txnId),
                    DbHelper.P("@UserID", Session.UserID));

                _status = "POSTED";
                UpdateButtons();
                UpdateTotals();
                MessageBox.Show("Transfer " + lblTxnNo.Text + " posted. Inventory updated.",
                    "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Post transfer"); }
        }

        private void btnNew_Click(object sender, EventArgs e) => NewDocument();

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reporting comes in a later part of the build.", "Coming soon",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            btnPrint.Enabled = _status == "POSTED";
            lblStatusBadge.Text = _status;
        }
    }
}