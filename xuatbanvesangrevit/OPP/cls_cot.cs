
using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("------")]
public class cls_Cot
{
    [XmlElement("Ten")]
    public string Ten { get; set; }

    [XmlElement("Diemdat")]
    public cls_Diem Diemdat { get; set; }

    [XmlElement("Lechgiaotruc")]
    public cls_Lech Lechgiaotruc { get; set; }
    [XmlElement("Goc")]
    public double Goc { get; set; }
}

[Serializable]
public class cls_LoaiCot
{
    [XmlElement("Tên")]
    public string Ten { get; set; }

    [XmlElement("Cao")]
    public double Cao { get; set; }

    [XmlElement("Rộng")]
    public double Rong { get; set; }

    public cls_LoaiCot()
    {
    }
}

[Serializable]
public class cls_Lech
{
    [XmlElement("X")]
    public double X { get; set; }

    [XmlElement("Y")]
    public double Y { get; set; }
}
