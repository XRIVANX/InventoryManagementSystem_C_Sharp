using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Forms.Trans;

namespace InventorySystem.Forms.Trans
{
    public partial class FrmStockIn : Form, ISavedDraftEditor
    {
        private DataTable _lines;
        private readonly DraftChanges _draftChanges = new DraftChanges();
        private int _txnId = 0;
        private string _status = "NEW";
        private readonly int _draftToOpen;
        public int TransactionId => _txnId;
        public event EventHandler TransactionChanged;

        public FrmStockIn() : this(0) { }

        public FrmStockIn(int draftId)
        {
            _draftToOpen = draftId;
            InitializeComponent();
            _draftChanges.Watch(pnlHeader);
            InventorySystem.Helpers.ModernTheme.Apply(this);
        }

        private void FrmStockIn_Load(object sender, EventArgs e)
        {
            InitLinesTable();
            LoadCombos();
            NewDocument();
            if (_draftToOpen > 0)
            {
                try { RestoreSavedDraft(InventorySystem.Services.DraftService.Load(_draftToOpen, "IN")); }
                catch (Exception ex) { ErrorHandler.Handle(ex, "Open saved draft"); Close(); }
            }
        }

        internal void RestoreSavedDraft(InventorySystem.Services.SavedDraft draft)
        {
            var header = draft.Header;
            var first = draft.Lines.Rows[0];
            dtpDate.Value = Convert.ToDateTime(header["TransactionDate"]);
            txtReferenceNo.Text = header["ReferenceNo"].ToString();
            txtRemarks.Text = header["Remarks"].ToString();
            SavedDraftEditor.SelectSavedValue(cboSupplier, header["SupplierID"], header["SupplierName"]);
            SavedDraftEditor.SelectSavedValue(cboWarehouse, first["ToWarehouseID"], first["ToWarehouseName"]);
            SavedDraftEditor.RestoreLines(_lines, draft.Lines);
            _txnId = Convert.ToInt32(header["TransactionID"]);
            _status = "DRAFT";
            lblTxnNo.Text = header["TransactionNo"].ToString();
            UpdateTotals();
            UpdateButtons();
            _draftChanges.MarkSaved();
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
            _draftChanges.Watch(_lines);
            dgvLines.DataSource = _lines;
        }

        private void LoadCombos()
        {
            cboSupplier.DataSource = DbHelper.GetData(
                "SELECT SupplierID, SupplierName FROM Suppliers WHERE IsActive=1 ORDER BY SupplierName");
            cboSupplier.DisplayMember = "SupplierName";
            cboSupplier.ValueMember = "SupplierID";
            cboSupplier.SelectedIndex = -1;

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
            cboSupplier.SelectedIndex = -1;
            cboWarehouse.SelectedIndex = -1;
            cboProduct.SelectedIndex = -1;
            cboBatch.DataSource = null;
            cboBatch.Enabled = false;
            btnNewBatch.Enabled = false;
            txtQty.Clear(); txtUnitCost.Clear();
            UpdateTotals();
            UpdateButtons();
            ep.Clear();
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cboProduct.SelectedValue is int pid)) return;

            bool batched = Convert.ToBoolean(DbHelper.Scalar(
                "SELECT IsBatchTracked FROM Products WHERE ProductID=@id",
                CommandType.Text, DbHelper.P("@id", pid)));

           

            cboBatch.Enabled = batched;
            btnNewBatch.Enabled = batched;
            if (batched) LoadBatches(pid); else cboBatch.DataSource = null;

