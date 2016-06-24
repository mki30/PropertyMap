using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyListModel;
using System.IO;
using System.Web.UI.WebControls;

public partial class Edit_ManageImageSelect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            City.CityList(ddlCity);
            if (Request.Cookies["CityID"] != null)
                ddlCity.SelectedValue = Request.Cookies["CityID"].Value;
        }
        ShowList();
    }

    void ShowList()
    {
        int CityID = Cmn.ToInt(ddlCity.SelectedValue);
        int SubCityID = Cmn.ToInt(ddSubCity.SelectedValue);

        List<Society> SocietyList = SubCityID == 0 ? Society.GetByCity(CityID) : Society.GetBySubCity(SubCityID);

        lstProject.Items.Clear();
        foreach (Society S in SocietyList)
        {
            if ((S.Verified == 1 && S.Verified != null && S.Deleted != 1))
                lstProject.Items.Add(new ListItem(S.SocietyName, S.ID.ToString()));
        }
    }
    
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCityList();
        Response.Cookies["CityID"].Value = ddlCity.SelectedValue;
        Response.Cookies["CityID"].Expires = DateTime.Now.AddDays(1);
    }

    protected void FillCityList()
    {
        City.CityList(ddSubCity, Cmn.ToInt(ddlCity.SelectedValue));
        ddSubCity.Items.Insert(0, new ListItem("<All>", "0"));
    }

   // public List<string> urls = new List<string>();

   // protected void btnUpdateImageUrlNames_Click(object sender, EventArgs e)
   // {
   //     List<Society> AllSocieties = Society.GetAll();
   //     List<ApartmentType> AllApartmentTypes = ApartmentType.GetAll();
   //     Dictionary<string, ImagesDetail> ImageList = new Dictionary<string, ImagesDetail>();
   //     List<ImagesDetail> IDS = ImagesDetail.GetList();

   //     foreach (ImagesDetail imgDetails in IDS)
   //     {
   //         Society S = null;
            
   //         string urlName = "";
   //         if (imgDetails.ImageReferenceType != 4)
   //         {
   //             S = AllSocieties.FirstOrDefault(m => m.ID == Cmn.ToInt(imgDetails.ReferenceID));
   //             if (S == null)
   //                 continue;
   //         }

   //         switch (imgDetails.ImageReferenceType)
   //         {
   //             case 1:    //Project Images
   //                 urlName = (Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-" + imgDetails.ImageID.Split('.')[0]).ToLower();
   //                 urlName = GetUniqueURL(IDS, S, urlName, "");
   //                 break;

   //             case 2:   //Laypot Plan
   //                 urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-layout-plan";
   //                 urlName = GetUniqueURL(IDS, S, urlName, "layout-plan");
   //                 break;

   //             case 3:   //Logo Image
   //                 urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-logo";
   //                 urlName = GetUniqueURL(IDS, S, urlName, "logo");
   //                 break;
               
   //             case 4:
   //                 ApartmentType AT = AllApartmentTypes.FirstOrDefault(m => m.ID == Cmn.ToInt(imgDetails.ReferenceID));
   //                 if (AT != null)
   //                 {
   //                     Society Soc = AllSocieties.FirstOrDefault(m => m.ID == Cmn.ToInt(AT.SocietyID));
   //                     if (Soc != null)
   //                     {
   //                         urlName = (Cmn.GenerateSlug(Soc.SocietyName) + "-" + Cmn.GenerateSlug(Soc.Subcity) + "-" + imgDetails.ImageID.Split('.')[0]).ToLower();
   //                         urlName = urlName = GetUniqueURL(IDS, S, urlName, "");
   //                     }
   //                 }
   //                 break;
   //         }
   //         imgDetails.UrlName = urlName;
   //     }
   //     UpdateInDatabase(IDS);
   // }

   // void UpdateInDatabase(List<ImagesDetail> IDS)
   // {
   //     DatabaseCE db = new DatabaseCE(Global.PropertyMapConnnection);
   //     try
   //     {
   //         foreach (ImagesDetail fp in IDS)
   //         {
   //             if (fp != null)
   //             {
   //                 db.RunQuery("Update ImagesDetails SET URLName='" + fp.UrlName + "' WHERE ID=" + fp.ID);
   //             }
   //         }
   //     }
   //     catch (Exception ex)
   //     {
   //     }
   //     finally
   //     {
   //         db.Close();
   //     }
   // }

   // string GetUniqueURL(List<ImagesDetail> IDS, Society S, string urlName, string appendText)
   // {
   //     //ImagesDetail id = IDS.FirstOrDefault(m => m.UrlName == urlName);
   //     string url = urlName;
   //     if (urls.Contains(urlName) || string.IsNullOrWhiteSpace(urlName))
   //         url = (S.URLName + (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
   //     urls.Add(url);
   //     return url;
   //}

   // protected void btnRenameFiles_Click(object sender, EventArgs e)
   // {
   //     List<ImagesDetail> IDs = ImagesDetail.GetList();
   //     string[] paths = { "/Data/Images_Society/", "/Data/Images_LayoutPlan/", "/Data/Images_SocietyLogo/", "/Data/Images_ApartmentType/" };
        
   //     foreach (ImagesDetail ID in IDs)
   //     {
   //         switch (ID.ImageReferenceType)
   //         {
   //             case 1:
   //                 string sourceFile = Server.MapPath(paths[0] + ID.ImageID);
   //                 if (File.Exists(sourceFile))
   //                 {
   //                     try
   //                     {
   //                         string destFile = Server.MapPath(paths[0]) + ID.UrlName + ".jpg";
   //                         File.Move(sourceFile, destFile);
   //                     }
   //                     catch{};
   //                 }
   //                 break;
   //             case 2:
   //                 string sourceFile2 = Server.MapPath(paths[1] + ID.ImageID);
   //                 if (File.Exists(sourceFile2))
   //                 {
   //                     try
   //                     {
   //                         string destFile = Server.MapPath(paths[1]) + ID.UrlName + ".jpg";
   //                         File.Move(sourceFile2, destFile);
   //                     }
   //                     catch { };
   //                 }
   //                 break;
   //             case 3:
   //                 string sourceFile3 = Server.MapPath(paths[2] + ID.ImageID);
   //                 if (File.Exists(sourceFile3))
   //                 {
   //                     try
   //                     {
   //                         string destFile = Server.MapPath(paths[2]) + ID.UrlName + ".jpg";
   //                         File.Move(sourceFile3, destFile);
   //                     }
   //                     catch { };
   //                 }
   //                 break;
   //             case 4:
   //                 string sourceFile4 = Server.MapPath(paths[3]) + ID.ImageID;
   //                 if (File.Exists(sourceFile4))
   //                 {
   //                     try
   //                     {
   //                         string destFile = Server.MapPath(paths[3]) + ID.UrlName + ".jpg";
   //                         File.Move(sourceFile4, destFile);
   //                     }
   //                     catch { };
   //                 }
   //                 break;
   //         }
   //     }
   // }
}