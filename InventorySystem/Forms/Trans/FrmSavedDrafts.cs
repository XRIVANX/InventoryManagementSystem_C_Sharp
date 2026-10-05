using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Trans
{
    public sealed class FrmSavedDrafts : Form
    {
        private readonly TextBox search = new TextBox { Width = 270 };
        private readonly ComboBox type = new ComboBox { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DataGridView grid = new DataGridView
        {
            Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AllowUserToDeleteRows = false, MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        private readonly Label count = new Label { AutoSize = true, Margin = new Padding(12, 9, 0, 0) };
        private readonly Button open = new Button { Text = "Open Draft", AutoSize = true, Height = 34 };
        private readonly Button remove = new Button { Text = "Remove Draft", AutoSize = true, Height = 34 };
        private readonly Timer searchTimer = new Timer { Interval = 300 };
        private bool ready;
        private bool bindingGrid;
        private bool selectionUpdatePending;
        public event Action<int, string> OpenDraftRequested;
        public event Action<int> DraftRemoved;

        public FrmSavedDrafts()
        {
            Text = "Saved Drafts";
            ClientSize = new Size(1100, 620);
            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Top, Height = 64, Padding = new Padding(16), WrapContents = false,
                AutoScroll = true
            };
            filters.Controls.Add(new Label { Text = "Search", AutoSize = true, Margin = new Padding(0, 7, 8, 0) });
            filters.Controls.Add(search);
            filters.Controls.Add(new Label { Text = "Type", AutoSize = true, Margin = new Padding(16, 7, 8, 0) });
            filters.Controls.Add(type);
            var refresh = new Button { Text = "Refresh", AutoSize = true };
            filters.Controls.Add(refresh);
            var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 62, Padding = new Padding(16, 12, 16, 12) };
            footer.Controls.Add(open);
            footer.Controls.Add(remove);
            footer.Controls.Add(count);
            Controls.Add(grid); Controls.Add(filters); Controls.Add(footer);
            ModernTheme.Apply(this);
            ModernTheme.Button(open, true);
            remove.ForeColor = Color.FromArgb(185, 55, 65);
            open.Enabled = remove.Enabled = false;
            type.Items.AddRange(new object[] { "All Types", "Stock In", "Stock Out", "Transfer" });
            if (Session.IsManager) type.Items.Add("Adjustment");
            type.SelectedIndex = 0;
            search.TextChanged += (s, e) => { searchTimer.Stop(); if (ready) searchTimer.Start(); };
            searchTimer.Tick += (s, e) => { searchTimer.Stop(); RefreshDrafts(); };
            type.SelectedIndexChanged += (s, e) => RefreshDrafts();
            refresh.Click += (s, e) => RefreshDrafts();
            open.Click += (s, e) => OpenSelectedDraft();
            remove.Click += (s, e) => RemoveSelectedDraft();
            grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenSelectedDraft(); };
            grid.SelectionChanged += (s, e) => QueueSelectionUpdate();
            grid.CurrentCellChanged += (s, e) => QueueSelectionUpdate();
            Load += (s, e) => { ready = true; RefreshDrafts(); };
        }

        public void RefreshDrafts()
        {
            if (!ready || IsDisposed) return;
            searchTimer.Stop();
            try
            {
                string[] types = { "", "IN", "OUT", "TRANSFER", "ADJUST" };
                var data = DraftService.Search(search.Text.Trim(), types[Math.Max(0, type.SelectedIndex)]);
                DisplayDrafts(data);
            }
            catch (Exception ex)
            {
                DisplayDrafts(null);
                count.Text = "Unable to load saved drafts. Use Refresh to retry.";
                ErrorHandler.Handle(ex, "Load saved drafts");
            }
        }

        internal void DisplayDrafts(DataTable data)
        {
            bindingGrid = true;
            try
            {
                grid.DataSource = data;
                if (data != null)
                {
                    grid.Columns["TransactionID"].Visible = false;
                    grid.Columns["TransactionType"].Visible = false;
                    grid.Columns["Date"].DefaultCellStyle.Format = "MMM dd, yyyy";
                    count.Text = data.Rows.Count == 0 ? "No saved drafts found." : data.Rows.Count + " saved draft(s)";
                }
            }
            finally
            {
                bindingGrid = false;
                QueueSelectionUpdate();
            }
        }

        private void QueueSelectionUpdate()
        {
            if (bindingGrid || IsDisposed || Disposing || selectionUpdatePending) return;
            if (!IsHandleCreated) { UpdateSelectionActions(); return; }
            // Disabling a focused button during a grid selection event moves focus back
            // into the grid while it is still changing its current cell. Defer that change.
            selectionUpdatePending = true;
            BeginInvoke(new Action(() =>
            {
                selectionUpdatePending = false;
                if (!IsDisposed && !Disposing) UpdateSelectionActions();
            }));
        }

        private void UpdateSelectionActions()
        {
            open.Enabled = remove.Enabled = grid.CurrentRow != null;
        }

        private void OpenSelectedDraft()
        {
            if (grid.CurrentRow == null) return;
            OpenDraftRequested?.Invoke(Convert.ToInt32(grid.CurrentRow.Cells["TransactionID"].Value),
                grid.CurrentRow.Cells["TransactionType"].Value.ToString());
        }

        private void RemoveSelectedDraft()
        {
            if (grid.CurrentRow == null) return;
            int id = Convert.ToInt32(grid.CurrentRow.Cells["TransactionID"].Value);
            string documentNo = grid.CurrentRow.Cells["Document No"].Value.ToString();
            if (MdiParent != null)
                foreach (Form child in MdiParent.MdiChildren)
                    if (child is ISavedDraftEditor editor && editor.TransactionId == id)
                    {
                        MessageBox.Show("Close the transaction form for " + documentNo + " before removing this draft.",
                            "Draft is open", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
            if (MessageBox.Show("Remove saved draft " + documentNo + " and all its items?\n\nThis cannot be undone.",
                "Remove Saved Draft", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            try
            {
                DraftService.Remove(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Remove saved draft");
                RefreshDrafts();
                return;
            }
            RefreshDrafts();
            DraftRemoved?.Invoke(id);
            MessageBox.Show(this, "Draft successfully removed.", "Removed",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) searchTimer.Dispose();
            base.Dispose(disposing);
        }
    }
}
