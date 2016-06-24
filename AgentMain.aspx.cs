using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AgentMain : BasePage
{
    public int CSVar;
    protected new void Page_Load(object sender, EventArgs e)
    {
        if (Global.LogInDone == true && Global.UserType == 0)
        {
            base.Page_Load(sender, e);
            string script = "PageType=" + (int)PageType.Agent_Main + "\n";
            WriteClientScript(script);
            CSVar = Cmn.ToInt(Global.UserID);
        }
        else
        {
            //Response.Write("Not Loged In as a Agent!");
            //lblWait.Text = "Wait";
            Response.Redirect("Login.aspx?status=agent");
        }
    }
}