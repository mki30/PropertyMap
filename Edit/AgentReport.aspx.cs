using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_AgentReport : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            City.CityList(lstCity);
            if (Request.Cookies["CityID"] != null)
                lstCity.SelectedValue = Request.Cookies["CityID"].Value;
       }
    }
    protected void lstCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowList();
    }

   void ShowList()
    {
        List<Agent> lst = Agent.GetByCity(Cmn.ToInt(lstCity.SelectedValue), 0);
        if (chkVerified.Checked)
            lst = lst.Where(m => m.Varified == 1 &&  m.Deleted == 0).OrderBy(m => m.AgentName).ToList();
        else if (ChkDeleted.Checked)
            lst = lst.Where(m => m.Deleted == 1).OrderBy(m => m.AgentName).ToList();
        
        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed'><tr><th>SN<th>ID<th>Agent<th>Company<th>Mobile<th><th>Add Avl<th>Updated On");
        int ctr = 1;
        foreach (Agent A in lst)
        {
            str.Append("<tr style='" + (A.Deleted == 1 ? "text-decoration:line-through;color:red;" : "") + ";'><td>" + ctr++ + "<td>" + A.ID + "<td ><a class='fancybox1 fancybox.iframe' href='EditAgent.aspx?ID=" + A.ID + "&ShowList=1' style='" + (A.Deleted == 1 ? "text-decoration:line-through;color:red;" : "") + ";' >" + A.AgentName + "</td>"
                + "<td><a class='fancybox1 fancybox.iframe' href='EditAgent.aspx?ID=" + A.ID + "&ShowList=1'>" + A.AgentCompany
                + "<a><td>" + A.Mobile1 + "<td><a class='fancybox1 fancybox.iframe' href='AgentAvailability.aspx?ID=" + A.ID+"'><i class='icon-briefcase'></i></a><td><a class='fancybox1 fancybox.iframe' href='EditAvailability.aspx?AgentID=" + A.ID + "'>Add<a><td>" + Cmn.ToDate(A.LUDate).ToString("dd-MMM-yy hh:mm tt"));
        }
        ltList.Text = str.ToString() + "</table>";
    }
    
    protected void chkVerified_CheckedChanged(object sender, EventArgs e)
    {
        ShowList();
    }
    
    protected void ChkDeleted_CheckedChanged(object sender, EventArgs e)
    {
        ShowList();
    }
    
    protected void ChkAll_CheckedChanged(object sender, EventArgs e)
    {
        ShowList();
    }
}