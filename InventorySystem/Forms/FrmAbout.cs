using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Helpers;

namespace InventorySystem.Forms
{
    public sealed class FrmAbout : Form
    {
        public FrmAbout()
        {
            Text="About InventorySystem";ClientSize=new Size(1000,740);BackColor=ModernTheme.Canvas;Font=ModernTheme.Font(10);AutoScaleMode=AutoScaleMode.Dpi;
            var layout=new TableLayoutPanel { Dock=DockStyle.Fill,Padding=new Padding(28),ColumnCount=1,RowCount=3 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));layout.RowStyles.Add(new RowStyle(SizeType.Absolute,188));layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));layout.RowStyles.Add(new RowStyle(SizeType.Absolute,40));
            var brand=new Panel { Dock=DockStyle.Fill,BackColor=ModernTheme.Navy,Margin=new Padding(0,0,0,20) };
            var eyebrow=ModernTheme.Label("YOUR OPERATIONS WORKSPACE",8,Color.FromArgb(102,211,194),true);eyebrow.Location=new Point(28,22);
            var title=ModernTheme.Label("InventorySystem",29,Color.White,true);title.Location=new Point(26,48);
            var description=ModernTheme.Label("A clearer way to manage products, stock, and daily operations.",11,Color.FromArgb(190,207,225));description.Location=new Point(28,108);
            var version=ModernTheme.Label("Version "+typeof(FrmAbout).Assembly.GetName().Version,8,Color.FromArgb(190,207,225));version.Location=new Point(28,140);
            brand.Controls.AddRange(new Control[] {eyebrow,title,description,version});layout.Controls.Add(brand,0,0);
            var guide=new FlowLayoutPanel { Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,WrapContents=false,AutoScroll=true };
            var heading=ModernTheme.Label("Make the most of your workspace",18,ModernTheme.Ink,true);heading.Margin=new Padding(0,0,0,18);guide.Controls.Add(heading);
            AddGuide(guide,"01  Set up your catalog","Maintain products, categories, suppliers, and warehouses before recording stock.");
            AddGuide(guide,"02  Record stock movements","Use Stock In, Stock Out, Transfers, Adjustments, and Physical Counts.\nSave a draft to prepare a transaction; post it to update stock balances.");
            AddGuide(guide,"03  Monitor your inventory","Use the overview and inquiry screens to check on-hand stock, low stock, expiry, and history.");
            AddGuide(guide,"04  Generate and share reports","Choose Reports, set your filters, and generate the results.\nExport CSV or open print preview. Stock cards and movements include posted transactions only.");
            var notes=ModernTheme.Label("Valuation uses current average product cost. Low stock compares aggregated stock to the reorder level.\nStock card opening balances are reconstructed from current stock and posted movements.",9,ModernTheme.Muted);notes.Margin=new Padding(0,12,0,12);guide.Controls.Add(notes);
            layout.Controls.Add(guide,0,1);
            var close=new Button { Text="Close",Width=105,Height=34,Anchor=AnchorStyles.Right };ModernTheme.Button(close);close.Click+=(s,e)=>Close();layout.Controls.Add(close,0,2);
            Controls.Add(layout);
        }
        private static void AddGuide(FlowLayoutPanel guide,string heading,string body)
        {
            var title=ModernTheme.Label(heading,11,ModernTheme.Teal,true);title.Margin=new Padding(0,0,0,6);
            var text=ModernTheme.Label(body,10,ModernTheme.Ink);text.Margin=new Padding(0,0,0,20);guide.Controls.Add(title);guide.Controls.Add(text);
        }
    }
}
