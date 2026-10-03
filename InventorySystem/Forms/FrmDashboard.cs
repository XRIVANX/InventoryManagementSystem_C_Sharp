using InventorySystem.Data;
using InventorySystem.Forms.Auth;
using InventorySystem.Forms.Inquiry;
using InventorySystem.Forms.Master;
using InventorySystem.Forms.Trans;
using InventorySystem.Forms.Reports;
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
            InitializeModernDashboard();
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
                if (f is T) { f.Show(); f.Activate(); f.WindowState = FormWindowState.Maximized; return; }
            }
            var child = new T { MdiParent = this, WindowState = FormWindowState.Maximized };
            child.FormClosed += (s, e) => LoadKpi();
            child.Show();
        }

        // ---- helper to open/switch FrmStockInquiry with filter flag ----
        private void OpenInquiryForm(bool lowStockOnly)
        {
            foreach (Form f in MdiChildren)
            {
                if (f is FrmStockInquiry existing)
                {
                    existing.ApplyLowStockFilter(lowStockOnly);
                    existing.Show(); existing.Activate();
                    existing.WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            var child = new FrmStockInquiry(lowStockOnly)
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized
            };
            child.FormClosed += (s, e) => LoadKpi();
            child.Show();
        }

        // ---- File menu ----
        private void mnuChangePassword_Click(object sender, EventArgs e) { using(var form=new FrmChangePassword()) form.ShowDialog(this); }

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
                    foreach (Form f in MdiChildren) if (f != overviewWindow) f.Close();
                    Show();
                    FrmDashboard_Load(null, null); overviewButton.PerformClick();
                }
                else Close();
            }
        }

        private void mnuExit_Click(object sender, EventArgs e) => Application.Exit();

        // ---- Masterfile menu ----
        private void mnuProducts_Click(object sender, EventArgs e) => OpenChild<FrmProducts>();
        private void mnuCategories_Click(object sender, EventArgs e) => OpenChild<FrmCategories>();
        private void mnuSuppliers_Click(object sender, EventArgs e) => OpenChild<FrmSuppliers>();
        private void mnuWarehouses_Click(object sender, EventArgs e) => OpenChild<FrmWarehouses>();
        private void mnuUsers_Click(object sender, EventArgs e) => OpenChild<FrmUsers>();

        // ---- Transactions menu ----
        private void mnuStockIn_Click(object sender, EventArgs e) => OpenChild<FrmStockIn>();
        private void mnuStockOut_Click(object sender, EventArgs e) => OpenChild<FrmStockOut>();
        private void mnuTransfer_Click(object sender, EventArgs e) => OpenChild<FrmTransfer>();
        private void mnuAdjustment_Click(object sender, EventArgs e) => OpenChild<FrmAdjustment>();
        private void mnuPhysicalCount_Click(object sender, EventArgs e) => OpenChild<FrmPhysicalCount>();

        // ---- Inquiry menu ----
        private void mnuStockOnHand_Click(object sender, EventArgs e) => OpenInquiryForm(false);
        private void mnuLowStockInquiry_Click(object sender, EventArgs e) => OpenInquiryForm(true);
        private void mnuExpiringItems_Click(object sender, EventArgs e) => OpenChild<FrmExpiringInquiry>();
        private void mnuTransactionHistory_Click(object sender, EventArgs e) => OpenChild<FrmMovementInquiry>();

        // ---- Reports menu ----
        private void mnuRptValuation_Click(object sender, EventArgs e) => OpenChild<FrmValuationReport>();
        private void mnuRptStockCard_Click(object sender, EventArgs e) => OpenChild<FrmStockCardReport>();
        private void mnuRptLowStock_Click(object sender, EventArgs e) => OpenChild<FrmLowStockReport>();
        private void mnuRptMovement_Click(object sender, EventArgs e) => OpenChild<FrmMovementReport>();

        // ---- Help menu ----
        private void mnuAbout_Click(object sender, EventArgs e) => OpenChild<FrmAbout>();
    }
}
