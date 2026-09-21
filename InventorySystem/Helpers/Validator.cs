using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    public static class Validator
    {
        public static bool Required(Control c, ErrorProvider ep, string label)
        {
            if (string.IsNullOrWhiteSpace(c.Text))
            {
                ep.SetError(c, label + " is required.");
                return false;
            }
            ep.SetError(c, "");
            return true;
        }

        public static bool ComboSelected(ComboBox cb, ErrorProvider ep, string label)
        {
            if (cb.SelectedIndex < 0)
            {
                ep.SetError(cb, "Please select a " + label + ".");
                return false;
            }
            ep.SetError(cb, "");
            return true;
        }

        public static bool PositiveNumber(Control c, ErrorProvider ep, string label,
                                          out decimal value, bool allowZero = false)
        {
            value = 0;
            if (!decimal.TryParse(c.Text, out value))
            {
                ep.SetError(c, label + " must be a number.");
                return false;
            }
            if (value < 0 || (!allowZero && value == 0))
            {
                ep.SetError(c, label + (allowZero ? " cannot be negative." : " must be greater than zero."));
                return false;
            }
            ep.SetError(c, "");
            return true;
        }

        public static bool Email(Control c, ErrorProvider ep, bool optional = true)
        {
            if (optional && string.IsNullOrWhiteSpace(c.Text)) { ep.SetError(c, ""); return true; }
            bool ok = Regex.IsMatch(c.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            ep.SetError(c, ok ? "" : "Invalid email address.");
            return ok;
        }

        /// <summary>Attach to a TextBox KeyPress to allow digits and one decimal point only.</summary>
        public static void NumericOnly(object sender, KeyPressEventArgs e)
        {
            var tb = (TextBox)sender;
            if (char.IsControl(e.KeyChar)) return;
            if (char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == '.' && !tb.Text.Contains(".")) return;
            e.Handled = true;
        }
    }
}