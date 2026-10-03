using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    internal static class ModernTheme
    {
        public static readonly Color Navy = Color.FromArgb(20, 34, 53);
        public static readonly Color Teal = Color.FromArgb(0, 128, 122);
        public static readonly Color Canvas = Color.FromArgb(244, 247, 251);
        public static readonly Color Ink = Color.FromArgb(31, 47, 66);
        public static readonly Color Muted = Color.FromArgb(100, 116, 139);
        public static readonly Color Line = Color.FromArgb(222, 230, 239);
        public static Font Font(float size, bool bold = false) => new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);

        public static Label Label(string text, float size, Color color, bool bold = false)
        {
            return new Label { Text = text, AutoSize = true, Font = Font(size, bold), ForeColor = color, BackColor = Color.Transparent };
        }

        public static void Button(Button button, bool primary = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = primary ? Teal : Color.White;
            button.ForeColor = primary ? Color.White : Ink;
            button.FlatAppearance.BorderColor = Line;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(0, 105, 101) : Canvas;
            button.FlatAppearance.MouseDownBackColor = primary ? Color.FromArgb(0, 88, 84) : Line;
            button.Cursor = Cursors.Hand;
            button.Font = Font(9, true);
        }

        public static void Apply(Form form)
        {
            Branding.ApplyIcon(form);
            form.SuspendLayout();
            form.BackColor = Canvas;
            Style(form);
            form.ResumeLayout(true);
        }

        private static void Style(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                // Keep the original font size and geometry of designer-positioned fields.
                control.Font = Font(control.Font.SizeInPoints, control.Font.Bold);
                if (control is Button button)
                {
                    string name = button.Name.ToLowerInvariant();
                    Button(button, name.Contains("save") || name.Contains("post") || name.Contains("new") || name.Contains("login"));
                    if (name.Contains("delete") || name.Contains("void")) button.ForeColor = Color.FromArgb(185, 55, 65);
                }
                else if (control is DataGridView grid)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Canvas, ForeColor = Muted, Font = Font(9, true), Padding = new Padding(8, 7, 8, 7), SelectionBackColor = Canvas, SelectionForeColor = Ink };
                    grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    grid.ColumnHeadersHeight = 44;
                    grid.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Ink, Font = Font(9), Padding = new Padding(8, 4, 8, 4), SelectionBackColor = Color.FromArgb(218, 241, 237), SelectionForeColor = Ink };
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 253);
                    grid.RowTemplate.Height = 34;
                    grid.GridColor = Line;
                    grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    grid.RowHeadersVisible = false;
                }
                else if (control is TextBox textbox) { textbox.BackColor = Color.White; textbox.ForeColor = Ink; textbox.BorderStyle = BorderStyle.FixedSingle; }
                else if (control is ComboBox combo) { combo.BackColor = Color.White; combo.ForeColor = Ink; }
                else if (control is Label) control.ForeColor = Ink;
                else if (control is GroupBox) { control.ForeColor = Muted; control.BackColor = Color.White; }
                else if (control is Panel) control.BackColor = Canvas;
                Style(control);
            }
        }
    }

    internal sealed class ModernCard : Panel
    {
        public Color Accent { get; set; } = ModernTheme.Teal;
        public ModernCard()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Padding = new Padding(20);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var border = new Pen(ModernTheme.Line))
                e.Graphics.DrawRectangle(border, 0, 0, Math.Max(0, Width - 1), Math.Max(0, Height - 1));
            using (var brush = new SolidBrush(Accent)) e.Graphics.FillRectangle(brush, 0, 0, 4, Height);
        }
    }
}


