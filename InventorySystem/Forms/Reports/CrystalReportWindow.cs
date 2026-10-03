using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using InventorySystem.Services;
#if CRYSTAL_REPORTS
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
#endif

namespace InventorySystem.Forms.Reports
{
    internal static class CrystalReportWindow
    {
        public static string TemplateName(InventoryReport report)
        {
            switch(report)
            {
                case InventoryReport.Valuation:return "InventoryValuation";
                case InventoryReport.StockCard:return "StockCard";
                case InventoryReport.LowStock:return "LowStock";
                default:return "Movement";
            }
        }

        public static DataSet CreateData(InventoryReport report,DataTable results,string title,string filters)
        {
            var data=new DataSet("InventoryReports");
            var rows=results.Copy();rows.TableName=TemplateName(report);data.Tables.Add(rows);
            var context=new DataTable("ReportContext");
            context.Columns.Add("Title",typeof(string));context.Columns.Add("Filters",typeof(string));context.Columns.Add("GeneratedAt",typeof(DateTime));
            context.Rows.Add(title,filters,DateTime.Now);data.Tables.Add(context);
            return data;
        }

        public static void Show(IWin32Window owner,InventoryReport report,DataTable results,string title,string filters)
        {
#if CRYSTAL_REPORTS
            string path=Path.Combine(Application.StartupPath,"Reports",TemplateName(report)+".rpt");
            if(!File.Exists(path))
            {
                MessageBox.Show("The Crystal report template is missing:\n\n"+path+"\n\nCreate this template using the matching XSD in the Reports folder.","Crystal Reports setup",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            try
            {
                using(var data=CreateData(report,results,title,filters))
                using(var document=new ReportDocument())
                using(var form=new Form { Text=title+" | Crystal Reports",Width=1200,Height=850,StartPosition=FormStartPosition.CenterParent })
                using(var viewer=new CrystalReportViewer { Dock=DockStyle.Fill,ToolPanelView=ToolPanelViewType.None,ShowRefreshButton=false })
                {
                    Helpers.Branding.ApplyIcon(form);
                    try
                    {
                        document.Load(path);
                        document.SetDataSource(data);
                        viewer.ReportSource=document;
                        form.Controls.Add(viewer);
                        form.ShowDialog(owner);
                    }
                    finally { viewer.ReportSource=null;document.Close(); }
                }
            }
            catch(Exception ex)
            {
                Helpers.ErrorHandler.Log(ex,"Crystal report preview");
                string detail=ex.GetBaseException().Message;
                MessageBox.Show("The Crystal report could not open. This application requires the SAP 64-bit runtime.\n\n"+detail,"Crystal Reports",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
#else
            MessageBox.Show("Crystal Reports is not enabled in this build.\n\nInstall SAP Crystal Reports for Visual Studio, add the four .rpt templates, then build with EnableCrystalReports=true.\n\nThe project includes matching XSD data definitions and setup instructions in Reports/CrystalReports-Setup.md.","Crystal Reports setup required",MessageBoxButtons.OK,MessageBoxIcon.Information);
#endif
        }
    }
}



