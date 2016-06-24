using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;
using System.Text;

public partial class UserControl_FloorPlans : System.Web.UI.UserControl
{
    Society project;
    Agent agent;
    public string MetaDescription = "", MetaKeywords = "", Title = "", ID = "0";
    
    public void InitCtl(System.Web.UI.Page _page, Society _Project, Agent _Agent)
    {
        project = _Project;
        agent = _Agent;

        ShowData();
    }
    
    void ShowData()
    {
        ID = project.ID.ToString();
        MetaDescription = ""+project.SocietyName+" floor plans";
        MetaKeywords = "" + project.SocietyName + " floor plan 2bhk," + project.SocietyName + " floor plan 3bhk, " + project.SocietyName + "floor plans";
        Title=project.SocietyName+" floor plans";

        ImagesDetail imgDetails = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == project.ID && m.ImageReferenceType == (int)ImagesLocations.Project_Logo);
        if (imgDetails != null)
        {
            if (File.Exists(Server.MapPath(@"~/Data/Images_SocietyLogo/") + imgDetails.UrlName + ".jpg"))
                lblLogo.Text = "<img src ='/Data/Images_SocietyLogo/" + imgDetails.UrlName + ".jpg" + "' title='" + project.SocietyName
                  + "' alt='" + project.SocietyName + "' class='builderlogo'/>";
        }

        else
            lblLogo.Text = "<img src ='/Images/ImageNotAvl.jpg' title='" + project.SocietyName + "'  alt='" + project.SocietyName
           + "' class='builderlogo'/>";
        ltrProjectName.Text = "<h1 style='line-height: 20px'>" + project.SocietyName + " Floor Plans</h1>";
        ltrAddress.Text = "<span style='font-weight:bold;color:#9E9E9E;'>Address : " + (string.IsNullOrEmpty(project.Subcity) ? "" : project.Subcity + ",") + project.City + ", " + project.State + (!string.IsNullOrEmpty(project.Pin) ? ("-" + project.Pin) : (""));

        if (project.Builder != null)
        {
            BuilderName.InnerText = project.Builder.AgentName;
            //builder.InnerHtml = "<a style='color:gainsboro;' href='/" + project.Builder.AgentName.Replace(" ", "-").ToLower() + "' target='_blank'>by " + project.Builder.AgentName + "</a>";
            if (!string.IsNullOrEmpty(project.Builder.AgentName))
                BuilderName.HRef = "/" + project.Builder.AgentName.Replace(" ", "-").ToLower();
            BuilderName.Target = "_blank";
        }

        int months = 0;
        if (DateTime.Now < project.EndDate && project.EndDate != null)
        {
            TimeSpan? dt1 = (project.EndDate - DateTime.Now);
            months = (int)((double)dt1.Value.Days / 30.436875);
        }
        
        ltpossessDate.Text = project.Status != 4 ? (Cmn.ToDate(project.EndDate).Year == 1900 ? "NA" : Cmn.ToDate(project.EndDate).ToString("MMM-yyyy")
                            + (months != 0 ? "<span style='font-size:12px;'> (&nbsp;in&nbsp;" + months + " months&nbsp;)" : "</span>")) : "<span style='color:orange;'>Compleated</span>";

        StringBuilder sb = new StringBuilder();
        //lblFloorPlans.Text = sb.ToString();
        lblFloorPlans.Text=GetAptDetails();
     }

    string GetAptDetails()
    {
        
        StringBuilder sb = new StringBuilder();
        //sb.Append("<table class='table'>");
        
        foreach (ApartmentType A in project.ApartmentList)
        {
            if (A.BSP == null)
                A.BSP = 0;

            string area = A.SuperArea != 0 ? A.SuperArea.ToString() : (A.PlotArea != 0 ? A.PlotArea.ToString() + "" : "");
            string bsp = A.BSP == 0 ? "-" : A.BSP.ToString();
            string price = "";

            if (A.BSP != 0)
            {
                float Area = (float)A.SuperArea;       //When SuperArea is not Available
                if (A.SuperArea == null || A.SuperArea == 0)
                    if (A.PlotArea != null)
                        Area = (float)A.PlotArea;

                price = ((int)A.BSP * Area / 100000).ToString("0") + " L";

                string temp = price.Replace("L", "").Trim();
                int z = (int)Cmn.ToDbl(temp) / 100;
                if (((int)Cmn.ToDbl(temp) / 100 > 0))
                {
                    price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
                }
            }
            
            //unit = ((AreaUnit)(A.Unit != null ? A.Unit : 0)).ToString();
            sb.Append("<div class='row-fluid' style='border:1px solid #E6E6E6; margin-top:5px;'>");
            sb.Append("<div class='span2'>");
            sb.Append("<table class='table'><tr style='font-weight:bold;' class='info' id=" + A.ID + "><td>" + A.TypeName + "</td></tr>"+
            "<tr><td>Type : " + (A.PropertyType != null ? ((PropertyTypes)A.PropertyType).ToString().Replace("_", " ") : "Apartment") + "</td></tr>"
                + "<tr><td>Bedroom : " + A.Bedroom + "</td></tr>"
                + "<tr><td>Toilets : " + A.Bathroom + "</td></tr>"
                + "<tr><td>Area : " + area + "</td></tr>"//(A.Unit!=0?(" "+(AreaUnit)(A.Unit!=null?A.Unit:0)):"")+ "</td>"
                + "<tr><td>BSP : " + bsp + "</td></tr>"
                //+ "<tr><td>Price : " + (price == "" ? "-" : price) + "</td></tr>"
                +"</table></div>");
            
            ImagesDetail imgLayout = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == A.ID && m.ImageReferenceType == (int)ImagesLocations.Apartment_Type);

            sb.Append("<div class='span10' style='margin-bottom:5px;'><img src='/Data/Images_ApartmentType/" + imgLayout.UrlName + ".jpg'></div></div>");
        }
        //sb.Append("</table>");
        return sb.ToString();
    }
}

 


