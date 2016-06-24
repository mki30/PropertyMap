using PropertyListModel;
using System;
using System.Linq;

public partial class ProjectDetailPage : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        
        Society project = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == Data1.ToLower());
        if (Data1.Length > 0 && project != null)
        {
            Response.StatusCode = 301;
            Response.Redirect("/" + project.URLName.ToLower());
        }
        else
        {
            Response.StatusCode = 301;
            Response.Redirect("/" + project.URLName.ToLower());
        }
    }
}