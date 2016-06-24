using PropertyListModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_ProjectReport : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {   
        base.Page_Load(sender, e);
        if (!IsPostBack)
        {
            City.CityList(lstProjects,-1,false,false,true);   //Fill city list
            if (Request.Cookies["CityID"] != null)              
                lstProjects.SelectedValue = Request.Cookies["CityID"].Value;
            lstProjects_SelectedIndexChanged(null, EventArgs.Empty);
        }
    }

    protected void lstProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Cookies["CityID"].Value = lstProjects.SelectedValue;   //set city value in cookie
        Response.Cookies["CityID"].Expires = DateTime.Now.AddDays(1);
        btnReload_Click(null, EventArgs.Empty);
    }

    protected void btnReload_Click(object sender, EventArgs e)
    {
        Global.LoadGlobalData();
        //int varified = chkVarified.Checked ? 1 : 0;
        //int deleted = ChkDeleted.Checked ? 1 : 0;

        List<Society> list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue) && (m.Verified == 1)).OrderBy(m => m.SocietyName).ToList();
        if (!chkVerified.Checked)
            list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue) && m.Verified == 0 && m.Deleted == 0).OrderBy(m => m.SocietyName).ToList();
        if (CheckShowAll.Checked)
            list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue)).OrderBy(m => m.SocietyName).ToList();
        if (ChkDeleted.Checked)
            list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue) && m.Deleted == 1).OrderBy(m => m.SocietyName).ToList();
        if (ChkLastUpdated.Checked)
        {
            list = list.OrderByDescending(m => m.LUDate).ToList();
        }
        if (ChkOrderByBuilder.Checked)
        {
            list = list.OrderBy(m => m.BuilderID).ThenBy(m => m.LUDate).ToList();
        }
        if (ChkWishTown.Checked)
        {
            list = Global.ProjectList.Values.Where(m => m.Town == "Wish Town" && (m.Verified == 1)).OrderBy(m => m.SocietyName).ToList();
        }
        
        //if (ChkDeleted.Checked)
        //list = list.Where(m => m.Deleted != 1).ToList();

        StringBuilder str = new StringBuilder("<table id='DataTable' class='table table-striped table-condensed'><thead><tr><th>SN&nbsp;&nbsp;<th>ID</th><th>view&nbsp;&nbsp;</th><th>Project</th><th>Map</th><th>Imp&nbsp;&nbsp;</th><th>Pos&nbsp;&nbsp;</th><th>Poly&nbsp;&nbsp;</th><th>Sub City</th><th>Area</th><th>Builder</th><th>Logo&nbsp;&nbsp;</th><th>Image&nbsp;&nbsp;</th><th>Layout&nbsp;&nbsp;</th><th>+&nbsp;&nbsp;</th><th>Apts</th><th>Updated On</th></tr><tbody>");
        int ctr = 0;
        foreach (Society s in list)
        {
            string NoAptImgCount = "";

            foreach (ApartmentType A in s.ApartmentList)
            {
                NoAptImgCount += "<a class='fancybox fancybox.iframe' href='EditApartmentType.aspx?SocietyID=" + s.ID + "&AptTypeID=" + A.ID + "'";
               
                //string[] Files = Directory.GetFiles(Server.MapPath(@"~/Data/Images_ApartmentType/"), A.ID + ".*").Union(Directory.GetFiles(Server.MapPath(@"~/Data/Images_ApartmentType/"), A.ID + "_*.*")).ToArray();
                List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(A.ID, (int)ImagesLocations.Apartment_Type);

                if (ids.Count <= 0 && A.PropertyType != 2)
                    NoAptImgCount += "style='color:red'";
                if (A.PropertyType == 1)
                    NoAptImgCount += "style='color:#FFCC00;'";
                else if (A.PropertyType == 2)
                    NoAptImgCount += "style='color:black;'";
                else if (A.PropertyType == 3)
                    NoAptImgCount += "style='color:green;'";
                else if (A.PropertyType == 4)
                    NoAptImgCount += "style='color:orange;'";
                else if (A.PropertyType == 5)
                    NoAptImgCount += "style='color:violet;'";
                NoAptImgCount += ">" + A.TypeName + "</a>, ";
            }

            //bool layout = File.Exists(Server.MapPath(@"~\Data\Images_LayoutPlan\" + s.ID + ".jpg"));
            string layout = ImageExist(s.ID, (int)ImagesLocations.Project_Layout, @"~\Data\Images_LayoutPlan\");
            
            try
            {
                str.Append("<tr><td>" + (++ctr) + "<td style='" + ((s.EndDate < DateTime.Now && s.Status != 4) ? "color:orange;" : "") + "'>"
                    + s.ID + "<td><a href='" + s.URL + "' target='_blank'><div><i class='icon-eye-open'></i></a><span style='" + (string.IsNullOrEmpty(s.BrochureURL) ? "color:red; width:20px;" : "") + "padding-left:3px;'>P</span></div><td style='" + (s.Deleted == 1 ? "text-decoration:line-through;color:red;" : "") + "; padding-left:5px;'><div style='width:180px;' id='name_+" + ctr + "'><a"
                    + (s.Verified != 1 ? "style='color:red'" : "") + " class='fancybox1 fancybox.iframe' href='ComboEdit.aspx?SocietyID=" + s.ID + "&SubCityID=" + s.SubCityID + "'><span>" + s.SocietyName + "</span></div></a>"
                    //+ "<td><a class='fancybox fancybox.iframe' href='../Map.htm?ID=" + s.ID + "'>" + (Cmn.ToInt(s.Lat) != 0 ? "<i class='icon-map-marker'></i>" : "<i class='icon-map-marker'></i>") + "<a/>"
                    + "<td>" + Math.Round((double)s.Lat, 2) + "," + Math.Round((double)s.Lng, 2)
                    + "<td style='color:green;'>" + (s.Impression == 0 ? "" : "<span class='label'>" + s.Impression.ToString()) + "</span>"
                    + "<td style='color:orange;'>" + (s.AvgPos == 0 ? "" : "<span class='label'>" + s.AvgPos.ToString()) + "</span>"
                    + "<td>" + ((s.PolyPoints != "" && s.PolyPoints != null) ? "Y" : "")
                    //+ "<td>" + s.City
                    + "<td style='white-space:nowrap'>" + s.Subcity
                    + "<td >" + s.Area
                    + (s.Builder != null ? "<td><div style='width:200px;' id='draggable'><a  class='fancybox fancybox.iframe' href='EditAgent.aspx?ID=" + s.BuilderID + "&UserType=2'>" + s.Builder.AgentName + "</a></div>" : "<td style='background-color:yellow'>")
                    //+ (File.Exists(Server.MapPath(@"~\Data\Images_SocietyLogo\" + s.ID + ".jpg")) ? "<td>Yes" : "<td style='background-color:yellow'>")
                    + ((ImageExist(s.ID,(int)ImagesLocations.Project_Logo,@"~\Data\Images_SocietyLogo\")!="") ? "<td>Yes" : "<td style='background-color:yellow'>")
                    + ((ImageCounter(s.ID, (int)ImagesLocations.Projects_Images) > 0) ? "<td><span class='badge badge-info'>" + ImageCounter(s.ID, (int)ImagesLocations.Projects_Images) + "</span>" : "<td style='background-color:yellow'>")
                    + (layout!="" ? "<td><a class='fancybox fancybox.iframe' href='/Data/Images_LayoutPlan/"+layout+"'>Yes</a>" : "<td style='background-color:yellow'>")
                    + "<td ><a class='fancybox fancybox.iframe' href='EditApartmentType.aspx?SocietyID=" + s.ID + "&AptTypeID=0'>+</a>"
                    + "<td>" + NoAptImgCount
                    + "<td><div style='width:100px; color:light-blue; font-size:11px;'>" + Cmn.ToDate(s.LUDate).ToString("dd-MMM-yy hh:mm tt") + "</div>"
                    );
            }
            catch(Exception ex)
            {
                string d = s.SocietyName;
            }
            }
            ltList.Text = str.ToString() + "</table>";
    }

    private string ImageExist(int id,int type,string path)
    {
        string url = "";
        ImagesDetail ID=Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == id && m.ImageReferenceType == type);
        if (ID != null)
            url= ID.UrlName;
        if (url != "")
        {
            if (File.Exists(Server.MapPath(path + ID.UrlName + ".jpg")))
                return ID.UrlName + ".jpg" ;
            else return "";
        }
        else return "";
    }
    private int ImageCounter(int SocID,int type)
    {
        List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(SocID, type);
        return ids.Count;
    }
    protected void chkVarified_CheckedChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
        CheckShowAll.Checked = false;
        ChkDeleted.Checked = false;
    }
    protected void CheckShowAll_CheckedChanged(object sender, EventArgs e)
    {
        chkVerified.Checked = false;
        ChkDeleted.Checked = false;
        btnReload_Click(null, EventArgs.Empty);
    }
    protected void ChkDeleted_CheckedChanged(object sender, EventArgs e)
    {
        CheckShowAll.Checked = false;
        chkVerified.Checked = false;
        btnReload_Click(null, EventArgs.Empty);
    }
    protected void ChkLastUpdated_CheckedChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
    }
    protected void ChkOrderByBuilder_CheckedChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
    }
    protected void CheckWishTown_CheckedChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
    }
}