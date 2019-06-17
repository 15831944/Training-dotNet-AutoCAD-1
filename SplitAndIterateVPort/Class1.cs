using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace SplitAndIterateVPort
{
    public class Class1
    {
        [CommandMethod("SplitAndIterateModelViewports")]
        public static void SplitAndIterateModelViewports()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDb = acDoc.Database;

            // Start transaction
            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                // Open the Viewport table for write
                ViewportTable acVportTbl;
                acVportTbl = acTrans.GetObject(acDb.ViewportTableId,
                                               OpenMode.ForWrite) as ViewportTable;

                // Open the active viewport for write
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                                                   OpenMode.ForWrite) as ViewportTableRecord;

                using (ViewportTableRecord acVportTblRecNew = new ViewportTableRecord())
                {
                    // Add the new viewport to the Viewport table and the transaction
                    acVportTbl.Add(acVportTblRecNew);
                    acTrans.AddNewlyCreatedDBObject(acVportTblRecNew, true);

                    // Assign the name '*Active' to the new Viewport

                    // Use the existing lower left corner for the new viewport
                    acVportTblRecNew.LowerLeftCorner = acVportTblRec.LowerLeftCorner;

                    // Get the half of the X of the existing upper corner
                    acVportTblRecNew.UpperRightCorner = new Point2d(acVportTblRec.UpperRightCorner.X,
                                                                    acVportTblRec.LowerLeftCorner.Y +
                                                                    ((acVportTblRec.UpperRightCorner.Y -
                                                                    acVportTblRec.LowerLeftCorner.Y) / 2));

                    // Recalculate the corner of the active viewport
                    acVportTblRec.LowerLeftCorner = new Point2d(acVportTblRec.LowerLeftCorner.X,
                                                                acVportTblRec.UpperRightCorner.Y);

                    // Update the display with the new tiled viewports arrangement
                    acDoc.Editor.UpdateTiledViewportsFromDatabase();

                    // Step through each object in the symbol table
                    foreach(ObjectId acObjId in acVportTbl)
                    {
                        // Open the object for read
                        ViewportTableRecord acVportTblRecCur;
                        acVportTblRecCur = acTrans.GetObject(acObjId,
                                                             OpenMode.ForRead) as ViewportTableRecord;

                        if (acVportTblRecCur.Name == "*Active")
                        {
                            Application.SetSystemVariable("CVPORT", acVportTblRecCur.Number);

                            Application.ShowAlertDialog("Viewport " + acVportTblRecCur.Number +
                                                        " is now active." +
                                                        "\nLower left corner: " +
                                                        acVportTblRecCur.LowerLeftCorner.X + ", " +
                                                        acVportTblRecCur.LowerLeftCorner.Y +
                                                        "\nUpper right corner: " +
                                                        acVportTblRecCur.UpperRightCorner.X + ", " +
                                                        acVportTblRecCur.UpperRightCorner.Y);
                        }


                    }
                }
                // Commit the changes and dispose of the transaction
                acTrans.Commit();
            }
        }
    }
}
