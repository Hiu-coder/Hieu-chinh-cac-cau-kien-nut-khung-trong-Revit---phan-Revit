using Autodesk.Revit.DB;
using System;
using System.IO;
using System.Xml.Serialization;

public static class md_Xml
{
public static cls_CongTrinh XMLMatBang(string file)
{
    XmlSerializer deserializer = new XmlSerializer(typeof(cls_CongTrinh));
    using (StreamReader reader = new StreamReader(file))
    {
        return (cls_CongTrinh)deserializer.Deserialize(reader);
    }
}
}
