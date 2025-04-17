using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("--------")]
public class cls_Dam
{
    [XmlElement("Tên")]
    public string Ten { get; set; }

    [XmlElement("Đầu")]
    public cls_Diem Dau { get; set; }

    [XmlElement("Cuối")]
    public cls_Diem Cuoi { get; set; }

    [XmlElement("Rộng")]
    public double Rong { get; set; }

    [XmlElement("Cao")]
    public double Cao { get; set; }

    [XmlElement("Lệchtrục")]
    public double LechTruc { get; set; }

    public cls_Dam()
    {
    }
}

[Serializable]
public class cls_LoaiDam
{
    [XmlElement("Tên")]
    public string Ten { get; set; }

    [XmlElement("Cao")]
    public double Cao { get; set; }

    [XmlElement("Rộng")]
    public double Rong { get; set; }

    public cls_LoaiDam()
    {
    }
}
