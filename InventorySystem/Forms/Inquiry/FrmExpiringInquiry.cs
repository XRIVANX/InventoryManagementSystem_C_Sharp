using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Forms.Reports;
using InventorySystem.Services;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace InventorySystem.Forms.Inquiry
{
    public partial class FrmExpiringInquiry : Form
    {
        private string _resultFilters;
        public FrmExpiringInquiry()
        {
            InitializeComponent();
            dgvExpiring.ReadOnly = true;
            dgvExpiring.AllowUserToAddRows = false;
            dgvExpiring.AllowUserToDeleteRows = false;
            InventorySystem.Helpers.ModernTheme.Apply(this);
            this.Text = "Expiring Items Inquiry";
        }

        private void FrmExpiringInquiry_Load(object sender, EventArgs e)
        {
            // Populate threshold dropdown
            cboThreshold.Items.Clear();
            cboThreshold.Items.AddRange(new object[] {
                "Expiring in 30 Days",
                "Expiring in 60 Days",
                "Expiring in 90 Days",
                "Already Expired"
            });
            cboThreshold.SelectedIndex = 0;

            LoadFilterCombos();
            SearchExpiringStock();
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
                      WHERE IsActive=1 AND IsBatchTracked=1
                      ORDER BY Display");
                cboProduct.DataSource = dtP;
                cboProduct.DisplayMember = "Display";
                cboProduct.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Load expiring inquiry filters");
            }
        }

        private void SearchExpiringStock()
        {
            try
            {
                int warehouseId = cboWarehouse.SelectedValue is int w ? w : 0;
                int productId = cboProduct.SelectedValue is int p ? p : 0;
                int thresholdMode = cboThreshold.SelectedIndex; // 0 = 30 days, 1 = 60 days, 2 = 90 days, 3 = Expired

                string dateCondition;
                switch (thresholdMode)
                {
                    case 3: // Already Expired
                        dateCondition = "pb.ExpiryDate < CAST(GETDATE() AS DATE)";
                        break;
                    case 1: // 60 Days
                        dateCondition = "pb.ExpiryDate BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 60, CAST(GETDATE() AS DATE))";
                        break;
                    case 2: // 90 Days
                        dateCondition = "pb.ExpiryDate BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 90, CAST(GETDATE() AS DATE))";
                        break;
                    case 0: // 30 Days (Default)
                    default:
                        dateCondition = "pb.ExpiryDate BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 30, CAST(GETDATE() AS DATE))";
                        break;
                }

                string query = $@"
                    SELECT 
                        w.WarehouseName AS [Warehouse],
                        p.SKU,
                        p.ProductName AS [Product],
                        pb.BatchNo AS [Batch No],
                        pb.ExpiryDate AS [Expiry Date],
                        DATEDIFF(day, CAST(GETDATE() AS DATE), pb.ExpiryDate) AS [Days Remaining],
                        sb.QtyOnHand AS [Qty On Hand],
                        p.AverageCost AS [Avg Cost],
                        (sb.QtyOnHand * p.AverageCost) AS [At-Risk Value]
                    FROM StockBalance sb
                    INNER JOIN Warehouses w ON sb.WarehouseID = w.WarehouseID
                    INNER JOIN Products p ON sb.ProductID = p.ProductID
                    INNER JOIN ProductBatches pb ON sb.BatchID = pb.BatchID
                    WHERE sb.QtyOnHand > 0
                      AND {dateCondition}
                      AND (@w = 0 OR sb.WarehouseID = @w)
                      AND (@p = 0 OR sb.ProductID = @p)
                    ORDER BY pb.ExpiryDate ASC, w.WarehouseName, p.ProductName";

                DataTable dt = DbHelper.GetData(query, CommandType.Text,
                    DbHelper.P("@w", warehouseId),
                    DbHelper.P("@p", productId));

                dgvExpiring.DataSource = dt;
                FormatGrid();
                CalculateSummary(dt);
                _resultFilters = cboThreshold.Text + " | " + cboWarehouse.Text + " | " + cboProduct.Text;
                btnPrint.Enabled = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                btnPrint.Enabled = false;
                ErrorHandler.Handle(ex, "Fetch expiring stock balance");
            }
        }

        private void FormatGrid()
        {
            if (dgvExpiring.Columns["Avg Cost"] != null)
                dgvExpiring.Columns["Avg Cost"].DefaultCellStyle.Format = "N2";
            if (dgvExpiring.Columns["At-Risk Value"] != null)
                dgvExpiring.Columns["At-Risk Value"].DefaultCellStyle.Format = "N2";
            if (dgvExpiring.Columns["Qty On Hand"] != null)
                dgvExpiring.Columns["Qty On Hand"].DefaultCellStyle.Format = "N2";
            if (dgvExpiring.Columns["Expiry Date"] != null)
                dgvExpiring.Columns["Expiry Date"].DefaultCellStyle.Format = "yyyy-MM-dd";

            // Visual indicator: Highlight expired or near-expiry rows in light red/orange
            foreach (DataGridViewRow row in dgvExpiring.Rows)
            {
                if (row.Cells["Days Remaining"].Value != null &&
                    int.TryParse(row.Cells["Days Remaining"].Value.ToString(), out int daysRemaining))
                {
                    if (daysRemaining < 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.MistyRose; // Expired
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (daysRemaining <= 15)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 245);
                    }
                }
            }
        }

        private void CalculateSummary(DataTable dt)
        {
            decimal totalQty = 0;
            decimal totalVal = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalQty += Convert.ToDecimal(row["Qty On Hand"]);
                totalVal += Convert.ToDecimal(row["At-Risk Value"]);
            }

            lblTotalRecords.Text = $"Records: {dt.Rows.Count}";
            lblTotalQty.Text = $"Total Qty Expiring: {totalQty:N2}";
            lblTotalValue.Text = $"Total At-Risk Value: ₱{totalVal:N2}";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!btnPrint.Enabled || !(dgvExpiring.DataSource is DataTable data)) return;
            try
            {
                // Use the displayed rows and sort order with the filters used for this result.
                CrystalReportWindow.Show(this, InventoryReport.ExpiringItems,
                    data.DefaultView.ToTable(), "Expiring Items", _resultFilters);
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Print expiring items"); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchExpiringStock();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboThreshold.SelectedIndex = 0;
            cboWarehouse.SelectedIndex = 0;
            cboProduct.SelectedIndex = 0;
            SearchExpiringStock();
        }
    }
}
