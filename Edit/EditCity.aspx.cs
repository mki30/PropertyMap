using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public partial class Edit_EditCity : BasePageEdit
{

    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            City.CityList(ddCity);
            
            if (Request.Cookies["CityID"] != null)
                ddCity.SelectedValue = Request.Cookies["CityID"].Value;
            //ddCity.SelectedValue= ddCity.Items.FindByText("City").Value;

            ddCity_SelectedIndexChanged(null, EventArgs.Empty);
        }

        int ID = Cmn.ToInt(ddCity.SelectedItem.Value);
        Scripts(City.CityList(null,ID));
    }

    void Scripts(List<City> list)
    {
        StringBuilder strPoints = new StringBuilder("var Points = [];");
        StringBuilder strLabels = new StringBuilder("var Labels = [];");

        int ctr = 0;
        foreach (City C in list)
        {

            if (C.PolyPoints != null)
            {
                strPoints.Append("Points[" + ctr + "]=[" + C.PolyPoints.TrimEnd('^').Replace("^",",") + "];" + Environment.NewLine);
                strLabels.Append("Labels[" + ctr + "]=['" + C.Name.Trim().Replace("'", "") + "'];" + Environment.NewLine);
            }
            ctr++;
        }
        
        Cmn.WriteClientScript(this, strPoints.ToString() + strLabels.ToString());
    }

    protected void ddCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        City.CityList(lstSubCity1, Cmn.ToInt(ddCity.SelectedItem.Value), chkNotVerified.Checked, chkHasPolygon.Checked);
    }
    protected void chkNotVerified_CheckedChanged(object sender, EventArgs e)
    {
        ddCity_SelectedIndexChanged(null, EventArgs.Empty);
    }
    protected void chkHasPolygon_CheckedChanged(object sender, EventArgs e)
    {
        ddCity_SelectedIndexChanged(null, EventArgs.Empty);
    }
}