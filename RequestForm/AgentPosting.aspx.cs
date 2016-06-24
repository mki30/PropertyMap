using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.Web.Script.Serialization;

public partial class RequestForm_AgentPosting :BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        string AgentID = Data1.ToString();

        string thead = "";
        string td = "";
        List<Availability> alist = Global.AvailabilityList.Values.Where(m => m.SellerID == Cmn.ToInt(AgentID)).OrderByDescending(m => m.PostedOnDate).ToList();
        StringBuilder sb = new StringBuilder("<table class='table table-striped table-condensed'>");
        sb.Append("<thead><tr><th>#</th><th>Society</th><th>BHK</th><th>Area</th><th>Floor No</th><th>Facing</th><th>Cons Age</th><th>Price</th><th>Posted On</th><th title='Available From'>Avl From</th>" + thead + "</tr><thead>");
        int ctr = 0;
        foreach (Availability A in alist)
        {
            string SocityName = "";
            string UrlName = "";
            if (A.Society != null)
            {
                SocityName = A.Society.SocietyName;
                UrlName = A.Society.URLName;
            }
            ctr++;
            
            sb.Append("<tr><td>" + ctr + "</td><td><a href='/project/"
                + UrlName + "' target='_blank'>"
                + SocityName + "</a></td><td>"
                + A.BHK + "B - " + A.Bathroom + "T</td><td>"
                + A.Area + "</td><td>"
                + A.FloorNo + "</td><td>"
                + A.Facing + "</td><td>"
                + A.AgeOfConstruction + "</td><td>"
                + A.Amount + "</td><td>"
                + Cmn.ToDate(A.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>"
                + Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yy") + "</td><td>"
                + td + "</tr>");
            }
        sb.Append("</table>");
        ltPostings.Text = sb.ToString();
        
        List<Society> sclist = alist.Select(m => m.Society).ToList();
        //var newList = alist.Select(a => new
        //{
        //    Society = a.Society
        //}).ToList();
        // ltData.Text = "<script> var s=AvlList=" + new JavaScriptSerializer().Serialize(sclist) + ";</script>";
     }
}