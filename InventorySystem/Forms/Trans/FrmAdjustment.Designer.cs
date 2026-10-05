namespace InventorySystem.Forms.Trans
{
    partial class FrmAdjustment
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblStatusBadge = new System.Windows.Forms.Label();
            this.Remarks = new System.Windows.Forms.Label();
            this.txtReferenceNo = new System.Windows.Forms.TextBox();
            this.cboReason = new System.Windows.Forms.ComboBox();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTxnNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblLineCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlLineEntry = new System.Windows.Forms.Panel();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.Quantity = new System.Windows.Forms.Label();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.cboAdjustType = new System.Windows.Forms.ComboBox();
            this.lblCurrentOnHand = new System.Windows.Forms.Label();
            this.cboBatch = new System.Windows.Forms.ComboBox();
            this.txtUnitCost = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ep = new System.Windows.Forms.ErrorProvider(this.components);
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlLineEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.txtRemarks);
            this.pnlHeader.Controls.Add(this.lblStatusBadge);
            this.pnlHeader.Controls.Add(this.Remarks);
            this.pnlHeader.Controls.Add(this.txtReferenceNo);
            this.pnlHeader.Controls.Add(this.cboReason);
            this.pnlHeader.Controls.Add(this.cboWarehouse);
            this.pnlHeader.Controls.Add(this.dtpDate);
            this.pnlHeader.Controls.Add(this.label);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Controls.Add(this.label8);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.lblTxnNo);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1174, 151);
            this.pnlHeader.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(476, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Status";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(332, 115);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(100, 22);
            this.txtRemarks.TabIndex = 14;
            // 
            // lblStatusBadge
            // 
            this.lblStatusBadge.AutoSize = true;
            this.lblStatusBadge.Location = new System.Drawing.Point(526, 80);
            this.lblStatusBadge.Name = "lblStatusBadge";
            this.lblStatusBadge.Size = new System.Drawing.Size(34, 16);
            this.lblStatusBadge.TabIndex = 13;
            this.lblStatusBadge.Text = "New";
            // 
            // Remarks
            // 
            this.Remarks.AutoSize = true;
            this.Remarks.Location = new System.Drawing.Point(235, 115);
            this.Remarks.Name = "Remarks";
            this.Remarks.Size = new System.Drawing.Size(62, 16);
            this.Remarks.TabIndex = 12;
            this.Remarks.Text = "Remarks";
            // 
            // txtReferenceNo
            // 
            this.txtReferenceNo.Location = new System.Drawing.Point(332, 77);
            this.txtReferenceNo.Name = "txtReferenceNo";
            this.txtReferenceNo.Size = new System.Drawing.Size(100, 22);
            this.txtReferenceNo.TabIndex = 11;
            // 
            // cboReason
            // 
            this.cboReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReason.FormattingEnabled = true;
            this.cboReason.Location = new System.Drawing.Point(101, 107);
            this.cboReason.Name = "cboReason";
            this.cboReason.Size = new System.Drawing.Size(121, 24);
            this.cboReason.TabIndex = 10;
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(101, 77);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 24);
            this.cboWarehouse.TabIndex = 9;
            this.cboWarehouse.SelectedIndexChanged += new System.EventHandler(this.cboWarehouse_SelectedIndexChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(78, 48);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(243, 22);
            this.dtpDate.TabIndex = 8;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(14, 110);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(55, 16);
            this.label.TabIndex = 7;
            this.label.Text = "Reason";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(235, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Reference No";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Ware House";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Date";
            // 
            // lblTxnNo
            // 
            this.lblTxnNo.AutoSize = true;
            this.lblTxnNo.Location = new System.Drawing.Point(75, 13);
            this.lblTxnNo.Name = "lblTxnNo";
            this.lblTxnNo.Size = new System.Drawing.Size(158, 16);
            this.lblTxnNo.TabIndex = 1;
            this.lblTxnNo.Text = "(auto-generated on save)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text No.";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblTotalCost);
            this.pnlFooter.Controls.Add(this.label12);
            this.pnlFooter.Controls.Add(this.btnNew);
            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Controls.Add(this.btnPost);
            this.pnlFooter.Controls.Add(this.btnSaveDraft);
            this.pnlFooter.Controls.Add(this.btnRemoveLine);
            this.pnlFooter.Controls.Add(this.lblTotalQty);
            this.pnlFooter.Controls.Add(this.lblLineCount);
            this.pnlFooter.Controls.Add(this.label7);
            this.pnlFooter.Controls.Add(this.label10);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 524);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1174, 100);
            this.pnlFooter.TabIndex = 1;
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Location = new System.Drawing.Point(121, 75);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(31, 16);
            this.lblTotalCost.TabIndex = 30;
            this.lblTotalCost.Text = "0.00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 16);
            this.label12.TabIndex = 29;
            this.label12.Text = "Total Cost";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(310, 14);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(81, 31);
            this.btnNew.TabIndex = 28;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(484, 14);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(76, 31);
            this.btnPrint.TabIndex = 24;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(397, 14);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(81, 31);
            this.btnPost.TabIndex = 23;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnSaveDraft
            // 
            this.btnSaveDraft.Location = new System.Drawing.Point(310, 51);
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(122, 35);
            this.btnSaveDraft.TabIndex = 22;
            this.btnSaveDraft.Text = "Draft Save";
            this.btnSaveDraft.UseVisualStyleBackColor = true;
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);
            // 
            // btnRemoveLine
            // 
            this.btnRemoveLine.Location = new System.Drawing.Point(438, 51);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(122, 35);
            this.btnRemoveLine.TabIndex = 21;
            this.btnRemoveLine.Text = "Remove Line";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Location = new System.Drawing.Point(121, 51);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(31, 16);
            this.lblTotalQty.TabIndex = 20;
            this.lblTotalQty.Text = "0.00";
            // 
            // lblLineCount
            // 
            this.lblLineCount.AutoSize = true;
            this.lblLineCount.Location = new System.Drawing.Point(121, 25);
            this.lblLineCount.Name = "lblLineCount";
            this.lblLineCount.Size = new System.Drawing.Size(31, 16);
            this.lblLineCount.TabIndex = 19;
            this.lblLineCount.Text = "0.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Total Quantity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 16);
            this.label10.TabIndex = 16;
            this.label10.Text = "Line Count";
            // 
            // pnlLineEntry
            // 
            this.pnlLineEntry.Controls.Add(this.txtQty);
            this.pnlLineEntry.Controls.Add(this.Quantity);
            this.pnlLineEntry.Controls.Add(this.cboProduct);
            this.pnlLineEntry.Controls.Add(this.label11);
            this.pnlLineEntry.Controls.Add(this.btnAddLine);
            this.pnlLineEntry.Controls.Add(this.cboAdjustType);
            this.pnlLineEntry.Controls.Add(this.lblCurrentOnHand);
            this.pnlLineEntry.Controls.Add(this.cboBatch);
            this.pnlLineEntry.Controls.Add(this.txtUnitCost);
            this.pnlLineEntry.Controls.Add(this.label9);
            this.pnlLineEntry.Controls.Add(this.label5);
            this.pnlLineEntry.Controls.Add(this.label2);
            this.pnlLineEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLineEntry.Location = new System.Drawing.Point(0, 151);
            this.pnlLineEntry.Name = "pnlLineEntry";
            this.pnlLineEntry.Size = new System.Drawing.Size(1174, 100);
            this.pnlLineEntry.TabIndex = 3;
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(332, 6);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(100, 22);
            this.txtQty.TabIndex = 16;
            // 
            // Quantity
            // 
            this.Quantity.AutoSize = true;
            this.Quantity.Location = new System.Drawing.Point(235, 14);
            this.Quantity.Name = "Quantity";
            this.Quantity.Size = new System.Drawing.Size(55, 16);
            this.Quantity.TabIndex = 23;
            this.Quantity.Text = "Quantity";
            // 
            // cboProduct
            // 
            this.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(101, 6);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 24);
            this.cboProduct.TabIndex = 22;
            this.cboProduct.SelectedIndexChanged += new System.EventHandler(this.cboProduct_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Product";
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(479, 20);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(75, 41);
            this.btnAddLine.TabIndex = 20;
            this.btnAddLine.Text = "Add Line";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // cboAdjustType
            // 
            this.cboAdjustType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAdjustType.FormattingEnabled = true;
            this.cboAdjustType.Location = new System.Drawing.Point(101, 65);
            this.cboAdjustType.Name = "cboAdjustType";
            this.cboAdjustType.Size = new System.Drawing.Size(121, 24);
            this.cboAdjustType.TabIndex = 19;
            // 
            // lblCurrentOnHand
            // 
            this.lblCurrentOnHand.AutoSize = true;
            this.lblCurrentOnHand.Location = new System.Drawing.Point(235, 73);
            this.lblCurrentOnHand.Name = "lblCurrentOnHand";
            this.lblCurrentOnHand.Size = new System.Drawing.Size(60, 16);
            this.lblCurrentOnHand.TabIndex = 18;
            this.lblCurrentOnHand.Text = "On hand:";
            // 
            // cboBatch
            // 
            this.cboBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBatch.FormattingEnabled = true;
            this.cboBatch.Location = new System.Drawing.Point(101, 37);
            this.cboBatch.Name = "cboBatch";
            this.cboBatch.Size = new System.Drawing.Size(121, 24);
            this.cboBatch.TabIndex = 18;
            this.cboBatch.SelectedIndexChanged += new System.EventHandler(this.cboBatch_SelectedIndexChanged);
            // 
            // txtUnitCost
            // 
            this.txtUnitCost.BackColor = System.Drawing.SystemColors.Window;
            this.txtUnitCost.Location = new System.Drawing.Point(332, 37);
            this.txtUnitCost.Name = "txtUnitCost";
            this.txtUnitCost.ReadOnly = true;
            this.txtUnitCost.Size = new System.Drawing.Size(100, 22);
            this.txtUnitCost.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(235, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Unit Cost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Batch";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Adjust Type ";
            // 
            // ep
            // 
            this.ep.ContainerControl = this;
            // 
            // dgvLines
            // 
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLines.Location = new System.Drawing.Point(0, 251);
            this.dgvLines.MultiSelect = false;
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.ReadOnly = true;
            this.dgvLines.RowHeadersWidth = 51;
            this.dgvLines.RowTemplate.Height = 24;
            this.dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLines.Size = new System.Drawing.Size(1174, 273);
            this.dgvLines.TabIndex = 4;
            // 
            // FrmAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 624);
            this.Controls.Add(this.dgvLines);
            this.Controls.Add(this.pnlLineEntry);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmAdjustment";
            this.Text = "Stock Adjustment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmAdjustment_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlLineEntry.ResumeLayout(false);
            this.pnlLineEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.ComboBox cboReason;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTxnNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblStatusBadge;
        private System.Windows.Forms.Label Remarks;
        private System.Windows.Forms.TextBox txtReferenceNo;
        private System.Windows.Forms.Panel pnlLineEntry;
        private System.Windows.Forms.ComboBox cboAdjustType;
        private System.Windows.Forms.ComboBox cboBatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Label lblCurrentOnHand;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblLineCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider ep;
        private System.Windows.Forms.Label Quantity;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUnitCost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.DataGridView dgvLines;
    }
}