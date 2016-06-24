using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.IO;

public partial class Edit_EditAvailability : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        lblStatus.Text = "";
        if (!IsPostBack)
        {
            for (int i = 1; i <= 50; i++)
            {
                ddlFlorNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            //if (QueryString("ReqType") == "user")
            //{
            //    string SocietyID = QueryString("SocietyID");
            //    string City = QueryString("City");
            //    string Area = QueryString("Area");
            //    string SellerID = QueryString("SellerID");
            //    string SellerType = QueryString("SellerType");
            //    radioButonListSellerType.SelectedIndex = SellerID == "0" ? 0 : SellerID == "1" ? 1 : SellerType == "2" ? 2 : 0;
            //    hidSellerID.Value = SellerID;
            //    trSellerType.Visible = false;
            //    btnAdd.Visible = false;
            //    btnDelete.Visible = false;
            //};
            ShowData(Cmn.ToInt(QueryString("ID")));
         }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }
    void ShowData(int ID)
    {
        Availability A = Availability.GetByID(ID);
        if (A != null)
        {
            Society S;
            if (A.SocietyID != null)
            {
                if (Global.ProjectList.TryGetValue((int)A.SocietyID, out S))
                {
                    txtSociety.Text = S.SocietyName;
                }
            }
            if (A.AvailabilityType == 0)
            {
                trRentFor.Visible = true;
                ddrentFor.SelectedValue = A.RentFor.ToString();
            }
            lblID.Text = A.ID.ToString();
            lblSocietyID.Text = A.SocietyID.ToString();
            hdSocietyID.Value = A.SocietyID.ToString();
            lblSellerID.Text = A.SellerID.ToString();
            hidSellerID.Value = A.SellerID.ToString();
            hdApartmentTypeID.Value = A.ApartmentTypeID.ToString();
            RadioButtonListAvlFor.SelectedIndex = Cmn.ToInt(A.AvailabilityType);
            radioButonListSellerType.SelectedIndex = A.SellerType.ToString() == "0" ? 0 : A.SellerType.ToString() == "1" ? 1 : 2;
            txtAmount.Text = A.Amount != null ? A.Amount.ToString() : "";
            lblPricePerSqf.Text = "";
            txtDateAvailableFrom.Text = A.DateAvailableFrom != null ? Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yyyy") : "";
            ddlFlorNo.SelectedValue = A.FloorNo.ToString();
            ddlFacingDir.SelectedValue = A.DirectionalFacing != null ? A.DirectionalFacing.ToString() : "";
            ddrentFor.SelectedValue = A.RentFor.ToString();
            txtDetails.Text = A.Description;
            imageDiv.InnerHtml = GetAvlImage(Cmn.ToInt(lblID.Text));
        }
    }

    void Update()
    {
        Availability A = Availability.GetByID(Cmn.ToInt(lblID.Text));
        if (A == null)
            A = new Availability();
        A.SocietyID = Cmn.ToInt(hdSocietyID.Value);
        A.AvailabilityType = RadioButtonListAvlFor.SelectedIndex;//0 sale,1 rent
        A.SellerType = Cmn.ToInt(radioButonListSellerType.SelectedIndex);//0 agent,1 owner,2 builder
        A.SellerID = Cmn.ToInt(hidSellerID.Value);
        A.ApartmentTypeID = Cmn.ToInt(hdApartmentTypeID.Value);
        A.SocietyName = txtSociety.Text;
        A.Amount = Cmn.ToInt(txtAmount.Text);
        A.DateAvailableFrom = Cmn.ToDate(txtDateAvailableFrom.Text);
        if(ddlFlorNo.SelectedValue!="-1")
        A.FloorNo = Cmn.ToInt(ddlFlorNo.SelectedValue);
        if (ddlFacingDir.SelectedValue != "-1")
        A.DirectionalFacing = ddlFacingDir.SelectedValue;
        A.RentFor = Cmn.ToInt(ddrentFor.SelectedValue);
        A.Description = txtDetails.Text;
        A.PostedOnDate = Cmn.GetIndiaTime();
        A.Save();
        if (A.Message.Length == 0)
        {
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
            lblStatus.Text = "Data Saved-" + A.Message;
            WriteClientScript("parent.RefreshAvlList(" + txtSociety.Text + ");");
        }
        A.Save();
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
        return "<span><img style='margin:2px; height:40px;width:40px;' src='" + ImgageSource + "'/><a href='#'  style='z-index:99999px; position:relative; background-color:white;left:-10px;top:-30px; color:red; ' onclick='return DeleteImage(\"" + ImgageSource + "\");'>X</a></span>";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //using (AvailabilityTableAdapter ta = new AvailabilityTableAdapter())
            //{
            //    PropertyMap.AvailabilityDataTable dt = ta.GetDataByID(Cmn.ToInt(lblID.Text));
            //    if (dt.Rows.Count > 0)
            //        dt[0].Delete();
            //    ta.Update(dt);
            //}
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
        ShowData(0);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
    }
    protected void RadioButtonListAvlFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonListAvlFor.SelectedValue == "0")
            trRentFor.Visible = true;
        else
            trRentFor.Visible = false;
    }
}