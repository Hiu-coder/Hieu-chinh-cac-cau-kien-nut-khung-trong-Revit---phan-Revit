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
    public static void Vecot(Document doc, cls_Matbang cls_ , Level baseLevel, cls_CongTrinh ct)
    {
        List<FamilySymbol> l = CreateConcreteColumnSymbol(doc, cls_);
        Level lowerLevel = null;
        using (Transaction trans = new Transaction(doc, "Tạo tầng kết thúc cho cột"))
        {
            trans.Start();
            List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderByDescending(lv => lv.Elevation).ToList();
            lowerLevel = levels.FirstOrDefault(lv => lv.Name == $"Level {cls_.TangKT}");
            if (lowerLevel == null)
            {
                lowerLevel = Level.Create(doc, cls_.CaoDoKt / 304.8);
                lowerLevel.Name = "Level " + Convert.ToString(cls_.TangKT);
            }
            else
            {
                lowerLevel.Elevation = cls_.CaoDoKt / 304.8;
            }
            // Tìm ViewFamilyType cho Floor Plan
            ViewFamilyType viewFamilyType = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().FirstOrDefault(x => x.ViewFamily == ViewFamily.StructuralPlan);

            if (viewFamilyType != null)
            {
                ViewPlan floorPlan = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan)).Cast<ViewPlan>().FirstOrDefault(x => x.Name == baseLevel.Name);
                if (floorPlan == null)
                {
                    floorPlan = ViewPlan.Create(doc, viewFamilyType.Id, lowerLevel.Id);
                    floorPlan.Name = lowerLevel.Name;
                }
            }
            trans.Commit();
        }
            using (Transaction trans = new Transaction(doc, "Create Column"))
        {
            trans.Start();
        
            foreach (var cot in cls_.DSCot)
            {
                FamilySymbol familySymbolcot = l.FirstOrDefault(fs => fs.Name.Equals(cot.Loai.Ten, StringComparison.OrdinalIgnoreCase));
                
                if (!familySymbolcot.IsActive)
                {
                    familySymbolcot.Activate();
                    doc.Regenerate();
                }
                cls_DiemGiao diemgiao = ct.LuoiTrucChung.DiemGiao.FirstOrDefault(fs => fs.Ten.Equals(cot.Diemdat, StringComparison.OrdinalIgnoreCase));
                XYZ point = new XYZ((diemgiao.Toadoxml.X - cot.LechgiaotrucX) / 304.88, (diemgiao.Toadoxml.Y - cot.LechgiaotrucY) / 304.88, 0);
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
                ElementTransformUtils.RotateElement(doc, columnInstance.Id, Line.CreateBound(point, point + XYZ.BasisZ), cot.Goc);
            }


            trans.Commit();
        }
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

        List<FamilySymbol> symbolList = new List<FamilySymbol>();

        using (Transaction tx = new Transaction(doc, "Tạo Beam Symbol mới"))
        {
            tx.Start();

            foreach (var loai in cls_.LoaiCot)
            {
                // Kiểm tra symbol đã tồn tại chưa
                FamilySymbol existingSymbol = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>()
                    .FirstOrDefault(fs => fs.Family.Name == familyName && fs.Name.Equals(loai.Ten, StringComparison.OrdinalIgnoreCase));

                FamilySymbol symbolToUse = existingSymbol;

                if (existingSymbol == null)
                {
                    symbolToUse = baseSymbol.Duplicate(loai.Ten) as FamilySymbol;

                    // Đổi đơn vị từ mm sang internal unit (feet)
                    double widthInternal = UnitUtils.ConvertToInternalUnits(loai.Rong, UnitTypeId.Millimeters);
                    double heightInternal = UnitUtils.ConvertToInternalUnits(loai.Cao, UnitTypeId.Millimeters);

                    Parameter bParam = symbolToUse.LookupParameter("b");
                    Parameter hParam = symbolToUse.LookupParameter("h");

                    if (bParam != null && bParam.StorageType == StorageType.Double)
                        bParam.Set(widthInternal);

                    if (hParam != null && hParam.StorageType == StorageType.Double)
                        hParam.Set(heightInternal);
                }

                symbolList.Add(symbolToUse);
            }

            tx.Commit();
        }

        return symbolList;
    }
}

