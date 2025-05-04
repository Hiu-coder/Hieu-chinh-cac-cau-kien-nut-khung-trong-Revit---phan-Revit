
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Autodesk.Revit.DB;

[Serializable]
[XmlRoot("LuoiTruc")]
public class cls_LuoiTruc
{
    [XmlElement("TrucDoc")]
    public List<cls_TrucDoc> TrucDoc { get; set; }

    [XmlElement("TrucNgang")]
    public List<cls_TrucNgang> TrucNgang { get; set; }
    [XmlElement("DiemGiao")]
    public List<cls_DiemGiao> DiemGiao { get; set; }

    // Constructor để tránh lỗi null khi khởi tạo
    public cls_LuoiTruc()
    {
        TrucDoc = new List<cls_TrucDoc>();
        TrucNgang = new List<cls_TrucNgang>();
        DiemGiao = new List<cls_DiemGiao>();
    }
}

public class cls_DiemGiao
{
    [XmlElement("Ten")]
    public string Ten { get; set; }
    [XmlElement("ToaDo")]
    public cls_Diem Toadoxml { get; set; }
    
}

public class cls_TrucNgang
{
    [XmlElement("Ten")]
    public string Ten { get; set; }

    [XmlElement("DiemDau")]
    public cls_Diem DiemDau { get; set; }

    [XmlElement("DiemCuoi")]
    public cls_Diem DiemCuoi { get; set; }

    [XmlIgnore]
    public Line Line { get; set; }
}

public class cls_TrucDoc
{
    [XmlElement("Ten")]
    public string Ten { get; set; }

    [XmlElement("DiemDau")]
    public cls_Diem DiemDau { get; set; }

    [XmlElement("DiemCuoi")]
    public cls_Diem DiemCuoi { get; set; }

    [XmlIgnore]
    public Line Line { get; set; }
}

public class cls_Diem
{
    [XmlElement("X")]
    public double X { get; set; }

    [XmlElement("Y")]
    public double Y { get; set; }

    [XmlElement("Z")]
    public double Z { get; set; }

    // Constructor mặc định
    public cls_Diem()
    {
    }

    // Constructor có tham số
    public cls_Diem(double px, double py, double pz)
    {
        X = px;
        Y = py;
        Z = pz;
    }
}
