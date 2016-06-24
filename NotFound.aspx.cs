using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class NotFound : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
   {
       Page.Title = "Page Not Found";
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Response.AddHeader("REFRESH", "10;URL=/");
    }
}