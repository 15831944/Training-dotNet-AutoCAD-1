using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Windows;


namespace WindowPosition
{
    public class Class1
    {
        [CommandMethod("MinMaxApplicationWindow")]
        public static void MinMaxApplicationWindow()
        {
            // Minimize the application window
            Application.MainWindow.WindowState = Window.State.Minimized;

            System.Windows.Forms.MessageBox.Show("Minimized", "MinMax",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.None,
                System.Windows.Forms.MessageBoxDefaultButton.Button1,
                System.Windows.Forms.MessageBoxOptions.ServiceNotification);

            // Maximize the application window
            Application.MainWindow.WindowState = Window.State.Maximized;
            System.Windows.Forms.MessageBox.Show("Maximized", "MinMax");
        }
    }
}
