using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Edit_ProjectList : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            City.CityList(lstProjects);
            if (Request.Cookies["CityID"] != null)
                lstProjects.SelectedValue = Request.Cookies["CityID"].Value;
            lstProjects_SelectedIndexChanged(null, EventArgs.Empty);
        }
    }
    
    protected void lstProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnReload_Click(null, EventArgs.Empty);
    }
    
    protected void btnReload_Click(object sender, EventArgs e)
    {
        Global.LoadGlobalData();

        List<Society> list = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(lstProjects.SelectedValue)&&m.Verified==1).OrderBy(m => m.SocietyName).ToList();

        if (!chkShowDeleted.Checked)
            list = list.Where(m => m.Deleted != 1).ToList();

        StringBuilder str = new StringBuilder("<table id='ProjectReportTable' style='width:auto;'><tr style='background-color:steelblue;color:white;'><th>SN<th>ID<th>Project<th>City<th>Sub City<th>Area<th>Builder<th><th>Logo<th>Image<th>Layout<th>Apts");
        int ctr = 0;
        foreach (Society s in list)
        {
            string NoAptImgCount = "";
            int AptImgCount = 0;
            
            foreach (ApartmentType A in s.ApartmentList)
            {
                if(!File.Exists(Server.MapPath(@"~\Data\Images_ApartmentType\" + A.ID + ".jpg")))
                    NoAptImgCount += A.TypeName + ",";
                else if (File.Exists(Server.MapPath(@"~\Data\Images_ApartmentType\" + A.ID + ".jpg")))
                {
                    AptImgCount++;
                }
            }

            bool layout=File.Exists(Server.MapPath(@"~\Data\Images_LayoutPlan\" + s.ID + ".jpg"));

            str.Append("<tr><td>" + (++ctr) + "<td style='border-right:1px dotted gainsboro; padding:0px 3px 0px 3px;'>" + s.ID +  "<td style='" + (s.Deleted == 1 ? "text-decoration:line-through;color:red" : "") + "; padding-left:5px;' onclick='ShowSociety(" + s.ID + ")'>" + s.SocietyName
                + "<td>" + s.City
                + "<td>" + s.Subcity
                + "<td style='width:10px;'>" + s.Area
                + (s.Builder != null ? "<td onclick='ShowBuilder(" + s.BuilderID + ")'>" + s.Builder.AgentName : "<td style='background-color:yellow'>")
                + "<td>" + (s.Verified == 1 ? "Verified" : "")
                + (File.Exists(Server.MapPath(@"~\Data\Images_SocietyLogo\" + s.ID + ".jpg")) ? "<td>Yes" : "<td style='background-color:yellow'>")
                + ((ImageCounter(s.ID)>0) ? "<td>Y(" + ImageCounter(s.ID) + ")" : "<td style='background-color:yellow'>")
                + (layout ? "<td>Yes" : "<td style='background-color:yellow'>")
                + (NoAptImgCount != "" ? "<td style='background-color:yellow;width:200px;'>" + NoAptImgCount : "<td style='"+(AptImgCount==0?"color:red;background-color:yellow;":"")+"'>"+(AptImgCount==0?"No(" +( AptImgCount + ")"):"Yes("+AptImgCount+")")) 
               );
        }
        ltList.Text = str.ToString() + "</table>";
    }

    private int ImageCounter(int SocID)
    {
        string Path = Server.MapPath(@"~\Data\Images_Society\");
        return Directory.GetFiles(Path, SocID + "_*.jpg").Length;
    }
}