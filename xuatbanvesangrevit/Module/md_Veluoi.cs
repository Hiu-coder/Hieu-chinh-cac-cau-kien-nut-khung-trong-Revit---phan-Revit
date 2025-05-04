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
    public static void Veluoi(Document doc,cls_Matbang cls_,cls_CongTrinh ct)
    {
        using (Transaction trans = new Transaction(doc, "Create Grid"))
        {
            trans.Start();

            foreach (var truc in ct.LuoiTrucChung.TrucDoc)
            {
                CreateGrid(doc, truc.Ten, truc.DiemDau, truc.DiemCuoi);
            }

            foreach (var truc in ct.LuoiTrucChung.TrucNgang)
            {
                CreateGrid(doc, truc.Ten, truc.DiemDau, truc.DiemCuoi);
            }
            trans.Commit();
        }
    }
    private static void CreateGrid(Document doc, string name, cls_Diem start, cls_Diem end)
    {
        // Kiểm tra xem Grid đã tồn tại chưa
        if (GridNameExists(doc, name))
            return; // Nếu đã có thì không tạo nữa

        XYZ pt1 = new XYZ(start.X / 304.8, start.Y / 304.8, start.Z / 304.8);
        XYZ pt2 = new XYZ(end.X / 304.8, end.Y / 304.8, end.Z / 304.8);
        Line gridLine = Line.CreateBound(pt1, pt2);
        Grid grid = Grid.Create(doc, gridLine);
        grid.Name = name;
    }
    private static bool GridNameExists(Document doc, string name)
    {
        FilteredElementCollector collector = new FilteredElementCollector(doc)
            .OfClass(typeof(Grid));
        foreach (Grid g in collector)
        {
            if (g.Name == name)
                return true;
        }
        return false;
    }
}

