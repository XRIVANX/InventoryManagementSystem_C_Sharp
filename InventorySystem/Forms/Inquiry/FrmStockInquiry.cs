using InventorySystem.Data;
using InventorySystem.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace InventorySystem.Forms.Inquiry
{
    public partial class FrmStockInquiry : Form
    {
        public FrmStockInquiry()
        {
            InitializeComponent();
        }

        private void FrmStockInquiry_Load(object sender, EventArgs e)
        {
            LoadFilterCombos();
            SearchStock();
        }

        private void LoadFilterCombos()
        {
            try
            {
                // Warehouse dropdown with '-- All Warehouses --' option
                var dtWarehouse = DbHelper.GetData(
                    @"SELECT 0 AS WarehouseID, '-- All Warehouses --' AS WarehouseName 
                      UNION ALL 
                      SELECT WarehouseID, WarehouseName 
                      FROM Warehouses 
                      WHERE IsActive=1 
                      ORDER BY WarehouseName");

                cboWarehouse.DataSource = dtWarehouse;
                cboWarehouse.DisplayMember = "WarehouseName";
                cboWarehouse.ValueMember = "WarehouseID";

                // Product dropdown with '-- All Products --' option
                var dtProduct = DbHelper.GetData(
                    @"SELECT 0 AS ProductID, '-- All Products --' AS Display 
                      UNION ALL 
                      SELECT ProductID, SKU + ' - ' + ProductName AS Display 
                      FROM Products 
                      WHERE IsActive=1 
                      ORDER BY Display");

                cboProduct.DataSource = dtProduct;
                cboProduct.DisplayMember = "Display";
                cboProduct.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Load inquiry filters");
            }
        }

        private void SearchStock()
        {
            try
            {
                int warehouseId = cboWarehouse.SelectedValue is int w ? w : 0;
                int productId = cboProduct.SelectedValue is int p ? p : 0;
                bool lowStockOnly = pnlFilters.Checked;

                string query = @"
                    SELECT 
                        w.WarehouseName AS [Warehouse],
                        p.SKU,
                        p.ProductName AS [Product],
                        ISNULL(pb.BatchNo, '-') AS [Batch No],
                        pb.ExpiryDate AS [Expiry Date],
                        sb.QtyOnHand AS [Qty On Hand],
                        p.AverageCost AS [Avg Cost],
                        (sb.QtyOnHand * p.AverageCost) AS [Total Value],
                        p.ReorderLevel AS [Reorder Level]
                    FROM StockBalance sb
                    INNER JOIN Warehouses w ON sb.WarehouseID = w.WarehouseID
                    INNER JOIN Products p ON sb.ProductID = p.ProductID
                    LEFT JOIN ProductBatches pb ON sb.BatchID = pb.BatchID
                    WHERE (@w = 0 OR sb.WarehouseID = @w)
                      AND (@p = 0 OR sb.ProductID = @p)
                      AND (@low = 0 OR sb.QtyOnHand <= p.ReorderLevel)
                      AND sb.QtyOnHand > 0
                    ORDER BY w.WarehouseName, p.ProductName, pb.ExpiryDate";

                DataTable dt = DbHelper.GetData(query, CommandType.Text,
                    DbHelper.P("@w", warehouseId),
                    DbHelper.P("@p", productId),
                    DbHelper.P("@low", lowStockOnly ? 1 : 0));

                dgvStock.DataSource = dt;
                FormatGrid();
                CalculateSummaries(dt);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Fetch stock balance");
            }
        }

        private void FormatGrid()
        {
            if (dgvStock.Columns["Avg Cost"] != null)
                dgvStock.Columns["Avg Cost"].DefaultCellStyle.Format = "N2";

            if (dgvStock.Columns["Total Value"] != null)
                dgvStock.Columns["Total Value"].DefaultCellStyle.Format = "N2";

            if (dgvStock.Columns["Qty On Hand"] != null)
                dgvStock.Columns["Qty On Hand"].DefaultCellStyle.Format = "N2";

            if (dgvStock.Columns["Expiry Date"] != null)
                dgvStock.Columns["Expiry Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
        }

        private void CalculateSummaries(DataTable dt)
        {
            decimal totalQty = 0;
            decimal totalVal = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalQty += Convert.ToDecimal(row["Qty On Hand"]);
                totalVal += Convert.ToDecimal(row["Total Value"]);
            }

            lblTotalRecords.Text = $"Records: {dt.Rows.Count}";
            lblTotalQty.Text = $"Total Qty: {totalQty:N2}";
            lblTotalValue.Text = $"Total Value: ₱{totalVal:N2}";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            SearchStock();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboWarehouse.SelectedIndex = 0;
            cboProduct.SelectedIndex = 0;
            pnlFilters.Checked = false;
            SearchStock();
        }
    }
}