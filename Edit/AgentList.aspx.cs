using System;
using PropertyListModel;

public partial class Edit_AgentList : BasePageEdit
{
    int SellType = 0;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        SellType = Cmn.ToInt(Request.QueryString["SellerType"] != null ? Request.QueryString["SellerType"] : "");

        if (!IsPostBack)
        {
            City.CityList(ddCity,0);
            
            City.CityList(ddCity);
            if (Request.Cookies["CityID"] != null)
                ddCity.SelectedValue = Request.Cookies["CityID"].Value;

            FillCity();
        }
    }
    protected void ddCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Save cookie value For persistence
        Response.Cookies["CityID"].Value = ddCity.SelectedValue;
        Response.Cookies["CityID"].Expires = DateTime.Now.AddDays(1);
        FillCity();
    }

    protected void FillCity()
    {
        Agent.GetUserList(lstAgent, (SellerType)SellType, 1, Cmn.ToInt(ddCity.SelectedValue));
    }
}