using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PropertyListModel;

public partial class Edit_BuilderReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Ch = Request.QueryString["Char"] != null ? Request.QueryString["Char"].ToString() : "A";

        SortedDictionary<Char, int> BuilderChar = new SortedDictionary<Char, int>();

        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed table-hover'><tr><th>Builder</th><th>Website</th><th>Projects</th><th>Add</th></tr>");

        foreach (Agent A in Global.BuilderList.Values)
        {
            if (A.AgentCompany != null)
            {
                if (A.AgentCompany.Length > 0)
                {
                    Char C = A.AgentCompany.ToUpper()[0];
                    if (!BuilderChar.ContainsKey(C))
                        BuilderChar.Add(C, 0);

                    BuilderChar[C]++;
                }
                if (A.AgentCompany.StartsWith(Ch))
                {
                    str.Append("<tr><td><a class='fancybox fancybox.iframe' href='EditAgent.aspx?ID=" + A.ID + "&UserType=2'>"
                        + A.AgentCompany + "</a></td><td><a href='" + A.URL + "' target='_blank'>"
                        + A.URL + "</a></td><td><span class='badge badge-info'>" + A.ProjectList.Count + "</span><td><a class='fancybox fancybox.iframe' href='EditSociety.aspx'><i class='icon-plus'></i>");
                }
            }
        }
        string S = "<table><tr class='well'>";
        foreach (Char C in BuilderChar.Keys)
            S += "<td><a href='?Char=" + C + "' title='" + BuilderChar[C] + "'>" + C + "</a>&nbsp;&nbsp;&nbsp;";
        S += "</table>";
        ltChar.Text = S;
        
        ltData.Text=str.ToString()+ "</table>";
    }
}