using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    internal static class Branding
    {
        public static PictureBox LogoPicture()
        {
            Bitmap logo;
            using(var stream=typeof(Branding).Assembly.GetManifestResourceStream("InventorySystem.Resources.BrandLogoDark.png"))
            using(var source=Image.FromStream(stream)) logo=new Bitmap(source);
            var picture=new PictureBox { Name="InventorySystemLogo",Image=logo,SizeMode=PictureBoxSizeMode.Zoom,BackColor=ModernTheme.Navy,TabStop=false };
            picture.Disposed+=(sender,args)=>logo.Dispose();
            return picture;
        }

        public static Icon CreateIcon()
        {
            using(var stream=typeof(Branding).Assembly.GetManifestResourceStream("InventorySystem.Resources.AppIcon.ico"))
            using(var icon=new Icon(stream)) return (Icon)icon.Clone();
        }

        public static void ApplyIcon(Form form)
        {
            var icon=CreateIcon();form.Icon=icon;
            form.Disposed+=(sender,args)=>icon.Dispose();
        }
    }
}
