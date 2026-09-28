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
            _productId = productId;
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