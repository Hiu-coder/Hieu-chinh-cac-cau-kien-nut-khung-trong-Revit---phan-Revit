using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
public class MainProgram : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Document doc = uiDoc.Document;
        Level baseLevel = null;
        cls_CongTrinh ct = md_Xml.XMLMatBang(@"D:\NCKH\LaybanveCAD_ver2\MATBANG.xml");
        foreach (var mb in ct.CongTrinh)
        {


            using (Transaction trans = new Transaction(doc, "Tạo tầng mặt bằng"))
            {
                trans.Start();
                List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderByDescending(lv => lv.Elevation).ToList();
                baseLevel = levels.FirstOrDefault(lv => lv.Name == $"Level {mb.Tang}");
                if (baseLevel == null)
                {
                    baseLevel = Level.Create(doc, mb.CaoDo / 304.8);
                    baseLevel.Name = "Level " + Convert.ToString(mb.Tang);
                }
                else
                {
                    baseLevel.Elevation = mb.CaoDo / 304.8;
                }


                // Tìm ViewFamilyType cho Floor Plan
                ViewFamilyType viewFamilyType = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().FirstOrDefault(x => x.ViewFamily == ViewFamily.StructuralPlan);

                if (viewFamilyType != null)
                {
                    ViewPlan floorPlan = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan)).Cast<ViewPlan>().FirstOrDefault(x => x.Name == baseLevel.Name);
                    if (floorPlan == null)
                    {
                        floorPlan = ViewPlan.Create(doc, viewFamilyType.Id, baseLevel.Id);
                        floorPlan.Name = baseLevel.Name;
                    }
                }

                trans.Commit();
            }
            md_Veluoi.Veluoi(doc, mb);
            md_Vedam.Vedam(doc, mb, baseLevel);
            md_Vecot.Vecot(doc, mb, baseLevel);
        }
        return Result.Succeeded;
    }
}
