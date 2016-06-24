using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "PropertyList.in - Easy way to find apartments";
        //images.InnerHtml = GetSocietyImage(343);
    }
    public static string GetSocietyImage(int DataID)
    {
        var Folder = "~/Data/Images_Society/";
        var FileName = "";
        var img = "";
        for (int i = 1; ; i++)
        {   
            string num = DataID.ToString() + "_" + i.ToString();
            FileName = HttpContext.Current.Server.MapPath(Folder + num) + ".jpg";
            if (File.Exists(FileName))
                img += "<img style='border:1px solid #5C9CCC; margin:4px; width:88px; height:69px;' src='Data/Images_Society/" + num + ".jpg'/><a href='#' style='z-index:99999px; position:relative; right:6px; back-color:white; width:0px; height:0px; top:-73px; ' onclick='return DeleteImage(\"Data/Images_Product/" + num + ".jpg\");'>X</a>";
            if (!File.Exists(FileName))
                break;
        }
        return img;
    }
}