using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace IterateLayers
{
    public class Class1
    {
        [CommandMethod("IterateLayers")]
        public static void IterateLayers()
        {
            // Get the current document
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            // Get the database
            Database acDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                // Get the layer table for the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acDb.LayerTableId, OpenMode.ForRead) as LayerTable;


                // Step through the layer table and print each layer name
                foreach (ObjectId acObjId in acLyrTbl)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;

                    acDoc.Editor.WriteMessage("\n" + acLyrTblRec.Name);
                }

                acTrans.Commit();
            }
        }

    }
}


