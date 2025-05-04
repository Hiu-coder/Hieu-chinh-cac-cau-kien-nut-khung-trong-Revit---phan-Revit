using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
using System;

[Transaction(TransactionMode.Manual)]
public static class md_Vedam
{
    public static void Vedam(Document doc, cls_Matbang cls_,Level baseLevel,cls_CongTrinh ct)
    {
        List<FamilySymbol> l = CreateConcreteBeamSymbol(doc, cls_);
        using (Transaction trans = new Transaction(doc, "Create Beam"))
        {
            trans.Start();
            var cd = baseLevel.Elevation;
            foreach (var beam in cls_.DSDam)
            {

                FamilySymbol familySymboldam = l.FirstOrDefault(fs => fs.Name.Equals(beam.Loaidam.Ten, StringComparison.OrdinalIgnoreCase));
                
                if (!familySymboldam.IsActive)
                {
                    familySymboldam.Activate();
                    doc.Regenerate();
                }
                XYZ start = null;
                    XYZ end=null;
                var trucxetdoc = ct.LuoiTrucChung.TrucDoc.FirstOrDefault(fs => fs.Ten.Equals(beam.Trucxet, StringComparison.OrdinalIgnoreCase));
                if (trucxetdoc != null)
                {
                     start=new XYZ((trucxetdoc.DiemDau.X - beam.LechTrucX1) / 304.88, (trucxetdoc.DiemDau.Y - beam.LechTrucY1) / 304.88, cd);
                     end = new XYZ((trucxetdoc.DiemCuoi.X - beam.LechTrucX2) / 304.88, (trucxetdoc.DiemCuoi.Y - beam.LechTrucY2) / 304.88, cd);
                }
                else
                {
                    var trucxetngang = ct.LuoiTrucChung.TrucNgang.FirstOrDefault(fs => fs.Ten.Equals(beam.Trucxet, StringComparison.OrdinalIgnoreCase));
                    start = new XYZ((trucxetngang.DiemDau.X- beam.LechTrucX1 )/ 304.88, (trucxetngang.DiemDau.Y - beam.LechTrucY1) / 304.88, cd);
                    end = new XYZ((trucxetngang.DiemCuoi.X - beam.LechTrucX2) / 304.88, (trucxetngang.DiemDau.Y - beam.LechTrucY2) / 304.88, cd);
                }
                
                Line beamLine = Line.CreateBound(start, end);

                FamilyInstance beamInstance = doc.Create.NewFamilyInstance(
                    beamLine, familySymboldam, baseLevel, StructuralType.Beam);

            }


            trans.Commit(); 
        }
    }

    private static List<FamilySymbol> CreateConcreteBeamSymbol(Document doc, cls_Matbang cls_)
    {
        string familyName = "M_Concrete-Rectangular Beam";

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

            foreach (var loai in cls_.LoaiDam)
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

