using System;
using System.Data;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;

namespace InventorySystem.Forms.Trans
{
    public partial class FrmBatchEntry : Form
    {
        private readonly int _productId;
        public int NewBatchID { get; private set; }

        public FrmBatchEntry(int productId)
        {
            InitializeComponent();
            InitializeModernBatch();
            Branding.ApplyIcon(this);
            _productId = productId;
        }

        private void InitializeModernBatch()
        {
            SuspendLayout();
            Controls.Clear();
            AutoScaleMode=AutoScaleMode.Dpi;
            AutoScaleDimensions=new System.Drawing.SizeF(96,96);
            Font=ModernTheme.Font(10);
            ClientSize=new System.Drawing.Size(620,470);
            StartPosition=FormStartPosition.CenterParent;
            FormBorderStyle=FormBorderStyle.FixedDialog;
            MaximizeBox=false;MinimizeBox=false;ShowInTaskbar=false;
            BackColor=ModernTheme.Canvas;

            var root=new TableLayoutPanel { Dock=DockStyle.Fill,Padding=new Padding(24),ColumnCount=1,RowCount=3 };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute,112));
            root.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute,64));
            var header=new Panel { Dock=DockStyle.Fill,BackColor=ModernTheme.Navy,Margin=new Padding(0,0,0,16) };
            var title=ModernTheme.Label("Create a new batch",22,System.Drawing.Color.White,true);
            title.Location=new System.Drawing.Point(20,14);
            var subtitle=ModernTheme.Label("Keep manufacture and expiry details together.",10,System.Drawing.Color.FromArgb(190,207,225));
            subtitle.Location=new System.Drawing.Point(22,57);
            header.Controls.AddRange(new Control[]{title,subtitle});root.Controls.Add(header,0,0);

            var card=new ModernCard { Dock=DockStyle.Fill,Padding=new Padding(22),Margin=new Padding(0) };
            var fields=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=1,RowCount=5,BackColor=System.Drawing.Color.White };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            foreach(int height in new int[]{26,42,16,70})fields.RowStyles.Add(new RowStyle(SizeType.Absolute,height));
            fields.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            label1.Text="Batch number";label1.Font=ModernTheme.Font(9,true);label1.ForeColor=ModernTheme.Ink;label1.Margin=new Padding(0);label1.AutoSize=true;
            txtBatchNo.Dock=DockStyle.Top;txtBatchNo.Margin=new Padding(0,2,24,0);txtBatchNo.Font=ModernTheme.Font(11);txtBatchNo.BackColor=System.Drawing.Color.White;txtBatchNo.ForeColor=ModernTheme.Ink;txtBatchNo.BorderStyle=BorderStyle.FixedSingle;txtBatchNo.TabIndex=0;
            fields.Controls.Add(label1,0,0);fields.Controls.Add(txtBatchNo,0,1);
            var dates=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=2,RowCount=2,Margin=new Padding(0),BackColor=System.Drawing.Color.White,TabIndex=1 };
            dates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));dates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            dates.RowStyles.Add(new RowStyle(SizeType.Absolute,26));dates.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            label2.Text="Manufacture date";label3.Text="Expiry date";
            foreach(var label in new[]{label2,label3}){label.Font=ModernTheme.Font(9,true);label.ForeColor=ModernTheme.Ink;label.Margin=new Padding(0);label.AutoSize=true;}
            foreach(var picker in new[]{dtpManufacture,dtpExpiry}) {picker.Dock=DockStyle.Top;picker.Font=ModernTheme.Font(10);picker.Format=DateTimePickerFormat.Custom;picker.CustomFormat="dd MMM yyyy";picker.Margin=new Padding(0,2,24,0);}
            dtpManufacture.TabIndex=0;dtpExpiry.TabIndex=1;
            dates.Controls.Add(label2,0,0);dates.Controls.Add(label3,1,0);dates.Controls.Add(dtpManufacture,0,1);dates.Controls.Add(dtpExpiry,1,1);
            fields.Controls.Add(dates,0,3);
            var hint=ModernTheme.Label("Use a unique batch number for this product.\nThe expiry date must be after the manufacture date.",9,ModernTheme.Muted);
            hint.Margin=new Padding(0,10,0,0);fields.Controls.Add(hint,0,4);
            card.Controls.Add(fields);root.Controls.Add(card,0,1);

            var actions=new FlowLayoutPanel { Dock=DockStyle.Fill,FlowDirection=FlowDirection.RightToLeft,WrapContents=false,Padding=new Padding(0,18,0,0),Margin=new Padding(0),TabIndex=2 };
            btnOK.Text="Create batch";btnOK.Size=new System.Drawing.Size(136,40);btnOK.Margin=new Padding(10,0,0,0);btnOK.TabIndex=1;ModernTheme.Button(btnOK,true);
            btnCancelDlg.Text="Cancel";btnCancelDlg.Size=new System.Drawing.Size(100,40);btnCancelDlg.Margin=new Padding(0);btnCancelDlg.TabIndex=0;btnCancelDlg.DialogResult=DialogResult.Cancel;ModernTheme.Button(btnCancelDlg);
            actions.Controls.Add(btnOK);actions.Controls.Add(btnCancelDlg);root.Controls.Add(actions,0,2);
            ep.SetIconAlignment(txtBatchNo,ErrorIconAlignment.MiddleRight);ep.SetIconPadding(txtBatchNo,5);
            ep.SetIconAlignment(dtpExpiry,ErrorIconAlignment.MiddleRight);ep.SetIconPadding(dtpExpiry,5);
            Controls.Add(root);AcceptButton=btnOK;CancelButton=btnCancelDlg;ActiveControl=txtBatchNo;
            ResumeLayout(true);
        }


        private void FrmBatchEntry_Load(object sender, EventArgs e)
        {
            dtpManufacture.Value = DateTime.Today;
            dtpExpiry.Value = DateTime.Today.AddYears(1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool ok = Validator.Required(txtBatchNo, ep, "Batch number");
            if (dtpExpiry.Value <= dtpManufacture.Value)
            {
                ep.SetError(dtpExpiry, "Expiry date must be later than the manufacture date.");
                ok = false;
            }
            else ep.SetError(dtpExpiry, "");

            if (!ok) return;

            try
            {
                NewBatchID = Convert.ToInt32(DbHelper.Scalar(@"
                    INSERT INTO ProductBatches (ProductID, BatchNo, ManufactureDate, ExpiryDate)
                    VALUES (@pid, @no, @md, @ed);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    CommandType.Text,
                    DbHelper.P("@pid", _productId),
                    DbHelper.P("@no", txtBatchNo.Text.Trim()),
                    DbHelper.P("@md", dtpManufacture.Value.Date),
                    DbHelper.P("@ed", dtpExpiry.Value.Date)));

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex, "Create batch"); }
        }

        private void btnCancelDlg_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

