using System;
using System.Drawing;
using System.Windows.Forms;
using InventorySystem.Helpers;
using InventorySystem.Services;

namespace InventorySystem.Forms.Auth
{
    public sealed class FrmChangePassword : Form
    {
        private readonly TextBox current = new TextBox { UseSystemPasswordChar=true, Dock=DockStyle.Fill };
        private readonly TextBox password = new TextBox { UseSystemPasswordChar=true, Dock=DockStyle.Fill };
        private readonly TextBox confirm = new TextBox { UseSystemPasswordChar=true, Dock=DockStyle.Fill };
        private readonly ErrorProvider errors = new ErrorProvider();
        public FrmChangePassword()
        {
            Text="Change Password"; ClientSize=new Size(480,370); MinimumSize=Size;
            StartPosition=FormStartPosition.CenterParent; MaximizeBox=false; MinimizeBox=false;
            errors.ContainerControl=this;
            var layout=new TableLayoutPanel { Dock=DockStyle.Fill, Padding=new Padding(28), ColumnCount=1, RowCount=8 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            foreach(int height in new int[]{42,26,38,26,38,26,38,48}) layout.RowStyles.Add(new RowStyle(SizeType.Absolute,height));
            layout.Controls.Add(new Label { Text="Keep your account secure", AutoSize=true, Font=new Font("Segoe UI",16,FontStyle.Bold) });
            layout.Controls.Add(new Label { Text="Current password",AutoSize=true }); layout.Controls.Add(current);
            layout.Controls.Add(new Label { Text="New password (at least 6 characters)",AutoSize=true }); layout.Controls.Add(password);
            layout.Controls.Add(new Label { Text="Confirm new password",AutoSize=true }); layout.Controls.Add(confirm);
            var actions=new FlowLayoutPanel { Dock=DockStyle.Fill, FlowDirection=FlowDirection.RightToLeft };
            var save=new Button { Text="Save password",AutoSize=true }; var cancel=new Button { Text="Cancel",AutoSize=true,DialogResult=DialogResult.Cancel };
            save.Click+=Save; actions.Controls.Add(save);actions.Controls.Add(cancel);layout.Controls.Add(actions);
            Controls.Add(layout); AcceptButton=save;CancelButton=cancel; ModernTheme.Apply(this);
        }
        private void Save(object sender,EventArgs args)
        {
            errors.Clear();
            bool valid=Validator.Required(current,errors,"Current password");
            if(password.Text.Length<6){errors.SetError(password,"Use at least 6 characters.");valid=false;}
            if(password.Text!=confirm.Text){errors.SetError(confirm,"Passwords do not match.");valid=false;}
            if(!valid)return;
            try
            {
                var result=AuthService.ChangePassword(current.Text,password.Text);
                if(!result.Success){errors.SetError(current,result.Message);return;}
                MessageBox.Show(result.Message,"Password updated",MessageBoxButtons.OK,MessageBoxIcon.Information);
                DialogResult=DialogResult.OK;Close();
            }
            catch(Exception ex){ErrorHandler.Handle(ex,"Change password");}
        }
        protected override void Dispose(bool disposing)
        {
            if(disposing)errors.Dispose();base.Dispose(disposing);
        }
    }
}

