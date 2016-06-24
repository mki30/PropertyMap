using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;
using System.Text;

public partial class UserControl_BuilderControl : System.Web.UI.UserControl
{
    Agent builder;
    public string MetaDescription = "", MetaKeywords = "", Title = "";
    System.Web.UI.Page ParentPage;
    
    public void InitCtl(System.Web.UI.Page _page,Agent _Builder)
    {
        builder = _Builder; 
        
        if (Request.Browser.IsMobileDevice)
        {
            Response.Clear();
            string txt = File.ReadAllText(Server.MapPath("~/mobile.htm"));
            if (builder != null)
                {
                    txt = txt.Replace("<title>Property Map</title>", "<title>" + builder.AgentName + "</title>").Replace("Loading Builder Detail...", builder.BuilderDetailMobile()).Replace("//customscript", "isChangePage=true; changeTo='#BuilderDetails';");
                }
            Response.Write(txt);
            Response.End();
        }

        
        Agent A = Agent.GetByID(builder.ID);

        if (A == null)
        {
            Response.Redirect("/NotFound.aspx");
        }
        Title = A.AgentName;
        
        if (A != null)
        {
            MetaDescription = A.BuilderDescription.Substring(0,200)+"..";
            //City C;
            //if(Global.CityList.TryGetValue((int)A.City,out C))
            //{
            //};
            MetaKeywords = A.AgentName;
            ltBuilderDetail.Text = A.BuilderDescription;
            if (File.Exists(Server.MapPath(@"~/Data/Images_BuilderLogo/") + A.ID + ".jpg"))
                ltBuilderLogo.Text = "<img src ='" + Global.GetRootPathVirtual + @"/Data/Images_BuilderLogo/" + A.ID + ".jpg" + "' title='" + A.AgentName
                    + "' alt='" + A.AgentName + "' class='builderlogo'/>";
            else if (File.Exists(Server.MapPath(@"~/Data/Images_BuilderLogo/") + A.ID + ".gif"))
                ltBuilderLogo.Text = "<img src ='" + Global.GetRootPathVirtual + @"/Data/Images_BuilderLogo/" + A.ID + ".gif" + "' title='" + A.AgentName
                    + "' alt='" + A.AgentName + "' class='builderlogo'/>";
            else
                ltBuilderLogo.Text = "<img src ='" + Global.GetRootPathVirtual + @"/Images/ImageNotAvl.jpg" + "' title='" + A.AgentName + "'  alt='" + A.AgentName
                    + "' class='builderlogo'/>";
            ltBuilderName.Text = A.AgentName;
            ltBuilderAdd.Text = A.Address;
            ltBuilderCont.Text = A.PhoneNo1 + (!string.IsNullOrEmpty(A.Mobile1) ? "<b>&nbsp;&nbsp;Mobile : </b>" + A.Mobile1 : "");

            if (A.URL != "" && A.URL != null)
                ltURL.Text = "<div><a href='" + A.URL + "' target='_blank'>" + (A.URL.Replace("http://", " ").Contains("/") ? A.URL.Replace("http://", " ").Replace("/", "") : A.URL.Replace("http://", " ")) + "</a></div>";
            
            List<Society> list = Society.GetByBuilder(A.ID).OrderBy(m => m.City).ThenBy(m => m.SocietyName).ToList();

            StringBuilder str = new StringBuilder(); 
            if (list.Count != 0)
            {
                ProjHead.Visible = true;
                str.Append("<table class='table table-bordered table-hover table-striped table-condensedltURL'><tr><th>Project<th>Start<th>Completion<th>Status<th>Area<th>City");
            }

            foreach (Society S in list)
            {
                DateTime s = Cmn.ToDate(S.StartDate);
                DateTime f = Cmn.ToDate(S.EndDate);

                str.Append("<tr><td><a  href='/" + S.URLName.ToLower() + "'>" +S.SocietyName + "<a><td>"
                    + (s != Cmn.MinDate ? s.ToString("MMM-yyyy") : "-")
                    + "<td>" + (f != Cmn.MinDate ? f.ToString("MMM-yyyy") : "-")
                    + "<td style='background-color:" + GetColor(S.Status) +"'>" + (S.Status != null ? ((Status)S.Status).ToString() : "-")      
                    + "<td>" + S.Subcity + "<td>" + S.City);
            }
            ProjHead.InnerHtml = "<h3>Projects </h3>";
            ltBuilderProjects.Text = str.ToString() + "</table>";
        }
     }
    
    private string GetColor(int? stat)
    {
        string col = "";
        switch (stat)
        {
            case 1:
                col = "#C0E7F3";
                break;
            case 2:
                col = "#FDFBB8";
                break;
            case 3:
                col = "#FACE9E";
                break;
             case 4:
                col = "#A0D1AD";
             break;
        }
        return col;
    }
}