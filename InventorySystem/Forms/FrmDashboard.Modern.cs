using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Helpers;

namespace InventorySystem.Forms
{
    public partial class FrmDashboard
    {
        private readonly Dictionary<ToolStripMenuItem, Button> navigation = new Dictionary<ToolStripMenuItem, Button>();
        private Panel overview;
        private Form overviewWindow;
        private Label workspaceTitle;
        private Button overviewButton;

        private void InitializeModernDashboard()
        {
            SuspendLayout();
            BackColor = ModernTheme.Canvas;
            MinimumSize = new Size(1100, 740);
            Font = ModernTheme.Font(9);
            Text = "InventorySystem | Workspace";
            menuStrip1.Visible = false;
            pnlKpi.Visible = false;
            foreach (Control control in Controls) if (control is MdiClient) control.BackColor = ModernTheme.Canvas;

            var sidebar = new Panel { Dock = DockStyle.Left, Width = 222, BackColor = ModernTheme.Navy, Padding = new Padding(16), AutoScroll = true };
            var nav = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = ModernTheme.Navy };
            var brand = ModernTheme.Label("INVENTORY\nSYSTEM", 18, Color.White, true);
            brand.Margin = new Padding(8, 12, 0, 5);
            nav.Controls.Add(brand);
            var tagline = ModernTheme.Label("Your operations, connected.", 8, Color.FromArgb(150, 170, 193));
            tagline.AutoSize = false; tagline.Size = new Size(160, 30);
            tagline.Margin = new Padding(8, 0, 0, 24);
            nav.Controls.Add(tagline);
            overviewButton = NavButton("Overview", null, nav);
            overviewButton.Click += (s, e) => { foreach (Form child in MdiChildren) child.Hide(); overview.Show(); overviewWindow.Show(); overviewWindow.WindowState = FormWindowState.Maximized; overviewWindow.Activate(); workspaceTitle.Text = "Overview"; SelectNavigation(overviewButton); };
            AddSection(nav, "CATALOG", new[] { mnuProducts, mnuCategories, mnuSuppliers, mnuWarehouses, mnuUsers });
            AddSection(nav, "OPERATIONS", new[] { mnuStockIn, mnuStockOut, mnuTransfer, mnuAdjustment, mnuPhysicalCount });
            AddSection(nav, "INSIGHTS", new[] { mnuStockOnHand, mnuLowStockInquiry, mnuExpiringItems, mnuTransactionHistory });
            AddSection(nav, "ACCOUNT", new[] { mnuChangePassword, mnuLogout, mnuExit });
            sidebar.Controls.Add(nav);
            var reportMenu = new MenuStrip { Dock = DockStyle.Top, BackColor = Color.White, Font = ModernTheme.Font(9), Padding = new Padding(18, 6, 18, 6) };
            menuStrip1.Items.Remove(mnuReports); menuStrip1.Items.Remove(mnuHelp);
            reportMenu.Items.Add(mnuReports); reportMenu.Items.Add(mnuHelp);
            MainMenuStrip = reportMenu;

            var header = new Panel { Dock = DockStyle.Top, Height = 86, BackColor = Color.White, Padding = new Padding(24, 16, 24, 12) };
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
            Controls.Add(reportMenu); Controls.Add(header); Controls.Add(sidebar);
            MdiClient workspace = null;
            foreach (Control control in Controls) if (control is MdiClient client) workspace = client;
            overviewWindow = new Form { Text = "Overview", MdiParent = this, FormBorderStyle = FormBorderStyle.None, ControlBox = false, ShowInTaskbar = false, BackColor = ModernTheme.Canvas };
            overviewWindow.Controls.Add(overview);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(sidebar, 0);
            Controls.SetChildIndex(header, 0);
            Controls.SetChildIndex(reportMenu, 0);
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
                    foreach (var entry in navigation)
                        if (string.Equals(entry.Key.Text, active.Text, StringComparison.OrdinalIgnoreCase)) SelectNavigation(entry.Value);
                }
            };
            Shown += (s, e) => { overviewWindow.Show(); overviewWindow.WindowState = FormWindowState.Maximized; };
            ResumeLayout(true);
        }

        private void AddSection(FlowLayoutPanel nav, string title, ToolStripMenuItem[] items)
        {
            var label = ModernTheme.Label(title, 7.5f, Color.FromArgb(143, 163, 184), true);
            label.Margin = new Padding(10, 18, 0, 8); nav.Controls.Add(label);
            foreach (var item in items) NavButton(item.Text, item, nav);
        }
        private Button NavButton(string text, ToolStripMenuItem item, FlowLayoutPanel nav)
        {
            var button = new Button { Text = text, Width = 168, Height = 32, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0), Margin = new Padding(0, 1, 0, 1), FlatStyle = FlatStyle.Flat, BackColor = ModernTheme.Navy, ForeColor = Color.FromArgb(211, 222, 234), Font = ModernTheme.Font(9), Cursor = Cursors.Hand };
            button.FlatAppearance.BorderSize = 0; button.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 55, 77);
            if (item != null)
            {
                navigation.Add(item, button);
                button.Click += (s, e) => { if (item.Enabled) { item.PerformClick(); if (ActiveMdiChild != null) SelectNavigation(button); } };
                item.EnabledChanged += (s, e) => button.Enabled = item.Enabled;
            }
            nav.Controls.Add(button); return button;
        }
        private void SelectNavigation(Button selected)
        {
            overviewButton.BackColor = overviewButton == selected ? ModernTheme.Teal : ModernTheme.Navy;
            foreach (var button in navigation.Values) button.BackColor = button == selected ? ModernTheme.Teal : ModernTheme.Navy;
        }
    }
}
