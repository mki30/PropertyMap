using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;

public partial class AgentAvailability :BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        int ID = Cmn.ToInt(Request.QueryString["ID"]);
        int AgentID = Cmn.ToInt(Request.QueryString["AgentID"]);
        //string SocityName = Request.QueryString["SocietyName"];

        ltID.Text = ID.ToString();
        ltAgentID.Text = AgentID.ToString();
        
        if (!IsPostBack)
        {
            ShowData(ID);
        }
    }

    private void ShowData(int ID)
    {
        Availability A = Availability.GetByID(ID);
        if (A != null)
        {
            ltID.Text = A.ID.ToString();
            hdSocietyID.Value = A.SocietyID.ToString();
            hdApartmentTypeID.Value = A.ApartmentTypeID.ToString();
            txtSociety.Text = GetSocietyName(A.SocietyID);
            if (A.AvailabilityType == 0)
            {
                trrentfor.Visible = true;
                ddrentFor.SelectedValue = A.RentFor.ToString();
            }
            ddFor.SelectedValue = A.AvailabilityType.ToString();
            ddFloorNo.SelectedValue =(A.FloorNo!=null)? A.FloorNo.ToString():"-1";
            txtAmount.Text = A.Amount.ToString();
            txtAvlFrom.Text = Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yyy");
            txtDescription.Text = A.Description;
            chkDeleted.Checked = A.Deleted == 1? true : false;
            imageDiv.InnerHtml = GetAvlImage(Cmn.ToInt(ltID.Text));
       }
    }

    public static string GetAvlImage(int DataID)
    {
        var img = "";
        string[] images = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Images_Availability/"), DataID + "_*.*");
        foreach (var im in images)
        {
            img += ImgSpan(@"../Data/Images_Availability/" + Path.GetFileName(im));
        }
        return img;
    }

    static string ImgSpan(string ImgageSource)
    {
        return "<span><img style='margin:2px; height:40px;width:40px;' src='" + ImgageSource + "'/><a href='#'  style='z-index:99999px; position:relative; background-color:white;left:-25px;top:0px; color:red; ' onclick='return DeleteImage(\"" + ImgageSource + "\");'>X</a></span>";
    }

    private string GetSocietyName(int? ID)
    {
        try
        {
            Society S = Society.GetByID(Cmn.ToInt(ID));
            return S.SocietyName;
        }
        catch(Exception ex)
        {
            return "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Update();
    }

    private void Update()
    {
        Availability A = Availability.GetByID(Cmn.ToInt(ltID.Text));
        if (A == null)
            A = new Availability();
        try
        {
            A.ID = Cmn.ToInt(ltID.Text);
            A.SellerID = Cmn.ToInt(ltAgentID.Text);
            A.SellerType = 0;//For Agent-0
            A.SocietyID = Cmn.ToInt(hdSocietyID.Value);
            A.AvailabilityType = Cmn.ToInt(ddFor.SelectedValue);
            if(ddFloorNo.SelectedValue!="-1")
            A.FloorNo = Cmn.ToInt(ddFloorNo.SelectedValue);
            A.Amount = Cmn.ToInt(txtAmount.Text);
            A.ApartmentTypeID = Cmn.ToInt(hdApartmentTypeID.Value);
            A.DateAvailableFrom = Cmn.ToDate(txtAvlFrom.Text.ToString());
            A.DateAvailableFrom= Cmn.ToDate(txtAvlFrom.Text);
            A.PostedOnDate = DateTime.Now;
            A.Description = txtDescription.Text;
            A.Deleted = chkDeleted.Checked == true ? 1 : 0;
            A.Save();
            
            if (A.Message.Length == 0)
            {
                Agent agnt = null;
                ApartmentType aprt = null;
                Society soc = null;
                string AgentName = "";
                string type = "";
                string socname = "";
                string url = "";
                string avlfor = (ddFor.SelectedValue == "1" ? "-for-sale" : "-for-rent");
                if (Global.AgentList.TryGetValue(Cmn.ToInt(A.SellerID), out agnt))
                {
                    AgentName = agnt.AgentName;
                }
                if (Global.ApartmentTypeList.TryGetValue(Cmn.ToInt(A.ApartmentTypeID), out aprt))
                {
                    type = "-" + aprt.Bedroom + "bhk-aprtment";
                }
                if (Global.ProjectList.TryGetValue(Cmn.ToInt(A.SocietyID), out soc))
                {
                    socname = "-in-" + soc.URLName;
                }
                url = (AgentName.Replace(" ","-")+ type + avlfor + socname + "-" + A.ID).ToLower();
                A.URL = url;
                A.Save();

                if (FileUpload1.HasFile)
                {
                    HttpFileCollection hfc = Request.Files;
                    for (int img = 0; img < hfc.Count; img++)
                    {
                        HttpPostedFile hpf = hfc[img];
                        if (hpf.ContentLength > 0)
                        {
                            hpf.SaveAs(GetImageFileName(A.ID));
                        }
                    }
                }
            }
            ShowData(A.ID);
            Global.LoadGlobalData();
            lblStatus.Text = "Saved!";
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Not Saved!" + ex.Message;
            lblStatus.ForeColor = System.Drawing.Color.Red;

        }
    }

    string GetImageFileName(int ID)
    {
        string FileName = "";

        var Folder = "~/Data/Images_Availability/";
        for (int i = 1; ; i++)
        {
            FileName = Server.MapPath(Folder + ID + "_" + i) + ".jpg";
            if (!File.Exists(FileName))
                break;
        }
        return FileName;
    }

    //protected void btnDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Availability A = new Availability()
    //          {
    //              ID = Cmn.ToInt(ltID.Text)
    //          }.Delete();
    //        Global.LoadGlobalData();
    //        lblStatus.Text = "Deleted!";
    //    }
    //    catch (Exception ex)
    //    {
    //        lblStatus.Text = "Not Deleted!"+ex;
    //        lblStatus.ForeColor = System.Drawing.Color.Red;
    //    }
    //}
    protected void ddFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddFor.SelectedValue == "0")
            trrentfor.Visible = true;
        else
        {
            trrentfor.Visible = false;
        }

    }
}