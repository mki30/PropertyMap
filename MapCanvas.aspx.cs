using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MapCanvas : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        
        Page.Title = "Landmarks Near "+QueryString("SocietyName");
        HiddenField1.Value = QueryString("SocietyName");
    }
}