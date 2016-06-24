using PropertyListModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public partial class UserControl_AgentEdit : System.Web.UI.UserControl
{
    public Agent agent;
    System.Web.UI.Page ParentPage;
    public string Lat = "", Lng = "", Edit = "0";
    public string Title = "";

    public void InitCtl(System.Web.UI.Page _page, Agent _agent)
    {
        ParentPage = _page;
        agent = _agent;

        if (!IsPostBack)
        {
            if (Global.LogInDone = false || Global.UserType != 0)
            {
                Global.FromPage = "/" + agent.URLName.ToLower() + "/edit";
                Response.Redirect("/login");
            }
        }
        Edit = "1";
        Title = agent.AgentName;
        if (agent.Lat != null)
        {
            Lat = agent.Lat.ToString();
            Lng= agent.Lng.ToString();
        }
        ShowEdit(agent);
    }

    private void ShowEdit(Agent A)           //Show Edit
    {
        //Panel1.Visible = false;
        addNewPost.HRef = "/AgentAvailability.aspx?ID=0&AgentID=" + A.ID;
        addNewClient.HRef = "/AgentClientEdit.aspx?ClientID=0&AgentID=" + A.ID;
        spnAgentID.InnerHtml = agent.ID.ToString();
        spnEmailID.InnerText = A.EmailID;
        txtCompanyName.Value = A.AgentCompany;
        City.CityList(ddCity);
        ddCity.SelectedValue = agent.City.ToString();
        txtAgentName.Value = A.AgentName;
        txtAddress.Value = A.Address;
        txtPhone1.Value = A.PhoneNo1;
        txtPhone2.Value = A.PhoneNo2;
        txtMobile1.Value = A.Mobile1;
        txtDetails.Value = A.BuilderDescription;
        txtLat.Value = A.Lat.ToString();
        txtLng.Value = A.Lng.ToString();
        ltAgentPostingEdit.Text = GetAgentPosting(A);
        ShowAgentClients(A.ID);
    }

    private string GetAgentPosting(Agent agent)
    {
        string td = "";
        List<Availability> alist = Global.AvailabilityList.Values.Where(m => m.SellerID == agent.ID).OrderByDescending(m => m.PostedOnDate).ToList();
        StringBuilder sb = new StringBuilder("<table class='table table-hover table-striped table-condensed'>");
        sb.Append("<thead style='background-color:#F8F8F8;'><tr><th>#</th><th>Society</th><th>BHK</th><th>For</th><th>Price</th><th>Posted On</th><th title='Available From'>Avl From</th><th>Area</th><th>Edit</th></tr><thead><tbody>");
        int ctr = 0;
        foreach (Availability A in alist)
        {
            string SocityName = "";
            string UrlName = "";
            string Area = "";
            string Price = "-";
            string avlFor = "-";
            if (A.Society != null)
            {
                SocityName = A.Society.SocietyName;
                UrlName = A.Society.URLName;
                Area = A.Society.Subcity + "-" + A.Society.City;
                Price = Cmn.ToInt(A.Amount).ToString("##,##0");
                avlFor = ((int)A.AvailabilityType).ToString() == "1" ? "sale" : "rent";
            }
            ctr++;
            td = "<td><a class='fancybox2 fancybox.iframe' href='/AgentAvailability.aspx?ID=" + A.ID + "&AgentID=" +A.SellerID + "&SocietyName=" + SocityName + "'>Edit</a></td>";

            ApartmentType apt;
            string type = "-";

            if (Global.ApartmentTypeList.TryGetValue((int)A.ApartmentTypeID, out apt))// if the apartment type exists in the global list
            {
                type = apt.Bedroom + "B-" + apt.Bathroom + "T";
                //price = apt.BSP.ToString();
            }
            string tr = "<tr><td>" + ctr + "</td><td><a href='/";
            if(A.Deleted==1)
                tr = "<tr style='text-decoration:line-through;'><td>" + ctr + "</td><td><a href='/";
            sb.Append(tr
                + UrlName.ToLower() + "/" + agent.URLName + "' target='_blank'>"
                + SocityName + "</a></td><td>"
                + type + "</td><td>" + avlFor + "</td><td>" + Price + "</td><td>"
                + Cmn.ToDate(A.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>"
                + Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yy") + "</td><td>"
                + Area + "</td>"
                + td + "</tr>");
        }
        sb.Append("</tbody></table>");
        return sb.ToString();
    }

    private void ShowAgentClients(int AgentID)
    {
        List<AgentClient> ClientList = AgentClient.GetListByAgentID(AgentID);
        StringBuilder str = new StringBuilder("<table class='datatable table table-hover table-striped table-condensed'>");
        str.Append("<thead style='background-color:#F8F8F8;'><tr><th>#</th><th>Name</th><th>Mobile No</th><th>EMail</th><th>City</th><th>Address</th><th>Assigned</th><th style='width:80px;'>Assign Avl</th></tr></thead><tbody>");
        int ctr = 1;
        foreach (AgentClient AC in ClientList)
        {
            int AssignCount = GetAssignedCount(AC.ID, AgentID);
            str.Append("<tr><td>" + ctr++ + "<td><a class='fancybox fancybox.iframe' href='/AgentClientEdit.aspx?ClientID=" + AC.ID + "&AgentID=" + AC.AgentID + "'>" + AC.Name + "<a></td><td>" + AC.MobileNo + "</td><td>" + AC.EmailID + "</td><td>" + AC.City + "</td><td>"
                + AC.Address + "</td><td><a href='#ClientAvl' id='fancyBoxLink' onclick='GetClientAvl(" + AgentID + "," + AC.ID + ")'><span class='label label-info'>" + AssignCount + "</span><a></td><td style='text-align:center;'><a class='fancybox fancybox.iframe' href='/AssignAvailability.aspx?AgentID=" + AgentID + "&ClientID=" + AC.ID + "'><i class='icon-hand-right'></i></a></td></tr>");
        }
        str.Append("</tbody></table>");
        ltClients.Text = str.ToString();
    }

    protected int GetAssignedCount(int ClientID, int AgentID)
    {
        return AsignClient.GetAssignedCount(ClientID, AgentID);
    }

    protected void UploadImageBtn_Click(object sender, EventArgs e)
    {
        if (uploadImage.HasFile)      //second single file upload for layout plan.
        {
            string Save_Location = Server.MapPath("~/Data/Images_Agent/") + spnAgentID.InnerHtml + Path.GetExtension(uploadImage.FileName);
            uploadImage.SaveAs(Save_Location);
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Global.LogoutReset();
        Session.Abandon();
        Response.Redirect("/" + agent.URLName.ToLower());
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Agent A = Global.AgentList.Values.FirstOrDefault(m => m.ID == agent.ID);
        //A.ID = Cmn.ToInt(spnAgentID.InnerText);
        A.AgentCompany = txtCompanyName.Value;
        A.AgentName = txtAgentName.Value;
        A.Address = txtAddress.Value;
        A.City = Cmn.ToInt(ddCity.SelectedValue); 
        A.PhoneNo1 = txtPhone1.Value;
        A.PhoneNo2 = txtPhone2.Value;
        A.Mobile1 = txtMobile1.Value;
        A.Mobile2 = txtMobile2.Value;
        A.BuilderDescription = txtDetails.Value;
        A.Lat = Cmn.ToDbl(txtLat.Value);
        A.Lng = Cmn.ToDbl(txtLng.Value);
        //A.Save();
        Global.LoadGlobalData();
    }
}