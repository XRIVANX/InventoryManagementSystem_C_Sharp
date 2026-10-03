using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Reports
{
    public class FrmInventoryReport : Form
    {
        private readonly InventoryReport report;
        private readonly ComboBox warehouse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 205 };
        private readonly ComboBox product = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 265 };
        private readonly DateTimePicker from = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 125, Value = DateTime.Today.AddDays(-30) };
        private readonly DateTimePicker to = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 125, Value = DateTime.Today };
        private readonly DataGridView grid = new DataGridView { Dock = DockStyle.Fill, Visible=false, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
        private readonly Label status = ModernTheme.Label("Choose your filters, then generate a report.", 9, ModernTheme.Muted);
        private readonly Label summary = ModernTheme.Label("No report generated", 11, ModernTheme.Ink, true);
        private readonly Label empty = new Label { Dock=DockStyle.Fill,Text="Your report starts here.\n\nChoose filters above, then select Generate report.",TextAlign=ContentAlignment.MiddleCenter,Font=ModernTheme.Font(12),ForeColor=ModernTheme.Muted,BackColor=Color.White };
        private readonly Button export = new Button { Text = "Export CSV", Width = 116, Height = 36, Enabled = false };
        private readonly Button print = new Button { Text = "Print preview", Width = 122, Height = 36, Enabled = false };
        private readonly Button crystal = new Button { Text = "Crystal preview", Width = 136, Height = 36, Enabled = false };
        private DataTable result;
        private string generatedFilters;
        private int printRow;
        private int printPage;
        private const float PrintRowHeight = 38;

        protected FrmInventoryReport(InventoryReport kind, string title, string description)
        {
            report = kind; Text = title; ClientSize = new Size(1160, 700); BackColor = ModernTheme.Canvas;
            Font = ModernTheme.Font(9); AutoScaleMode = AutoScaleMode.Dpi;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(24) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute,88)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,174));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent,100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,60));
            var heading = new Panel { Dock = DockStyle.Fill };
            var eyebrow = ModernTheme.Label("REPORTS / " + title.ToUpperInvariant(),8,ModernTheme.Teal,true); eyebrow.Location = new Point(0,0);
            var name = ModernTheme.Label(title,23,ModernTheme.Ink,true); name.Location = new Point(0,21);
            var detail = ModernTheme.Label(description,9,ModernTheme.Muted); detail.Location = new Point(2,63);
            heading.Controls.AddRange(new Control[] { eyebrow,name,detail }); layout.Controls.Add(heading,0,0);
            var filters = new ModernCard { Dock = DockStyle.Fill, Margin = new Padding(0,0,0,14) };
            var fields = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 76, WrapContents = true, Padding = new Padding(0) };
            fields.Controls.Add(Field("Warehouse",warehouse)); fields.Controls.Add(Field(kind == InventoryReport.StockCard ? "Product (required)" : "Product",product));
            if (kind == InventoryReport.StockCard || kind == InventoryReport.Movement) { fields.Controls.Add(Field("From",from)); fields.Controls.Add(Field("To",to)); }
            var toolbar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44, WrapContents = false };
            var generate = new Button { Text = "Generate report", Width = 146, Height = 36 };
            var reset = new Button { Text = "Reset filters", Width = 110, Height = 36 };
            ModernTheme.Button(generate,true); ModernTheme.Button(reset); ModernTheme.Button(export); ModernTheme.Button(print); ModernTheme.Button(crystal);
            toolbar.Controls.AddRange(new Control[] { generate,reset,export,print,crystal }); filters.Controls.Add(fields); filters.Controls.Add(toolbar); layout.Controls.Add(filters,0,1);
            var table = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(1), Margin = new Padding(0) }; table.Controls.Add(grid);table.Controls.Add(empty);empty.BringToFront();layout.Controls.Add(table,0,2);
            var footer = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            summary.Location = new Point(0,12); status.Location = new Point(0,37); status.AutoSize = false; status.Width = 1000; status.Height = 24;
            footer.Controls.AddRange(new Control[] { summary,status }); layout.Controls.Add(footer,0,3); Controls.Add(layout);
            ModernTheme.Apply(this); ModernTheme.Button(generate,true);eyebrow.ForeColor=ModernTheme.Teal;detail.ForeColor=ModernTheme.Muted;
            heading.BackColor = ModernTheme.Canvas; filters.BackColor = Color.White; fields.BackColor = Color.White; toolbar.BackColor = Color.White;
            foreach(Control field in fields.Controls)field.BackColor=Color.White;
            Load += (s,e) => LoadFilters();
            generate.Click += (s,e) => Generate();
            reset.Click += (s,e) => { if (warehouse.Items.Count>0) warehouse.SelectedIndex=0; if(product.Items.Count>0)product.SelectedIndex=0; from.Value=DateTime.Today.AddDays(-30);to.Value=DateTime.Today; MarkDirty(); };
            warehouse.SelectedIndexChanged += (s,e) => MarkDirty(); product.SelectedIndexChanged += (s,e) => MarkDirty(); from.ValueChanged += (s,e) => MarkDirty(); to.ValueChanged += (s,e) => MarkDirty();
            export.Click += (s,e) => ExportCsv(); print.Click += (s,e) => PrintPreview();
            crystal.Click += (s,e) => { if(result!=null && crystal.Enabled)CrystalReportWindow.Show(this,report,result,Text,generatedFilters); };
        }

        private static Panel Field(string title,Control control)
        {
            var panel = new Panel { Width=control.Width+14,Height=68,Margin=new Padding(0,0,6,0),BackColor=Color.White };
            var label=ModernTheme.Label(title,8,ModernTheme.Muted,true); label.Location=new Point(0,0);control.Location=new Point(0,24);panel.Controls.AddRange(new Control[] {label,control});return panel;
        }
        private static void Bind(ComboBox combo,DataTable data,string id,string display,string first)
        {
            var row=data.NewRow();row[id]=0;row[display]=first;data.Rows.InsertAt(row,0);
            combo.DisplayMember=display;combo.ValueMember=id;combo.DataSource=data;combo.SelectedIndex=0;
        }
        private void LoadFilters()
        {
            try { Bind(warehouse,ReportService.Warehouses(),"WarehouseID","WarehouseName","All warehouses"); Bind(product,ReportService.Products(),"ProductID","Display",report==InventoryReport.StockCard?"Choose a product":"All products"); }
            catch(Exception) { status.ForeColor=Color.Firebrick;status.Text="Filters could not load. Check your database connection, then reopen this report."; }
        }
        private void MarkDirty()
        {
            export.Enabled=print.Enabled=crystal.Enabled=false;
            status.ForeColor=ModernTheme.Muted;status.Text=result==null?"Choose your filters, then generate a report.":"Filters changed. Generate the report again to update the results.";
        }
        private void Generate()
        {
            if(warehouse.DataSource==null || product.DataSource==null) { LoadFilters(); if(warehouse.DataSource==null || product.DataSource==null)return; }
            if ((report==InventoryReport.StockCard || report==InventoryReport.Movement) && from.Value.Date>to.Value.Date) { status.ForeColor=Color.Firebrick;status.Text="The start date must be on or before the end date.";return; }
            int w=warehouse.SelectedValue is int wid?wid:0,p=product.SelectedValue is int pid?pid:0;
            if(report==InventoryReport.StockCard && p==0) { status.ForeColor=Color.Firebrick;status.Text="Select a product to generate its stock card.";product.Focus();return; }
            export.Enabled=print.Enabled=crystal.Enabled=false;Cursor=Cursors.WaitCursor;
            try
            {
                result=ReportService.Generate(report,w,p,from.Value,to.Value);grid.DataSource=result;grid.Visible=result.Rows.Count>0;empty.Visible=result.Rows.Count==0;empty.Text="No records match these filters.\n\nTry a different warehouse, product, or date range.";
                foreach(DataGridViewColumn column in grid.Columns)
                {
                    column.SortMode=DataGridViewColumnSortMode.NotSortable;
                    if(column.ValueType==typeof(decimal)) { column.DefaultCellStyle.Format="N2";column.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight; }
                    if(column.ValueType==typeof(DateTime))column.DefaultCellStyle.Format="yyyy-MM-dd HH:mm";
                }
                generatedFilters=warehouse.Text+" | "+product.Text;
                if(report==InventoryReport.StockCard || report==InventoryReport.Movement)generatedFilters+=" | "+from.Value.ToString("yyyy-MM-dd")+" to "+to.Value.ToString("yyyy-MM-dd");
                summary.Text=Summary(result);status.ForeColor=ModernTheme.Muted;
                status.Text=result.Rows.Count==0?"No records match these filters.":"Generated "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+" • "+(report==InventoryReport.Valuation||report==InventoryReport.LowStock?"Current stock balances":"Posted transactions only");
                export.Enabled=print.Enabled=crystal.Enabled=result.Rows.Count>0;
            }
            catch(Exception) { status.ForeColor=Color.Firebrick;status.Text="The report could not be generated. Check your database connection and retry."; }
            finally { Cursor=Cursors.Default; }
        }
        private string Summary(DataTable data)
        {
            string text=data.Rows.Count.ToString("N0")+" rows";
            if(report==InventoryReport.Valuation)text+="   |   Stock value: "+Total(data,"Stock Value").ToString("N2");
            if(report==InventoryReport.LowStock)text+="   |   Products at or below reorder level";
            if(report==InventoryReport.StockCard && data.Rows.Count>0)text="Opening: "+Convert.ToDecimal(data.Rows[0]["Balance"]).ToString("N2")+"   |   Closing: "+Convert.ToDecimal(data.Rows[data.Rows.Count-1]["Balance"]).ToString("N2")+"   |   "+(data.Rows.Count-1)+" movements";
            if(report==InventoryReport.Movement)text+="   |   Posted transaction lines (includes transfers and signed adjustments)";
            return text;
        }
        private static decimal Total(DataTable data,string column) => data.Rows.Cast<DataRow>().Sum(row=>row.IsNull(column)?0:Convert.ToDecimal(row[column]));
        internal static string CsvField(object value)
        {
            string text=value==null || value==DBNull.Value?"":value is DateTime date?date.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture):Convert.ToString(value,CultureInfo.InvariantCulture);
            // Export user text as literal text when opened in spreadsheet software.
            if(value is string && text.Length>0 && ("=+-@".IndexOf(text.TrimStart().FirstOrDefault())>=0 || text[0]=='\t' || text[0]=='\r'))text="'"+text;
            return "\""+text.Replace("\"","\"\"")+"\"";
        }
        private void ExportCsv()
        {
            if(result==null || !export.Enabled)return;
            using(var dialog=new SaveFileDialog { Filter="CSV file (*.csv)|*.csv",FileName=Text.Replace(" ","-")+"-"+DateTime.Now.ToString("yyyyMMdd")+".csv",OverwritePrompt=true })
            {
                if(dialog.ShowDialog(this)!=DialogResult.OK)return;
                try
                {
                    using(var writer=new StreamWriter(dialog.FileName,false,new UTF8Encoding(true)))
                    {
                        writer.WriteLine(string.Join(",",result.Columns.Cast<DataColumn>().Select(c=>CsvField(c.ColumnName))));
                        foreach(DataRow row in result.Rows)writer.WriteLine(string.Join(",",row.ItemArray.Select(CsvField)));
                    }
                    status.Text="Exported "+result.Rows.Count.ToString("N0")+" rows.";
                }
                catch(Exception) { status.ForeColor=Color.Firebrick;status.Text="The export could not be saved. Choose a writable location and retry."; }
            }
        }
        private void PrintPreview()
        {
            if(result==null || !print.Enabled)return;
            try
            {
                using(var document=new PrintDocument())
                using(var preview=new PrintPreviewDialog { Document=document,Width=1100,Height=800 })
                {
                    document.DocumentName=Text;document.DefaultPageSettings.Landscape=true;
                    document.DefaultPageSettings.Margins=new Margins(40,40,40,40);
                    document.BeginPrint+=(s,e)=> { printRow=0;printPage=0; };document.PrintPage+=DrawReportPage;
                    preview.ShowDialog(this);
                }
            }
            catch(Exception) { status.ForeColor=Color.Firebrick;status.Text="Print preview is unavailable. Check that a printer is installed, or export CSV."; }
        }
        private void DrawReportPage(object sender,PrintPageEventArgs e)
        {
            printPage++;
            using(var titleFont=ModernTheme.Font(18,true))
            using(var bodyFont=ModernTheme.Font(8))
            using(var boldFont=ModernTheme.Font(8,true))
            using(var rule=new Pen(ModernTheme.Line))
            {
                Rectangle bounds=e.MarginBounds;float y=bounds.Top;
                e.Graphics.DrawString(Text,titleFont,Brushes.Black,bounds.Left,y);y+=34;
                e.Graphics.DrawString(generatedFilters,bodyFont,Brushes.DimGray,new RectangleF(bounds.Left,y,bounds.Width,35));y+=36;
                e.Graphics.DrawString(Summary(result),boldFont,Brushes.Black,bounds.Left,y);y+=28;
                float width=(float)bounds.Width/result.Columns.Count;
                using(var format=new StringFormat { Trimming=StringTrimming.EllipsisCharacter })
                {
                    for(int i=0;i<result.Columns.Count;i++)e.Graphics.DrawString(result.Columns[i].ColumnName,boldFont,Brushes.Black,new RectangleF(bounds.Left+i*width+3,y,width-6,32),format);
                    y+=38;
                    while(printRow<result.Rows.Count && y+PrintRowHeight<bounds.Bottom-24)
                    {
                        e.Graphics.DrawLine(rule,bounds.Left,y,bounds.Right,y);
                        for(int i=0;i<result.Columns.Count;i++)
                        {
                            object value=result.Rows[printRow][i];string text=value is DateTime date?date.ToString("yyyy-MM-dd HH:mm"):value is decimal number?number.ToString("N2"):Convert.ToString(value);
                            e.Graphics.DrawString(text,bodyFont,Brushes.Black,new RectangleF(bounds.Left+i*width+3,y+4,width-6,PrintRowHeight-6),format);
                        }
                        printRow++;y+=PrintRowHeight;
                    }
                }
                e.Graphics.DrawString("InventorySystem  •  Page "+printPage,bodyFont,Brushes.DimGray,bounds.Left,bounds.Bottom-16);
                e.HasMorePages=printRow<result.Rows.Count;
            }
        }
    }
    public sealed class FrmValuationReport : FrmInventoryReport { public FrmValuationReport():base(InventoryReport.Valuation,"Inventory Valuation","Current stock valued at each product's average cost.") {} }
    public sealed class FrmStockCardReport : FrmInventoryReport { public FrmStockCardReport():base(InventoryReport.StockCard,"Stock Card","Trace a product's posted movements, opening stock, and running balance.") {} }
    public sealed class FrmLowStockReport : FrmInventoryReport { public FrmLowStockReport():base(InventoryReport.LowStock,"Low Stock Report","Find replenishment priorities, including products with no stock.") {} }
    public sealed class FrmMovementReport : FrmInventoryReport { public FrmMovementReport():base(InventoryReport.Movement,"Movement Report","Review posted stock transactions across a selected date range.") {} }
}
