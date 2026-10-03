using System;
using System.Data;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Reports
{
    internal static class TransactionReportWindow
    {
        public static void Show(IWin32Window owner,int transactionId)
        {
            try
            {
                var header=DbHelper.GetData("SELECT TransactionNo, TransactionType, TransactionDate, ReferenceNo, Remarks FROM StockTransaction WHERE TransactionID=@id AND Status='POSTED'",CommandType.Text,DbHelper.P("@id",transactionId));
                if(header.Rows.Count==0)throw new InvalidOperationException("Select a posted transaction to print.");
                var h=header.Rows[0];
                var rows=ReportService.TransactionLines(transactionId);
                CrystalReportWindow.Show(owner,InventoryReport.Movement,rows,h["TransactionType"]+" | "+h["TransactionNo"],"Posted document | "+Convert.ToDateTime(h["TransactionDate"]).ToString("yyyy-MM-dd")+" | Reference: "+h["ReferenceNo"]);
            }
            catch(Exception ex){ErrorHandler.Handle(ex,"Print transaction");}
        }
    }
}
