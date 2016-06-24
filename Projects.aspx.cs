using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using PropertyListModel;
using System.Linq;

public partial class Projects : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title = Global.AppTitle;

        if (Data1 != "")
            ShowData(Data1);
        else
            Response.Redirect("~\\");

    }

    void ShowProjectList()      //for project page edit
    {
        PanelProjectList.Visible = true;
        List<Society> list = Society.GetAll();

        StringBuilder str = new StringBuilder("<table  style='border:1px solid gray; border-spacing:1px;'><tr><td></td><td>society name</td><td>address</td><td>description</td><td>map loc</td><td>floor plans</td></tr>");
        int datacount = 0;
        foreach (Society s in list)
        {
            string layoutfile = Server.MapPath(@"~\data\images_layoutplan\" + s.ID + ".jpg");
            if (File.Exists(layoutfile))
            {
                List<ApartmentType> at = ApartmentType.GetList(s.ID);
                str.Append("<tr><td>" + datacount + "</td><td><a href='" + ResolveClientUrl(@"~\project\") + s.ID + "' title='" + s.ID + "'>"
                    + s.SocietyName + "</a><a style='cursor:pointer; float:right; color:red;' onclick='openSocietyEdit(" + s.ID + ")'>edit</a></td><td>" +
                    s.Address + "</td><td>" +
                    (!string.IsNullOrWhiteSpace(s.Description) ? s.Description.Length > 50 ? s.Description.Substring(0, 50) : s.Description : "") + "</td><td>" +
                    Cmn.ToDbl(s.Lat).ToString("0") + "</td><td>" + at.Count + "<a  style='cursor:pointer; float:right;color:red;' onclick='openFloorPlanEdit(" + s.ID + ")'>edit</a></td></tr>");
                datacount++;
            }
        }
        str.Append("<tr><td  colspan='5' style='text-align:right;'><b>total added:" + datacount + "</b></td></tr>");
        str.Append("</table>");
        ltProjectList.Text = str.ToString();
    }

    void ShowData(string URLName)
    {
        PanelProjectDetail.Visible = true;

        Society society = Global.ProjectList.Values.FirstOrDefault(m => m.URLName == URLName);
        if (society == null)
            return;

        StringBuilder str = new StringBuilder();

        Title = society.SocietyName + Global.AppTitle;

        if (File.Exists(Server.MapPath(@"~/Data/Images_BuilderLogo/") + society.BuilderID + ".jpg"))
            lblLogo.Text = "<img src ='" + Global.GetRootPathVirtual + @"/Data/Images_BuilderLogo/" + society.BuilderID + ".jpg" + "' title='" + society.SocietyName
                + "' alt='" + society.SocietyName + "' class='builderlogo'/>";
        else
            lblLogo.Text = "<img src ='" + Global.GetRootPathVirtual + @"/Images/NotAvailable.jpg" + "' title='" + society.SocietyName + "'  alt='" + society.SocietyName
                + "' class='builderlogo'/>";

        ltrProjectName.Text = society.SocietyName;
        ltrAddress.Text = (society.Address != null ? society.Address + ", " : "") + society.Subcity + "," + society.City + ", " + society.State + (society.Pin != null ? "-" + society.Pin : "");

        ltrDetail.Text = society.Description;
        fancyBoxVedio.Attributes["href"] = society.VedioLink != null ? society.VedioLink : "";

        MapLink.HRef = "PropertyMap.aspx?ProjectID=" + society.ID;
        MapLink.Target = "_blank";

        Agent A;
        if (Global.BuilderList.TryGetValue( Cmn.ToInt(society.BuilderID),out A))
        {
            BuilderName.InnerText = A.AgentName;
            BuilderName.HRef = Global.GetRootPathVirtual + @"/builder/" + A.AgentName;
            BuilderName.Target = "_blank";
        }

        string[] Lm = { "Bank", "School", "Market" };
        ltLndmrkSideist.Text = "<ul class='Landmarkul'><li>Landmarks</li>";
        foreach (string s in Lm)
            ltLndmrkSideist.Text += "<li><a hre='#'>" + s + "</a></li>";

        ltLndmrkSideist.Text += "</ul>";
        ltrAminities.Text = GetAmenitiesImages(society);
        ltrGallary.Text = GetSocietyImage(society.ID);
        ltAptTypeTable.Text = society.GetAptDetails();       //Apartment Type Table

        ltSiteplan.Text = "<a class='fancyBox' href='" + Global.GetRootPathVirtual + @"/Data/Images_LayoutPlan/" + society.ID + ".jpg'>Site Plan</a>";
    }
    
    public string GetAmenitiesImages(Society society)
    {
        string amn = society.Amenities != null ? society.Amenities.ToString().PadRight(10, '0') : "00000000000";
        StringBuilder strAmenities = new StringBuilder();
        foreach (var V in Enum.GetValues(typeof(Amenities)))
        {
            if (amn[(int)V] == '1')
            {
                strAmenities.Append("<div style='float:left; padding:2px;'>");
                switch (V.ToString())
                {
                    case "Sec":
                        strAmenities.Append("<img  title='Security' src='" + Global.GetRootPathVirtual + "/Images/amenities/security1.gif'   alt='Security'>");
                        break;
                    case "PwrBck":
                        strAmenities.Append("<img  title='Power Backup' src='" + Global.GetRootPathVirtual + "/Images/amenities/power-backup1.gif'  alt='power backup'>");
                        break;
                    case "Lift":
                        strAmenities.Append("<img  title='Lift' src='" + Global.GetRootPathVirtual + "/Images/amenities/lift1.gif'   alt='Lift'>");
                        break;
                    case "Swimpool":
                        strAmenities.Append("<img  title='Swiming Pool' src='" + Global.GetRootPathVirtual + "/Images/amenities/swimming1.gif'  alt='Gym'>");
                        break;
                    case "Park":
                        strAmenities.Append("<img  title='Park' src='" + Global.GetRootPathVirtual + "/Images/amenities/park1.gif'  alt='Park'>");
                        break;
                    case "Gym":
                        strAmenities.Append("<img  title='Gym' src='" + Global.GetRootPathVirtual + "/Images/amenities/gym1.gif'  alt='Gym'>");
                        break;
                    case "Club":
                        strAmenities.Append("<img  title='Club' src='" + Global.GetRootPathVirtual + "/Images/amenities/club1.gif'   alt='Club'>");
                        break;
                    case "RainWaterHarvesting":
                        strAmenities.Append("<img  title='Rain Water Harvesting' src='" + Global.GetRootPathVirtual + "/Images/amenities/rain1.gif'   alt='Rain Water harvesting'>");
                        break;
                    case "Vastu":
                        strAmenities.Append("<img  title='Vastu Compalaint' src='" + Global.GetRootPathVirtual + "/Images/amenities/vaastu1.gif'   alt='Vaastu'>");
                        break;
                }
                strAmenities.Append("</div>");
            }
        }
        return strAmenities.ToString();
    }

    public string GetSocietyImage(int DataID)  //Society Image
    {
        String[] ImageFiles;
        var img = "";
        if (File.Exists(Server.MapPath("~/Data/Images_Society/" + DataID + "_1.jpg")))
            ImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Images_Society/"), DataID + "_*.jpg");
        else
            ImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Images_Society/"), DataID + "_*.gif");
        int ctr = 0;
        foreach (var image in ImageFiles)
        {
            //if (ctr == 0)
            //    ltSocietySingleImage.Text = "<img class='projimg' src='"
            //        + ResolveClientUrl(@"~/Data/Images_Society/") + Path.GetFileName(image) + "' alt='Prolect_Image'/>";
            ctr++;
            img += "<span  style='margin:0px 3px 0px 3px;'><img style='width:47.5%; height:200px; border:1px solid #C0C5C5; padding:5px;' src='" + ResolveClientUrl(@"~/Data/Images_Society/") + Path.GetFileName(image) + "' alt=''/></span>";
            //img += "<a class='grouped' rel='group1'  href='" + ResolveClientUrl(@"~/Data/Images_Society/") + Path.GetFileName(image) + "'></a>";
        }
        return img;
    }
}