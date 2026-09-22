using InventorySystem.Data;
using InventorySystem.Forms.Auth;
using InventorySystem.Forms.Master;
using InventorySystem.Helpers;
using System;
using System.Data;
using System.Windows.Forms;


namespace InventorySystem.Forms
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            lblUser.Text = $"{Session.FullName}  ({Session.RoleName})";
            ApplyRolePermissions();
            LoadKpi();
        }

        private void LoadKpi()
        {
            try
            {
                var dt = DbHelper.GetData("sp_DashboardKPI", CommandType.StoredProcedure);
                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];

                lblTotalProducts.Text = r["TotalProducts"].ToString();
                lblLowStock.Text = r["LowStockCount"].ToString();
                lblExpiry.Text = r["ExpiryAlerts"].ToString();
                lblValue.Text = Convert.ToDecimal(r["InventoryValue"]).ToString("C2");
                lblTodayIn.Text = r["TodayStockIn"].ToString();
                lblTodayOut.Text = r["TodayStockOut"].ToString();
                lblDrafts.Text = r["PendingDrafts"].ToString();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Dashboard KPI"); }
        }

        private void ApplyRolePermissions()
        {
            mnuUsers.Enabled = Session.IsAdmin;
            mnuAdjustment.Enabled = Session.IsManager;
            mnuPhysicalCount.Enabled = Session.IsManager;
        }

        // ---- generic MDI-child opener, ready for when forms exist ----
        private void OpenChild<T>() where T : Form, new()
        {
            foreach (Form f in MdiChildren)
            {
                if (f is T) { f.Activate(); f.WindowState = FormWindowState.Maximized; return; }
            }
            var child = new T { MdiParent = this, WindowState = FormWindowState.Maximized };
            child.FormClosed += (s, e) => LoadKpi();
            child.Show();
        }

        private void NotYetBuilt(string formName)
            => MessageBox.Show($"{formName} hasn't been built yet.", "Coming soon",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

        // ---- File menu ----
        private void mnuChangePassword_Click(object sender, EventArgs e) => NotYetBuilt("Change Password");

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out of the system?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Session.Clear();
            Hide();
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    foreach (Form f in MdiChildren) f.Close();
                    Show();
                    FrmDashboard_Load(null, null);
                }
                else Close();
            }
        }

        private void mnuExit_Click(object sender, EventArgs e) => Application.Exit();

        // ---- Masterfile menu (stubs for now) ----
        private void mnuProducts_Click(object sender, EventArgs e) => OpenChild<FrmProducts>();
        private void mnuCategories_Click(object sender, EventArgs e) => NotYetBuilt("Categories");
        private void mnuSuppliers_Click(object sender, EventArgs e) => NotYetBuilt("Suppliers");
        private void mnuWarehouses_Click(object sender, EventArgs e) => NotYetBuilt("Warehouses");
        private void mnuUsers_Click(object sender, EventArgs e) => NotYetBuilt("Users");

        // ---- Transactions menu (stubs for now) ----
        private void mnuStockIn_Click(object sender, EventArgs e) => NotYetBuilt("Stock In");
        private void mnuStockOut_Click(object sender, EventArgs e) => NotYetBuilt("Stock Out");
        private void mnuTransfer_Click(object sender, EventArgs e) => NotYetBuilt("Stock Transfer");
        private void mnuAdjustment_Click(object sender, EventArgs e) => NotYetBuilt("Stock Adjustment");
        private void mnuPhysicalCount_Click(object sender, EventArgs e) => NotYetBuilt("Physical Count");

        // ---- Inquiry menu (stubs for now) ----
        private void mnuStockOnHand_Click(object sender, EventArgs e) => NotYetBuilt("Stock On Hand");
        private void mnuLowStockInquiry_Click(object sender, EventArgs e) => NotYetBuilt("Low Stock");
        private void mnuExpiringItems_Click(object sender, EventArgs e) => NotYetBuilt("Expiring Items");
        private void mnuTransactionHistory_Click(object sender, EventArgs e) => NotYetBuilt("Transaction History");

        // ---- Reports menu (stubs for now) ----
        private void mnuRptValuation_Click(object sender, EventArgs e) => NotYetBuilt("Inventory Valuation Report");
        private void mnuRptStockCard_Click(object sender, EventArgs e) => NotYetBuilt("Stock Card Report");
        private void mnuRptLowStock_Click(object sender, EventArgs e) => NotYetBuilt("Low Stock Report");
        private void mnuRptMovement_Click(object sender, EventArgs e) => NotYetBuilt("Movement Report");

        // ---- Help menu ----
        private void mnuAbout_Click(object sender, EventArgs e)
            => MessageBox.Show("Inventory Management System\nBuilt with C#, MSSQL, and Crystal Reports.",
                                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}