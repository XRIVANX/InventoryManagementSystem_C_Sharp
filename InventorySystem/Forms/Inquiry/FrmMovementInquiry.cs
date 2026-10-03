using InventorySystem.Data;
using InventorySystem.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace InventorySystem.Forms.Inquiry
{
    public partial class FrmMovementInquiry : Form
    {
        public FrmMovementInquiry()
        {
            InitializeComponent();
            InventorySystem.Helpers.ModernTheme.Apply(this);
            this.Text = "Transaction History / Audit Trail";
        }

        private void FrmMovementInquiry_Load(object sender, EventArgs e)
        {
            // Default date filter to the last 30 days
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            // Load transaction type options
            cboTxnType.Items.Clear();
            cboTxnType.Items.AddRange(new object[] { "-- All Types --", "IN", "OUT", "TRANSFER", "ADJUST" });
            cboTxnType.SelectedIndex = 0;

            LoadFilterCombos();
            SearchHistory();
        }

        private void LoadFilterCombos()
        {
            try
            {
                var dtW = DbHelper.GetData(
                    @"SELECT 0 AS WarehouseID, '-- All Warehouses --' AS WarehouseName 
                      UNION ALL 
                      SELECT WarehouseID, WarehouseName 
                      FROM Warehouses 
                      WHERE IsActive=1 
                      ORDER BY WarehouseName");
                cboWarehouse.DataSource = dtW;
                cboWarehouse.DisplayMember = "WarehouseName";
                cboWarehouse.ValueMember = "WarehouseID";

                var dtP = DbHelper.GetData(
                    @"SELECT 0 AS ProductID, '-- All Products --' AS Display 
                      UNION ALL 
                      SELECT ProductID, SKU + ' - ' + ProductName AS Display 
                      FROM Products 
                      WHERE IsActive=1 
                      ORDER BY Display");
                cboProduct.DataSource = dtP;
                cboProduct.DisplayMember = "Display";
                cboProduct.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Load transaction history filters");
            }
        }

        private void SearchHistory()
        {
            try
            {
                int warehouseId = cboWarehouse.SelectedValue is int w ? w : 0;
                int productId = cboProduct.SelectedValue is int p ? p : 0;
                string txnType = cboTxnType.SelectedIndex > 0 ? cboTxnType.Text : "";

                string query = @"
                    SELECT 
                        t.TransactionNo AS [Doc No],
                        t.TransactionType AS [Type],
                        t.TransactionDate AS [Date],
                        p.SKU,
                        p.ProductName AS [Product],
                        ISNULL(pb.BatchNo, '-') AS [Batch No],
                        fw.WarehouseName AS [From WH],
                        tw.WarehouseName AS [To WH],
                        tl.Quantity AS [Qty],
                        tl.UnitCost AS [Unit Cost],
                        (tl.Quantity * tl.UnitCost) AS [Total Cost],
                        t.Reason,
                        t.Status
                    FROM StockTransactionLine tl
                    INNER JOIN StockTransaction t ON tl.TransactionID = t.TransactionID
                    INNER JOIN Products p ON tl.ProductID = p.ProductID
                    LEFT JOIN ProductBatches pb ON tl.BatchID = pb.BatchID
                    LEFT JOIN Warehouses fw ON tl.FromWarehouseID = fw.WarehouseID
                    LEFT JOIN Warehouses tw ON tl.ToWarehouseID = tw.WarehouseID
                    WHERE t.TransactionDate >= @from AND t.TransactionDate <= @to
                      AND (@type = '' OR t.TransactionType = @type)
                      AND (@w = 0 OR tl.FromWarehouseID = @w OR tl.ToWarehouseID = @w)
                      AND (@p = 0 OR tl.ProductID = @p)
                    ORDER BY t.TransactionDate DESC, t.TransactionID DESC";

                DataTable dt = DbHelper.GetData(query, CommandType.Text,
                    DbHelper.P("@from", dtpFrom.Value.Date),
                    DbHelper.P("@to", dtpTo.Value.Date.AddDays(1).AddTicks(-1)),
                    DbHelper.P("@type", txnType),
                    DbHelper.P("@w", warehouseId),
                    DbHelper.P("@p", productId));

                dgvHistory.DataSource = dt;
                FormatGrid();
                CalculateSummary(dt);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Search transaction history");
            }
        }

        private void FormatGrid()
        {
            if (dgvHistory.Columns["Unit Cost"] != null)
                dgvHistory.Columns["Unit Cost"].DefaultCellStyle.Format = "N2";
            if (dgvHistory.Columns["Total Cost"] != null)
                dgvHistory.Columns["Total Cost"].DefaultCellStyle.Format = "N2";
            if (dgvHistory.Columns["Qty"] != null)
                dgvHistory.Columns["Qty"].DefaultCellStyle.Format = "N2";
            if (dgvHistory.Columns["Date"] != null)
                dgvHistory.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        private void CalculateSummary(DataTable dt)
        {
            decimal totalQty = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalQty += Convert.ToDecimal(row["Qty"]);
            }

            lblTotalRecords.Text = $"Records: {dt.Rows.Count}";
            lblTotalQty.Text = $"Total Qty Moved: {totalQty:N2}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchHistory();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            cboTxnType.SelectedIndex = 0;
            cboWarehouse.SelectedIndex = 0;
            cboProduct.SelectedIndex = 0;
            SearchHistory();
        }
    }
}