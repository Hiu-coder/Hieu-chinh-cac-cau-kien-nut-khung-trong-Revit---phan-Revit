
using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("------")]
public class cls_Cot
{
    [XmlElement("LoaiCot")]
    public cls_LoaiCot Loai { get; set; }

    [XmlElement("DiemDat")]
    public string Diemdat { get; set; }

    [XmlElement("LechgiaotrucX")]
    public double LechgiaotrucX { get; set; }

    [XmlElement("LechgiaotrucY")]
    public double LechgiaotrucY { get; set; }

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
