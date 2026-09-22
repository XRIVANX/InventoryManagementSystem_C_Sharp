namespace InventorySystem.Forms.Master
{
    partial class FrmProducts
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.cboStatusFilter = new System.Windows.Forms.ComboBox();
            this.cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.pnlEntry = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.chkBatchTracked = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSellingPrice = new System.Windows.Forms.TextBox();
            this.txtReorderQty = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtReorderLevel = new System.Windows.Forms.TextBox();
            this.Recorder = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSupplier = new System.Windows.Forms.ComboBox();
            this.cboUom = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Product = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.Barcode = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.SKU = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ep = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pnlEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSearch);
            this.splitContainer1.Panel1.Controls.Add(this.lblCount);
            this.splitContainer1.Panel1.Controls.Add(this.cboStatusFilter);
            this.splitContainer1.Panel1.Controls.Add(this.cboCategoryFilter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvProducts);
            this.splitContainer1.Panel2.Controls.Add(this.pnlEntry);
            this.splitContainer1.Size = new System.Drawing.Size(1087, 501);
            this.splitContainer1.SplitterDistance = 82;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(45, 31);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(121, 24);
            this.txtSearch.TabIndex = 0;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.Location = new System.Drawing.Point(441, 35);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 20);
            this.lblCount.TabIndex = 3;
            // 
            // cboStatusFilter
            // 
            this.cboStatusFilter.FormattingEnabled = true;
            this.cboStatusFilter.Location = new System.Drawing.Point(299, 31);
            this.cboStatusFilter.Name = "cboStatusFilter";
            this.cboStatusFilter.Size = new System.Drawing.Size(121, 24);
            this.cboStatusFilter.TabIndex = 2;
            // 
            // cboCategoryFilter
            // 
            this.cboCategoryFilter.FormattingEnabled = true;
            this.cboCategoryFilter.Location = new System.Drawing.Point(172, 31);
            this.cboCategoryFilter.Name = "cboCategoryFilter";
            this.cboCategoryFilter.Size = new System.Drawing.Size(121, 24);
            this.cboCategoryFilter.TabIndex = 1;
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.Location = new System.Drawing.Point(0, 0);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 24;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1087, 195);
            this.dgvProducts.TabIndex = 0;
            // 
            // pnlEntry
            // 
            this.pnlEntry.Controls.Add(this.btnCancel);
            this.pnlEntry.Controls.Add(this.btnDelete);
            this.pnlEntry.Controls.Add(this.btnSave);
            this.pnlEntry.Controls.Add(this.btnNew);
            this.pnlEntry.Controls.Add(this.chkActive);
            this.pnlEntry.Controls.Add(this.chkBatchTracked);
            this.pnlEntry.Controls.Add(this.label7);
            this.pnlEntry.Controls.Add(this.txtSellingPrice);
            this.pnlEntry.Controls.Add(this.txtReorderQty);
            this.pnlEntry.Controls.Add(this.label6);
            this.pnlEntry.Controls.Add(this.txtReorderLevel);
            this.pnlEntry.Controls.Add(this.Recorder);
            this.pnlEntry.Controls.Add(this.label4);
            this.pnlEntry.Controls.Add(this.cboSupplier);
            this.pnlEntry.Controls.Add(this.cboUom);
            this.pnlEntry.Controls.Add(this.label3);
            this.pnlEntry.Controls.Add(this.cboCategory);
            this.pnlEntry.Controls.Add(this.label2);
            this.pnlEntry.Controls.Add(this.label1);
            this.pnlEntry.Controls.Add(this.txtDescription);
            this.pnlEntry.Controls.Add(this.txtName);
            this.pnlEntry.Controls.Add(this.Product);
            this.pnlEntry.Controls.Add(this.txtBarcode);
            this.pnlEntry.Controls.Add(this.Barcode);
            this.pnlEntry.Controls.Add(this.txtSKU);
            this.pnlEntry.Controls.Add(this.SKU);
            this.pnlEntry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEntry.Location = new System.Drawing.Point(0, 195);
            this.pnlEntry.Name = "pnlEntry";
            this.pnlEntry.Size = new System.Drawing.Size(1087, 220);
            this.pnlEntry.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(891, 179);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 31);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(824, 179);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(61, 29);
            this.btnNew.TabIndex = 23;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(630, 188);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(66, 20);
            this.chkActive.TabIndex = 22;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // chkBatchTracked
            // 
            this.chkBatchTracked.AutoSize = true;
            this.chkBatchTracked.Location = new System.Drawing.Point(507, 190);
            this.chkBatchTracked.Name = "chkBatchTracked";
            this.chkBatchTracked.Size = new System.Drawing.Size(117, 20);
            this.chkBatchTracked.TabIndex = 21;
            this.chkBatchTracked.Text = "Batch Tracked";
            this.chkBatchTracked.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(328, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "Selling Price";
            // 
            // txtSellingPrice
            // 
            this.txtSellingPrice.Location = new System.Drawing.Point(328, 174);
            this.txtSellingPrice.Multiline = true;
            this.txtSellingPrice.Name = "txtSellingPrice";
            this.txtSellingPrice.Size = new System.Drawing.Size(150, 37);
            this.txtSellingPrice.TabIndex = 19;
            // 
            // txtReorderQty
            // 
            this.txtReorderQty.Location = new System.Drawing.Point(172, 174);
            this.txtReorderQty.Multiline = true;
            this.txtReorderQty.Name = "txtReorderQty";
            this.txtReorderQty.Size = new System.Drawing.Size(150, 37);
            this.txtReorderQty.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(172, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Reorder Qty";
            // 
            // txtReorderLevel
            // 
            this.txtReorderLevel.Location = new System.Drawing.Point(16, 174);
            this.txtReorderLevel.Multiline = true;
            this.txtReorderLevel.Name = "txtReorderLevel";
            this.txtReorderLevel.Size = new System.Drawing.Size(150, 37);
            this.txtReorderLevel.TabIndex = 16;
            // 
            // Recorder
            // 
            this.Recorder.AutoSize = true;
            this.Recorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Recorder.Location = new System.Drawing.Point(12, 151);
            this.Recorder.Name = "Recorder";
            this.Recorder.Size = new System.Drawing.Size(127, 20);
            this.Recorder.TabIndex = 15;
            this.Recorder.Text = "Reorder Level";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(328, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Supplier";
            // 
            // cboSupplier
            // 
            this.cboSupplier.FormattingEnabled = true;
            this.cboSupplier.Location = new System.Drawing.Point(328, 113);
            this.cboSupplier.Name = "cboSupplier";
            this.cboSupplier.Size = new System.Drawing.Size(150, 24);
            this.cboSupplier.TabIndex = 13;
            // 
            // cboUom
            // 
            this.cboUom.FormattingEnabled = true;
            this.cboUom.Location = new System.Drawing.Point(172, 113);
            this.cboUom.Name = "cboUom";
            this.cboUom.Size = new System.Drawing.Size(150, 24);
            this.cboUom.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(168, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "UOM";
            // 
            // cboCategory
            // 
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(16, 113);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(150, 24);
            this.cboCategory.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Category";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(480, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(484, 39);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(150, 37);
            this.txtDescription.TabIndex = 7;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(328, 39);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 37);
            this.txtName.TabIndex = 6;
            // 
            // Product
            // 
            this.Product.AutoSize = true;
            this.Product.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Product.Location = new System.Drawing.Point(324, 16);
            this.Product.Name = "Product";
            this.Product.Size = new System.Drawing.Size(74, 20);
            this.Product.TabIndex = 5;
            this.Product.Text = "Product";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(172, 39);
            this.txtBarcode.Multiline = true;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(150, 37);
            this.txtBarcode.TabIndex = 4;
            // 
            // Barcode
            // 
            this.Barcode.AutoSize = true;
            this.Barcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Barcode.Location = new System.Drawing.Point(168, 16);
            this.Barcode.Name = "Barcode";
            this.Barcode.Size = new System.Drawing.Size(79, 20);
            this.Barcode.TabIndex = 3;
            this.Barcode.Text = "Barcode";
            // 
            // txtSKU
            // 
            this.txtSKU.Location = new System.Drawing.Point(16, 39);
            this.txtSKU.Multiline = true;
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(150, 37);
            this.txtSKU.TabIndex = 2;
            // 
            // SKU
            // 
            this.SKU.AutoSize = true;
            this.SKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SKU.Location = new System.Drawing.Point(12, 16);
            this.SKU.Name = "SKU";
            this.SKU.Size = new System.Drawing.Size(46, 20);
            this.SKU.TabIndex = 1;
            this.SKU.Text = "SKU";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(485, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 7;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(958, 179);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 29);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1024, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 29);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ep
            // 
            this.ep.ContainerControl = this;
            // 
            // FrmProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 501);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmProducts";
            this.Text = "Products";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmProducts_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.pnlEntry.ResumeLayout(false);
            this.pnlEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cboStatusFilter;
        private System.Windows.Forms.ComboBox cboCategoryFilter;
        private System.Windows.Forms.Panel pnlEntry;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Label SKU;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label Product;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label Barcode;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSupplier;
        private System.Windows.Forms.ComboBox cboUom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBatchTracked;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSellingPrice;
        private System.Windows.Forms.TextBox txtReorderQty;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReorderLevel;
        private System.Windows.Forms.Label Recorder;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider ep;
    }
}