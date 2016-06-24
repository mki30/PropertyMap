using PropertyListModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenerateKML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Data1 = RouteData.Values["Data1"] != null ? RouteData.Values["Data1"].ToString() : "";

        if (Data1 != "")
        {
            Society society = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == Data1.ToLower());
            if (society != null)
            {
                FileInfo file = new FileInfo(Server.MapPath(society.CreateKML()));
                Stream s = File.OpenRead(file.FullName);
                Byte[] buffer = new Byte[s.Length];
                try
                {
                    s.Read(buffer, 0, (Int32)s.Length);
                }
                finally { s.Close(); }

                Response.ClearHeaders();
                Response.ClearContent();
                
                //Response.ContentType = "application/vnd.google-earth.kml";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.BinaryWrite(buffer);
            }
        }
    }
}