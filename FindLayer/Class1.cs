using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace FindLayer
{
    public class FindLayer
    { 
        [CommandMethod("FindMyLayer")]
        public static void FindMyLayer()
        {
            // Get the current document
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            // Get the current database
            Database acDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                // Returns the layer table for the current database
                LayerTable LyrTbl = acTrans.GetObject(acDb.LayerTableId, OpenMode.ForRead) as LayerTable;

                // Check to see if "MyLayer" exists in the layer table
                if (LyrTbl.Has("MyLayer") == false)
                {
                    acDoc.Editor.WriteMessage("\n'MyLayer' does not exist.");
                }
                else
                {
                    acDoc.Editor.WriteMessage("\n'MyLayer' exists.");
                }
                acTrans.Commit();
            }
        }
    }
}