using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.IO;

public partial class Edit_PdfDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Society> list = Global.ProjectList.Values.Where(m=>m.Verified == 1).OrderBy(m => m.SocietyName).ToList();
        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed'><thead><tr><th>SN<th>ID<th>Project<th>Download");
        StringBuilder str1 = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed'><thead><tr><th>SN<th>ID<th>Project<th>File Size");
        int ctr=0;
        int ctr1 = 0;
        foreach (Society s in list)
        {
            if (!string.IsNullOrEmpty(s.BrochureURL))
            {
                string ext = System.IO.Path.GetExtension(s.BrochureURL);
                if (ext == ".pdf")
                {
                    if (!System.IO.File.Exists(Server.MapPath(@"/Data/PDF/" + s.URLName + ".pdf")))
                        str.Append("<tr><td>" + (++ctr) + "<td>" + s.ID + "<td>" + s.SocietyName + "<td><a id='download_" + s.ID + "' href='#' onclick='return Download(\"" + s.BrochureURL + "\",\"" + s.URLName + "\",\"" + s.ID + "\")'>Download</a>");
                    else
                    {
                        var f = new FileInfo(Server.MapPath(@"/Data/PDF/" + s.URLName + ".pdf"));
                        double size = (double)f.Length/(1024*1024);
                        str1.Append("<tr><td>" + (++ctr1) + "<td>" + s.ID + "<td><a href='/Data/PDF/" + s.URLName + ".pdf' target='_blank'>" + s.SocietyName + "</a><td>"+size.ToString("0.00")+" Mb");
                    }
                }
            }
        }
        ltList.Text = str.ToString()+"</table>"+str1+"</table>";
    }
}