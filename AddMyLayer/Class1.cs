using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

[assembly: CommandClass(typeof(AddMyLayer.Class1))]

namespace AddMyLayer
{
    public class Class1
    {
        [CommandMethod("AddMyLayer")]
        public void AddMyLayerCommand()
        {
            // Get the current document
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            // Get the database
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Returns the layer table from the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                    OpenMode.ForRead) as LayerTable;

                // Check to see if MyLayer exists in the Layer Table
                if(acLyrTbl.Has("MyLayer") != true)
                {
                    // Open the Layer Table for write
                    acLyrTbl.UpgradeOpen();

                    // Create a new layer table record and name the layer "My Layer"
                    using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                    {
                        acLyrTblRec.Name = "MyLayer";

                        // Add the new layer table record to the layer table and the transaction
                        acLyrTbl.Add(acLyrTblRec);
                        acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);
                    }

                    // Commit the changes
                    acTrans.Commit();
                }
            }
        }

    }
}
