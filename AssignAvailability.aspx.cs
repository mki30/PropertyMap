using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;

public partial class AssignAvailability : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        int AgentID = Cmn.ToInt(QueryString("AgentID"));    //6903;
        int ClientID = Cmn.ToInt(QueryString("ClientID"));

        List<Availability> alist = Global.AvailabilityList.Values.Where(m => m.SellerID == AgentID).OrderByDescending(m => m.PostedOnDate).ToList();
        StringBuilder sb = new StringBuilder("<table class='table table-hover table-striped table-condensed'>");
        sb.Append("<thead style='background-color:#F8F8F8;'><tr><th>#</th><th>Society</th><th>BHK</th><th>Price</th><th>Posted On</th><th title='Available From'>Avl From</th><th>Area</th><th>Assign</th></tr><thead>");
        int ctr = 1;
        foreach (Availability A in alist)
        {
            string SocityName = "";
            string UrlName = "";
            string Area = "";
            string price = "-";
            if (A.Society != null)
            {
                SocityName = A.Society.SocietyName;
                UrlName = A.Society.URLName;
                Area = A.Society.Subcity + "-" + A.Society.City;
                price = A.Amount.ToString();
            }

            ApartmentType apt;
            string type = "-";
            

            if (Global.ApartmentTypeList.TryGetValue((int)A.ApartmentTypeID, out apt))// if the apartment type exists in the global list
            {
                type = apt.Bedroom + "B-" + apt.Bathroom + "T";
                //price = apt.BSP.ToString();
            }

            string checkbox = "<input class='chk' type='checkbox' data-id='" + A.ID + "-" + AgentID + "-" + ClientID + "'>";
            if(AsignClient.GetByAllIDs(A.ID, AgentID, ClientID)!=null)
                checkbox = "<input class='chk' type='checkbox' checked='checked' data-id='" + A.ID + "-" + AgentID + "-" + ClientID + "'>";
            sb.Append("<tr><td>" + ctr++ + "</td><td><a>" + SocityName + "</a></td><td>"
            + type + "</td><td>" + price + "</td><td>"
            + Cmn.ToDate(A.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>"
            + Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yy") + "</td>"
            + "<td>" + Area + "</td>"
            + "<td><label class='checkbox' >" + checkbox + "</label></td></tr>");
        }
        ltAssignAvailability.Text =sb.ToString();
     }
}