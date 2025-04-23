
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
[Serializable]
public class cls_CongTrinh
{
    [XmlElement("MatBang")]
    public List<cls_Matbang> CongTrinh { get; set; }

    public cls_CongTrinh()
    {
        CongTrinh = new List<cls_Matbang>();
    }
}
[Serializable]
[XmlRoot("-----------MặtBằng-----------")]
public class cls_Matbang
{
    [XmlElement("Tầng")]
    public int Tang { get; set; }

    [XmlElement("CaoĐộ")]
    public double CaoDo { get; set; }
    [XmlElement("TầngKetThuc")]
    public int TangKT { get; set; }

    [XmlElement("CaoĐộKetThuc")]
    public double CaoDoKt { get; set; }

    [XmlElement("DanhSáchDầm")]
    public List<cls_Dam> DSDam { get; set; }

    [XmlElement("CácLoạiDầm")]
    public List<cls_LoaiDam> LoaiDam { get; set; }

    [XmlElement("DanhSáchCột")]
    public List<cls_Cot> DSCot { get; set; }

    [XmlElement("CácLoạiCot")]
    public List<cls_LoaiCot> LoaiCot { get; set; }

    [XmlElement("LướiTrục")]
    public cls_LuoiTruc LuoiTruc { get; set; }

    public cls_Matbang()
    {
        LuoiTruc = new cls_LuoiTruc();
        DSDam = new List<cls_Dam>();
        LoaiDam = new List<cls_LoaiDam>();
        DSCot = new List<cls_Cot>();
        LoaiCot = new List<cls_LoaiCot>();
    }
}
