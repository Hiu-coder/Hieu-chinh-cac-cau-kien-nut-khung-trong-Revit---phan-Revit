using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Transaction(TransactionMode.Manual)]
public static class md_Vecot
    {
    public static void Vecot(Document doc, cls_Matbang cls_)
    {
        List<FamilySymbol> l = CreateConcreteColumnSymbol(doc, cls_);
        List<Level> levels = new FilteredElementCollector(doc)
        .OfClass(typeof(Level))
        .Cast<Level>()
        .OrderByDescending(lv => lv.Elevation)
        .ToList();

        Level baseLevel = levels.FirstOrDefault(lv => lv.Name == $"Level {cls_.Tang}");
        Level lowerLevel = levels.FirstOrDefault(lv => lv.Name == $"Level {cls_.Tang - 1}");

        if (baseLevel == null || lowerLevel == null)
            throw new InvalidOperationException("Không tìm thấy Level phù hợp.");
        using (Transaction trans = new Transaction(doc, "Create Column"))
        {
            trans.Start();
        


            foreach (var cot in cls_.DSCot)
            {
                FamilySymbol familySymbolcot = GetConcreteColumnSymbol(doc, cot.Ten);
                if (familySymbolcot == null)
                {

                    familySymbolcot = l.FirstOrDefault(fs => fs.Name.Equals(cot.Ten, StringComparison.OrdinalIgnoreCase));
                }
                if (!familySymbolcot.IsActive)
                {
                    familySymbolcot.Activate();
                    doc.Regenerate();
                }
                XYZ point = new XYZ(cot.Diemdat.X, cot.Diemdat.Y, 0);
                FamilyInstance columnInstance = doc.Create.NewFamilyInstance(point, familySymbolcot, baseLevel, StructuralType.Column);
                Parameter topLevelParam = columnInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
                if (topLevelParam != null)
                    topLevelParam.Set(lowerLevel.Id);
                Parameter topOffset = columnInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM);
                if (topOffset != null)
                    topOffset.Set(0);
                Parameter baseOffset = columnInstance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM);
                if (baseOffset != null)
                    baseOffset.Set(0);
            }


            trans.Commit();
        }
    }

    private static FamilySymbol GetConcreteColumnSymbol(Document doc, string familyType)
    {
        string familyName = "M_Concrete-Rectangular-Column";

        return new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .Cast<FamilySymbol>()
            .FirstOrDefault(fs => fs.Family.Name == familyName && fs.Name == familyType);
    }
    private static List<FamilySymbol> CreateConcreteColumnSymbol(Document doc, cls_Matbang cls_)
    {
        string familyName = "M_Concrete-Rectangular-Column";

        FamilySymbol baseSymbol = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .Cast<FamilySymbol>()
            .FirstOrDefault(fs => fs.Family.Name == familyName);

        if (baseSymbol == null)
            throw new InvalidOperationException($"Không tìm thấy family '{familyName}' trong dự án.");

        using (Transaction tx = new Transaction(doc, "Tạo Beam Symbol mới"))
        {
            tx.Start();
            List<FamilySymbol> symbolList = new List<FamilySymbol>();
            foreach (var loai in cls_.LoaiCot)
            {
                FamilySymbol newSymbol = baseSymbol.Duplicate(loai.Ten) as FamilySymbol;

                // Đổi đơn vị từ mm sang internal unit (feet)
                double widthInternal = UnitUtils.ConvertToInternalUnits(loai.Rong, UnitTypeId.Millimeters);
                double heightInternal = UnitUtils.ConvertToInternalUnits(loai.Cao, UnitTypeId.Millimeters);

                Parameter bParam = newSymbol.LookupParameter("b");
                Parameter hParam = newSymbol.LookupParameter("h");

                if (bParam != null && bParam.StorageType == StorageType.Double)
                    bParam.Set(widthInternal);

                if (hParam != null && hParam.StorageType == StorageType.Double)
                    hParam.Set(heightInternal);
                symbolList.Add(newSymbol);
            }
            tx.Commit();
            return symbolList;
        }
    }
}

