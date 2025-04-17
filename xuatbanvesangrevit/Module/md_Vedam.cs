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
    public static void Vedam(Document doc, cls_Matbang cls_)
    {
        List<FamilySymbol> l = CreateConcreteBeamSymbol(doc, cls_);
        using (Transaction trans = new Transaction(doc, "Create Beam"))
        {
            trans.Start();
            Level Levelmb = Level.Create(doc, cls_.CaoDo / 304.8);

            double levelElevation = Levelmb.Elevation;

            
            foreach (var beam in cls_.DSDam)
            {

                FamilySymbol familySymboldam = l.FirstOrDefault(fs => fs.Name.Equals(beam.Ten, StringComparison.OrdinalIgnoreCase));
                
                if (!familySymboldam.IsActive)
                {
                    familySymboldam.Activate();
                    doc.Regenerate();
                }
                XYZ start = new XYZ(beam.Dau.X/304.8, beam.Dau.Y / 304.8, levelElevation);
                XYZ end = new XYZ(beam.Cuoi.X/304.8, beam.Cuoi.Y / 304.8, levelElevation);
                Line beamLine = Line.CreateBound(start, end);

                FamilyInstance beamInstance = doc.Create.NewFamilyInstance(
                    beamLine, familySymboldam, Levelmb, StructuralType.Beam);

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

