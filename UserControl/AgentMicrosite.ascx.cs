using PropertyListModel;
using System;
using System.IO;

public partial class UserControl_AgentMicrosite : System.Web.UI.UserControl
{
    public Agent agent;
    public string Title = "";
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitCtrl(Agent A, string Option)     //Show Profile
    {
        agent = A;
        string path="";

        string img = (A.URLName != null ? A.URLName : "blank") + ".jpg";

        if (File.Exists(Server.MapPath(@"~/Data/Images_Agent/") + img))
            path = @"/Data/Images_Agent/" + img;

        else
            path = @"/Images/blank.jpg";
        Title = agent.AgentName!=null?agent.AgentName:agent.AgentCompany;

        ltLogo.Text = "<img id='bannerImg' style='margin: 10px; height: 60px; float:left;' class='img-polaroid' src='"+path+"' title='"+agent.AgentName+"'  runat='server' alt='"+A.AgentName+"'>";
        lblMobile.InnerText = A.Mobile1;
        lblEmail.InnerText = A.EmailID;
        spnAgentName.InnerText = A.AgentName;
        lblAddress.InnerText = A.Address;
        PropList.InitCtlAgentList(agent, ((Option == "sale"||Option=="") ? 1 : 0));
        
    }
}