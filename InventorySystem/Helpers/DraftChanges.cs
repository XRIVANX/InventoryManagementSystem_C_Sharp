using System.Data;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    internal sealed class DraftChanges
    {
        public bool IsDirty { get; private set; } = true;
        public void MarkSaved() { IsDirty=false; }
        public void Watch(DataTable table)
        {
            table.RowChanged+=(s,e)=>IsDirty=true;
            table.RowDeleted+=(s,e)=>IsDirty=true;
            table.TableCleared+=(s,e)=>IsDirty=true;
        }
        public void Watch(Control parent)
        {
            foreach(Control control in parent.Controls)
            {
                if(control is TextBox) control.TextChanged+=(s,e)=>IsDirty=true;
                if(control is ComboBox combo) combo.SelectedValueChanged+=(s,e)=>IsDirty=true;
                if(control is DateTimePicker date) date.ValueChanged+=(s,e)=>IsDirty=true;
                Watch(control);
            }
        }
    }
}
