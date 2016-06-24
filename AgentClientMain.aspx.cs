using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClientMain : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
            base.Page_Load(sender, e);
            WriteClientScript("PageType=" + (int)PageType.PAGE_CLIENT_MAIN);
    }
}