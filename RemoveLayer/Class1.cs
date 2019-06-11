using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace RemoveLayer
{
    public class Class1
    {
        [CommandMethod("RemoveMyLayer")]
        public static void RemoveMyLayer()
        {
            // Get the current document
            Document acCurDoc = Application.DocumentManager.MdiActiveDocument;
            // Get the current database
            Database acCurDb = acCurDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Returns the layer table for the current database
                LayerTable acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, 
                                                        OpenMode.ForRead) as LayerTable;

                // Check to see if MyLayer exists in the layer table
                if (acLyrTbl.Has("MyLayer") == true)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acLyrTbl["MyLayer"],
                        OpenMode.ForWrite) as LayerTableRecord;

                    try
                    {
                        acLyrTblRec.Erase();
                        acCurDoc.Editor.WriteMessage("\n'MyLayer' was erased.");

                        // Commit the change
                        acTrans.Commit();
                    }
                    catch 
                    {
                        acCurDoc.Editor.WriteMessage("\n'MyLayer' could not be erased.");
                    }
                }
                else
                {
                    acCurDoc.Editor.WriteMessage("\n'MyLayer' does not exist.");
                }
            }
        }
    }
}
