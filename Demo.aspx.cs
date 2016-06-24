using System;
//using PdfToText;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;
using PropertyListModel;
using System.Collections.Generic;
using PropertyListModel;

public partial class Demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string s = "Dr. Neha G. Goel (Indirapuram)";
        //string s2 = s.Replace("(Indirapuram)","").Trim();
        StringBuilder sb=new StringBuilder("<table>");
        List<ImagesDetail> ids=ImagesDetail.GetList();
        foreach(ImagesDetail id in ids)
        {
            sb.Append("<tr><td>" + id.ID + "</td><td>" + id.ImageID + "<td>"+id.ReferenceID+"</td><td>"+id.ImageReferenceType+"<td><td>" + id.UrlName + "");
        }
        imgData.InnerHtml = sb.ToString();

        //StringBuilder sb1 = new StringBuilder("<table>");
        //Society s = Society.GetByID(403);
        //sb1.Append(s.SocietyName + s.City + s.CityID);
        //Society s2 = Society.GetByID(250);
        //sb1.Append("<br/>"+s2.SocietyName + s2.City + s2.CityID);
        //Error_Society.InnerHtml =sb1.ToString() ;
    }
}

