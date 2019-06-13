using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Windows;

namespace SizeDocumentWindow
{
    public class Class1
    {
        [CommandMethod("SizeWindow")]
        public static void SizeWindow()
        {
            // Get the current document
            Document acDoc = Application.DocumentManager
                            .MdiActiveDocument;

            // Works around what looks to be a refresh problem
            // with the application window
            acDoc.Window.WindowState = Window.State.Normal;

            // Set the position of the Document window
            System.Windows.Point ptDoc = new System.Windows.Point(50, 50);
            acDoc.Window.DeviceIndependentLocation = ptDoc;

            // Set the size of the Document window
            System.Windows.Size szDoc = new System.Windows.Size(400, 400);
            acDoc.Window.DeviceIndependentSize = szDoc;
        }
    }
}
