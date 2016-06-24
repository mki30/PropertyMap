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

public enum Language
{
    English,
    Hindi
}

public class BasePage : System.Web.UI.Page
{
    public string Action = "";
    public string Data1 = "";
    public string Data2 = "";
    public string Data3 = "";
    
    public string MenuSelection = "product";

    public BasePage()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    protected override void InitializeCulture()
    {
        if (Request.Form["ctl00$txtLanguage"] != null)
        {
            if (Request.Form["ctl00$txtLanguage"].ToString() != "")
                Global.Culture = Request.Form["ctl00$txtLanguage"].ToString();
        }

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Global.Culture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(Global.Culture);
        base.InitializeCulture();
    }

    public Boolean IsLocalHost
    {
        get
        {
            return Request.Url.Host.ToLower().Contains("localhost");
        }
    }   

    public int GetFormInt(string FieldName)
    {
        NameValueCollection nvc = Request.Form;
        if (nvc[FieldName] != null)
            return Cmn.ToInt(nvc[FieldName]);
        
        return 0;
    }

    public double GetFormDbl(string FieldName)
    {
        NameValueCollection nvc = Request.Form;
        if (nvc[FieldName] != null)
            return Cmn.ToDbl(nvc[FieldName]);
        
        return 0;
    }

    public string GetFormString(string FieldName)
    {
        NameValueCollection nvc = Request.Form;
        if (nvc[FieldName] != null)
            return nvc[FieldName];
        return "";
    }

    public void WriteClientScript()
    {
        string str = "";
        GetAllClientID(this, ref str);
        WriteClientScript(str);
    }

    public string RouteString(string Key, string Default = "")
    {
        return RouteData.Values[Key] != null ? RouteData.Values[Key].ToString() : Default;
    }

    public string QueryString(string Key, string Default = "")
    {
        return Request.QueryString[Key] != null ? Request.QueryString[Key].ToString() : Default;
    }

    public int QueryStringInt(string Key, int Default = 0)
    {
        return Request.QueryString[Key] != null ? Cmn.ToInt( Request.QueryString[Key].ToString()) : Default;
    }

    public string RouteValue(string Name)
    {
        return RouteData.Values[Name] != null ? RouteData.Values[Name].ToString() : "";
    }

    public string GetCookie(string Key)
    {
        HttpCookie c = Request.Cookies.Get(Key);
        return c != null ? c.Value : "";
    }

    public void SetCookie(string Key, string Value)
    {
        HttpCookie c = Response.Cookies.Get(Key);
        if (c != null)
            c.Value = Value;
        else
            Response.Cookies.Add(new HttpCookie(Key, Value));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string Debug = Request.QueryString["Debug"] != null ? Request.QueryString["Debug"].ToString() : "";
        
        Action = RouteData.Values["Action"] != null ? RouteData.Values["Action"].ToString() : "";
        Data1 = RouteData.Values["Data1"] != null ? RouteData.Values["Data1"].ToString() : "";
        Data2 = RouteData.Values["Data2"] != null ? RouteData.Values["Data2"].ToString() : "";
        Data3 = RouteData.Values["Data3"] != null ? RouteData.Values["Data3"].ToString() : "";

        if (!string.IsNullOrEmpty(Request.Headers["Accept-Encoding"]) && false)
        {
            string enc = Request.Headers["Accept-Encoding"].ToUpperInvariant();

            //preferred: gzip or wildcard 
            if (enc.Contains("GZIP") || enc.Contains("*"))
            {
                Response.AppendHeader("Content-encoding", "gzip");
                Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            }
            
            //deflate 
            else if (enc.Contains("DEFLATE"))
            {
                Response.AppendHeader("Content-encoding", "deflate");
                Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
            }
        }
    }

    public void GetAllClientID(Control parent, ref string strCtl)
    {
        foreach (Control ctl in parent.Controls)
        {
            //if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
            if (ctl.ID != null)
                strCtl += "var " + ctl.ID + "=\"#" + ctl.ClientID + "\";\n";
            try
            {
                if (ctl.Controls.Count > 0)
                    GetAllClientID(ctl, ref strCtl);
            }
            catch (Exception Ex)
            {
                string str = Ex.Message;
            }
        }
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
    protected void Page_Prerender(object sender, EventArgs e)
    {
        //string Script = "PageType=" + (int)pageType + ";\n";
        string Script = "Action='" + Action + "';" + Environment.NewLine +
        "ID='" + Data1 + "';" + Environment.NewLine +
        "ID2='" + Data2 + "';" + Environment.NewLine +
        "SelectedMenu='" + MenuSelection + "';"+Environment.NewLine+
        "var BasePath='" + Global.GetRootPathVirtual + "'" + Environment.NewLine;
        WriteClientScript(Script);
    }
    
    public Bitmap ResizeImage(Bitmap src, int newWidth, int newHeight)
    {
        Bitmap result = new Bitmap(newWidth, newHeight);
        using (Graphics Gr = Graphics.FromImage((System.Drawing.Image)result))
        {
            Gr.DrawImage(src, 0, 0, newWidth, newHeight);
        }
        return result;
    }
}