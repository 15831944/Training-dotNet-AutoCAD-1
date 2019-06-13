using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace MakeInvisibleOrVisible
{
    public class MakeInvisibleOrVisible
    {
        [CommandMethod("InvisibleOrVisible")]
        public static void InvisibleOrVisible()
        {
            // Hide the Application window
            Application.MainWindow.Visible = false;
            System.Windows.Forms.MessageBox.Show("The Application is invisible.", "Make visible");

            // Show the Application window
            Application.MainWindow.Visible = true;
            System.Windows.Forms.MessageBox.Show("The Application is visible", "Ok, it's jut it");
        }
    }
}
