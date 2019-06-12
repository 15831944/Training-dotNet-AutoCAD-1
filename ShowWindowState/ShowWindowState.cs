using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace ShowWindowState
{
    public class ShowWindowState
    {
        [CommandMethod("ShowCurrentWindowState")]
        public static void ShowCurrentWindowState()
        {
            System.Windows.Forms.MessageBox
                .Show("The application window is " +
                       Application.MainWindow.WindowState.ToString());
        }
    }
}
