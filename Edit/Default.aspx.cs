using System;
public partial class _Default : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);//call for basepage load event first
    }
    
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Global.IsAdmin = false;
        Response.Redirect(Global.GetRootPathVirtual + "/Login.aspx");
    }
    
    protected void btnReload_Click(object sender, EventArgs e)
    {
        Global.LoadGlobalData();
    }
}