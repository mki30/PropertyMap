using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Collections.Generic;
using System.Linq;

public partial class Edit_EditSociety : BasePageEdit
{
    List<string> urls = new List<string>();
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        
        int ctr = 0;
        foreach (ImagesDetail id in Global.ImageDetailsList)
        {
            urls.Add(id.UrlName);
        }
        
        foreach (var amen in Enum.GetValues(typeof(Amenities)))
        {
            CheckBox chk = new CheckBox();
            chk.Text = amen.ToString().Replace('_', ' ');
            chk.ID = "chkAmenities" + (int)amen;
            lblCheckbox.Controls.Add(new Literal() { Text = "<span style=''padding-bottom:1px;></span>" });
            lblCheckbox.Controls.Add(chk);
            if(++ctr % 6==0)
                lblCheckbox.Controls.Add(new Literal(){Text="<br/>"});
        }
        
        if (!IsPostBack)
        {
            lblBuilderID.Text = hidBuilderID.Value = QueryString("BuilderID");
            FillCity(ddlCity, 0);
            ShowData(Cmn.ToInt(QueryString("ID")));

            foreach (ProjectPriceType pricetype in Enum.GetValues(typeof(ProjectPriceType)))
            {
                ddProjectPriceType.Items.Add(new ListItem(Global.GetText(pricetype).ToString(), ((int)pricetype).ToString()));
            }
        }
    }

    void UpdateData()
    {
        string aminityValue = "";
        
        foreach (var BA in Enum.GetValues(typeof(Amenities)))
        {
            aminityValue += (lblCheckbox.FindControl("chkAmenities" + (int)BA) as CheckBox).Checked ? "1" : "0";
        }

        Society s = Society.GetByID(Cmn.ToInt(lblID.Text));
        
        if (s == null)
            s = new Society();

        s.ID = Cmn.ToInt(lblID.Text);
        s.SocietyName = txtSocietyName!=null?txtSocietyName.Text:"";
        s.Town = txtTown.Text;
        
        s.Lat = Cmn.ToDbl(txtLat.Text);
        s.Lng = Cmn.ToDbl(txtLng.Text);
        s.Address = txtAddress.Text;
        //s.UseType = Cmn.ToInt(ddUseType.SelectedItem.Value);
        //s.PropertyType = Cmn.ToInt(ddPropertyType.SelectedValue);

        s.City = ddlCity.SelectedItem.Text;
        s.CityID = Cmn.ToInt( ddlCity.SelectedItem.Value);

        //if (ddlSubCity.SelectedValue != (-1).ToString())
        //{
            s.Subcity = ddlSubCity.SelectedValue != "-1" ? ddlSubCity.SelectedItem.Text : "";
            s.SubCityID = Cmn.ToInt(ddlSubCity.SelectedValue != "-1" ? ddlSubCity.SelectedItem.Value : null);
        //}

        s.Area = "";
        s.AreaID = null;

        if (ddArea.SelectedItem != null && ddArea.SelectedValue != (-1).ToString())
        {
            s.Area = ddArea.SelectedItem.Text;
            s.AreaID = Cmn.ToInt(ddArea.SelectedItem.Value);
        }
        
        if (string.IsNullOrWhiteSpace(txtURLName.Text))
            txtURLName.Text = s.SocietyName.Trim() + (string.IsNullOrEmpty(s.Subcity)?"":"-" + s.Subcity )+"-" + s.City;
        if ((((string)txtURLName.Text)).Contains("--"))
            txtURLName.Text = txtURLName.Text.Replace("--", "-");
        s.URLName = txtURLName.Text.Replace(" ", "-");
        s.State = ddlState.SelectedValue.ToString();
        s.Pin = txtPin.Text;
        s.Country = "India";
        s.Amenities = aminityValue;
        s.StartDate = Cmn.ToDate(txtStartDate.Text);
        s.EndDate = Cmn.ToDate(txtCompDate.Text);
        s.Status = Cmn.ToInt(ddStatus.SelectedValue);
        s.VedioLink = txtURL.Text;
        s.Description = txtDetail.Text;
        s.BuilderID = Cmn.ToInt(hidBuilderID.Value);
        s.Deleted= chkDeleted.Checked?1:0;
        s.Verified= chkVerified.Checked ? 1 : 0;
        s.URL = txtURLBuilder.Text;
        s.BrochureURL = txtBrochureURL.Text;
        s.PricelistURL = txtPriceList.Text;
        s.PolyPoints=txtPoly.Text;
        s.Save();

        SaveProjectPricing(s.ID);

        string urlName = "";
        if (s.Message.Length == 0)
        {
            if (ImagesUpload.HasFile)
            {
                //HttpFileCollection hfc = Request.Files;
                //for (int img = 0; img < hfc.Count; img++)
                //{
                //    HttpPostedFile hpf = hfc[img];
                //    if (hpf.ContentLength > 0)
                //    {
                //        hpf.SaveAs(GetSocietyImageFileName(s.ID));
                //    }
                //}
                
                HttpFileCollection hfc = Request.Files;
                string[] allkeys = hfc.AllKeys;
                int imgLogoCount=0,imgLayoutCount=0; 
                foreach(string imgkey in allkeys)
                 {
                     if (imgkey == "LogoUpload")
                         imgLogoCount++;
                     if (imgkey == "LayoutUpload")
                         imgLayoutCount++;
                 }

                int imgCounter = 0;
                for (int img = 0; img < hfc.Count; img++)
                {
                    if (imgLogoCount > 0 || imgLayoutCount>0)  //skip logo and layout image from image collection collection
                    {
                        if (img == 0) continue;
                        if (img == hfc.Count - 1) continue;
                    }
                    imgCounter++;
                    HttpPostedFile hpf = hfc[img];
                    if (hpf.ContentLength > 0)
                    { 
                        //hpf.SaveAs(GetSocietyImageFileName(s.ID));
                        urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-" + s.ID + "_" + (imgCounter);
                        urlName = GetUniqueURL(Global.ImageDetailsList, s, urlName, "");
                        ImagesDetail id = new ImagesDetail();
                        id.ReferenceID = s.ID;
                        id.ImageReferenceType = (int)ImagesLocations.Projects_Images;
                        id.ImageID = s.ID + "_" + (imgCounter) + ".jpg";
                        id.UrlName = urlName;
                        id.Save();
                        hpf.SaveAs(Server.MapPath("~/Data/Images_Society/") + urlName + ".jpg");
                    }
                }
            }
            
            if (LayoutUpload.HasFile)      //second single file upload for layout plan.
            {
                //string Save_Location = Server.MapPath("~/Data/Images_LayoutPlan/") + s.ID + Path.GetExtension(FileUpload2.FileName);
                //FileUpload2.SaveAs(Save_Location);
                urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-layout-plan";
                urlName = GetUniqueURL(Global.ImageDetailsList, s, urlName, "layout-plan");
                string Save_Location = Server.MapPath("~/Data/Images_LayoutPlan/") + urlName + Path.GetExtension(LayoutUpload.FileName);
                ImagesDetail id = new ImagesDetail();
                id.ReferenceID = s.ID;
                id.ImageReferenceType = (int)ImagesLocations.Project_Layout;
                id.ImageID = s.ID + ".jpg";
                id.UrlName = urlName;
                id.Save();
                LayoutUpload.SaveAs(Save_Location);
            }
            
            if (LogoUpload.HasFile)      //third single file upload for Society Logo.
            {
                //string Save_Location = Server.MapPath("~/Data/Images_SocietyLogo/") + s.ID + Path.GetExtension(LogoUpload.FileName);
                //LogoUpload.SaveAs(Save_Location);
                urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-logo";
                urlName = GetUniqueURL(Global.ImageDetailsList, s, urlName, "logo");
                string Save_Location = Server.MapPath("~/Data/Images_SocietyLogo/") + urlName + Path.GetExtension(LogoUpload.FileName);
                ImagesDetail id = new ImagesDetail();
                id.ReferenceID = s.ID;
                id.ImageReferenceType = (int)ImagesLocations.Project_Logo;
                id.ImageID = s.ID + ".jpg";
                id.UrlName = urlName;
                id.Save();
                LogoUpload.SaveAs(Save_Location);
            }
            Global.LoadGlobalData();
            ShowData(s.ID);
            //WriteClientScript("parent.RefreshSocietyList(" + s.ID + ")");
         }
        else
            lblStatus.Text = s.Message;
    }

    private string SaveProjectPricing(int ProjectID)
    {
        ProjectPricing PT = new ProjectPricing();
        try
        {
            PT.ProjectID = ProjectID;
            PT.Name = txtPriceName.Text;
            PT.Value = txtPriceValue.Text;
            PT.Type = Cmn.ToInt(ddProjectPriceType.SelectedValue);
            PT.Save();
            return "";
        }
        catch{
            return "Price not updated";
        }
    }

    string GetSocietyImageFileName(int ID)
    {
        string FileName = "";

        var Folder = "~/Data/Images_Society/";
        for (int i = 1; ; i++)
        {
            FileName = Server.MapPath(Folder + ID + "_" + i) + ".jpg";
            if (!File.Exists(FileName))
                break;
        }
        return FileName;
    }

    //List<string> urls = new List<string>();
    //List<ImagesDetail> ids=new List<ImagesDetail>();
    //ids=Global.ImageDetailsList;

    string GetUniqueURL(List<ImagesDetail> IDS, Society S, string urlName, string appendText)
    {
        string url = urlName;
        if (urls.Contains(urlName) || string.IsNullOrWhiteSpace(urlName))
            url = (S.URLName + (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
        urls.Add(url);
        return url;
    }

    public static string GetSocietyLogo(int DataID)
    {
            var img = "";
            ImagesDetail imgDetails = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID ==DataID && m.ImageReferenceType == (int)ImagesLocations.Project_Logo);
            if (imgDetails != null)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/Data/Images_SocietyLogo/" + imgDetails.UrlName) + ".jpg"))
                {
                    img += ImgSpan("/Data/Images_SocietyLogo/" + Path.GetFileName(imgDetails.UrlName) + ".jpg",imgDetails.ID);
                }
            }
            return img;
    }

    static string ImgSpan(string ImgageSource, int imageID)
    {
        return "<span><img style='margin:2px; height:40px;width:40px;' src='" + ImgageSource + "'/><a href='#'  style='z-index:99999px; position:relative; background-color:white;left:-40px;top:-30px; color:red; ' onclick='return DeleteImage(\"" + ImgageSource + "\","+imageID+");'>X</a></span>";
    }

    public static string GetSocietyImage(int DataID)
    {
        var img = "";
        List<ImagesDetail> imgDetails = Global.ImageDetailsList.Where(m => m.ReferenceID == DataID && m.ImageReferenceType == (int)ImagesLocations.Projects_Images).ToList();
        
        //string[] images = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Images_Society/"), DataID + "_*.*");
        //foreach (var im in images)
        //{
        //    img += ImgSpan(@"../Data/Images_Society/" + Path.GetFileName(im));
        //}
        foreach (ImagesDetail im in imgDetails)
        {
            img += ImgSpan(@"/Data/Images_Society/" + Path.GetFileName(im.UrlName)+".jpg",im.ID);
        }
        return img;
    }

    public static string GetLayoutImg(int DataID)
    {
        var img = "";
        var initialPath = "/Data/Images_LayoutPlan/";
        ImagesDetail imgDetails = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID ==DataID && m.ImageReferenceType == (int)ImagesLocations.Project_Layout);
        if (imgDetails != null)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(initialPath + imgDetails.UrlName) + ".jpg"))
            {
                img += ImgSpan(initialPath + Path.GetFileName(imgDetails.UrlName) + ".jpg", imgDetails.ID);
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(initialPath + imgDetails.UrlName) + ".gif"))
            {
                img += ImgSpan(initialPath + Path.GetFileName(imgDetails.UrlName) + ".gif", imgDetails.ID);
            }
        }
        return img;
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if ((ddlCity.SelectedValue != "-1"))
        {
            UpdateData();
            lblStatus.Text = "Updated";
        }
        else
        {
            lblStatus.Text = "Not Saved! Please select city and subcity Properly!";
        }
    }
    
    void ShowData(int ID)
    {
        int currentYear = Cmn.GetIndiaTime().Year;
        
        if(QueryString("CityId")!="")        //city selection based on cookie value
        ddlCity.SelectedValue = QueryString("CityId");
        FillCity(ddlSubCity, Cmn.ToInt(ddlCity.SelectedItem.Value));
        
        Society S = Society.GetByID(ID);
        
        if (S != null)
        {
            ltGoogleEarthLink.Text = "<a href='" +  S.CreateKML() + "'>GE</a>" ;
            
            lblID.Text = S.ID.ToString();
            if(S.DataSourceURL!="" && S.DataSourceURL!=null)
            ltSourceUrl.Text = "<a href=" + S.DataSourceURL + " target='_blank'>Source</a>";
            ltProjectUrl.Text = "<a href='/project/" + S.URLName + "' target='_blank'>Project</a>";
            txtSocietyName.Text = S.SocietyName != null ? S.SocietyName.Trim() : "";
            txtTown.Text=S.Town!=null?S.Town:"";
            ddStatus.SelectedValue = S.Status.ToString();
            txtURLName.Text = S.URLName != null ? S.URLName : "";
            txtLat.Text = S.Lat != null ? S.Lat.ToString() : "";
            txtLng.Text = S.Lng != null ? S.Lng.ToString() : "";
            txtAddress.Text = S.Address != null ? S.Address : "";
            //ddUseType.SelectedIndex = Cmn.ToInt(S.UseType);
            //ddPropertyType.SelectedIndex = Cmn.ToInt(S.PropertyType);
            
            try
            {
                ddlCity.SelectedValue = S.CityID.ToString();
            }
            catch { }

            FillCity(ddlSubCity, S.CityID);

            try
            {
                ddlSubCity.SelectedValue = S.SubCityID.ToString();
            }
            catch { }

            FillCity(ddArea, S.SubCityID);
            try
            {
                ddArea.SelectedValue = S.AreaID.ToString();
            }
            catch {}
            
            ddlState.SelectedValue = S.State;
            txtPin.Text = S.Pin != null ? S.Pin : "";
            //ddlBuiltYear.SelectedValue = S.BuiltYear != null ? S.BuiltYear.ToString() : "";
        
            lblBuilderID.Text = S.BuilderID.ToString();
            hidBuilderID.Value = S.BuilderID.ToString();
            lblWiki.Text = "<a target='_blank' href='http://wikimapia.org/#lang=en&lat=" + txtLat.Text + "&lon="+txtLng.Text+"&z=18&m=b'>Wiki</a>";
            Agent A;
            ltBuilder.Text = "Builder";

            if (Global.BuilderList.TryGetValue((int)(S.BuilderID == null ? 0 : S.BuilderID), out A) && S.BuilderID != null)
            {
                txtBuilder.Text = A.AgentCompany;
                ltBuilder.Text = "<a href='" + A.URL + "' target='_blank'>Builder</a>";
            }

            if(!(S.StartDate==Cmn.MinDate))
            txtStartDate.Text = S.StartDate != null ? Cmn.ToDate(S.StartDate).ToString("MMM-yyyy") : "";
            if (!(S.EndDate == Cmn.MinDate))
            txtCompDate.Text = S.EndDate != null ? Cmn.ToDate(S.EndDate).ToString("MMM-yyyy") : "";
            txtURL.Text = S.VedioLink != null ? S.VedioLink : "";

            ltBuilderURL.Text = "URL";
            txtURLBuilder.Text = S.URL != null ? S.URL: "";
            if(txtURLBuilder.Text!="")
            ltBuilderURL.Text = "<a href='" + txtURLBuilder.Text + "' target='_blank'>URL</a>";

            ltBrochureURL.Text = "Brochure";
            txtBrochureURL.Text = S.BrochureURL != null ? S.BrochureURL : "";
            txtPriceList.Text=S.PricelistURL!=null? S.PricelistURL : "";

            if(txtBrochureURL.Text!="")
                ltBrochureURL.Text = "<a href='" + txtBrochureURL.Text + "' target='_blank'>Brochure</a>";
            
            txtDetail.Text = S.Description != null ? S.Description.ToString() : "";

            string amn = (S.Amenities != null ? S.Amenities : "").PadRight(10, '0');
            foreach (var BA in Enum.GetValues(typeof(Amenities)))
            {
                (lblCheckbox.FindControl("chkAmenities" + (int)BA) as CheckBox).Checked = amn[(int)BA] == '1' ? true : false;
            }
            
            txtPoly.Text = S.PolyPoints;
            
            //if (Cmn.ToDbl(txtLat.Text) != 0 && Cmn.ToDbl(txtLng.Text) != 0)
            //    WriteClientScript("parent.SetCenter(" + txtLat.Text + "," + txtLng.Text + ")");
            //lblImages.Text = GetSocietyImage(Cmn.ToInt(lblID.Text));
            LogoImgDiv.InnerHtml = GetSocietyLogo(Cmn.ToInt(lblID.Text));

            //if(File.Exists(Server.MapPath("~/Data/Images_SocietyLogo/"+lblID.Text+".jpg")))
            imageDiv.InnerHtml = GetSocietyImage(Cmn.ToInt(lblID.Text));
            LayoutImgDiv.InnerHtml = GetLayoutImg(Cmn.ToInt(lblID.Text));
            
            chkVerified.Checked = (S.Verified!=null?(Cmn.ToInt(S.Verified) == 0 ? false : true):false);
            chkDeleted.Checked = (S.Deleted != null ? (Cmn.ToInt(S.Deleted) == 0 ? false : true) : false);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        imageDiv.InnerHtml = "";
        Cmn.ClearTextBoxes(Page);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string[] Lines = TextBox1.Text.Split('\n');
        foreach (string s in Lines)
        {
            string[] F = s.Split('\t');

            new Society()
            {
                SocietyName = F[0].Trim(),
                Lat = Cmn.ToDbl(F[1].Trim()),
                Lng = Cmn.ToDbl(F[2].Trim()),
                Address = F[3].Trim(),
                City = F[4].Trim(),
                State = F[5].Trim(),
                Pin = F[6].Trim(),
                Country = F[7].Trim()
            }.Save();
        }
    }
    
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCity(ddlSubCity, Cmn.ToInt( ddlCity.SelectedItem.Value));
    }

    protected void ddlSubCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCity(ddArea, Cmn.ToInt(ddlSubCity.SelectedItem.Value));
    }
    
    private void FillCity(ListControl lst, int? ParentID)
    {
        lst.Items.Clear();
        if (ParentID == null)
            return;
        lst.Items.Add(new ListItem("<Select>","-1"));
        List<City> city = City.GetByParentID((int)ParentID);
        foreach (City C in city)
        {
            lst.Items.Add(new ListItem(C.Name, C.ID.ToString()));
        }
    }
    protected void txtSocietyName_TextChanged(object sender, EventArgs e)
    {
    }
}