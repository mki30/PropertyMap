using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_BuilderProjectList : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        int BuilderID=Cmn.ToInt(QueryString("builderID").ToString());
        //ltBuilder.Text = QueryString("builderName").ToString();

        List<Society> list = Society.GetByBuilder(BuilderID).OrderBy(m => m.City).ThenBy(m => m.SocietyName).ToList();

        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-bordered table-hover table-striped table-condensed'><tr><th>Project</th><th>City</th></tr>");

        foreach (Society S in list)
        {
            DateTime s = Cmn.ToDate(S.StartDate);
            DateTime f = Cmn.ToDate(S.EndDate);

            str.Append("<tr><td><a>" + S.SocietyName + "<a>"
                //+ (s != Cmn.MinDate ? s.ToString("MMM-yyyy") : "")
                //+ "<td>" + (f != Cmn.MinDate ? f.ToString("MMM-yyyy") : "")
                //+ "<td>" + S.Subcity 
                + "<td>" + S.City);
        }
        ltProjectList.Text = str.ToString() + "</table>";
    }
}