using System;
using System.Collections.Generic;
using PropertyListModel;

public partial class NewLogin : System.Web.UI.Page
{
    protected void Page_PreLoad(object sender, EventArgs e)
    {
        Page.Title = "Login";
        //if (Global.LogInDone == true && Global.UserType == 0)
           //Response.Redirect("dealer/edit");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
        lblResponseMessage.Visible = false;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Label1.Text = "";

        if (txtUserName.Text == "admin@admin.com" && txtPass.Text == "mki")
        {
            Global.IsAdmin = true;
            Response.Redirect(Global.GetRootPathVirtual + "/Edit/Default.aspx");
        }
        else 
        {
            Agent a = Agent.Validate(txtUserName.Text, txtPass.Text);
            if (a != null)
            {
                Global.UserType = Cmn.ToInt(a.UserType != null ? a.UserType : -1);
                Global.UserName = a.AgentName.ToString();
                Global.UserID = a.ID;
                Global.LogInDone = true;
                if (Global.LogInDone == true && Global.UserType == 0)  //Agent case
                {
                    if (Global.FromPage != "")
                        Response.Redirect(Global.FromPage);
                    else
                        Response.Redirect("/"+a.URLName + "/edit");
                }
             }
            else
            {
                Label1.Text = "Wrong userid or password!";
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Visible = true;
                txtUserName.Text = "";
                txtPass.Text = "";
            }
        }
    }
}