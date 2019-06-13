using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace WorkingZoom
{
    public class MyClass
    {
        public static void Zoom(Point3d pMin, Point3d pMax, Point3d pCenter, double dFactor)
        {
            // Get the current document
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            // Get the current database
            Database acDb = acDoc.Database;

            int nCurVport = System.Convert.ToInt32(Application.GetSystemVariable("CVPORT"));

            // Get the extents of the current space when no points
            // or only a center point is provided
            // Check to see if Model space is current
            if (acDb.TileMode == true)
            {
                if(pMin.Equals(new Point3d()) == true &&
                    pMax.Equals(new Point3d()) ==true)
                {
                    pMin = acDb.Extmin;
                    pMax = acDb.Extmax;
                }
            }
            else
            {
                // Check to see if Paper space is current
                if(nCurVport == 1)
                {
                    // Get the extents of Paper space
                    if(pMin.Equals(new Point3d()) == true &&
                        pMax.Equals(new Point3d())  == true)
                    {
                        pMin = acDb.Pextmin;
                        pMax = acDb.Pextmax;
                    }
                }
            }

            // Start a transaction
            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                // Get the current view
                using (ViewTableRecord acView = acDoc.Editor.GetCurrentView())
                {
                    Extents3d eExtents;

                    // Translate WCS coordinates do DCS
                    Matrix3d matWCS2DCS;
                    matWCS2DCS = Matrix3d.PlaneToWorld(acView.ViewDirection);
                    matWCS2DCS = Matrix3d.Displacement(acView.Target - Point3d.Origin) * matWCS2DCS;
                    matWCS2DCS = Matrix3d.Rotation(-acView.ViewTwist,
                                                   acView.ViewDirection,
                                                   acView.Target) * matWCS2DCS;

                    // If a center point is specified, define the min and max
                    // point of the extents
                    // for Center and   Scale modes
                    if(pCenter.DistanceTo(Point3d.Origin) != 0)
                    {
                        pMin = new Point3d(pCenter.X - (acView.Width / 2),
                                           pCenter.Y - (acView.Height / 2),
                                           0);

                        pMax = new Point3d((acView.Width / 2) + pCenter.X,
                                           (acView.Height / 2) + pCenter.Y,
                                           0);
                    }

                    // Create an extents object using line
                    using (Line acLine = new Line(pMin, pMax))
                    {
                        eExtents = new Extents3d(acLine.Bounds.Value.MinPoint,
                                                 acLine.Bounds.Value.MaxPoint);
                    }

                    // Calculate the ratio between the width and height of the current view
                    double dViewRatio;
                    dViewRatio = (acView.Width / acView.Height);

                    // Transform the extents of the view
                    matWCS2DCS = matWCS2DCS.Inverse();
                    eExtents.TransformBy(matWCS2DCS);

                    double dWidth;
                    double dHeigth;
                    Point2d pNewCentPt;

                    // Check to see if a center point was provided(Center and Scale modes)
                    if (pCenter.DistanceTo(Point3d.Origin) != 0)
                    {
                        dWidth = acView.Width;
                        dHeigth = acView.Height;

                        if (dFactor == 0)
                        {
                            pCenter = pCenter.TransformBy(matWCS2DCS);
                        }

                        pNewCentPt = new Point2d(pCenter.X, pCenter.Y);
                    }
                    else // Working in Window, Extents and Limits mode
                    {
                        // Calculate the new width and height of the current view
                        dWidth = eExtents.MaxPoint.X - eExtents.MinPoint.X;
                        dHeigth = eExtents.MaxPoint.Y - eExtents.MinPoint.Y;

                        // Get the center of the view
                        pNewCentPt = new Point2d(((eExtents.MaxPoint.X + eExtents.MinPoint.X) * 0.5),
                                                 ((eExtents.MaxPoint.Y + eExtents.MinPoint.Y) * 0.5));
                    }


                    // Check to see if the new width fits in current window
                    if (dWidth > (dHeigth * dViewRatio)) dHeigth = dWidth / dViewRatio;

                    // Resize and scale the view
                    if(dFactor != 0)
                    {
                        acView.Height = dHeigth * dFactor;
                        acView.Width = dWidth * dFactor;
                    }

                    // Set the center of the view
                    acView.CenterPoint = pNewCentPt;

                    // Set the current view
                    acDoc.Editor.SetCurrentView(acView);
                }

                // Commit the changes
                acTrans.Commit();
            }
        }
    }
}