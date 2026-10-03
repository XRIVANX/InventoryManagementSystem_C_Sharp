using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Helpers;

namespace InventorySystem.Forms
{
    public partial class FrmDashboard
    {
        private readonly Dictionary<ToolStripMenuItem, ToolStripMenuItem> navigation = new Dictionary<ToolStripMenuItem, ToolStripMenuItem>();
        private Panel overview;
        private Form overviewWindow;
        private Label workspaceTitle;
        private ToolStripMenuItem overviewButton;

        private void InitializeModernDashboard()
        {
            SuspendLayout();
            BackColor = ModernTheme.Canvas;
            MinimumSize = new Size(1100, 740);
            Font = ModernTheme.Font(9);
            Text = "InventorySystem | Workspace";
            Branding.ApplyIcon(this);
            menuStrip1.Visible = false;
            pnlKpi.Visible = false;
            foreach (Control control in Controls) if (control is MdiClient) control.BackColor = ModernTheme.Canvas;

            var topBar = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = ModernTheme.Navy };
            var brand = Branding.LogoPicture();
            brand.AutoSize = false; brand.Dock = DockStyle.Left; brand.Width = 230;
            brand.Padding = new Padding(16,8,16,8);
            menuStrip1.Items.Clear();
            menuStrip1.Dock = DockStyle.Fill; menuStrip1.AutoSize = false;
            menuStrip1.BackColor = ModernTheme.Navy; menuStrip1.ForeColor = Color.White;
            menuStrip1.Font = ModernTheme.Font(10, true); menuStrip1.Padding = new Padding(4, 12, 12, 12);
            menuStrip1.Renderer = new WorkspaceMenuRenderer(); menuStrip1.Visible = true;
            mnuMasterfile.Text = "Catalog"; mnuTransactions.Text = "Operations";
            mnuInquiry.Text = "Insights"; mnuFile.Text = "Account"; mnuHelp.Text = "Help";
            mnuMasterfile.Click -= FrmDashboard_Load;
            overviewButton = new ToolStripMenuItem("Overview");
            overviewButton.Click += (s, e) =>
            {
                foreach (Form child in MdiChildren) child.Hide();
                overview.Show(); overviewWindow.Show(); overviewWindow.WindowState = FormWindowState.Maximized;
                overviewWindow.Activate(); workspaceTitle.Text = "Overview"; SelectNavigation(overviewButton);
            };
            menuStrip1.Items.AddRange(new ToolStripItem[] { overviewButton, mnuMasterfile, mnuTransactions, mnuInquiry, mnuReports, mnuHelp, mnuFile });
            foreach (ToolStripMenuItem group in menuStrip1.Items)
            {
                group.Padding = new Padding(12, 6, 12, 6);
                group.BackColor = ModernTheme.Navy; group.ForeColor = Color.White;
                foreach (ToolStripItem entry in group.DropDownItems)
                {
                    entry.Font = ModernTheme.Font(10); entry.Padding = new Padding(8, 5, 12, 5);
                    if (entry is ToolStripMenuItem item)
                    {
                        navigation.Add(item, group);
                        item.Click += (s, e) =>
                        {
                            if (ActiveMdiChild != null && ActiveMdiChild != overviewWindow) SelectNavigation(group);
                        };
                    }
                }
                group.DropDown.Renderer = menuStrip1.Renderer;
            }
            topBar.Controls.Add(menuStrip1); topBar.Controls.Add(brand);
            MainMenuStrip = menuStrip1;
            var header = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Color.White, Padding = new Padding(24, 16, 24, 12) };
            workspaceTitle = ModernTheme.Label("Overview", 21, ModernTheme.Ink, true);
            workspaceTitle.Location = new Point(24, 12);
            header.Controls.Add(workspaceTitle);
            var subtitle = ModernTheme.Label("Inventory workspace", 9, ModernTheme.Muted);
            subtitle.Location = new Point(26, 52); header.Controls.Add(subtitle);
            lblUser.Parent = header;
            lblUser.AutoSize = false; lblUser.Dock = DockStyle.Right; lblUser.Width = 310;
            lblUser.TextAlign = ContentAlignment.MiddleRight; lblUser.Font = ModernTheme.Font(10, true); lblUser.ForeColor = ModernTheme.Ink;
            header.Controls.Add(lblUser);

