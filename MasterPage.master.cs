using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //loginhref.ServerClick += new EventHandler(hlnk_ServerClick);
        ////if (!IsPostBack)
        ////{
        //    if(Global.LogInDone != false)
        //        lblUserName.Text = "Welcome! " +(Global.UserName!=""?Global.UserName:"") + " - " + (Global.UserType == 0 ? "Agent" : "User");
        ////}
        //else
        //    lblUserName.Text = "";
        //if(Global.LogInDone==true)
        //lblLogin.Text = "Logout";
        //if (Request.Url.Host.ToLower().Contains("localhost"))
        //{
        //    PanelAdd.Visible = false;
        //    ltAdSpaceHolder.Text = "<div style='width:300px'></div>";
        //}
        //string a = Server.HtmlDecode(Request.Cookies["PriceTrendSelectedCity"].Value.Trim());
    }

    void hlnk_ServerClick(object sender, EventArgs e)
    {
        //if (lblLogin.Text == "Login")
        //    Response.Redirect("Login.aspx");
        //else
        //Response.Redirect("Logout.aspx");
    }
}
