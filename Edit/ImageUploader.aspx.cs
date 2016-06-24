using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;

public partial class Edit_ImageUploader : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        lblApartmentID.Text = QueryString("AptID", "0");
        lblProjectID.Text = QueryString("ProjectID", "");
        int FloorNo = QueryStringInt("FloorNo", -1);
        PropertyType = QueryStringInt("PropertyType", -1);
        if (PropertyType != -1)
        {
            int a = PropertyType;
            FillFloors(PropertyType);
        }

        if (!IsPostBack)
            ShowData(Cmn.ToInt(lblApartmentID.Text), FloorNo);
        else
        {   
            //Postback case
            string urlName = "";
            if (FileUpload1.HasFile)
            {
            //    string Save_Location = Server.MapPath(@"~/Data/Images_ApartmentType/") + lblApartmentID.Text + (ddFloor.SelectedValue != "" ? "_" + ddFloor.SelectedValue : "") + ".jpg";
            //    FileUpload1.SaveAs(Save_Location);
            //    btnSave_Click(sender, EventArgs.Empty);
                Society s = Society.GetByID(Cmn.ToInt(lblProjectID.Text));
                string AppendText = lblApartmentID.Text + (ddFloor.SelectedValue != "" ? "_" + ddFloor.SelectedValue : "");
                urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-" + AppendText;
                urlName = Global.GetUniqueURL(s, urlName, AppendText);
                string Save_Location = Server.MapPath("~/Data/Images_ApartmentType/") + urlName + ".jpg";
                ImagesDetail id = new ImagesDetail();
                id.ReferenceID = Cmn.ToInt(lblApartmentID.Text);
                id.ImageReferenceType = (int)ImagesLocations.Apartment_Type;
                id.ImageID = lblApartmentID.Text + (ddFloor.SelectedValue != "" ? "_" + ddFloor.SelectedValue : "") + ".jpg";
                id.UrlName = urlName;
                id.Save();
                FileUpload1.SaveAs(Save_Location);
                Global.LoadGlobalData();
            }
        }
    }

    //string GetUniqueURL(Society S, string urlName, string appendText)
    //{
    //    string url = urlName;
    //    if (Global.ImageDetailsList.FirstOrDefault(m=>m.UrlName==urlName)!=null || string.IsNullOrWhiteSpace(urlName))
    //        url = (S.URLName+ (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
    //    return url;
    //}

    private int PropertyType { get; set; }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        new FloorInfo()
        {
            FloorName = ddFloor.SelectedItem.Text,
            AptTypeID = Cmn.ToInt(lblApartmentID.Text),
            FloorNo = Cmn.ToInt(ddFloor.SelectedItem.Value),
            BuiltupArea = Cmn.ToInt(txtBuiltUpArea.Text),
            TotalArea = Cmn.ToInt(txtTotalArea.Text),
            Lawn = Cmn.ToInt(txtLawn.Text),
            Terrace = Cmn.ToInt(txtTerrace.Text)
        }.Save();
        WriteClientScript("parent.RefreshFloorInfoGrid()");
    }
    void FillFloors(int ApartmentType)
    {
        ddFloor.Items.Clear();
        foreach (KeyValuePair<string, string> kvp in FloorInfo.GetFloorList((PropertyTypes)ApartmentType))
            ddFloor.Items.Add(new ListItem(kvp.Key, kvp.Value));
    }

    void ShowData(int ApartmentID, int FloorNo)
    {
        ApartmentType Apt = ApartmentType.GetByID(ApartmentID);
        if (Apt == null) return;

        if (Apt.PropertyType != null && PropertyType == -1)
            FillFloors((int)Apt.PropertyType);

        FloorInfo FI = FloorInfo.GetByAptIDandFloorNo(ApartmentID, FloorNo);
        if (FI != null)
        {
            txtBuiltUpArea.Text = FI.BuiltupArea.ToString();
            txtLawn.Text = FI.Lawn.ToString();
            txtTerrace.Text = FI.Terrace.ToString();
            txtTotalArea.Text = FI.TotalArea.ToString();
            ListItem li = ddFloor.Items.FindByValue(FI.FloorNo.ToString());

            if (li != null)
                ddFloor.SelectedValue = li.Value;
        }
    }
}