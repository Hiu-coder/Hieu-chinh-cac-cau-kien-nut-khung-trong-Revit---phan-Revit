using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
public static class md_Veluoi 
{
    public static void Veluoi(Document doc,cls_Matbang cls_)
    {
        using (Transaction trans = new Transaction(doc, "Create Grid"))
        {
            trans.Start();

            foreach (var truc in cls_.LuoiTruc.TrucDoc)
            {
                CreateGrid(doc, truc.Ten, truc.DiemDau, truc.DiemCuoi);
            }

            foreach (var truc in cls_.LuoiTruc.TrucNgang)
            {
                CreateGrid(doc, truc.Ten, truc.DiemDau, truc.DiemCuoi);
            }
            trans.Commit();
        }
    }
    private static void CreateGrid(Document doc, string name, cls_Diem start, cls_Diem end)
    {
        XYZ pt1 = new XYZ(start.X, start.Y, start.Z);
        XYZ pt2 = new XYZ(end.X, end.Y, end.Z);
        Line gridLine = Line.CreateBound(pt1, pt2);
        Grid grid = Grid.Create(doc, gridLine);
        grid.Name = name;
    }
}

