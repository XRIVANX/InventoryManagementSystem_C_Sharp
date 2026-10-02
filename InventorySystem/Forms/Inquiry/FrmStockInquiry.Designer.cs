namespace InventorySystem.Forms.Inquiry
{
    partial class FrmStockInquiry
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
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.chkLowStock = new System.Windows.Forms.CheckBox();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.flowLayoutPanel1);
            this.pnlFilters.Controls.Add(this.btnFilter);
            this.pnlFilters.Controls.Add(this.cboWarehouse);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1059, 98);
            this.pnlFilters.TabIndex = 0;
            // 
            // chkLowStock
            // 
            this.chkLowStock.AutoSize = true;
            this.chkLowStock.Location = new System.Drawing.Point(3, 3);
            this.chkLowStock.Name = "chkLowStock";
            this.chkLowStock.Size = new System.Drawing.Size(120, 20);
            this.chkLowStock.TabIndex = 8;
            this.chkLowStock.Text = "Low Stock Only";
            this.chkLowStock.UseVisualStyleBackColor = true;
            // 
            // cboProduct
            // 
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(129, 3);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 24);
            this.cboProduct.TabIndex = 6;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(256, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(128, 33);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset Filters";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(261, 12);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(166, 96);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Search / Refresh";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Product";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(449, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Warehouse";
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(95, 8);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(159, 24);
            this.cboWarehouse.TabIndex = 0;
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.Location = new System.Drawing.Point(0, 98);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersWidth = 51;
            this.dgvStock.RowTemplate.Height = 24;
            this.dgvStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStock.Size = new System.Drawing.Size(1059, 549);
            this.dgvStock.TabIndex = 1;
            // 
            // pnlSummary
            // 
            this.pnlSummary.Controls.Add(this.lblTotalValue);
            this.pnlSummary.Controls.Add(this.lblTotalQty);
            this.pnlSummary.Controls.Add(this.lblTotalRecords);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSummary.Location = new System.Drawing.Point(0, 547);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(1059, 100);
            this.pnlSummary.TabIndex = 2;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Location = new System.Drawing.Point(12, 75);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(115, 16);
            this.lblTotalValue.TabIndex = 11;
            this.lblTotalValue.Text = "Total Value: ₱0.00";
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Location = new System.Drawing.Point(12, 47);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(91, 16);
            this.lblTotalQty.TabIndex = 10;
            this.lblTotalQty.Text = "Total Qty: 0.00";
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Location = new System.Drawing.Point(12, 20);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(72, 16);
            this.lblTotalRecords.TabIndex = 9;
            this.lblTotalRecords.Text = "Records: 0";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.chkLowStock);
            this.flowLayoutPanel1.Controls.Add(this.cboProduct);
            this.flowLayoutPanel1.Controls.Add(this.btnReset);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 13);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(998, 641);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // FrmStockInquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 647);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.dgvStock);
            this.Controls.Add(this.pnlFilters);
            this.Name = "FrmStockInquiry";
            this.Text = "Stock On Hand";
            this.Load += new System.EventHandler(this.FrmStockInquiry_Load);
            this.pnlFilters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.CheckBox chkLowStock;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}