            var cost = DbHelper.Scalar("SELECT AverageCost FROM Products WHERE ProductID=@id",
                CommandType.Text, DbHelper.P("@id", pid));
            txtUnitCost.Text = Convert.ToDecimal(cost).ToString("0.00");
        }

        private void LoadBatches(int productId)
        {
            cboBatch.DataSource = DbHelper.GetData(
                @"SELECT BatchID, BatchNo + ISNULL('  (exp ' + CONVERT(VARCHAR(10), ExpiryDate, 101) + ')','') AS Display
                  FROM ProductBatches WHERE ProductID=@id AND IsActive=1 ORDER BY ExpiryDate",
                CommandType.Text, DbHelper.P("@id", productId));
            cboBatch.DisplayMember = "Display";
            cboBatch.ValueMember = "BatchID";
            cboBatch.SelectedIndex = -1;
        }

        private void btnNewBatch_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue == null) return;
            using (var f = new FrmBatchEntry((int)cboProduct.SelectedValue))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadBatches((int)cboProduct.SelectedValue);
                    cboBatch.SelectedValue = f.NewBatchID;
                }
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            decimal qty, cost;
            bool ok = Validator.ComboSelected(cboProduct, ep, "product");
            ok &= Validator.PositiveNumber(txtQty, ep, "Quantity", out qty);
            ok &= Validator.PositiveNumber(txtUnitCost, ep, "Unit cost", out cost, true);
            if (cboBatch.Enabled && cboBatch.SelectedValue == null)
            {
                ep.SetError(cboBatch, "This product is batch-tracked. Select or create a batch.");
                ok = false;
            }
            if (!ok) return;

            int pid = (int)cboProduct.SelectedValue;
            object bid = cboBatch.Enabled ? cboBatch.SelectedValue : null;

            foreach (DataRow r in _lines.Rows)
            {
                bool sameBatch = (r["BatchID"] == DBNull.Value ? (object)null : r["BatchID"])?.Equals(bid) ?? (bid == null);
                if ((int)r["ProductID"] == pid && sameBatch)
                {
                    r["Quantity"] = (decimal)r["Quantity"] + qty;
                    r["UnitCost"] = cost;
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
            row["BatchNo"] = cboBatch.Enabled ? cboBatch.Text : "-";
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
            btnNewBatch.Enabled = false;
            txtQty.Clear();
            txtUnitCost.Clear();
            ep.Clear();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow == null) return;
            _lines.Rows.RemoveAt(dgvLines.CurrentRow.Index);
            UpdateTotals();
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
        }

        private void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (_lines.Rows.Count == 0)
            {
                MessageBox.Show("Add at least one item before saving.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Validator.ComboSelected(cboWarehouse, ep, "destination warehouse")) return;

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
                            cmd.Parameters.AddWithValue("@SeriesCode", "IN");
                            var outp = cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 30);
                            outp.Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            docNo = outp.Value.ToString();
                        }

                        using (var cmd = new SqlCommand(
                            @"INSERT INTO StockTransaction
                                (TransactionNo, TransactionType, TransactionDate, SupplierID,
                                 ReferenceNo, Remarks, Status, CreatedBy)
                              VALUES (@no,'IN',@dt,@sup,@ref,@rem,'DRAFT',@user);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", docNo);
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@sup",
                                (object)cboSupplier.SelectedValue ?? DBNull.Value);
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
                            @"IF NOT EXISTS (SELECT 1 FROM StockTransaction WITH (UPDLOCK, HOLDLOCK) WHERE TransactionID=@id AND Status='DRAFT')
                                  THROW 53010, 'This draft is no longer editable. Refresh Saved Drafts.', 1;
                              UPDATE StockTransaction
                                 SET TransactionDate=@dt, SupplierID=@sup, ReferenceNo=@ref, Remarks=@rem
                               WHERE TransactionID=@id AND Status='DRAFT';
                              DELETE FROM StockTransactionLine WHERE TransactionID=@id;", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@sup",
                                (object)cboSupplier.SelectedValue ?? DBNull.Value);
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
                                (TransactionID, ProductID, BatchID, ToWarehouseID, Quantity, UnitCost)
                              VALUES (@tid,@pid,@bid,@wh,@qty,@cost);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@tid", _txnId);
                            cmd.Parameters.AddWithValue("@pid", r["ProductID"]);
                            cmd.Parameters.AddWithValue("@bid", r["BatchID"]);
                            cmd.Parameters.AddWithValue("@wh", cboWarehouse.SelectedValue);
                            cmd.Parameters.AddWithValue("@qty", r["Quantity"]);
                            cmd.Parameters.AddWithValue("@cost", r["UnitCost"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                _draftChanges.MarkSaved();
                _status = "DRAFT";
                UpdateButtons();
                TransactionChanged?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Draft saved as " + lblTxnNo.Text +
                                ".\nClick POST to move the stock.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Save stock-in draft"); }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (_txnId == 0) { MessageBox.Show("Save the draft first."); return; }
            if (_draftChanges.IsDirty) { MessageBox.Show("Save Draft before posting your changes.","Unsaved draft",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
            if (MessageBox.Show(
                "Posting will update stock levels and this document can no longer be edited.\n\nContinue?",
                "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DbHelper.Execute("sp_PostTransaction", CommandType.StoredProcedure,
                    DbHelper.P("@TransactionID", _txnId),
                    DbHelper.P("@UserID", Session.UserID));

                _status = "POSTED";
                UpdateButtons();
                TransactionChanged?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Stock-in " + lblTxnNo.Text + " posted. Inventory updated.",
                    "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Post stock-in"); }
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
            btnSaveDraft.Enabled = editable;
            btnPost.Enabled = editable && _txnId > 0;
            btnPrint.Enabled = _status == "POSTED" && _txnId > 0;
            lblStatusBadge.Text = _status;
        }
    }
}


