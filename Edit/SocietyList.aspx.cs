using System;
using PropertyListModel;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;

public partial class Edit_SocietyList : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            City.CityList(ddlCity);
            if (Request.Cookies["CityID"] != null)
                ddlCity.SelectedValue = Request.Cookies["CityID"].Value;
        }
        ShowList();
    }

    void ShowList()
    {
        int CityID = Cmn.ToInt(ddlCity.SelectedValue);
        int SubCityID = Cmn.ToInt(ddSubCity.SelectedValue);

        List<Society> SocietyList = SubCityID == 0 ? Society.GetByCity(CityID) : Society.GetBySubCity(SubCityID);

        lstSociety.Items.Clear();
        foreach (Society S in SocietyList)
        {
            Boolean AddRecord = true;

            if (chkVerified.Checked && (S.Verified == 0 || S.Verified == null))
                AddRecord = false;

            if (S.Deleted == 1)
                AddRecord = false;

            if (chkDeleted.Checked)
                AddRecord = S.Deleted== 1;

            if (AddRecord)
                lstSociety.Items.Add(new ListItem(S.SocietyName, S.ID.ToString()));
        }
    }

    protected void chkVerified_CheckedChanged(object sender, EventArgs e)
    {
        ShowList();
    }

    protected void chkDeleted_CheckedChanged(object sender, EventArgs e)
    {
        ShowList();
    }
    
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCityList();
        Response.Cookies["CityID"].Value = ddlCity.SelectedValue;
        Response.Cookies["CityID"].Expires = DateTime.Now.AddDays(1);
    }

    protected void FillCityList()
    {
        City.CityList(ddSubCity, Cmn.ToInt(ddlCity.SelectedValue));
        ddSubCity.Items.Insert(0, new ListItem("<All>", "0"));
    }

    protected void ddSubCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowList();
    }
}