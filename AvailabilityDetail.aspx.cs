using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;

public partial class AvailabilityDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Availability A = Global.AvailabilityList.Values.FirstOrDefault(m => m.ID == 538);
        StringBuilder sb=new StringBuilder("<table class='table table-striped'>");
        sb.Append("<tr><td>Posted On :"+Cmn.ToDate(A.PostedOnDate).ToString("dd-MMM-yy")+" </td><td></td></tr>");
        sb.Append("<tr><td>Sociey :"+A.Society.SocietyName+" </td><td>Type : "+GetAptType(Cmn.ToInt(A.ApartmentTypeID))+" </td></tr>");
        sb.Append("<tr><td>Area : "+A.Society.Area+"</td><td>For :"+(A.AvailabilityType.ToString()=="1"?"Sale":"Rent")+"</td></tr>");
        sb.Append("<tr><td>For</td><td>Floor No : "+A.FloorNo+"</td></tr>");
        sb.Append("<tr><td>Price/Rent :"+A.Amount+"</td><td>Avl From :"+Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yy")+"</td></tr>");
        sb.Append("<tr><td>Description</td><td></td></tr>");
        sb.Append("<tr><td colspan='2'>"+A.Description+"</td></tr>");
        
        sb.Append("</table>");
        ltAvaldetails.Text = sb.ToString();
        ltImages.Text = GetImages(A.ID);
    }
    
    private string GetAptType(int ApartmentTypeID)
    {
        ApartmentType AT=Global.ApartmentTypeList.Values.FirstOrDefault(m=>m.ID==ApartmentTypeID);
        string type=(AT.Bedroom+"B - "+AT.Bedroom +"T");
        return type;
    }
    private string GetImages(int ID)
        {
            var img = "";
            var path = "~/Data/Watermark/Images_Society/";
            string[] imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), ID + "_*.*");
            if (imageFiles.Length == 0)
            {
                path = "~/Data/Images_Availability/";
                imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), ID + "_*.*");
            }
            int ctr = 1;
            var isfirst = true;
            foreach (var image in imageFiles)
            {
                if (ctr == 1 && isfirst)
                {
                    img += "<div class='row-fluid'>";
                    isfirst = false;
                }
                img += "<div class='span4' style='margin-top:5px;'><img class='img-polaroid' style='border:1px solid #eee; width:98%; height:230px;' src='" + ResolveClientUrl(path) + Path.GetFileName(image) + "' alt=''/></div>";

                if (ctr == 3)
                {
                    img += "</div>";
                    ctr = 1;
                    isfirst = true;
                }
                if (ctr == 1 && isfirst != true)
                {
                    ctr = 2;
                }
            }
            return img;
        }
}