            overview = new Panel { Dock = DockStyle.Fill, BackColor = ModernTheme.Canvas, Padding = new Padding(24), AutoScroll = true };
            var content = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, BackColor = ModernTheme.Canvas };
            var intro = new Panel { Height = 88, Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 16) };
            var eyebrow = ModernTheme.Label("WORKSPACE / OVERVIEW", 8, ModernTheme.Teal, true); eyebrow.Location = new Point(0, 0);
            var heading = ModernTheme.Label("A clear view of your inventory.", 24, ModernTheme.Ink, true); heading.Location = new Point(0, 23);
            var copy = ModernTheme.Label("Monitor stock, spot priorities, and keep your operations moving.", 10, ModernTheme.Muted); copy.Location = new Point(2, 65);
            intro.Controls.AddRange(new Control[] { eyebrow, heading, copy }); content.Controls.Add(intro);

            var metrics = new TableLayoutPanel { Dock = DockStyle.Top, Height = 278, ColumnCount = 4, RowCount = 2, Margin = new Padding(0) };
            for (int i = 0; i < 4; i++) metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            metrics.RowStyles.Add(new RowStyle(SizeType.Percent, 50)); metrics.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Label[] values = { lblTotalProducts, lblValue, lblLowStock, lblExpiry, lblTodayIn, lblTodayOut, lblDrafts };
            string[] titles = { "TOTAL PRODUCTS", "INVENTORY VALUE", "LOW STOCK", "EXPIRY ALERTS", "STOCK IN TODAY", "STOCK OUT TODAY", "PENDING DRAFTS" };
            string[] hints = { "Your product catalog", "Current stock valuation", "Review replenishment", "Check upcoming expiry", "Today's incoming stock", "Today's outgoing stock", "Awaiting completion" };
            for (int i = 0; i < values.Length; i++)
            {
                var card = new ModernCard { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 12), Accent = i == 2 ? Color.FromArgb(206, 134, 35) : i == 3 ? Color.FromArgb(196, 80, 84) : ModernTheme.Teal };
                var title = ModernTheme.Label(titles[i], 8, ModernTheme.Muted, true); title.Location = new Point(18, 16);
                Label value = values[i]; value.Parent = card; value.AutoSize = false; value.Text = "—";
                value.Font = ModernTheme.Font(i == 1 ? 19 : 25, true); value.ForeColor = ModernTheme.Ink;
                value.SetBounds(18, 38, 190, 46); value.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right; value.AutoEllipsis = true;
                var hint = ModernTheme.Label(hints[i], 8, ModernTheme.Muted); hint.Location = new Point(18, 94);
                card.Controls.AddRange(new Control[] { title, value, hint }); metrics.Controls.Add(card, i % 4, i / 4);
            }
            var refresh = new Button { Text = "Refresh overview", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 12) };
            ModernTheme.Button(refresh); refresh.Click += (s, e) => LoadKpi(); metrics.Controls.Add(refresh, 3, 1);
            content.Controls.Add(metrics);
            var actionsTitle = ModernTheme.Label("Keep things moving", 16, ModernTheme.Ink, true); actionsTitle.Margin = new Padding(0, 18, 0, 6); content.Controls.Add(actionsTitle);
            var actionsCopy = ModernTheme.Label("Jump into your most frequent inventory tasks.", 9, ModernTheme.Muted); actionsCopy.Margin = new Padding(0, 0, 0, 16); content.Controls.Add(actionsCopy);
            var actions = new TableLayoutPanel { Dock = DockStyle.Top, Height = 114, ColumnCount = 3, Margin = new Padding(0) };
            for (int i = 0; i < 3; i++) actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333f));
            ToolStripMenuItem[] shortcuts = { mnuStockIn, mnuStockOut, mnuStockOnHand };
            string[] actionTexts = { "Receive stock\nRecord incoming inventory  →", "Issue stock\nManage outgoing inventory  →", "Explore inventory\nReview stock on hand  →" };
            for (int i = 0; i < 3; i++)
            {
                var target = shortcuts[i];
                var action = new Button { Dock = DockStyle.Fill, Text = actionTexts[i], TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(18), Margin = new Padding(0, 0, 12, 0) };
                ModernTheme.Button(action, i == 0); action.Font = ModernTheme.Font(11, true);
                action.Click += (s, e) => target.PerformClick(); actions.Controls.Add(action);
            }
            content.Controls.Add(actions); overview.Controls.Add(content);
            Controls.Add(header); Controls.Add(topBar);
            MdiClient workspace = null;
            foreach (Control control in Controls) if (control is MdiClient client) workspace = client;
            overviewWindow = new Form { Text = "Overview", MdiParent = this, FormBorderStyle = FormBorderStyle.None, ControlBox = false, ShowInTaskbar = false, BackColor = ModernTheme.Canvas };
            overviewWindow.Controls.Add(overview);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(topBar, 0);
            Controls.SetChildIndex(header, 0);
            Controls.SetChildIndex(workspace, 0);
            statusStrip1.BackColor = Color.White; statusStrip1.ForeColor = ModernTheme.Muted;
            statusStrip1.Items.Add(new ToolStripStatusLabel("InventorySystem   •   Ready") { Spring = true, TextAlign = ContentAlignment.MiddleLeft });
            SelectNavigation(overviewButton);
            MdiChildActivate += (s, e) =>
            {
                var active = ActiveMdiChild;
                if (active == null) return;
                workspaceTitle.Text = active == overviewWindow ? "Overview" : active.Text;
                if (active == overviewWindow) SelectNavigation(overviewButton);
                else
                {
                    if (overviewWindow.Visible) overviewWindow.Hide();
                    SelectNavigation(active is FrmAbout ? mnuHelp : null);
                    foreach (var entry in navigation)
                        if (string.Equals(entry.Key.Text, active.Text, StringComparison.OrdinalIgnoreCase)) SelectNavigation(entry.Value);
                }
            };
            Shown += (s, e) => { overviewWindow.Show(); overviewWindow.WindowState = FormWindowState.Maximized; };
            ResumeLayout(true);
        }

        private void SelectNavigation(ToolStripMenuItem selected)
        {
            foreach (ToolStripItem item in menuStrip1.Items)
                item.BackColor = item == selected ? ModernTheme.Teal : ModernTheme.Navy;
            menuStrip1.Invalidate();
        }

        private sealed class WorkspaceMenuRenderer : ToolStripProfessionalRenderer
        {
            public WorkspaceMenuRenderer() { RoundedEdges = false; }
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                using (var brush = new SolidBrush(e.ToolStrip is MenuStrip ? ModernTheme.Navy : Color.White))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                bool top = e.Item.Owner is MenuStrip;
                Color color = top ? (e.Item.Selected || e.Item.Pressed ? ModernTheme.Teal : e.Item.BackColor)
                    : (e.Item.Selected ? Color.FromArgb(218, 241, 237) : Color.White);
                using (var brush = new SolidBrush(color)) e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = !e.Item.Enabled ? ModernTheme.Muted : e.Item.Owner is MenuStrip ? Color.White : ModernTheme.Ink;
                base.OnRenderItemText(e);
            }
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                    using (var pen = new Pen(ModernTheme.Line)) e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            }
        }

    }
}


