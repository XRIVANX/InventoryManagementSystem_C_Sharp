using System;
using System.Data;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    internal interface ISavedDraftEditor
    {
        int TransactionId { get; }
        event EventHandler TransactionChanged;
    }

    internal static class SavedDraftEditor
    {
        public static void RestoreLines(DataTable destination, DataTable saved)
        {
            destination.Clear();
            foreach (DataRow source in saved.Rows)
            {
                var row = destination.NewRow();
                foreach (DataColumn column in destination.Columns)
                    if (column.Expression.Length == 0) row[column.ColumnName] = source[column.ColumnName];
                destination.Rows.Add(row);
            }
        }

        public static void SelectSavedValue(ComboBox combo, object id, object name)
        {
            if (id == DBNull.Value || id == null) { combo.SelectedIndex = -1; return; }
            var table = (DataTable)combo.DataSource;
            bool found = false;
            foreach (DataRow row in table.Rows)
                if (Equals(row[combo.ValueMember], id)) { found = true; break; }
            // Keep saved references visible even if they were subsequently deactivated.
            if (!found)
            {
                var row = table.NewRow();
                row[combo.ValueMember] = id;
                row[combo.DisplayMember] = name;
                table.Rows.Add(row);
            }
            combo.SelectedValue = id;
        }

        public static void SelectSavedReason(ComboBox combo, object reason)
        {
            string text = Convert.ToString(reason);
            if (text.Length == 0) { combo.SelectedIndex = -1; return; }
            if (!combo.Items.Contains(text)) combo.Items.Add(text);
            combo.SelectedItem = text;
        }
    }
}
