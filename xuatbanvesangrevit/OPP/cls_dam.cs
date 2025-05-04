using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("--------")]
public class cls_Dam
{
    [XmlElement("LoaiDam")]
    public cls_LoaiDam Loaidam { get; set; }

    [XmlElement("TrucXet")]
    public string Trucxet { get; set; }

    [XmlElement("LechTrucY1")]
    public double LechTrucY1 { get; set; }

    [XmlElement("LechTrucY2")]
    public double LechTrucY2 { get; set; }

    [XmlElement("LechTrucX1")]
    public double LechTrucX1 { get; set; }

    [XmlElement("LechTrucX2")]
    public double LechTrucX2 { get; set; }

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

