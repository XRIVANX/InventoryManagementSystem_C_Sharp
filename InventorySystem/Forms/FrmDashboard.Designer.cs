namespace InventorySystem.Forms
{
    partial class FrmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMasterfile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProducts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSuppliers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWarehouses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransactions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStockIn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStockOut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransfer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdjustment = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPhysicalCount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInquiry = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStockOnHand = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLowStockInquiry = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExpiringItems = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransactionHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRptValuation = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRptStockCard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRptLowStock = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRptMovement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pnlKpi = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblDrafts = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblTodayOut = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblTodayIn = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblValue = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblExpiry = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblLowStock = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalProducts = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.pnlKpi.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuMasterfile,
            this.mnuTransactions,
            this.mnuInquiry,
            this.mnuReports,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1251, 30);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChangePassword,
            this.mnuLogout,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(46, 26);
            this.mnuFile.Text = "File";
            // 
            // mnuChangePassword
            // 
            this.mnuChangePassword.Name = "mnuChangePassword";
            this.mnuChangePassword.Size = new System.Drawing.Size(207, 26);
            this.mnuChangePassword.Text = "Change Password";
            this.mnuChangePassword.Click += new System.EventHandler(this.mnuChangePassword_Click);
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(207, 26);
            this.mnuLogout.Text = "Logout";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(207, 26);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuMasterfile
            // 
            this.mnuMasterfile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuProducts,
            this.mnuCategories,
            this.mnuSuppliers,
            this.mnuWarehouses,
            this.mnuUsers});
            this.mnuMasterfile.Name = "mnuMasterfile";
            this.mnuMasterfile.Size = new System.Drawing.Size(89, 26);
            this.mnuMasterfile.Text = "Masterfile";
            this.mnuMasterfile.Click += new System.EventHandler(this.FrmDashboard_Load);
            // 
            // mnuProducts
            // 
            this.mnuProducts.Name = "mnuProducts";
            this.mnuProducts.Size = new System.Drawing.Size(171, 26);
            this.mnuProducts.Text = "Products";
            this.mnuProducts.Click += new System.EventHandler(this.mnuProducts_Click);
            // 
            // mnuCategories
            // 
            this.mnuCategories.Name = "mnuCategories";
            this.mnuCategories.Size = new System.Drawing.Size(171, 26);
            this.mnuCategories.Text = "Categories";
            this.mnuCategories.Click += new System.EventHandler(this.mnuCategories_Click);
            // 
            // mnuSuppliers
            // 
            this.mnuSuppliers.Name = "mnuSuppliers";
            this.mnuSuppliers.Size = new System.Drawing.Size(171, 26);
            this.mnuSuppliers.Text = "Suppliers";
            this.mnuSuppliers.Click += new System.EventHandler(this.mnuSuppliers_Click);
            // 
            // mnuWarehouses
            // 
            this.mnuWarehouses.Name = "mnuWarehouses";
            this.mnuWarehouses.Size = new System.Drawing.Size(171, 26);
            this.mnuWarehouses.Text = "Warehouses";
            this.mnuWarehouses.Click += new System.EventHandler(this.mnuWarehouses_Click);
            // 
            // mnuUsers
            // 
            this.mnuUsers.Name = "mnuUsers";
            this.mnuUsers.Size = new System.Drawing.Size(171, 26);
            this.mnuUsers.Text = "Users";
            this.mnuUsers.Click += new System.EventHandler(this.mnuUsers_Click);
            // 
            // mnuTransactions
            // 
            this.mnuTransactions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStockIn,
            this.mnuStockOut,
            this.mnuTransfer,
            this.mnuAdjustment,
            this.mnuPhysicalCount});
            this.mnuTransactions.Name = "mnuTransactions";
            this.mnuTransactions.Size = new System.Drawing.Size(104, 26);
            this.mnuTransactions.Text = "Transactions";
            // 
            // mnuStockIn
            // 
            this.mnuStockIn.Name = "mnuStockIn";
            this.mnuStockIn.Size = new System.Drawing.Size(208, 26);
            this.mnuStockIn.Text = "Stock In";
            this.mnuStockIn.Click += new System.EventHandler(this.mnuStockIn_Click);
            // 
            // mnuStockOut
            // 
            this.mnuStockOut.Name = "mnuStockOut";
            this.mnuStockOut.Size = new System.Drawing.Size(208, 26);
            this.mnuStockOut.Text = "Stock Out";
            this.mnuStockOut.Click += new System.EventHandler(this.mnuStockOut_Click);
            // 
            // mnuTransfer
            // 
            this.mnuTransfer.Name = "mnuTransfer";
            this.mnuTransfer.Size = new System.Drawing.Size(208, 26);
            this.mnuTransfer.Text = "Stock Transfer";
            this.mnuTransfer.Click += new System.EventHandler(this.mnuTransfer_Click);
            // 
            // mnuAdjustment
            // 
            this.mnuAdjustment.Name = "mnuAdjustment";
            this.mnuAdjustment.Size = new System.Drawing.Size(208, 26);
            this.mnuAdjustment.Text = "Stock Adjustment";
            this.mnuAdjustment.Click += new System.EventHandler(this.mnuAdjustment_Click);
            // 
            // mnuPhysicalCount
            // 
            this.mnuPhysicalCount.Name = "mnuPhysicalCount";
            this.mnuPhysicalCount.Size = new System.Drawing.Size(208, 26);
            this.mnuPhysicalCount.Text = "Physical Count";
            this.mnuPhysicalCount.Click += new System.EventHandler(this.mnuPhysicalCount_Click);
            // 
            // mnuInquiry
            // 
            this.mnuInquiry.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStockOnHand,
            this.mnuLowStockInquiry,
            this.mnuExpiringItems,
            this.mnuTransactionHistory});
            this.mnuInquiry.Name = "mnuInquiry";
            this.mnuInquiry.Size = new System.Drawing.Size(68, 26);
            this.mnuInquiry.Text = "Inquiry";
            // 
            // mnuStockOnHand
            // 
            this.mnuStockOnHand.Name = "mnuStockOnHand";
            this.mnuStockOnHand.Size = new System.Drawing.Size(218, 26);
            this.mnuStockOnHand.Text = "Stock On Hand";
            this.mnuStockOnHand.Click += new System.EventHandler(this.mnuStockOnHand_Click);
            // 
            // mnuLowStockInquiry
            // 
            this.mnuLowStockInquiry.Name = "mnuLowStockInquiry";
            this.mnuLowStockInquiry.Size = new System.Drawing.Size(218, 26);
            this.mnuLowStockInquiry.Text = "Low Stock";
            this.mnuLowStockInquiry.Click += new System.EventHandler(this.mnuLowStockInquiry_Click);
            // 
            // mnuExpiringItems
            // 
            this.mnuExpiringItems.Name = "mnuExpiringItems";
            this.mnuExpiringItems.Size = new System.Drawing.Size(218, 26);
            this.mnuExpiringItems.Text = "Expiring Items";
            this.mnuExpiringItems.Click += new System.EventHandler(this.mnuExpiringItems_Click);
            // 
            // mnuTransactionHistory
            // 
            this.mnuTransactionHistory.Name = "mnuTransactionHistory";
            this.mnuTransactionHistory.Size = new System.Drawing.Size(218, 26);
            this.mnuTransactionHistory.Text = "Transaction History";
            this.mnuTransactionHistory.Click += new System.EventHandler(this.mnuTransactionHistory_Click);
            // 
            // mnuReports
            // 
            this.mnuReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRptValuation,
            this.mnuRptStockCard,
            this.mnuRptLowStock,
            this.mnuRptMovement});
            this.mnuReports.Name = "mnuReports";
            this.mnuReports.Size = new System.Drawing.Size(74, 26);
            this.mnuReports.Text = "Reports";
            // 
            // mnuRptValuation
            // 
            this.mnuRptValuation.Name = "mnuRptValuation";
            this.mnuRptValuation.Size = new System.Drawing.Size(219, 26);
            this.mnuRptValuation.Text = "Inventory Valuation";
            this.mnuRptValuation.Click += new System.EventHandler(this.mnuRptValuation_Click);
            // 
            // mnuRptStockCard
            // 
            this.mnuRptStockCard.Name = "mnuRptStockCard";
            this.mnuRptStockCard.Size = new System.Drawing.Size(219, 26);
            this.mnuRptStockCard.Text = "Stock Card";
            this.mnuRptStockCard.Click += new System.EventHandler(this.mnuRptStockCard_Click);
            // 
            // mnuRptLowStock
            // 
            this.mnuRptLowStock.Name = "mnuRptLowStock";
            this.mnuRptLowStock.Size = new System.Drawing.Size(219, 26);
            this.mnuRptLowStock.Text = "Low Stock Report";
            this.mnuRptLowStock.Click += new System.EventHandler(this.mnuRptLowStock_Click);
            // 
            // mnuRptMovement
            // 
            this.mnuRptMovement.Name = "mnuRptMovement";
            this.mnuRptMovement.Size = new System.Drawing.Size(219, 26);
            this.mnuRptMovement.Text = "Movement Report";
            this.mnuRptMovement.Click += new System.EventHandler(this.mnuRptMovement_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(59, 26);
            this.mnuHelp.Text = " Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(133, 26);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 577);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1251, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pnlKpi
            // 
            this.pnlKpi.Controls.Add(this.panel7);
            this.pnlKpi.Controls.Add(this.panel6);
            this.pnlKpi.Controls.Add(this.panel5);
            this.pnlKpi.Controls.Add(this.panel4);
            this.pnlKpi.Controls.Add(this.panel3);
            this.pnlKpi.Controls.Add(this.panel2);
            this.pnlKpi.Controls.Add(this.panel1);
            this.pnlKpi.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpi.Location = new System.Drawing.Point(0, 30);
            this.pnlKpi.Name = "pnlKpi";
            this.pnlKpi.Size = new System.Drawing.Size(1251, 106);
            this.pnlKpi.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.lblDrafts);
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Location = new System.Drawing.Point(1066, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(168, 90);
            this.panel7.TabIndex = 6;
            // 
            // lblDrafts
            // 
            this.lblDrafts.AutoSize = true;
            this.lblDrafts.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrafts.Location = new System.Drawing.Point(32, 30);
            this.lblDrafts.Name = "lblDrafts";
            this.lblDrafts.Size = new System.Drawing.Size(0, 29);
            this.lblDrafts.TabIndex = 7;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(32, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 29);
            this.label12.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(34, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 18);
            this.label13.TabIndex = 0;
            this.label13.Text = "Pending Drafts";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.lblTodayOut);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Location = new System.Drawing.Point(892, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(168, 90);
            this.panel6.TabIndex = 2;
            // 
            // lblTodayOut
            // 
            this.lblTodayOut.AutoSize = true;
            this.lblTodayOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayOut.Location = new System.Drawing.Point(32, 32);
            this.lblTodayOut.Name = "lblTodayOut";
            this.lblTodayOut.Size = new System.Drawing.Size(0, 29);
            this.lblTodayOut.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(32, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 29);
            this.label10.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 18);
            this.label11.TabIndex = 0;
            this.label11.Text = "Today Stock Out";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lblTodayIn);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(716, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(168, 90);
            this.panel5.TabIndex = 2;
            // 
            // lblTodayIn
            // 
            this.lblTodayIn.AutoSize = true;
            this.lblTodayIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayIn.Location = new System.Drawing.Point(32, 34);
            this.lblTodayIn.Name = "lblTodayIn";
            this.lblTodayIn.Size = new System.Drawing.Size(0, 29);
            this.lblTodayIn.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(32, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 29);
            this.label8.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "Today Stock";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblValue);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Location = new System.Drawing.Point(538, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(168, 90);
            this.panel4.TabIndex = 2;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.Location = new System.Drawing.Point(32, 34);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(0, 29);
            this.lblValue.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(32, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 29);
            this.label6.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Inventory Value";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblExpiry);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(364, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(168, 90);
            this.panel3.TabIndex = 2;
            // 
            // lblExpiry
            // 
            this.lblExpiry.AutoSize = true;
            this.lblExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpiry.Location = new System.Drawing.Point(32, 33);
            this.lblExpiry.Name = "lblExpiry";
            this.lblExpiry.Size = new System.Drawing.Size(0, 29);
            this.lblExpiry.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(32, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 29);
            this.label4.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Expiry Alerts";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblLowStock);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(189, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 90);
            this.panel2.TabIndex = 2;
            // 
            // lblLowStock
            // 
            this.lblLowStock.AutoSize = true;
            this.lblLowStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowStock.Location = new System.Drawing.Point(32, 34);
            this.lblLowStock.Name = "lblLowStock";
            this.lblLowStock.Size = new System.Drawing.Size(0, 29);
            this.lblLowStock.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 29);
            this.label2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Low Stock";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTotalProducts);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(14, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(168, 90);
            this.panel1.TabIndex = 0;
            // 
            // lblTotalProducts
            // 
            this.lblTotalProducts.AutoSize = true;
            this.lblTotalProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalProducts.Location = new System.Drawing.Point(32, 34);
            this.lblTotalProducts.Name = "lblTotalProducts";
            this.lblTotalProducts.Size = new System.Drawing.Size(0, 29);
            this.lblTotalProducts.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Products";
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(706, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(0, 29);
            this.lblUser.TabIndex = 6;
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 599);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.pnlKpi);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmDashboard";
            this.Text = "Inventory Management System — Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlKpi.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuMasterfile;
        private System.Windows.Forms.ToolStripMenuItem mnuTransactions;
        private System.Windows.Forms.ToolStripMenuItem mnuInquiry;
        private System.Windows.Forms.ToolStripMenuItem mnuReports;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuChangePassword;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuProducts;
        private System.Windows.Forms.ToolStripMenuItem mnuCategories;
        private System.Windows.Forms.ToolStripMenuItem mnuSuppliers;
        private System.Windows.Forms.ToolStripMenuItem mnuWarehouses;
        private System.Windows.Forms.ToolStripMenuItem mnuUsers;
        private System.Windows.Forms.ToolStripMenuItem mnuStockIn;
        private System.Windows.Forms.ToolStripMenuItem mnuStockOut;
        private System.Windows.Forms.ToolStripMenuItem mnuTransfer;
        private System.Windows.Forms.ToolStripMenuItem mnuAdjustment;
        private System.Windows.Forms.ToolStripMenuItem mnuPhysicalCount;
        private System.Windows.Forms.ToolStripMenuItem mnuStockOnHand;
        private System.Windows.Forms.ToolStripMenuItem mnuLowStockInquiry;
        private System.Windows.Forms.ToolStripMenuItem mnuExpiringItems;
        private System.Windows.Forms.ToolStripMenuItem mnuTransactionHistory;
        private System.Windows.Forms.ToolStripMenuItem mnuRptValuation;
        private System.Windows.Forms.ToolStripMenuItem mnuRptStockCard;
        private System.Windows.Forms.ToolStripMenuItem mnuRptLowStock;
        private System.Windows.Forms.ToolStripMenuItem mnuRptMovement;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.Panel pnlKpi;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTotalProducts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblTodayOut;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblTodayIn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblExpiry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblLowStock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDrafts;
        private System.Windows.Forms.Label lblUser;
    }
}