namespace InventorySystem.Forms.Trans
{
    partial class FrmTransfer
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
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblStatusBadge = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReferenceNo = new System.Windows.Forms.TextBox();
            this.cboToWarehouse = new System.Windows.Forms.ComboBox();
            this.cboFromWarehouse = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTxnNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLineEntry = new System.Windows.Forms.Panel();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.txtUnitCost = new System.Windows.Forms.TextBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboBatch = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblLineCount = new System.Windows.Forms.Label();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ep = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlLineEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblStatusBadge);
            this.pnlHeader.Controls.Add(this.label7);
            this.pnlHeader.Controls.Add(this.txtRemarks);
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.txtReferenceNo);
            this.pnlHeader.Controls.Add(this.cboToWarehouse);
            this.pnlHeader.Controls.Add(this.cboFromWarehouse);
            this.pnlHeader.Controls.Add(this.dtpDate);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Controls.Add(this.lblTxnNo);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(960, 150);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblStatusBadge
            // 
            this.lblStatusBadge.AutoSize = true;
            this.lblStatusBadge.Location = new System.Drawing.Point(661, 20);
            this.lblStatusBadge.Name = "lblStatusBadge";
            this.lblStatusBadge.Size = new System.Drawing.Size(39, 16);
            this.lblStatusBadge.TabIndex = 14;
            this.lblStatusBadge.Text = "NEW";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(611, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(377, 115);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(100, 22);
            this.txtRemarks.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Remarks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Reference No.";
            // 
            // txtReferenceNo
            // 
            this.txtReferenceNo.Location = new System.Drawing.Point(377, 85);
            this.txtReferenceNo.Name = "txtReferenceNo";
            this.txtReferenceNo.Size = new System.Drawing.Size(100, 22);
            this.txtReferenceNo.TabIndex = 9;
            // 
            // cboToWarehouse
            // 
            this.cboToWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToWarehouse.FormattingEnabled = true;
            this.cboToWarehouse.Location = new System.Drawing.Point(134, 119);
            this.cboToWarehouse.Name = "cboToWarehouse";
            this.cboToWarehouse.Size = new System.Drawing.Size(121, 24);
            this.cboToWarehouse.TabIndex = 8;
            // 
            // cboFromWarehouse
            // 
            this.cboFromWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFromWarehouse.FormattingEnabled = true;
            this.cboFromWarehouse.Location = new System.Drawing.Point(134, 80);
            this.cboFromWarehouse.Name = "cboFromWarehouse";
            this.cboFromWarehouse.Size = new System.Drawing.Size(121, 24);
            this.cboFromWarehouse.TabIndex = 7;
            this.cboFromWarehouse.SelectedIndexChanged += new System.EventHandler(this.cboFromWarehouse_SelectedIndexChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(83, 50);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 22);
            this.dtpDate.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "Warehouse To ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Warehouse From";
            // 
            // lblTxnNo
            // 
            this.lblTxnNo.AutoSize = true;
            this.lblTxnNo.Location = new System.Drawing.Point(80, 20);
            this.lblTxnNo.Name = "lblTxnNo";
            this.lblTxnNo.Size = new System.Drawing.Size(158, 16);
            this.lblTxnNo.TabIndex = 2;
            this.lblTxnNo.Text = "(auto-generated on save)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text No.";
            // 
            // pnlLineEntry
            // 
            this.pnlLineEntry.Controls.Add(this.btnAddLine);
            this.pnlLineEntry.Controls.Add(this.label12);
            this.pnlLineEntry.Controls.Add(this.label13);
            this.pnlLineEntry.Controls.Add(this.lblAvailable);
            this.pnlLineEntry.Controls.Add(this.txtUnitCost);
            this.pnlLineEntry.Controls.Add(this.txtQty);
            this.pnlLineEntry.Controls.Add(this.label9);
            this.pnlLineEntry.Controls.Add(this.cboBatch);
            this.pnlLineEntry.Controls.Add(this.label8);
            this.pnlLineEntry.Controls.Add(this.cboProduct);
            this.pnlLineEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLineEntry.Location = new System.Drawing.Point(0, 150);
            this.pnlLineEntry.Name = "pnlLineEntry";
            this.pnlLineEntry.Size = new System.Drawing.Size(960, 87);
            this.pnlLineEntry.TabIndex = 1;
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(496, 39);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(75, 38);
            this.btnAddLine.TabIndex = 18;
            this.btnAddLine.Text = "Add Line";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(273, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 16);
            this.label12.TabIndex = 16;
            this.label12.Text = "Unit Cost";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(273, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 16);
            this.label13.TabIndex = 15;
            this.label13.Text = "Quantity";
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(497, 14);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(74, 16);
            this.lblAvailable.TabIndex = 15;
            this.lblAvailable.Text = "Available: -";
            // 
            // txtUnitCost
            // 
            this.txtUnitCost.Location = new System.Drawing.Point(377, 39);
            this.txtUnitCost.Name = "txtUnitCost";
            this.txtUnitCost.Size = new System.Drawing.Size(100, 22);
            this.txtUnitCost.TabIndex = 16;
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(377, 11);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(100, 22);
            this.txtQty.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "Batch";
            // 
            // cboBatch
            // 
            this.cboBatch.FormattingEnabled = true;
            this.cboBatch.Location = new System.Drawing.Point(134, 41);
            this.cboBatch.Name = "cboBatch";
            this.cboBatch.Size = new System.Drawing.Size(121, 24);
            this.cboBatch.TabIndex = 16;
            this.cboBatch.SelectedIndexChanged += new System.EventHandler(this.cboBatch_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "Product";
            // 
            // cboProduct
            // 
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(134, 11);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 24);
            this.cboProduct.TabIndex = 0;
            this.cboProduct.SelectedIndexChanged += new System.EventHandler(this.cboProduct_SelectedIndexChanged);
            // 
            // dgvLines
            // 
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLines.Location = new System.Drawing.Point(0, 237);
            this.dgvLines.MultiSelect = false;
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.ReadOnly = true;
            this.dgvLines.RowHeadersWidth = 51;
            this.dgvLines.RowTemplate.Height = 24;
            this.dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLines.Size = new System.Drawing.Size(960, 345);
            this.dgvLines.TabIndex = 2;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnNew);
            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Controls.Add(this.btnPost);
            this.pnlFooter.Controls.Add(this.btnSaveDraft);
            this.pnlFooter.Controls.Add(this.btnRemoveLine);
            this.pnlFooter.Controls.Add(this.label14);
            this.pnlFooter.Controls.Add(this.lblTotalQty);
            this.pnlFooter.Controls.Add(this.lblLineCount);
            this.pnlFooter.Controls.Add(this.lblTotalCost);
            this.pnlFooter.Controls.Add(this.label10);
            this.pnlFooter.Controls.Add(this.label11);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 491);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(960, 91);
            this.pnlFooter.TabIndex = 3;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(331, 15);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(70, 30);
            this.btnNew.TabIndex = 30;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(484, 16);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 29);
            this.btnPrint.TabIndex = 29;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(407, 15);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(70, 30);
            this.btnPost.TabIndex = 28;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnSaveDraft
            // 
            this.btnSaveDraft.Location = new System.Drawing.Point(331, 49);
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(100, 30);
            this.btnSaveDraft.TabIndex = 27;
            this.btnSaveDraft.Text = "Save Draft";
            this.btnSaveDraft.UseVisualStyleBackColor = true;
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);
            // 
            // btnRemoveLine
            // 
            this.btnRemoveLine.Location = new System.Drawing.Point(453, 49);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(101, 30);
            this.btnRemoveLine.TabIndex = 26;
            this.btnRemoveLine.Text = "Remove Line";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(170, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 16);
            this.label14.TabIndex = 25;
            this.label14.Text = "Total Cost";
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Location = new System.Drawing.Point(112, 59);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(31, 16);
            this.lblTotalQty.TabIndex = 24;
            this.lblTotalQty.Text = "0.00";
            // 
            // lblLineCount
            // 
            this.lblLineCount.AutoSize = true;
            this.lblLineCount.Location = new System.Drawing.Point(112, 29);
            this.lblLineCount.Name = "lblLineCount";
            this.lblLineCount.Size = new System.Drawing.Size(31, 16);
            this.lblLineCount.TabIndex = 23;
            this.lblLineCount.Text = "0.00";
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Location = new System.Drawing.Point(252, 29);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(31, 16);
            this.lblTotalCost.TabIndex = 21;
            this.lblTotalCost.Text = "0.00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 16);
            this.label10.TabIndex = 20;
            this.label10.Text = "Total Quantity";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 16);
            this.label11.TabIndex = 19;
            this.label11.Text = "Line Count";
            // 
            // ep
            // 
            this.ep.ContainerControl = this;
            // 
            // FrmTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 582);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.dgvLines);
            this.Controls.Add(this.pnlLineEntry);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmTransfer";
            this.Text = "Stock Transfer";
            this.Load += new System.EventHandler(this.FrmTransfer_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLineEntry.ResumeLayout(false);
            this.pnlLineEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.ComboBox cboFromWarehouse;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTxnNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReferenceNo;
        private System.Windows.Forms.ComboBox cboToWarehouse;
        private System.Windows.Forms.Label lblStatusBadge;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlLineEntry;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.TextBox txtUnitCost;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboBatch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblLineCount;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.ErrorProvider ep;
    }
}