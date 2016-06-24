using ImageResizer;
using PropertyListModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_ManageImages : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
            int  referenceID = Cmn.ToInt(QueryString("referenceid")),
                  imageType = Cmn.ToInt(QueryString("imagetype"));
            string img = QueryString("img");

        if (referenceID != 0 && imageType != 0)
        {
            lblRefID.Text = referenceID.ToString(); 
            lblImageType.Text = ((ImagesLocations)imageType).ToString().Replace("_", " ");
            txtImageTypeID.Value = imageType.ToString();
            txtReferenceID.Value = referenceID.ToString();
            txtImageLocation.Value = Global.ImagesRootLocation[imageType];

            if (string.IsNullOrWhiteSpace(img))
            {
                pnlcmn.Visible = true;
                imgDiv.Visible = true;
                pnledt.Visible = false;
                ltmce.Visible = false;
                getImages(referenceID, imageType);
            }
            else
            {
                btnAddImage.Attributes.Add("onclick", "$('#" + drpImg.ClientID + "').click();return false;");
                pnlcmn.Visible = false;
                imgDiv.Visible = false;
                pnledt.Visible = true;
                divEditImg.Visible = true;
                ltmce.Visible = true;
                if (!IsPostBack)
                {
                    txtSelectedImg.Value = img;
                    getImageDetails(referenceID, imageType, img);
                }
            }
        }
    }

    void getImageDetails(int ReferenceID, int ImageType, string ImageID)
    {
        ImagesDetail imgD = ImagesDetail.GetByIDs(ReferenceID, ImageType, ImageID);
        if (imgD != null)
        {
            btnDelete.Visible = true;
            lblImgID.Text = "<a href='/" + txtImageLocation.Value + "/" + ImageID + "' target='_blank'>" + ImageID + "</a>";
            imgPrev.Src = "/" + txtImageLocation.Value + "/" + ImageID;
            btnAddImage.Text = "Chanege Image";
            lblDetailID.Text = imgD.ID.ToString();
            lblSize.Text = imgD.Width + "x" + imgD.Height + " <small>(saved size for large image)</small>";

            if (imgD.Data != null)
                txtImgDesc.Text = Cmn.GetUnCompressed(imgD.Data, (int)imgD.DataSize);

            txtLat.Value = imgD.Lat.ToString();
            txtLng.Value = imgD.Lng.ToString();
            txtCourtesy.Value = imgD.Courtesy;
            txtReferenceLink.Value = imgD.Reference;
            txtTagline.Value = imgD.Tagline;
            txtUrlName.Value = imgD.UrlName;
        }
    }

    void getImages(int referenceID, int imagesType)
    {
       //dispaly images for the reference
       imgDiv.InnerHtml= GetSocietyImage(referenceID,imagesType);
    }

    static string GetSocietyImage(int DataID,int imagesType)
    {
        var img = "";
        string folder="";
        if(imagesType==2)
        folder="Images_LayoutPlan";
        if (imagesType == 3)
            folder = "Images_SocietyLogo";
        switch (imagesType)
        {
            case 1:
                {
                    string[] images = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Images_Society/"), DataID + "_*.*");
                    foreach (var im in images)
                    {
                        img += "<span><a href='/Edit/manageImages.aspx?referenceid=" + DataID + "&imagetype=" + imagesType + "&img=" + Path.GetFileName(im) + "'><img style='margin:2px; height:200px;width:300px;' src='/Data/Images_Society/" + Path.GetFileName(im) + "'/><a></span>";
                    }
                    break;
                }
            case 2:
            case 3:
                img += "<span><a href='/Edit/manageImages.aspx?referenceid=" + DataID + "&imagetype=" + imagesType + "&img=" + DataID + ".jpg'><img style='margin:2px; height:200px;width:300px;' src='/Data/" + folder + "/" + DataID + ".jpg'/><a></span>";
                break;
            case 4:
                {
                    List<ApartmentType> AT = ApartmentType.GetBySocietyID(DataID);
                    foreach (ApartmentType A in AT)
                    {
                        img += "<span><a href='/Edit/manageImages.aspx?referenceid=" + A.ID + "&imagetype=" + imagesType + "&img=" + A.ID + ".jpg'><img style='margin:2px; height:200px;width:300px;border:1px dotted gray;' src='/Data/Images_ApartmentType/" + A.ID + ".jpg'/><a></span>";
                    }
                    break;
                }
        }
        return img;
    }

    protected void btnDoneEdit_ServerClick(object sender, EventArgs e)
    {
        int id = Cmn.ToInt(lblDetailID.Text),
            CompanyID = Cmn.ToInt(txtCompanyID.Value),
            ReferenceID = Cmn.ToInt(txtReferenceID.Value),
            ImageType = Cmn.ToInt(txtImageTypeID.Value);
        string ImageID = txtSelectedImg.Value;
        if (ImageID == "0_0.jpg")
            ImageID = string.Empty;

        if (string.IsNullOrWhiteSpace(ImageID) && id == 0 && !drpImg.HasFile)
        {
            lblStatus.Text = "Please Select Image";
            return;
        }
        if (drpImg.HasFile)
        {
            string imageName = Cmn.GetCompanyProjectImageFileName(ReferenceID, "~/" + Global.ImagesRootLocation[(int)ImagesLocations.Projects_Images]),
                 folder = HttpContext.Current.Server.MapPath("~/" + Global.ImagesRootLocation[(int)ImagesLocations.Projects_Images]), message = string.Empty;

            if (imageName == "405" && string.IsNullOrWhiteSpace(ImageID))
            {
                lblStatus.Text = "Can Not Upload More Images, Max 6 Images Allowed";
                return;
            }
            if (imageName == "405" && !string.IsNullOrWhiteSpace(ImageID))
                Global.GCCollect();
            try
            {
                if (string.IsNullOrWhiteSpace(ImageID))
                    ImageID = imageName + ".jpg";

                HttpPostedFile hpf = drpImg.PostedFile;
                hpf.SaveAs(folder + ImageID);
                ImageBuilder.Current.Build(hpf, folder + ImageID.Replace(".jpg", ""), new ResizeSettings("width=254&height=254&crop=auto&format=jpg&quality=90"), false, true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message; return;
            }
        }

        ImagesDetail imgD = (id != 0 ? ImagesDetail.GetByID(id) : ImagesDetail.GetByIDs(ReferenceID, ImageType, ImageID));
        if (imgD == null)
            imgD = new ImagesDetail();

        imgD.ReferenceID = ReferenceID;
        imgD.ImageReferenceType = ImageType;
        imgD.ImageID = ImageID;        
        imgD.Data = Cmn.GetCompressed(txtImgDesc.Text);
        imgD.DataSize = txtImgDesc.Text.Length;
        imgD.Lat = Cmn.ToDbl(txtLat.Value);
        imgD.Lng = Cmn.ToDbl(txtLng.Value);
        imgD.Courtesy = txtCourtesy.Value.Trim();
        imgD.Reference = txtReferenceLink.Value.Trim();
        imgD.Tagline = txtTagline.Value.Trim();
        imgD.UrlName = txtUrlName.Value.Trim();
        string fullpath = HttpContext.Current.Server.MapPath("~/data/CompanyDetails/LI_Designer_Projects/" + ImageID);
        if (File.Exists(fullpath))
        {
            System.Drawing.Image objImage = System.Drawing.Image.FromFile(fullpath);
            imgD.Width = Cmn.ToDbl(objImage.Width);
            imgD.Height = Cmn.ToDbl(objImage.Height);
            imgD.SizeKb = Cmn.ToDbl((new FileInfo(fullpath).Length / 1024));
        }
        imgD.Save();
        if (imgD != null && imgD.ID != 0)
        {
            lblDetailID.Text = imgD.ID.ToString();
            txtSelectedImg.Value = ImageID;
            lblImgID.Text = "<a href='/" + txtImageLocation.Value + "/" + ImageID + "' target='_blank'>" + ImageID + "</a>";
            imgPrev.Src = "/" + txtImageLocation.Value + "/" + ImageID;
            btnDelete.Visible = true;
            WriteClientScript("reloadWindow=2;");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string ImageID = txtSelectedImg.Value;
        ImagesDetail imgd = ImagesDetail.GetByID(Cmn.ToInt(lblDetailID.Text));
        if (imgd != null)
            imgd.Delete();

        lblImgID.Text = "0";

        Global.GCCollect();

        string imgafolder = Global.ImagesRootLocation[(int)ImagesLocations.Projects_Images];
        File.Delete(Server.MapPath("~/" + imgafolder + "/" + ImageID));
        File.Delete(Server.MapPath("~/" + imgafolder + "/" + "600x600/" + ImageID));
        lblStatus.Text = "Image Deleted";
        WriteClientScript("reloadWindow=1;");
    }
}