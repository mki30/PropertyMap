using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class PostProperty : BasePage
{
    public int SocietyID;
    public string City = "Ghaziabad";
    protected new void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Global.LogInDone == false)
                Response.Redirect("Login.aspx?status=post");
            var SellerID = Global.UserID;
            var SellerType = Global.UserType;
            WriteClientScript("societyId =" + SocietyID + ", city ='" + ddlCity.SelectedValue + "' , area ='" + ddlArea.SelectedValue + "',sellerId="+Global.UserID+",sellerType="+Global.UserType+";");
        }
    }
    
    protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
    {
        WriteClientScript("societyId =" + SocietyID + ";");
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        WriteClientScript("city =" +ddlArea.SelectedValue + ";");
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        WriteClientScript("area =" +ddlArea.SelectedValue+ ";");
    }
}