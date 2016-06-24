using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditQueAns : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        StringBuilder sb = new StringBuilder("<table class='table table-striped table-condensed' style='width:60%; font-size:12px;'><tr><th>ID</th><th>ProjectID</th><th>Subject</th><th>Question</th><th>++</th><th>PostedOn</th><th>Answer</th></tr>");
        List<ProjectQuestion> PQ = ProjectQuestion.GetAll().OrderByDescending(m=>m.LUDate).ToList();
        foreach (ProjectQuestion p in PQ)
        {
            string lnkText=string.IsNullOrEmpty(p.Answer) ? "Ans" : "Edit";
          
            string link = "<a href='./EditQueAns.aspx?ID=" + p.ID + "' class='fancybox1 fancybox.iframe'>" + lnkText + "</a>";
            sb.Append("<tr><td>" + p.ID + "</td><td>" + p.ProjectID + "</td><td style='width:200px;'>" + p.Subject + "</td><td>" + p.Question
                + "</td><td>" +link
                + "</td><td style='width:70px;'>" + Cmn.ToDate(p.LUDate).ToString("dd-MMM-yy") + "</td><td style='width:300px;'>" + p.Answer + "</td></tr>");
        }
        sb.Append("</table>");
        ltDetail.Text = sb.ToString();
    }
}