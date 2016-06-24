using System;
using System.IO;
using System.Net;
using PropertyListModel;

public partial class Downloads_Availibility : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtData.Text = Server.HtmlEncode(File.ReadAllText(Server.MapPath(@"~\Downloads\SearchResult.htm")));
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtResult.Text == "")
            return;

        string[] Lines = txtResult.Text.Split('~');

        int ctr = 0;
        foreach (string s in Lines)
        {
            ctr++;
            string filename=Server.MapPath(@"~\Downloads\Data\" + ctr + ".htm");
            if(!File.Exists(filename))
                new WebClient().DownloadFile(s,filename);

            if(ctr>10)
                break;
        }
       File.WriteAllLines(Server.MapPath(@"~\Downloads\SearchResult.txt"), Lines);
    }
}