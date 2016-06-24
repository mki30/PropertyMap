using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;

public partial class AgentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder Sb = new StringBuilder("<table class='table table-hover table-striped table-condensed'>");
        Sb.Append("<thead style='background-color:#F8F8F8;'><tr><th>#</th><th>Name</th><th>Mobile No</th><th>EMail</th><th>Address</th></tr><thead>");
        int ctr = 1;
        foreach (KeyValuePair<int, Agent> item in Global.AgentList)
        {
            if (item.Value.City != 1)
            continue;
            Agent A = item.Value;
            Sb.Append("<tr><td>"+ctr+++"</td><td><a href='/dealer/"+A.ID+"'>" + A.AgentName + "</a></td><td>" + A.Mobile1 + "<td>" + A.EmailID + "</td><td>" + A.Address + "</td></tr>");
        }
        Sb.Append("</table>");
        ltAgentList.Text = Sb.ToString();
    }
}