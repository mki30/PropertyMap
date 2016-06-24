using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.IO;

public partial class Edit_ApartmentTypeList : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            City.CityList(lstProjects);
            if (Request.Cookies["CityID"] != null)
                lstProjects.SelectedValue = Request.Cookies["CityID"].Value;
            lstProjects_SelectedIndexChanged(null, null);
        }
    }

    protected void lstProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
    }

    protected void btnReload_Click(object sender, EventArgs e)
    {
        Global.LoadGlobalData();
        
        List<Society> list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue)).OrderBy(m => m.SocietyName).ToList();

        if (!chkShowDeleted.Checked)
            list = list.Where(m => m.Deleted != 1).ToList();

        StringBuilder str = new StringBuilder("<table class='DataTable' style='width:100% auto;'><tr><th><th>ID<td>Project<th>City<th>Sub City<th>Area<th>Logo");
        int ctr = 0;

        foreach (Society s in list)
        {
            str.Append("<tr class='trhead' style='background-color:steelblue;color:white;'><th>" + (++ctr) + "<th>" + s.ID + "<td style='" + (s.Deleted == 1 ? "text-decoration:line-through;color:red" : "") + "' onclick='ShowApartments(" + s.ID + ")'>" + s.SocietyName
                + "<th>" + s.City
                + "<th>" + s.Subcity
                + "<th>" + s.Area+"<th><th>");
            
                foreach (ApartmentType a in s.ApartmentList)
                {
                    str.Append("<tr><td><td>" + a.ID
                        + "<td>" + a.TypeName
                        + "<td>" + a.Bedroom
                        + "<td>" + a.Bathroom
                        + "<td>" + a.SuperArea
                        + (File.Exists(Server.MapPath(@"~\Data\Images_ApartmentType\" + a.ID + ".jpg")) ? "<td>Yes" : "<td style='background-color:yellow'>")
                        );
                }
        }
        ltList.Text = str.ToString() + "</table>";
    }
}