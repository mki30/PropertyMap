using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;

public partial class Edit_AgentAvailability : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int AgentID = Request.QueryString["ID"] != null ? Cmn.ToInt(Request.QueryString["ID"]) : 0;

        List<Availability> lst = Availability.GetAvlByAgentID(AgentID);

        int ctr = 1;
        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed'><tr><th>SN<th>ID<th>BHK<th>Toil<th>Area<th>Amount<th>Updated On");
        foreach (Availability A in lst)
        {
            str.Append("<tr><td>" + ctr++ + "<td>" + A.ID + "<td>" + A.BHK + "<td>" + A.Bathroom + "<td>" + A.Area + "<td>" + A.Amount + "<td>" + Cmn.ToDate(A.LUDate).ToString("dd-MMM-yy hh:mm tt"));
        }
        str.Append("</table>");
        ltAvlList.Text = str.ToString();
    }
}