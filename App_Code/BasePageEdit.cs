using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.IO.Compression;
using PropertyListModel;

public class BasePageEdit : System.Web.UI.Page
{
    public BasePageEdit()
    {
        //
        // TODO: Add constructor logic here
        //
        
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string host = HttpContext.Current.Request.Url.Host.ToLower();

        if (host != "localhost")
        {
            if (!Global.IsAdmin)
                Response.Redirect(Global.GetRootPathVirtual + "/Login.aspx");
        }
    }

    public string QueryString(string Key, string Default = "")
    {
        return Request.QueryString[Key] != null ? Request.QueryString[Key].ToString() : Default;
    }

    public void WriteClientScript(string Client_Script)
    {
        ClientScriptManager cs = ClientScript;
        string csname1 = "S1";
        if (!cs.IsClientScriptBlockRegistered(GetType(), csname1))
        {
            StringBuilder cstext2 = new StringBuilder();
            cstext2.Append("<script language='javascript' type=text/javascript> \n");
            cstext2.Append(Client_Script);
            cstext2.Append("</script>");
            cs.RegisterClientScriptBlock(GetType(), csname1, cstext2.ToString(), false);
        }
    }
}