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

        cls_Matbang mb = md_Xml.XMLMatBang(@"D:\NCKH\LaybanveCAD_ver2\MATBANG.xml");
        md_Veluoi.Veluoi(doc, mb);
        md_Vedam.Vedam(doc, mb);
        return Result.Succeeded;
    }
}
