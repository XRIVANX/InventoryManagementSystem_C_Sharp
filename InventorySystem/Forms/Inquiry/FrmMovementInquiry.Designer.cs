namespace InventorySystem.Forms.Inquiry
{
    partial class FrmMovementInquiry
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.cboTxnType = new System.Windows.Forms.ComboBox();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.btnReset);
            this.pnlFilters.Controls.Add(this.btnSearch);
            this.pnlFilters.Controls.Add(this.label5);
            this.pnlFilters.Controls.Add(this.cboProduct);
            this.pnlFilters.Controls.Add(this.label4);
            this.pnlFilters.Controls.Add(this.cboWarehouse);
            this.pnlFilters.Controls.Add(this.cboTxnType);
            this.pnlFilters.Controls.Add(this.dtpTo);
            this.pnlFilters.Controls.Add(this.dtpFrom);
            this.pnlFilters.Controls.Add(this.label3);
            this.pnlFilters.Controls.Add(this.label2);
            this.pnlFilters.Controls.Add(this.label1);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1033, 112);
            this.pnlFilters.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "From Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "To Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Type";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(88, 15);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(259, 22);
            this.dtpFrom.TabIndex = 3;
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(88, 48);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(259, 22);
            this.dtpTo.TabIndex = 4;
            // 
            // cboTxnType
            // 
            this.cboTxnType.FormattingEnabled = true;
            this.cboTxnType.Location = new System.Drawing.Point(465, 7);
            this.cboTxnType.Name = "cboTxnType";
            this.cboTxnType.Size = new System.Drawing.Size(121, 24);
            this.cboTxnType.TabIndex = 5;
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(465, 43);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 24);
            this.cboWarehouse.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Warehouse";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(382, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Product";
            // 
            // cboProduct
            // 
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(465, 80);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 24);
            this.cboProduct.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(88, 80);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(108, 25);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(213, 80);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(134, 26);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(0, 112);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersWidth = 51;
            this.dgvHistory.RowTemplate.Height = 24;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new System.Drawing.Size(1033, 429);
            this.dgvHistory.TabIndex = 1;
            // 
            // pnlSummary
            // 
            this.pnlSummary.Controls.Add(this.lblTotalQty);
            this.pnlSummary.Controls.Add(this.lblTotalRecords);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSummary.Location = new System.Drawing.Point(0, 441);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(1033, 100);
            this.pnlSummary.TabIndex = 2;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Location = new System.Drawing.Point(12, 55);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(136, 16);
            this.lblTotalQty.TabIndex = 13;
            this.lblTotalQty.Text = "Total Qty Moved: 0.00";
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Location = new System.Drawing.Point(12, 19);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(72, 16);
            this.lblTotalRecords.TabIndex = 12;
            this.lblTotalRecords.Text = "Records: 0";
            // 
            // FrmMovementInquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 541);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.pnlFilters);
            this.Name = "FrmMovementInquiry";
            this.Text = "Transaction History";
            this.Load += new System.EventHandler(this.FrmMovementInquiry_Load);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.ComboBox cboTxnType;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblTotalRecords;
    }
}