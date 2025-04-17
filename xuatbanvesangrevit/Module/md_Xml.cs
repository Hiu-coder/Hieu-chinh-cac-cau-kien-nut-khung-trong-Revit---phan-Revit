using Autodesk.Revit.DB;
using System;
using System.IO;
using System.Xml.Serialization;

public static class md_Xml
{
public static cls_Matbang XMLMatBang(string file)
{
    XmlSerializer deserializer = new XmlSerializer(typeof(cls_Matbang));
    using (StreamReader reader = new StreamReader(file))
    {
        return (cls_Matbang)deserializer.Deserialize(reader);
    }
}
}
