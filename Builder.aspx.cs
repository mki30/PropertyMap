using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;
using System.Text;

public partial class Builder : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        //Agent builder = Global.BuilderList.Values.FirstOrDefault(m => m.AgentName.Replace(" ", "-").ToLower() == Data1);
        Agent builder = Global.BuilderList.Values.FirstOrDefault(m => m.URLName == Data1);
        if (Data1.Length > 0 && builder != null)
        {
            Response.StatusCode = 301;
            Response.Redirect("/" + builder.URLName.ToLower());
        }
        else
        {
            Response.StatusCode = 301;
            Response.Redirect("/" + builder.URLName.ToLower());
        }
    }
}