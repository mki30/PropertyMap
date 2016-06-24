using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using PropertyListModel;
using System.Web;
using System.Net;
using System.Net.Mail;

public partial class Data : BasePage
{
    StringBuilder str = new StringBuilder();
    Boolean AppendError = true;
    
    protected new void Page_Load(object sender, EventArgs e)
    {
        Action = QueryString("Action");
        string Data1 = Request.QueryString["Data1"] != null ? Request.QueryString["Data1"].ToString() : "";
        string Data2 = Request.QueryString["Data2"] != null ? Request.QueryString["Data2"].ToString() : "";
        string Data3 = Request.QueryString["Data3"] != null ? Request.QueryString["Data3"].ToString() : "";
        string Data4 = Request.QueryString["Data4"] != null ? Request.QueryString["Data4"].ToString() : "";
        string Data5 = Request.QueryString["Data5"] != null ? Request.QueryString["Data5"].ToString() : "";
        string Data6 = Request.QueryString["Data6"] != null ? Request.QueryString["Data6"].ToString() : "";
        string Data7 = Request.QueryString["Data7"] != null ? Request.QueryString["Data7"].ToString() : "";
        string abc = HttpContext.Current.Request.UserAgent;
        string term = Request.QueryString["term"] != null ? Request.QueryString["term"].ToString() : "";
        string Error = "";

        try
        {
            switch (Action)
            {
                case "GetProjectPricingRecord":
                    {
                        ProjectPricing PP = ProjectPricing.GetByID(Cmn.ToInt(Data1));
                        if (PP != null)
                        {
                            str.Append(new JavaScriptSerializer().Serialize(new {ID=PP.ID,Name=PP.Name,Value=PP.Value,Type=PP.Type}));
                            AppendError = false;
                        }
                    }
                    break;

                case "GetProjectPricingTable":
                    {
                        List<ProjectPricing> PPL = ProjectPricing.GetByProjectID(Cmn.ToInt(Data1));
                        StringBuilder sb = new StringBuilder("<table border='1'><tr><th>Name<th>Value<th>Type");
                        if (PPL.Count > 0)
                        {
                            foreach (ProjectPricing PP in PPL)
                            {
                                sb.Append("<tr><td><a onclick='GetPricingRecord(" + PP.ID + ")'>" + PP.Name + "</a><td>" + PP.Value + "<td>" + Global.GetText((ProjectPriceType)PP.Type) + "");
                            }
                        }
                        sb.Append("</table>");
                        str.Append(sb);
                    }
                    break;

                case "SaveProjectPriceing":
                    {
                        AppendError = false;
                        NameValueCollection nvc = Request.Form;
                        ProjectPricing PP = ProjectPricing.GetByID(Cmn.ToInt(Data1));
                        if (PP == null)
                            PP = new ProjectPricing();
                        try
                        {
                            PP.ProjectID = Cmn.ToInt(Data2);
                            PP.Name = nvc["txtPriceName"];
                            PP.Value = nvc["txtPriceValue"];
                            PP.Type = Cmn.ToInt(nvc["ddProjectPriceType"]);
                            PP.Save();
                        }
                        catch
                        {
                            AppendError = true;
                            Error += "Error";
                        }
                    }
                    break;
                case "GetPDF": GetPDF(); break;
                case "ShowPDF": ShowPDF(); break;

                case "UpdateAssignClientDesc":
                    {
                        NameValueCollection nvc = Request.Form;
                        AsignClient AC = AsignClient.GetByID(Cmn.ToInt(Data1));
                        AC.Description = nvc["Desc"];
                        AC.Save();
                    }
                    break;

                case "GetClientAssign":
                    {
                        List<AsignClient> AC = AsignClient.GetAssigned(Cmn.ToInt(Data1), Cmn.ToInt(Data2));
                        StringBuilder sb = new StringBuilder("<table class='table table-striped table-hover table-condensed'><tr><th>#</th><th>Society</th><th>BHK</th><th>Price</th><th>Posted On</th><th>Avl From</th></tr>");
                        int ctr = 1;
                        foreach (AsignClient A in AC)
                        {
                            Availability Avl = Global.AvailabilityList.Values.FirstOrDefault(m => m.ID == A.AvlID);
                            string SocityName = "";
                            string UrlName = "";
                            string Area = "";
                            string price = "-";

                            if (Avl.Society != null)
                            {
                                SocityName = Avl.Society.SocietyName;
                                UrlName = Avl.Society.URLName;
                                Area = Avl.Society.Subcity + "-" + Avl.Society.City;
                                price = Avl.Amount.ToString();
                            }
                            ApartmentType apt;
                            string type = "-";
                            if (Global.ApartmentTypeList.TryGetValue((int)Avl.ApartmentTypeID, out apt)) // if the apartment type exists in the global list
                            {
                                type = apt.Bedroom + "B-" + apt.Bathroom + "T";
                            }
                            sb.Append("<tr><td>" + ctr++ + "</td><td>" + SocityName + "</td><td>" + type + "</td><td>" + Avl.Amount + "</td><td>" + Cmn.ToDate(Avl.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>" + Cmn.ToDate(Avl.DateAvailableFrom).ToString("dd-MMM-yy") + "</td></tr>");
                            sb.Append("<tr id='" + ctr++ + "' class='info'><td colspan='5'>" +
                            "<span id='desc" + ctr + "'>" + A.Description + "</span>" +
                            "<span style='display:none;' id='edit" + ctr + "'><input type='text' id='descText' value='" + A.Description + "' style='height:30px;'><input class='btn' type='button' value='save' onclick='SaveDesc(" + A.ID + ")'></span></td>" +
                            "<td><a href='#' onclick='ShowHiddenTextBox(" + ctr + ")'>Edit</a></td><tr>");
                        }
                        sb.Append("</table>");
                        str.Append(sb);
                    }
                    break;

                case "SaveAssignClient":
                    {
                        string task = Data1;
                        int AvlID = Cmn.ToInt(Data2.Split('-')[0]);
                        int AgentID = Cmn.ToInt(Data2.Split('-')[1]);
                        int ClientID = Cmn.ToInt(Data2.Split('-')[2]);

                        if (task == "add")
                        {
                            AsignClient AC = AsignClient.GetByAllIDs(AvlID, AgentID, ClientID);
                            if (AC == null)
                            {
                                AC = new AsignClient();
                                AC.AvlID = AvlID;
                                AC.AgentID = AgentID;
                                AC.ClientID = ClientID;
                                AC.Save();
                            }
                        }
                        else if (task == "remove")
                        {
                            AsignClient AC = AsignClient.GetByAllIDs(AvlID, AgentID, ClientID);
                            if (AC != null)
                                AC.Delete();
                        }
                    }
                    break;

                case "GetApartmentTypeJSON":
                    {
                        string s = Data1;
                        List<ApartmentType> AT = ApartmentType.GetBySocietyID(Cmn.ToInt(s));
                        str.Append(new JavaScriptSerializer().Serialize(AT));
                        AppendError = false;
                    }
                    break;

                case "GetAllFiles":
                    {
                        #region
                        String[] allfiles = System.IO.Directory.GetFiles(Server.MapPath(@"~\Data\"), "*.*", System.IO.SearchOption.AllDirectories);

                        StringBuilder strTemp = new StringBuilder();

                        int ctr = 0;
                        foreach (string f in allfiles)
                        {
                            FileInfo info = new FileInfo(f);
                            if (info.Extension == ".jpg" || info.Extension == ".gif")
                                strTemp.AppendLine((++ctr) + "~" + f.ToLower() + "~" + info.Length + "~" + info.LastWriteTime + "^");
                        }
                        str.Append(strTemp.ToString());
                        #endregion
                    }
                    break;

                #region GET_CHILDREN
                case "GET_CHILDREN":
                    {
                        int ParentID = Cmn.ToInt(Data1);
                        List<object> newList = new List<object>();
                        City C;
                        if (Global.CityList.TryGetValue(Cmn.ToInt(Data1), out C))
                        {
                            int ctr = 0;

                            newList = C.ChildCityList.OrderBy(m => m.SortName).Select(a => new
                            {
                                data = a.Name,
                                id = a.ID,
                                title = a.Name,
                                attr =
                                new
                                {
                                    Id = a.ID,
                                    Name = a.Name,
                                    ParentID = a.ParentID,
                                    PolyPoints = (a.PolyPoints != null ? a.PolyPoints : ""),
                                    Lat = (a.Lat == null ? 0 : a.Lat),
                                    Lng = (a.Lng == null ? 0 : a.Lng),
                                    Price = GetCityLatestAveragePrice(a),
                                    ChildCount = a.ChildCityList.Count,
                                    ChildSocietyCount = a.ChildSocietyCount,
                                    Level = 0,
                                    Index = ctr++
                                },
                                state = "closed",
                            }).ToList<object>();
                            str.Append(new JavaScriptSerializer().Serialize(newList));
                        }
                        AppendError = false;
                    } break;

                #endregion

                case "WikimapiaBoxObjectInfo":
                    {
                        #region
                        using (var client = new WebClient())
                        {
                            try
                            {
                                string FileName = Server.MapPath(@"~\Data\WikiMapia\") + Data1 + ".txt";

                                if (!File.Exists(FileName))
                                {
                                    string URL = @"http://api.wikimapia.org/?function=object"
                                    + "&key=5354BD14-C520BF6C-2BD56B87-F6969939-082E0C10-B1DC41D6-7A21EE39-348EC9A7"
                                    + "&id=" + Data1
                                    + "&format=json";
                                    // +"&pack=gzip";
                                    client.DownloadFile(URL, FileName);
                                    File.SetCreationTime(FileName, DateTime.Now);
                                }
                                str.Append(File.ReadAllText(FileName));
                                AppendError = false;
                            }
                            catch
                            {
                            }
                        }
                        #endregion
                    }
                    break;

                case "WikimapiaBoxImport":
                    {
                        #region
                        string Folder = Server.MapPath(@"~\Data\WikiMapia\");
                        if (!Directory.Exists(Folder))
                            Directory.CreateDirectory(Folder);

                        string FileName = Folder + Data1 + ".txt";

                        if (!File.Exists(FileName))
                        {
                            using (var client = new WebClient())
                            {
                                try
                                {
                                    string URL = @"http://api.wikimapia.org/?function=box"
                                    + "&key=5354BD14-C520BF6C-2BD56B87-F6969939-082E0C10-B1DC41D6-7A21EE39-348EC9A7"
                                    + "&lon_min=" + Data2
                                    + "&lat_min=" + Data3
                                    + "&lon_max=" + Data4
                                    + "&lat_max=" + Data5
                                    + "&page=" + Data6
                                    + "&format=json";
                                    // +"&pack=gzip";
                                    client.DownloadFile(URL, FileName);
                                    File.SetCreationTime(FileName, DateTime.Now);
                                }
                                catch
                                {
                                }
                            }
                        }
                        str.Append(File.ReadAllText(FileName));
                        AppendError = false;
                        #endregion
                    }
                    break;

                #region CITY
                case "City":
                    {
                        #region
                        City a;

                        if (Global.CityList.TryGetValue(Cmn.ToInt(Data1), out a))
                        {
                            var newList = new
                            {
                                ID = a.ID,
                                ParentID = a.ParentID,
                                Name = a.Name,
                                Lat = a.Lat == null ? 0 : a.Lat,
                                Lng = a.Lng == null ? 0 : a.Lng,
                                ChildCount = a.ChildCityList.Count,
                                ChildSocietyCount = a.ChildSocietyCount,
                                PolyPoints = a.PolyPoints != null ? a.PolyPoints : "",
                                Level = 0,
                                Index = 0
                            };
                            str.Append(new JavaScriptSerializer().Serialize(newList));
                        }
                        #endregion
                    }
                    break;
                #endregion

                case "CityList":
                    {
                        #region
                        City C;

                        if (Global.CityList.TryGetValue(Cmn.ToInt(Data1), out C))
                        {
                            int ctr = 0;
                            var newList = C.ChildCityList.OrderBy(m => m.SortName).Select(a => new
                            {
                                ID = a.ID,
                                ParentID = a.ParentID,
                                Name = a.Name,
                                Lat = a.Lat == null ? 0 : a.Lat,
                                Lng = a.Lng == null ? 0 : a.Lng,
                                ChildCount = a.ChildCityList.Count,
                                ChildSocietyCount = a.ChildSocietyCount,
                                PolyPoints = a.PolyPoints != null ? a.PolyPoints : "",
                                Level = 0,
                                Price = GetCityLatestAveragePrice(a),
                                Index = ctr++
                            }).ToList();
                            str.Append(new JavaScriptSerializer().Serialize(newList));
                        }
                        AppendError = false;
                        #endregion
                    }
                    break;

                case "GetNextRecord":
                    str.Append(GetNextRecord(Data1, Cmn.ToInt(Data2)));
                    AppendError = false;
                    break;

                case "GetImages":
                    {
                        List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data1), 4);
                        foreach (ImagesDetail id in ids)
                        {
                            ApartmentType AT = ApartmentType.GetByID((int)id.ReferenceID);

                            if (AT == null)
                                return;

                            string floor = "";
                            string s = Server.MapPath(@"~/Data/Images_ApartmentType/") + id.UrlName + ".jpg";

                            if (AT.PropertyType == (int)PropertyTypes.Villa)
                            {
                                if (Path.GetFileName(s).Contains("_-1")) floor = "Basement";
                                if (Path.GetFileName(s).Contains("_0")) floor = "Ground";
                                if (Path.GetFileName(s).Contains("_1")) floor = "First";
                                if (Path.GetFileName(s).Contains("_2")) floor = "Second";
                                if (Path.GetFileName(s).Contains("_3")) floor = "Third";
                            }
                            if (AT.PropertyType == (int)PropertyTypes.Independent_Floor)
                            {
                                if (Path.GetFileName(s).Contains("_-1")) floor = "Basement";
                                if (Path.GetFileName(s).Contains("_0")) floor = "Ground";
                                if (Path.GetFileName(s).Contains("_1")) floor = "First";
                                if (Path.GetFileName(s).Contains("_2")) floor = "Second";
                                if (Path.GetFileName(s).Contains("_3")) floor = "Terrace";
                            }
                            else if (AT.PropertyType == (int)PropertyTypes.Penthouse)
                            {
                                if (Path.GetFileName(s).Contains("_0")) floor = "Terrace";
                                if (Path.GetFileName(s).Contains("_1")) floor = "Upper";
                                if (Path.GetFileName(s).Contains("_2")) floor = "Lower";
                            }
                            str.Append("<br/>" + floor + "<br/><div style='position:relative'><img style=' width:585px;' src='" + Global.GetRootPathVirtual + @"/Data/Images_ApartmentType/" + Path.GetFileName(s) + "' /><a href='#'  style='z-index:99999px; position:absolute; background-color:black; right:20px; top:0px; color:white; ' onclick='return DeleteImage(\"/Data/Images_ApartmentType/" + Path.GetFileName(s) + "\"," + id.ID + ");'><span>&nbsp;X&nbsp;<span></a><div>");
                        }

                        //ApartmentType AT = ApartmentType.GetByID(Cmn.ToInt(Data1));
                        //if (AT == null)
                        //    return;

                        //string[] Files = Directory.GetFiles(Server.MapPath(@"~/Data/Images_ApartmentType/"), Data1 + ".*");
                        //Files = Files.Concat(Directory.GetFiles(Server.MapPath(@"~/Data/Images_ApartmentType/"), Data1 + "_*.*")).ToArray();

                        //foreach (string s in Files)
                        //{
                        //    string floor = "";

                        //    if (AT.PropertyType == (int)PropertyTypes.Villa)
                        //    {
                        //        if (Path.GetFileName(s).Contains("_-1")) floor = "Basement";
                        //        if (Path.GetFileName(s).Contains("_0")) floor = "Ground";
                        //        if (Path.GetFileName(s).Contains("_1")) floor = "First";
                        //        if (Path.GetFileName(s).Contains("_2")) floor = "Second";
                        //        if (Path.GetFileName(s).Contains("_3")) floor = "Third";
                        //    }
                        //    if (AT.PropertyType == (int)PropertyTypes.Independent_Floor)
                        //    {
                        //        if (Path.GetFileName(s).Contains("_-1")) floor = "Basement";
                        //        if (Path.GetFileName(s).Contains("_0")) floor = "Ground";
                        //        if (Path.GetFileName(s).Contains("_1")) floor = "First";
                        //        if (Path.GetFileName(s).Contains("_2")) floor = "Second";
                        //        if (Path.GetFileName(s).Contains("_3")) floor = "Terrace";
                        //    }
                        //    else if (AT.PropertyType == (int)PropertyTypes.Penthouse)
                        //    {
                        //        if (Path.GetFileName(s).Contains("_0")) floor = "Terrace";
                        //        if (Path.GetFileName(s).Contains("_1")) floor = "Upper";
                        //        if (Path.GetFileName(s).Contains("_2")) floor = "Lower";
                        //    }
                        //    str.Append("<br/>" + floor + "<br/><div style='position:relative'><img style=' width:585px;' src='" + Global.GetRootPathVirtual + @"/Data/Images_ApartmentType/" + Path.GetFileName(s) + "' /><a href='#'  style='z-index:99999px; position:absolute; background-color:black; right:20px; top:0px; color:white; ' onclick='return DeleteImage(\"../Data/Images_ApartmentType/" + Path.GetFileName(s) + "\");'><span>&nbsp;X&nbsp;<span></a><div>");
                        //}
                        AppendError = false;
                    }
                    break;
                case "DeleteFloors":
                    {
                        FloorInfo fi = FloorInfo.GetByAptIDandFloorNo(Cmn.ToInt(Data1), Cmn.ToInt(Data2));
                        if (fi != null)
                            fi.Delete();
                        AppendError = false;
                    }
                    break;
                case "GetFloorInfo":
                    List<FloorInfo> FI = FloorInfo.GetByAptID(Cmn.ToInt(Data1));
                    str.Append("<table id='floorInfo'>");
                    str.Append("<tr><th>Floor</th><th>TotalArea</th><th>BUA</th><th>Lawn</th><th>Terrace</th><th></th></tr>");
                    foreach (FloorInfo f in FI)
                    {
                        str.Append("<tr><td onclick='return ShowFloor(" + f.AptTypeID + "," + f.FloorNo + ")'>" + f.FloorNo + "-" + f.FloorName + "</td><td>" + f.TotalArea + "</td><td>" + f.BuiltupArea + "</td><td>" + f.Lawn + "</td><td>" + f.Terrace + "<td onclick='DeleteFloors(" + f.AptTypeID + "," + f.FloorNo + ")'>X</td></td></tr>");
                    }
                    str.Append("</table>");
                    AppendError = false;
                    break;

                case "FileUpload":
                    {
                        foreach (string f in Request.Files.AllKeys)
                        {
                            HttpPostedFile file = Request.Files[f];
                            if (Data2 == "Apartment")
                            {
                                string[] arr = file.FileName.Split('+');

                                if (arr.Length < 2) return;

                                char[] temp = arr[3].ToCharArray();
                                ApartmentType A = ApartmentType.GetByID(Cmn.ToInt(Data3));
                                if (A == null)
                                    A = new ApartmentType();

                                A.SocietyID = Cmn.ToInt(Data1);
                                A.Bedroom = Cmn.ToInt(arr[0]);
                                A.Bathroom = Cmn.ToInt(arr[1]);
                                A.Balcony = Cmn.ToInt(arr[2]);
                                A.SuperArea = Cmn.ToInt(arr[4]);
                                A.PropertyType = 0;
                                A.TypeName = arr[5];
                                A.PropertyType = Cmn.ToInt(arr[6]);
                                A.PlotArea = Cmn.ToInt(arr[7]);
                                A.Unit = Cmn.ToInt(arr[8].Split('.')[0]);

                                //ApartmentType tempA = A.Check();
                                //if (tempA != null)// already exists
                                //A = tempA;
                                A.SetOtherFeatures(OtherFeatures.Study, arr[3].Contains("S"));
                                A.SetOtherFeatures(OtherFeatures.Pooja, arr[3].Contains("P"));
                                A.SetOtherFeatures(OtherFeatures.Servant, arr[3].Contains("R"));
                                A.SetOtherFeatures(OtherFeatures.Store, arr[3].Contains("O"));
                                A.SetOtherFeatures(OtherFeatures.Dress, arr[3].Contains("D"));
                                A.SetOtherFeatures(OtherFeatures.Terrace, arr[3].Contains("T"));
                                A.SetOtherFeatures(OtherFeatures.Lobby, arr[3].Contains("L"));
                                A.SetOtherFeatures(OtherFeatures.Foyer, arr[3].Contains("F"));
                                A.SetOtherFeatures(OtherFeatures.Utility, arr[3].Contains("U"));
                                A.Save();

                                Society S = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(Data1));
                                List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data3), (int)ImagesLocations.Apartment_Type);
                                string Save_Location = "";
                                if (ids.Count == 0)
                                {
                                    string urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-" + Data3;
                                    urlName = GetUniqueURL(S, urlName, Data3);

                                    ImagesDetail imgd = new ImagesDetail();
                                    imgd.ReferenceID = Cmn.ToInt(Data3);
                                    imgd.ImageReferenceType = (int)ImagesLocations.Apartment_Type;
                                    imgd.ImageID = Data3 + ".jpg";
                                    imgd.UrlName = urlName;
                                    imgd.Save();
                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_ApartmentType\") + urlName + ".jpg";
                                }
                                else
                                {
                                    ImagesDetail imgd = ids.First();
                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_ApartmentType\") + imgd.UrlName + ".jpg";
                                }
                                file.SaveAs(Save_Location);
                                Global.LoadGlobalData();

                                //file.SaveAs(Server.MapPath(@"~\Data\Images_ApartmentType\" + A.ID + ".jpg"));
                            }
                            //else if (Data2 == "Apartment" && Data3 != "0")
                            //{
                            //    file.SaveAs(Server.MapPath(@"~\Data\Images_ApartmentType\" + Data3 + ".jpg"));
                            //}
                            else if (Data2 == "Logo")
                            {
                                //file.SaveAs(Server.MapPath(@"~\Data\Images_SocietyLogo\" + Data1 + ".jpg"));
                                Society s = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(Data1));
                                List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data1), (int)ImagesLocations.Project_Logo);
                                string Save_Location = "";
                                if (ids.Count == 0)
                                {
                                    string urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-logo";
                                    urlName = GetUniqueURL(s, urlName, "logo");

                                    ImagesDetail imgd = new ImagesDetail();
                                    imgd.ReferenceID = Cmn.ToInt(Data1);
                                    imgd.ImageReferenceType = (int)ImagesLocations.Project_Logo;
                                    imgd.ImageID = Data1 + ".jpg";
                                    imgd.UrlName = urlName;
                                    imgd.Save();

                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_SocietyLogo\") + urlName + ".jpg";
                                }
                                else
                                {
                                    ImagesDetail imgd = ids.First();
                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_SocietyLogo\") + imgd.UrlName + ".jpg";
                                }
                                file.SaveAs(Save_Location);
                                Global.LoadGlobalData();
                            }

                            else if (Data2 == "Layout")
                            {
                                //file.SaveAs(Server.MapPath(@"~\Data\Images_LayoutPlan\" + Data1 + ".jpg"));
                                Society s = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(Data1));
                                List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data1), (int)ImagesLocations.Project_Layout);
                                string Save_Location = "";
                                if (ids.Count == 0)
                                {
                                    string urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-layout-plan";
                                    urlName = GetUniqueURL(s, urlName, "layout-plan");
                                    ImagesDetail imgd = new ImagesDetail();
                                    imgd.ReferenceID = Cmn.ToInt(Data1);
                                    imgd.ImageReferenceType = (int)ImagesLocations.Project_Layout;
                                    imgd.ImageID = Data1 + ".jpg";
                                    imgd.UrlName = urlName;
                                    imgd.Save();
                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_LayoutPlan\") + imgd.UrlName + ".jpg";
                                }
                                else
                                {
                                    ImagesDetail imgd = ids.First();
                                    Save_Location = Server.MapPath(@"~\Data\temp\Images_LayoutPlan\") + imgd.UrlName + ".jpg";
                                }
                                file.SaveAs(Save_Location);
                                Global.LoadGlobalData();
                            }

                            else if (Data2 == "ProjectImages")
                            {
                                //for (int i = 1; i < 10; i++)
                                //{
                                //    if (File.Exists(Server.MapPath(@"~\Data\Images_Society\" + Data1 + "_" + i + ".jpg")))
                                //        continue;
                                //    else
                                //    {
                                //        file.SaveAs(Server.MapPath(@"~\Data\Images_Society\" + Data1 + "_" + i + ".jpg"));
                                //        break;
                                //    }
                                //}
                                Society s = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(Data1));
                                List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data1), (int)ImagesLocations.Projects_Images);
                                string Save_Location = "";
                                int imgCounter = 1;
                                if (ids.Count != 0)
                                {
                                    imgCounter = ids.Count + 1;
                                }
                                string urlName = Cmn.GenerateSlug(s.SocietyName) + "-" + Cmn.GenerateSlug(s.Subcity) + "-" + s.ID + "_" + (imgCounter);
                                urlName = GetUniqueURL(s, urlName, "");
                                ImagesDetail imgd = new ImagesDetail();
                                imgd.ReferenceID = Cmn.ToInt(Data1);
                                imgd.ImageReferenceType = (int)ImagesLocations.Projects_Images;
                                imgd.ImageID = s.ID + "_" + (imgCounter) + ".jpg";
                                imgd.UrlName = urlName;
                                imgd.Save();
                                Save_Location = Server.MapPath("~/Data/Images_Society/") + urlName + ".jpg";
                                file.SaveAs(Save_Location);
                                Global.LoadGlobalData();
                            }

                            else if (Data2 == "ad")
                            {
                                Society society = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(Data1));
                                file.SaveAs(Server.MapPath(@"~\Data\Newspaper\" + society.URLName + ".jpg"));
                            }

                            else if (Data2 == "FloorInfo")
                            {
                                string[] arr = file.FileName.Split('+');
                                int FloorNo = Cmn.ToInt(arr[4].Split('.')[0].Split('_')[1]);
                                FloorInfo Fi = FloorInfo.GetByAptIDandFloorNo(Cmn.ToInt(Data3), FloorNo);
                                if (Fi == null)
                                    Fi = new FloorInfo();
                                Fi.AptTypeID = Cmn.ToInt(Data3);
                                Fi.TotalArea = Cmn.ToInt(arr[0]);
                                Fi.BuiltupArea = Cmn.ToInt(arr[1]);
                                Fi.Lawn = Cmn.ToInt(arr[2]);
                                Fi.Terrace = Cmn.ToInt(arr[3]);
                                Fi.FloorNo = FloorNo;
                                Fi.FloorName = ((Floor)(FloorNo)).ToString();

                                //Fi.Save();
                                //file.SaveAs(Server.MapPath(@"~\Data\Images_ApartmentType\" + arr[4]));

                                //List<ImagesDetail> ids = ImagesDetail.GetByReferenceIDandReferenceType(Cmn.ToInt(Data3), (int)ImagesLocations.Apartment_Type);
                                //string Save_Location = "";
                                //if (ids.Count == 0)
                                //{
                                //    string urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-" + Data3;
                                //    urlName = GetUniqueURL(S, urlName, Data3);
                                //    ImagesDetail imgd = new ImagesDetail();
                                //    imgd.ReferenceID = Cmn.ToInt(Data3);
                                //    imgd.ImageReferenceType = (int)ImagesLocations.Apartment_Type;
                                //    imgd.ImageID = Data3 + ".jpg";
                                //    imgd.UrlName = urlName;
                                //    imgd.Save();
                                //    Save_Location = Server.MapPath(@"~\Data\temp\Images_ApartmentType\") + urlName + ".jpg";
                                //}
                                //else
                                //{
                                //    ImagesDetail imgd = ids.First();
                                //    Save_Location = Server.MapPath(@"~\Data\temp\Images_ApartmentType\") + imgd.UrlName + ".jpg";
                                //}
                            }

                            else if (Data2 == "SyncUpload")
                            {
                                string ServerFileName = Data1.Split(new string[] { "propertymap" }, StringSplitOptions.None)[1];
                                string Path = Server.MapPath(@"~" + ServerFileName.Trim());
                                if (!File.Exists(Server.MapPath(@"~" + ServerFileName)))
                                {
                                    file.SaveAs(Server.MapPath(@"~" + ServerFileName));
                                }
                            }
                        }
                    }
                    break;

                case "DownloadPdf":
                    {
                        string a = Data1;
                        var client = new System.Net.WebClient();
                        if (!Directory.Exists(Server.MapPath(@"~\Data\PDF\")))
                            Directory.CreateDirectory(Server.MapPath(@"~\Data\PDF\"));
                        string PathToSave = Server.MapPath(@"~\Data\PDF\" + Data2 + ".pdf");
                        client.DownloadFile(Data1, PathToSave);
                        AppendError = false;
                        str.Append("Done");
                    }
                    break;

                case "GetSocietyByBuilder":
                    {
                        int BuildID = Cmn.ToInt(Data1);

                        Agent A;
                        Global.AgentList.TryGetValue(BuildID, out A);
                        List<Society> list = Society.GetByBuilder(Cmn.ToInt(Data1));

                        var newList = list.Select(a => new
                        {
                            ID = a.ID,
                            Name = a.SocietyName,
                            City = a.City,
                            SubCity = a.Subcity
                        }).ToList();

                        str.Append(new JavaScriptSerializer().Serialize(newList));
                        AppendError = false;
                    }
                    break;

                case "GetProjectByCityID":
                    {
                        City C;
                        if (Global.CityList.TryGetValue(Cmn.ToInt(Data1), out C))
                        {
                            int ctr = 0;

                            var list = C.SocietyList.Where(m => m.Verified == 1).ToList();

                            List<Society> ToRemove = new List<Society>();
                            foreach (Society S in list)
                            {
                                //Boolean ImageNotfound = false;
                                foreach (ApartmentType A in S.ApartmentList)
                                {
                                    //if (File.Exists(Server.MapPath(@"~\Data\Images_ApartmentType\" + A.ID + ".jpg")))
                                    //      ImageNotfound = true;
                                }
                                //if (ImageNotfound)
                                //ToRemove.Add(S);
                            }

                            foreach (Society s in ToRemove)
                                list.Remove(s);

                            var newList = list.Select(a => new
                            {
                                ID = a.ID,
                                Name = a.SocietyName,
                                Lat = a.Lat == null ? 0 : a.Lat,
                                Lng = a.Lng == null ? 0 : a.Lng,
                                PolyPoints = a.PolyPoints,
                                ChildCount = 0,
                                ChildSocietyCount = 0,
                                City = a.City,
                                Index = ctr++,
                                CityID = a.CityID != null ? a.CityID : 0,
                                SubCityID = a.SubCityID != null ? a.SubCityID : 0,
                                AreaID = a.AreaID != null ? a.AreaID : 0,
                                URLName = a.URLName,
                                BuilderName = (a.Builder != null) ? a.Builder.AgentCompany : ""
                            }).ToList();

                            str.Append(new JavaScriptSerializer().Serialize(newList));
                        }
                        AppendError = false;
                    }
                    break;

                case "ApartmentType":
                    {
                        Society S;
                        if (Global.ProjectList.TryGetValue(Cmn.ToInt(Data1), out S))
                        {
                            var newList = ApartmentType.GetBySocietyID(S.ID).Select(a => new
                            {
                                ID = a.ID,
                                Bedroom = a.Bedroom,
                                Bathroom = a.Bathroom,
                                Balcony = a.Balcony != null ? a.Balcony : 0,
                                SuperArea = a.SuperArea != null ? a.SuperArea : 0,
                                PlotArea = a.PlotArea != null ? a.PlotArea : 0,
                                Unit = a.Unit != null ? a.Unit : 0,
                                OtherFeatures = a.OtherFeatures,
                                PropertyType = ((PropertyTypes)(a.PropertyType != null ? a.PropertyType : 0)).ToString().Replace("_", " "),
                                Name = a.TypeName + "-" + File.Exists(Server.MapPath(@"~\Data\Images_ApartmentType\" + a.ID + ".jpg"))
                            }).ToList();
                            str.Append(new JavaScriptSerializer().Serialize(newList));
                        }
                        AppendError = false;
                    }
                    break;

                case "FloorInfo":
                    {
                        var newList = FloorInfo.GetByAptID(Cmn.ToInt(Data1)).Select(a => new
                        {
                            ApartmentID = a.AptTypeID,
                            FloorNo = a.FloorNo,
                            BuiltupArea = a.BuiltupArea,
                            Terrace = a.Terrace,
                            Lawn = a.Lawn,
                            TotalArea = a.TotalArea,
                            Name = a.FloorName
                        }).ToList();
                        str.Append(new JavaScriptSerializer().Serialize(newList));
                        AppendError = false;
                        break;
                    }

                case "ImageDetail":
                    {
                        int refid = Cmn.ToInt(Data1.Split('-')[0]);
                        int refType = Cmn.ToInt(Data1.Split('-')[1]);

                        var newList = ImagesDetail.GetByReferenceIDandReferenceType(refid, refType).Select(a => new
                        {
                            ID = a.ID,
                            ReferenceID = a.ReferenceID,
                            ImageReferenceType = a.ImageReferenceType,
                            ImageID = a.ImageID,
                            UrlName = a.UrlName,
                        }).ToList();
                        str.Append(new JavaScriptSerializer().Serialize(newList));
                        AppendError = false;
                        break;
                    }

                case "GetSocietyAvailability":
                    {
                        List<Availability> list = GetSocietyAvailability(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Cmn.ToInt(Data3), Cmn.ToInt(Data6), Cmn.ToInt(Data4));

                        if (Data4 == "mobile")
                        {
                            str.Clear();
                            str.Append(new JavaScriptSerializer().Serialize(list));
                        }
                    }
                    break;

                case "GetAgentPosting":
                    {
                        string td = "";
                        List<Availability> alist = Global.AvailabilityList.Values.Where(m => m.SellerID == Cmn.ToInt(Data1)).OrderByDescending(m => m.PostedOnDate).ToList();
                        StringBuilder sb = new StringBuilder("<table id='AvlTable' style='border-spacing:1px;'>");
                        sb.Append("<thead style='background-color:#F8F8F8;'><tr><th>#</th><th>Society</th><th>BHK</th><th>For</th><th>Price</th><th>Posted On</th><th title='Available From'>Avl From</th><th>Area</th><th>Edit</th></tr><thead><tbody>");
                        int ctr = 0;
                        foreach (Availability A in alist)
                        {
                            string SocityName = "";
                            string UrlName = "";
                            string Area = "";
                            string Price = "-";
                            string avlFor = "-";
                            if (A.Society != null)
                            {
                                SocityName = A.Society.SocietyName;
                                UrlName = A.Society.URLName;
                                Area = A.Society.Subcity + "-" + A.Society.City;
                                Price = Cmn.ToInt(A.Amount).ToString("##,##0");
                                avlFor = ((int)A.AvailabilityType).ToString() == "1" ? "sale" : "rent";
                            }
                            ctr++;
                            td = "<td><a href='#' onclick='return ShowAvailabilityDetail(" + A.ID + ")'>Edit</a></td>";

                            ApartmentType apt;
                            string type = "-";
                            if (!string.IsNullOrEmpty(A.ApartmentTypeID.ToString()))
                                if (Global.ApartmentTypeList.TryGetValue((int)A.ApartmentTypeID, out apt))// if the apartment type exists in the global list
                                {
                                    type = apt.Bedroom + "B-" + apt.Bathroom + "T";
                                }
                            string tr = "<tr><td>" + ctr + "</td><td><a href='/";
                            if (A.Deleted == 1)
                                tr = "<tr style='text-decoration:line-through;'><td>" + ctr + "</td><td><a href='/";
                            sb.Append(tr
                                + UrlName.ToLower() + "' target='_blank'>"
                                + SocityName + "</a></td><td>"
                                + type + "</td><td>" + avlFor + "</td><td>" + Price + "</td><td>"
                                + Cmn.ToDate(A.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>"
                                + Cmn.ToDate(A.DateAvailableFrom).ToString("dd-MMM-yy") + "</td><td>"
                                + Area + "</td>"
                                + td + "</tr>");
                        }
                        sb.Append("</tbody></table");
                        str.Append(sb.ToString());
                    }
                    break;
                case "UpdatePolyPoints": UpdatePolyPoints(); break;
                case "GetAvailibilityText": GetAvailibilityText(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Cmn.ToInt(Data3), Cmn.ToInt(Data6), Cmn.ToInt(Data5), Cmn.ToInt(Data4)); break;
                case "GetAvailabilityDetail": GetAvailabilityDetail(Cmn.ToInt(Data1)); break;
                case "GetAgentDetail": GetAgentDetail(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "GetProjectDetail": GetProjectDetail(Cmn.ToInt(Data1)); break;
                case "GetProjectDetailJson": GetProjectDetailJson(Cmn.ToInt(Data1)); break;
                case "GetAllProjectDetailJson": GetAllProjectDetailJson(Cmn.ToInt(Data1)); break;
                case "CheckIfPropertyAlreadyDownloaded": CheckIfPropertyAlreadyDownloaded(Data1, Data2); break;
                case "UpdatePropertyAvailability": UpdatePropertyAvailability(); break;
                case "UpdatePolyPointsWiki": UpdatePolyPointsWiki(); break;
                case "UpdateAvailibility": UpdateAvailibility(); break;
                case "GetClientList": GetClientList(Cmn.ToInt(Data1)); break;
                case "AssignClientToProperty": AssignClientToProperty(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Cmn.ToInt(Data3)); break;
                case "GetAvlClientList": GetAvlClientList(Cmn.ToInt(Data5)); break;
                case "GetAsignClient": GetAsignClient(Cmn.ToInt(Data1)); break;
                case "GetLatLng": GetLatLng(Cmn.ToInt(Data1)); break;
                case "GetClietIDByMobileNo": GetClietIDByMobileNo(Data1); break;
                case "SearchSociety": SearchSociety(term, Data1); Response.Write(str.ToString()); return;
                case "SearchBuilder": SearchBuilder(term); Response.Write(str.ToString()); return;
                case "AllSearch":
                    {
                        if (string.IsNullOrWhiteSpace(term))
                            return;
                        term = term.ToLower();

                        var Route = Data2.Length == 0 ? @"/project/" : "/" + Data2 + "/";
                        var listS = new List<Society>();

                        if (Data1 != "1")  //Exclude Societies
                            listS = Global.ProjectList.Values.Where(m => m.SocietyName.ToLower().Contains(term) && m.Deleted == 0 && m.Verified == 1).Take(15).ToList();
                        var newList = listS.Select(m => new { id = m.ID.ToString(), urlname = Global.GetRootPathVirtual + Route + m.URLName.ToLower(), label = m.SearchName, value = m.SearchName, type = "P" }).ToList();

                        if (Data1 == "0" || Data1 == "1") //Exclude builders from map search
                        {
                            int take = 5;
                            if (Data1 == "1") take = 10;

                            var listBuilder = Global.BuilderList.Values.Where(m => m.AgentCompany != null && m.AgentCompany.ToLower().Contains(term)).Take(take).ToList();
                            newList.AddRange(listBuilder.Select(m => new { id = m.ID.ToString(), urlname = Global.GetRootPathVirtual + "/" + m.URLName, label = m.AgentCompany, value = m.AgentCompany, type = "B" }).ToList());
                        }

                        var listC = new List<City>();
                        if (Data1 != "1")
                        {
                            listC = Global.CityList.Values.Where(m => m.Name.ToLower().StartsWith(term)).Take(5).ToList();
                            newList.AddRange(listC.Select(m => new { id = m.ID.ToString(), urlname = "", label = m.Name, value = m.Name, type = "C" }).ToList());
                        }

                        Response.Write(new JavaScriptSerializer().Serialize(newList));
                        return;
                    }

                case "GlobalSearch":
                    {
                        if (string.IsNullOrWhiteSpace(term))
                            return;
                        term = term.ToLower();

                        var listS = Global.ProjectList.Values.Where(m => m.SocietyName.ToLower().Contains(term)).Take(15).ToList();
                        var newList = listS.Select(m => new { id = m.ID.ToString(), urlname = m.URLName.ToLower(), label = m.SearchName, value = m.URLName, type = "P" }).ToList();

                        var listBuilder = Global.BuilderList.Values.Where(m => m.AgentCompany != null && m.AgentCompany.ToLower().Contains(term)).Take(5).ToList();
                        newList.AddRange(listBuilder.Select(m => new { id = m.ID.ToString(), urlname = m.AgentName, label = m.AgentCompany, value = m.AgentCompany, type = "B" }).ToList());

                        var listC = Global.CityList.Values.Where(m => m.Name.ToLower().StartsWith(term)).Take(5).ToList();
                        newList.AddRange(listC.Select(m => new { id = m.ID.ToString(), urlname = "", label = m.Name, value = m.Name, type = "C" }).ToList());

                        Response.Write(new JavaScriptSerializer().Serialize(newList));
                        return;
                    }
                case "UpdateCurrentProject": UpdateCurrentProject(); return;
                case "UpdateAminityData": UpdateAminityData(); return;
                case "UpdateServicesData": UpdateServicesData(); break;
                case "DelteImages": DelteImages(Data1, Cmn.ToInt(Data2)); break;
                case "DeleteServerImage":
                    {
                        DeleteServerImage(Data1);
                        break;
                    }

                case "SaveStnLayOut":
                    {
                        AppendError = false;
                        string FileName = Server.MapPath(@"~\Layouts\" + Data1 + ".txt");
                        NameValueCollection nvc = Request.Form;
                        if (nvc["Layout"] != null)
                        {
                            File.WriteAllText(FileName, nvc["Layout"]);
                        }
                    }
                    break;

                case "LoadStnLayOut":
                    {
                        AppendError = false;
                        string FileName = Server.MapPath(@"~\Layouts\" + Data1 + ".txt");
                        if (File.Exists(FileName))
                        {
                            Cmn.WriteResponse(this, File.ReadAllText(FileName));
                        }
                    }
                    break;
                case "GetDistanceFromList": GetDistanceFromList(Data1); break;
                case "GetDistanceToList": GetDistanceToList(Data1, Data2); break;
                case "UpdateDistance": UpdateDistance(Cmn.ToInt(Data1), Data2, Cmn.ToInt(Data3), Cmn.ToInt(Data4), Cmn.ToInt(Data5)); break;
                case "GetPriceTrend": GetPriceTrend(Data1); break;
                case "UpdateSocietyPoly": UpdateSocietyPoly(Cmn.ToInt(Data1)); break;
                case "UpdateCityPoly": UpdateCityPoly(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Data3); break;
                case "ShowImages": ShowImages(Data1); break;
                case "GetClientScript": GetClientScript(); return;
                //case "UpdateProjectData": UpdateProjectData(); break;
                //case "UpdateProjectDetail": UpdateProjectDetail(); break;
                case "UpdateProjectURL": UpdateProjectURL(); break;
                case "UpdateProjectAllChk": UpdateProjectAllChk(); break;
                case "UpdateBuilderURL": UpdateBuilderURL(); break;
                case "UpdateBuilderCF": UpdateBuilderCF(); break;
                case "UpdatePriceTrend": UpdatePriceTrend(); break;
                case "UpdateAgentData": UpdateAgentData(); break;
                case "SaveQuestionForm":
                    {
                        AppendError = false;
                        NameValueCollection nvc = Request.Form;
                        if (nvc != null)
                        {
                            if (rpHash(nvc["realPerson"]) == nvc["realPersonHash"])
                            {
                                try
                                {
                                    ProjectQuestion PQ = new ProjectQuestion();
                                    PQ.ID = 0;
                                    PQ.ProjectID = Cmn.ToInt(Data1);
                                    PQ.Subject = nvc["Subject"];
                                    PQ.Name = nvc["Name"];
                                    PQ.Email = nvc["Email"];
                                    PQ.Question = nvc["Question"];
                                    PQ.Approved = 0;
                                    PQ.Save();
                                    SendEmail(PQ.Subject, "psu.singh@gmail.com", PQ.Name, PQ.Email, PQ.Question);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                AppendError = true;
                                Error += "CVFAILED";
                            }
                        }
                    }
                    break;

                case "UpdateAgent":
                    {
                        AppendError = false;
                        NameValueCollection nvc = Request.Form;
                        try
                        {
                            if (nvc != null)
                            {
                                Agent A = Agent.GetByID(Cmn.ToInt(Data1));
                                if (A != null)
                                {
                                    A.AgentName = nvc["Name"];
                                    A.AgentCompany = nvc["Company"];
                                    A.Address = nvc["Address"];
                                    A.City = Cmn.ToInt(nvc["City"]);
                                    A.Mobile1 = nvc["Mobile1"];
                                    A.Mobile2 = nvc["Mobile2"];
                                    A.PhoneNo1 = nvc["Phone1"];
                                    A.PhoneNo2 = nvc["Phone2"];
                                    A.BuilderDescription = nvc["Details"];
                                    A.Save();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendError = true;
                            Error += "FAILED";
                        }
                    }
                    break;
                case "ProjectListMobile": ProjectListMobile(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Data3, Data4, Data5); break;
                case "ProjectDetailMobile":
                    {
                        Society society;
                        if (Global.ProjectList.TryGetValue(Cmn.ToInt(Data1.Trim()), out society))
                            str.Append(society.ProjectDetailMobile());
                        AppendError = false;
                    } break;
                //case "BuilderDetailMobile": BuilderDetailMobile(Cmn.ToInt(Data1.Trim())); break;
                case "BuilderDetailMobile":
                    {
                        Agent builder;
                        if (Global.BuilderList.TryGetValue(Cmn.ToInt(Data1.Trim()), out builder))
                            str.Append(builder.BuilderDetailMobile());
                        AppendError = false;
                    } break;
            }
            Response.Write((AppendError ? Error + "~" : "") + str.ToString());    //at last write responce to caller
        }

        catch (Exception ex)
        {
            ErrorWriter(ex);
            Error = ex.Message + ex.StackTrace + "-" + Error;
            Response.Write((AppendError ? Error + "~" : "") + str.ToString());
        }
        finally
        {
            if (str.Length == 0 && Action != "GetServices")
                Error += "No Data Found";
        }
    }

    string GetUniqueURL(Society S, string urlName, string appendText)
    {
        string url = urlName;
        if (Global.ImageDetailsList.FirstOrDefault(m => m.UrlName == urlName) != null || string.IsNullOrWhiteSpace(urlName))
            url = (S.URLName + (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
        return url;
    }
    //private void BuilderDetailMobile(int BuilderID)
    //{
    //    Agent A;
    //    if (Global.BuilderList.TryGetValue(BuilderID, out A))
    //    {
    //        try
    //        {
    //            StringBuilder sb = new StringBuilder();
    //            //string fileExtention = Path.GetExtension("/Data/Images_BuilderLogo/" + A.ID);
    //            string ext = "jpg";
    //            if (File.Exists(Server.MapPath(@"~/Data/Images_BuilderLogo/") + A.ID + ".gif"))
    //                ext = "gif";
    //            str.Append("<a href='#' class='ui-btn'><img src='/Data/Images_BuilderLogo/" + A.ID + "." + ext + "' style='height:100%;'>" +
    //            "<h1>" + A.AgentName + "</h1>");
    //            if (A.Address != "")
    //                str.Append("<p>Add: " + A.Address + "</p>");
    //            if (A.Mobile1 != "")
    //                str.Append("<p>Cont: " + A.Mobile1 + "</p>");
    //            str.Append("</a>" +
    //            "<p style='text-align:justify;padding:1em;text-indent:2em;'>" + A.BuilderDescription + "</p>");
    //            AppendError = false;
    //        }
    //        catch
    //        {
    //            AppendError = true;
    //        }
    //    }
    //}

    private void ProjectListMobile(int CityID, int Bhk, string Budget, string lat, string lng)
    {
        int min = 0, max = 99999999;
        switch (Budget)
        {
            case "1": min = 200000; max = 4000000;
                break;
            case "2": min = 400000; max = 6000000;
                break;
            case "3": min = 600000; max = 8000000;
                break;
            case "4": min = 8000000; max = 10000000;
                break;
        }

        try
        {
            string a = Data1;
            List<Society> slist = Global.ProjectList.Values.Where(m => m.Verified == 1 && m.CityID == CityID).ToList();
            List<Society> newSlist = new List<Society>();

            if (lat != "0")    //Filter Within Distance
            {
                List<Society> tempList = new List<Society>();
                slist = Global.ProjectList.Values.Where(m => m.Verified == 1).ToList();
                foreach (Society s in slist)
                {
                    if (Cmn.CalcDistance(Cmn.ToDbl(lat), Cmn.ToDbl(lng), Cmn.ToDbl(s.Lat), Cmn.ToDbl(s.Lng), 'K') <= 10.00)
                    {
                        tempList.Add(s);
                    }
                }
                slist = tempList;
            }

            foreach (Society s in slist)
            {
                try
                {
                    if (Filter(s.ID, Bhk, min, max, lat, lng))
                        newSlist.Add(s);
                }
                catch (Exception ex)
                {
                }
            }

            if (newSlist.Count > 0)
            {
                str.Append("<ul data-role='listview'>");
                int ctr = 1;
                foreach (Society SL in newSlist)
                {
                    //onclick='SetProjectId(\"" + SL.ID + "\")'
                    str.Append("<li id='ProjectID' data-ProjectID='" + SL.ID + "'><a href='/" + SL.URLName.ToLower() + "' data-ajax='false' onclick='RunCommand(CMD_FindProjectDetil)'><span style='font-size:.7em'>" + (ctr++) + "</span>. " + SL.SocietyName + "<p>" + SL.Subcity + ", " + SL.City + "</p></a></li>");
                }
                str.Append("</ul>");
            }
            AppendError = false;
        }
        catch
        {
            AppendError = true;
        }
    }

    private bool Filter(int ID, int BHK, int min, int max, string lat, string lng)
    {
        bool Add = false;

        if (min == 0 && BHK == 0)
        {
            Add = true;
            return Add;
        }

        List<ApartmentType> list = Global.ApartmentTypeList.Values.Where(m => m.SocietyID == ID).ToList();
        if (min != 0 && BHK != 0)
        {
            foreach (ApartmentType AT in list)
            {
                if (AT.Bedroom == BHK && ChekPriceRange(min, max, AT))
                {
                    Add = true;
                    break;
                }
            }
        }
        else if (min == 0 && BHK != 0)
        {
            foreach (ApartmentType AT in list)
            {
                if (AT.Bedroom == BHK)
                {
                    Add = true;
                    break;
                }
            }
        }
        else if (min != 0 && BHK == 0)
        {
            foreach (ApartmentType AT in list)
            {
                if (ChekPriceRange(min, max, AT))
                {
                    Add = true;
                    break;
                }
            }
        }
        return Add;
    }

    private bool ChekPriceRange(int min, int max, ApartmentType AT)
    {
        int price = 0;
        price = (int)AT.BSP * (int)AT.SuperArea;
        if (price >= min && price <= max)
            return true;
        else
            return false;
    }

    //private void ProjectDetailMobile(int ID)
    //{
    //    try
    //    {
    //        Society society = Global.SocietyList.Values.FirstOrDefault(m => m.ID == ID);
    //        str.Append("<a href='#' class='ui-btn'><img src='/Data/Images_societyLogo/" + society.ID + ".jpg' style='height:100%;'><h1 id='Soc_Name'>" + society.SocietyName + "</h1>");
    //        str.Append("<p><span id='Soc_SubCity'>" + (string.IsNullOrEmpty(society.Subcity) ? "" : society.Subcity + ",") + "</span>" +
    //            "<span id='Soc_City'>" + society.City + ", " + society.State + (!string.IsNullOrEmpty(society.Pin) ? ("-" + society.Pin) : ("")) + "</span></p></a>");
    //        string aptTable = GetApartmentTable(society.ID);
    //        str.Append("<p>" + aptTable + "</p>");
    //        str.Append("<p style='text-align:justify;padding:1em;text-indent:2em;'>" + society.Description + "</p>");
    //        str.Append("<a id='btnFav' class='ui-btn' onclick='AddFavouriteProjects(\"" + society.ID + "\",\"" + society.SocietyName + "\",\"" + society.URLName + "\",\"" + society.City + "\",\"" + society.Subcity + "\")' href=''>Add To Fav</a>");
    //        str.Append("<a class='ui-btn' target='_blank' href=" + society.URL + ">Official Website</a>" +
    //        "<a class='ui-btn' href='https://maps.google.com/maps?q=" + society.Lat + "," + society.Lng + "'>Map</a>" +
    //        "<a id='BuilderID' class='ui-btn' onclick='RunCommand(CMD_FindBuilderDetail)' href='#' data-BuilderID='" + society.Builder.ID + "'>" + society.Builder.AgentCompany + "</a>");
    //        //str.Append("<div id='Gallery'>");
    //        //str.Append("<a class='Swipe' href='/Data/Images_societyLogo/" + society.ID + ".jpg'><img src='/Data/Images_societyLogo/" + society.ID + ".jpg' alt='Image 01' /></a></div>");
    //        AppendError = false;
    //    }
    //    catch
    //    {
    //        AppendError = true;
    //    }
    //}

    //private string GetApartmentTable(int Id)    //Apatrment table for project detail page
    //{
    //    StringBuilder sb = new StringBuilder("<table style='width:100%;'  class='ui-body-d ui-shadow table-stripe'><tr><th>Type</th><th>Area</th><th>BSP</th><th>Price</th></tr>");
    //    List<ApartmentType> list = Global.ApartmentTypeList.Values.Where(m => m.SocietyID == Id).OrderBy(m => m.Bedroom).ThenBy(m => m.SuperArea).ToList();
    //    foreach (ApartmentType a in list)
    //    {
    //        int price = 0;
    //        price = (int)a.BSP * (int)a.SuperArea;
    //        sb.Append("<tr style='text-align:center;'><td>" + a.Bedroom + "B-" + a.Bathroom + "T </td><td>" + a.SuperArea + "</td><td>" + a.BSP + "</td><td>" + ConvertPrice(price.ToString()) + "</td></tr>");
    //    }
    //    sb.Append("</table>");
    //    //sb.Append("<div id='Gallery'>");
    //    //sb.Append("<a class='Swipe' href='/Data/Images_societyLogo/" + Id + ".jpg'><img src='/Data/Images_societyLogo/" + Id + ".jpg' alt='Image 01' /></a></div>");
    //    return sb.ToString();
    //}

    //private string ConvertPrice(string price)        //Convert price to Lacks and Crores 
    //{
    //    price = (Cmn.ToInt(price) / 100000).ToString("0") + " L";
    //    string temp = price.Replace("L", "").Trim();
    //    int z = (int)Cmn.ToDbl(temp) / 100;
    //    if (((int)Cmn.ToDbl(temp) / 100 > 0))
    //    {
    //        price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
    //    }
    //    return price;
    //}

    private string rpHash(string value)     //real-persion capchas hash conversion
    {
        int hash = 5381;
        value = value.ToUpper();
        for (int i = 0; i < value.Length; i++)
        {
            hash = ((hash << 5) + hash) + value[i];
        }
        return hash.ToString();
    }

    private void UpdatePolyPointsWiki()
    {
        NameValueCollection nvc = Request.Form;
        string[] PolyPoints = nvc["PolyPoints"].Replace("[", "").Replace("]", "").Split(',');
        StringBuilder pnts = new StringBuilder();
        int ctr = 0;
        foreach (string a in PolyPoints)
        {
            pnts.Append(a);
            if (ctr % 2 == 0)
                pnts.Append(",");
            else
                pnts.Append("^");
            ctr++;
        }

        int ProjID = Cmn.ToInt(nvc["ProjId"]);

        try
        {
            Society S = Society.GetByID(Cmn.ToInt(ProjID));
            S.PolyPoints = pnts.ToString();
            S.Save();
            AppendError = false;
        }
        catch (Exception ex)
        {
        }
        AppendError = false;
    }

    void UpdatePolyPoints()
    {
        int ID = GetFormInt("ID");

        Society S = Society.GetByID(ID);
        if (S != null)
        {
            S.PolyPoints = GetFormString("Points");
            S.Lat = GetFormDbl("Lat");
            S.Lng = GetFormDbl("Lng");
            S.Save();
            str.Append("Save");
        }
        else
            str.Append("Project not found");
    }

    int GetCityLatestAveragePrice(City city)
    {
        PriceTrend PT = city.PriceTrendList.OrderByDescending(m => m.Year).ThenByDescending(m => m.Quarter).FirstOrDefault();
        if (PT != null)
            return (int)(((int)PT.Max + (int)PT.Min) * .5);
        else
            return 0;
    }

    private void UpdatePriceTrend()
    {
        NameValueCollection nvc = Request.Form;
        string lowerRange = nvc["LowerRange"] != null ? nvc["LowerRange"] : "";
        string upperRange = nvc["UpperRange"] != null ? nvc["UpperRange"] : "";
        string quarterValues = nvc["QuarterValues"] != null ? nvc["QuarterValues"] : "";
        string citySubcity = nvc["CitySubcity"] != null ? nvc["CitySubcity"] : "";

        string[] LR = lowerRange.Split(',');
        string[] UR = upperRange.Split(',');
        string[] QV = quarterValues.Split(',');

        for (int i = 0; i < QV.Length; i++)
        {
            if (Cmn.ToInt((LR[i].Trim())) <= 200) continue;

            string City = citySubcity.Split('+')[0].Trim();
            string SubCity = citySubcity.Split('+')[1].Trim();
            SubCity = SubCity.Contains("-") ? SubCity.Replace("-", " ") : SubCity;
            int Year = Cmn.ToInt("20" + QV[i].Split('\'')[1].Trim());
            int Min = DoRound(Cmn.ToInt((LR[i].Trim())));
            int Max = DoRound(Cmn.ToInt((UR[i].Trim())));

            if (Min > 500)
            {
                string temp = QV[i].Split('\'')[0].Trim();
                int Quarter = 0;
                switch (temp)
                {
                    case "Jan-Mar":
                        Quarter = 1;
                        break;
                    case "Apr-Jun":
                        Quarter = 2;
                        break;
                    case "Jul-Sep":
                        Quarter = 3;
                        break;
                    case "Oct-Dec":
                        Quarter = 4;
                        break;
                }

                PriceTrend PT = PriceTrend.GetBySubCityQuarterYear(City, SubCity, Quarter, Year);
                if (PT == null)
                    PT = new PriceTrend();
                PT.City = City;
                PT.Subcity = SubCity;
                PT.Type = "apt-sale";
                PT.Min = Min;
                PT.Max = Max;
                PT.Quarter = Quarter;
                PT.Year = Year;
                PT.Save();
            }
        }
    }

    int DoRound(int t)
    {
        if (t < 1000)
            return t;
        int m = t % 100;
        if (m < 50)
            t = t - m;
        else
            t = t + (100 - m);
        return t;
    }

    private string GetNextRecord(string FileName, int RecordIndex)
    {
        FileName = Server.MapPath(@"~\Data\PortalURL\") + FileName;
        if (File.Exists(FileName))
        {
            string[] data = File.ReadAllLines(FileName);
            if (RecordIndex < data.Length)
                return data[RecordIndex];
        }
        return "";
    }

    private void UpdateBuilderURL()
    {
        NameValueCollection nvc = Request.Form;
        string Portal = nvc["Portal"] != null ? nvc["Portal"] : "";
        string URL = nvc["URL"] != null ? nvc["URL"] : "";

        string Folder = Server.MapPath(@"~\Data\PortalURL\");
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);

        string FileName = Folder + Portal + ".txt";

        Dictionary<string, string> URLs = new Dictionary<string, string>();
        if (File.Exists(FileName))
        {
            string[] Lines = File.ReadAllLines(FileName);
            foreach (string s in Lines)
            {
                //if (s == "#inline5") continue;
                URLs.Add(s, "");
            }
        }

        string[] URLData = URL.Split('~');

        foreach (String s in URLData)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "undefined")
                continue;

            if (!URLs.ContainsKey(s))
                URLs.Add(s, "");
        }

        StringBuilder str = new StringBuilder();
        foreach (string s in URLs.Keys)
        {
            str.Append(s + Environment.NewLine);
        }
        File.WriteAllText(FileName, str.ToString());
    }

    private void UpdateBuilderCF()
    {
        try
        {
            NameValueCollection nvc = Request.Form;

            string BuilderInfo = nvc["BuilderData"] != null ? Server.HtmlDecode(nvc["BuilderData"]) : "";
            if (BuilderInfo != "")
            {
                string[] temp = BuilderInfo.Split('^');
                string ImgeUrl = temp[4].Trim();
                Agent A = Agent.GetByName(temp[0].Trim());
                if (A == null)
                {
                    A = new Agent();
                    A.UserType = 2;
                    A.AgentName = temp[0].Trim();
                    A.AgentCompany = temp[0].Trim();
                    A.Address = temp[1].Trim();
                    A.PhoneNo1 = temp[2].Trim();
                    A.BuilderDescription = temp[3].Trim();
                    A.SourceURL = temp[5].Trim();
                    A.Varified = 0;
                    A.Save();

                    if (A.Message.Length == 0)
                    {
                        string remoteImgPath = ImgeUrl;
                        string fileExtention = Path.GetExtension(remoteImgPath);
                        string RemoteFileName = Path.GetFileName(remoteImgPath);
                        string fileName = A.ID.ToString();
                        if (RemoteFileName.Split('.')[0].Trim() != "defaultlogo")
                        {
                            string localPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Images_BuilderLogo\\" + fileName + fileExtention;
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile(remoteImgPath, localPath);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string s = ex.Message;
        }
    }

    //private void UpdateProjectData()
    //{
    //    string AmnData = GetFormString("ProjectData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
    //    string[] Lines = AmnData.Split('^');

    //    ProjData P = ProjData.GetByURL(Lines[0]);

    //    if (P == null)
    //        P = new ProjData();

    //    P.URL = Lines[0];
    //    P.Name = Lines[1].Replace(" ", "").Trim();
    //    P.Builder = Lines[2].Replace(" ", "").Trim();
    //    //P.Possession = Lines[2];
    //    P.City = Lines[3];
    //    P.PriceRange = Lines[4];
    //    P.Location = Lines[5];
    //    P.Save();

    //    str.Append(P.ID);
    //}

    //private void UpdateProjectDetail()
    //{
    //    string AmnData = GetFormString("ProjectDetailData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
    //    int ProjID = Cmn.ToInt(GetFormString("ProjID"));

    //    if (ProjID == 0)
    //        return;

    //    string[] Lines = AmnData.Split('~');

    //    for (int i = 0; i < Lines.Length; i++)
    //    {
    //        if (string.IsNullOrWhiteSpace(Lines[i]) || i == 0)
    //            continue;

    //        string[] temp = Lines[i].Split('^');

    //        try
    //        {
    //            ProjPriceDetail P = ProjPriceDetail.GetByProjIDBHKName(ProjID, temp[1].Trim());

    //            if (P == null)
    //                P = new ProjPriceDetail();

    //            P.ProjID = ProjID;
    //            P.BHK = temp[1].Trim().Replace(" ", "").Trim();
    //            P.Area = temp[2].Replace(" ", "").Trim();
    //            P.Price = temp[3].Replace(" ", "").Trim();
    //            P.Save();
    //        }
    //        catch (Exception ex)
    //        {
    //            str.Append("Error " + ProjID + "-" + ex.Message);
    //        }
    //    }
    //    str.Append("Updated " + ProjID);
    //}

    private void UpdateProjectURL()        //Write all urls in a page
    {
        NameValueCollection nvc = Request.Form;
        string Portal = nvc["Portal"] != null ? nvc["Portal"] : "";
        string URL = nvc["URL"] != null ? nvc["URL"] : "";

        string Folder = Server.MapPath(@"~\Data\PortalURL\");
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);

        string FileName = Folder + "AllCheckDeals" + ".txt";

        Dictionary<string, string> URLs = new Dictionary<string, string>();
        if (File.Exists(FileName))
        {
            string[] Lines = File.ReadAllLines(FileName);
            foreach (string s in Lines)
            {
                //if (s == "#inline5") continue;
                URLs.Add(s, "");
            }
        }

        string[] URLData = URL.Split('~');

        foreach (String s in URLData)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "#inline5")
                continue;

            if (!URLs.ContainsKey(s))
                URLs.Add(s, "");
        }

        StringBuilder str = new StringBuilder();
        foreach (string s in URLs.Keys)
        {
            str.Append(s + Environment.NewLine);
        }
        File.WriteAllText(FileName, str.ToString());
    }

    private void UpdateProjectAllChk()
    {
        NameValueCollection nvc = Request.Form;

        string ProjectInfo = nvc["ProjectData"] != null ? nvc["ProjectData"] : "";
        string BhkDetail = nvc["BhkDetail"] != null ? nvc["BhkDetail"] : "";
        string SourceURL = nvc["DataSourceURL"] != null ? nvc["DataSourceURL"].Split('?')[0].Trim() : "";

        string[] temp = ProjectInfo.Split('^');

        string a = temp[0].Split('-')[0].Trim();
        Society SChk = Society.GetByName(temp[0].Split('-')[0].Trim());
        if (SChk != null)
        {
            //Society S = Society.GetByID(0);
            //if (S == null)
            //    S = new Society();
            Society S = SChk;
            S.DataSourceURL = SourceURL;

            //S.SocietyName = temp[0].Split('-')[0].Trim();
            //S.DataSourceURL = SourceURL;
            //S.Lat = Cmn.ToDbl(temp[6].Trim());
            //S.Lng = Cmn.ToDbl(temp[7].Trim());
            //S.Description = temp[9].Trim();
            //S.City = temp[0].Split('-')[1].Trim();
            //S.Subcity = temp[2].Trim();


            //if (S.City == "Noida" || S.City == "Ghaziabad" || S.City == "Lucknow" || S.City == "Greater Noida")
            //    S.State = "Uttar Pradesh";
            //else if (S.City == "Delhi")
            //    S.State = "Delhi";
            //else if (S.City == "Gurgaon" || S.City == "Faridabad")
            //    S.State = "Haryana";
            //else if (S.City == "Kolkata" || S.City == "Durgapur")
            //    S.State = "West Bengal";
            //else if (S.City == "Pune" || S.City == "Mumbai")
            //    S.State = "Maharashtra";
            //else if (S.City == "Hyderabad")
            //    S.State = "Andhra Pradesh";
            //else if (S.City == "Indore")
            //    S.State = "Madhya Pradesh";
            //else if (S.City == "Chennai")
            //    S.State = "Tamil Nadu";
            //else if (S.City == "Bhubaneshwar")
            //    S.State = "Odisha";
            //else if (S.City == "Chandigarh")
            //    S.State = "Punjab";
            //else if (S.City == "Ahmedabad")
            //    S.State = "Gujarat";

            //S.Country = "India";

            //if (temp[5].Trim() != "NA")
            //{
            //    string Year = temp[5].Trim();
            //    S.EndDate = new DateTime(Cmn.ToInt(Year), 1, 1);
            //}

            //Agent A = Agent.GetByName(temp[10]);
            ////S.BuilderID = (A != null ? S.BuilderID = A.ID : S.BuilderID = 0);
            //if (A != null)
            //{
            //    S.BuilderID = A.ID;
            //}
            //else
            //{
            //    S.BuilderID = 0;
            //    string Folder = Server.MapPath(@"~\Data\BulderData\");
            //    if (!Directory.Exists(Folder))
            //        Directory.CreateDirectory(Folder);
            //    string FileName = Folder + "BuilderNames.txt";
            //    //StringBuilder BuilderName = new StringBuilder(temp[10]);
            //    File.WriteAllText(FileName, "Society-" + temp[0].Split('-')[0] + "-" + temp[10] + Environment.NewLine);
            //}

            //S.PropertyType = 0;
            //City C = City.GetByName(temp[0].Split('-')[1].Trim());
            //if (C != null)
            //    S.CityID = C.ID;
            //City SubityChk = City.GetByName(temp[2].Trim());
            //if (SubityChk != null)
            //{
            //    S.SubCityID = SubityChk.ID;
            //}
            S.Save();

            string[] Lines = BhkDetail.Split('~');
            int ctr = 0;
            foreach (string L in Lines)
            {
                if (ctr == 0)
                {
                    ctr++;
                    continue;
                }
                string[] val = L.Split('^');
                ApartmentType AT = ApartmentType.GetByID(0);
                if (AT == null)
                    AT = new ApartmentType();
                AT.SocietyID = S.ID;
                string[] loctemp = val[0].Split('-');
                AT.Bedroom = Cmn.ToInt(loctemp[0].Replace("BR", "").Trim());
                AT.Bathroom = Cmn.ToInt(loctemp[1].Replace("T", "").Trim());
                if (loctemp.Length == 3)
                {
                    if (loctemp[2].Contains("SR"))
                        AT.OtherFeatures = "0010";
                    else if (loctemp[2].Contains("Study"))
                        AT.OtherFeatures = "0100";
                }

                AT.SuperArea = Cmn.ToInt(val[1].Split(' ')[0].Trim());
                AT.TypeName = val[0];

                string BSP = val[2].Split('(')[1].Split('/')[0].Trim();
                AT.BSP = Cmn.ToInt(BSP);
                //AT.Save();
                //UpdatePriceList(S.ID, AT.ID, Cmn.ToInt(BSP));
                ctr++;
            }
        }
    }

    private void UpdatePriceList(int SocID, int AptTypeID, int BSP)
    {
        PriceList PL = PriceList.GetByID(AptTypeID);
        if (PL == null)
            PL = new PriceList();
        PL.SocietyID = SocID;
        PL.ApartmentTypeID = AptTypeID;
        PL.BSP = BSP;
        PL.Save();
    }

    private void ShowImages(string SocietyID)
    {
        //string str = id;
        String[] ImageFiles = Directory.GetFiles(Server.MapPath("~/Data/Images_Society/"), SocietyID + "_*.jpg");
        str.Append("<div style='width:1070px;overflow:auto;height:180px'><table style='width:auto;'><tr>");

        foreach (string image in ImageFiles)
        {
            str.Append("<td><img style='height:150px;border:1px solid #C0C5C5;' src='" + ResolveClientUrl(@"~/Data/Images_Society/") + Path.GetFileName(image) + "' alt=''/>");
        }
        str.Append("</div>");
        AppendError = false;
    }

    private void UpdateSocietyPoly(int SocietyID)
    {
        NameValueCollection nvc = Request.Form;
        string LatLng = "";
        if (nvc["LatLng"] != null)
        {
            LatLng = nvc["LatLng"];
        }

        if (!string.IsNullOrWhiteSpace(LatLng))
        {
            Society s = Society.GetByID(SocietyID);
            if (s != null)
            {
                s.PolyPoints = LatLng.TrimEnd('^');
                s.Save();
            }
        }
    }

    private void UpdateCityPoly(int ParentCityID, int CityID, string CityName)
    {
        NameValueCollection nvc = Request.Form;
        string LatLng = "";
        if (nvc["LatLng"] != null)
        {
            LatLng = nvc["LatLng"];
        }

        if (!string.IsNullOrWhiteSpace(LatLng))
        {
            City C = City.GetByID(CityID);

            if (C == null && CityID == -1 && CityName != "") //check if the city exists if not then create a new city
            {
                C = City.GetByName(ParentCityID, CityName);

                if (C == null)
                {
                    C = new City();

                    C.ParentID = ParentCityID;
                    C.Name = CityName;
                    C.SortName = CityName;

                }
            }

            if (C != null)
            {
                LatLng = LatLng.TrimEnd('^');

                if (LatLng.Length > 4000)
                {
                    C.PolyPoints = "0";
                    C.Polygon = Cmn.GetCompressed(LatLng);
                    C.PolygonDataSize = LatLng.Length;
                }
                else
                {
                    C.PolyPoints = LatLng;
                    C.Polygon = null;
                    C.PolygonDataSize = 0;
                }

                C.Verified = 1;
                C.Save();
            }

        }
    }

    private void GetPriceTrend(string SubCityUrlName)
    {

        City C = Global.CityList.Values.FirstOrDefault(m => m.UrlName == SubCityUrlName);
        if (C == null)
            return;

        DateTime dtMin = new DateTime(1970, 1, 1);

        var newList = C.PriceTrendList.OrderBy(m => m.Year).ThenBy(m => m.Quarter).Select(a => new
        {
            Max = a.Max,
            Min = a.Min,
            Quarter = "Q" + a.Quarter + "<br/>" + a.Year.ToString().Substring(2),
            PriceDate = (Cmn.ToDate(a.PriceDate) - dtMin).TotalMilliseconds
        }).ToList();

        str.Append(new JavaScriptSerializer().Serialize(newList));
        AppendError = false;
    }

    void GetClientScript()
    {
        Response.Write(File.ReadAllText(Server.MapPath(@"~/Js/ClientScript.js")));
    }

    private void UpdateDistance(int societyId, string landmarkId, int distance, int time, int type)
    {
        var id = 0;
        var DL = Distance.GetDistanceBySocIDandLandMrkID(societyId, Cmn.ToInt(landmarkId));

        foreach (Distance Dis in DL)
        {
            id = Dis.ID;
        }

        NameValueCollection nvc = Request.Form;
        string Instructions = nvc["Instructions"];
        string LatLng = nvc["LatLng"];
        Distance D = new Distance();
        D.ID = id;
        D.SocietyID = societyId;
        D.Type = type;
        D.LandmarkID = Cmn.ToInt(landmarkId);
        D.RoadDistance = distance;
        D.Time = time;
        D.DrivingInstructions = Cmn.GetCompressed(Instructions);
        D.DataSize = Instructions.Length;
        D.LatLng = Cmn.GetCompressed(LatLng);
        D.LatLngDataSize = LatLng.Length;
        D.Save();
    }

    private void GetDistanceFromList(string City)
    {
        List<Society> SocList = Society.GetIfLocAvl();
        foreach (Society S in SocList)
        {
            str.Append(S.ID + "^" + S.SocietyName + "^" + S.Lat.ToString() + "^" + S.Lng.ToString() + "~");
        }
    }

    private void GetDistanceToList(string data, string LanndmarkType)
    {
        //string d = data;
        //string LanndmarkType = "railstation";
        List<Landmark> LandmarkList = Landmark.GetAll(LanndmarkType);
        foreach (Landmark L in LandmarkList)
        {
            str.Append(L.ID + "^" + L.LandmarkName + "^" + L.Lat.ToString() + "^" + L.Lng.ToString() + "~");
        }
    }

    public void DelteImages(string path, int ImageID)
    {
        //bool isLocal = HttpContext.Current.Request.IsLocal;
        //if (isLocal)
        //{
        //    WebClient wc = new WebClient();

        //    string result = wc.DownloadString("http://propertymap.info/Data.aspx?Action=DeleteServerImage&Data1=" + path);  //Request deleteServeverImage Action in Data page 
        //    string t = result.Replace("~", "");
        //    if (result.Replace("~", "") == "")
        //    {
        //        File.Delete(Server.MapPath(path));   //delete in local if succesfully deleted in server.
        //    }
        //    else
        //    {
        //        str.Append("Server Error" + result);
        //    }
        //}
        //else

        try
        {
            ImagesDetail id = ImagesDetail.GetByID(ImageID);
            if (id != null)
            {
                id.Delete();
                Global.LoadGlobalData();
            }
            File.Delete(Server.MapPath(path));     //simply delete if application running online.
        }
        catch (Exception ex)
        {
            str.Append("Server Error" + ex.Message);
        }
    }

    public void DeleteServerImage(string path)
    {
        try
        {
            //int d = 10;
            //int n = 0;
            //int a = d/n;
            File.Delete(Server.MapPath(path));        //delete in server before deleting client side.
        }
        catch (Exception ex)
        {
            if (ex != null)
                str.Append(ex.Message);
        }
    }

    private void UpdateServicesData()
    {
        string AmnData = GetFormString("ServicesData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
        string[] Lines = AmnData.Split('~');
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i] == "")
                return;

            string[] temp = Lines[i].Split('^');

            var check = Service.CheckByConact(ContactNo: temp[2]);

            if (check.Count == 0)
            {
                Service S = new Service();
                S.ID = 0;
                S.Type = 0;
                S.Name = temp[0];
                S.Address = temp[1];
                S.Contact = temp[2];
                S.City = temp[3].Split('-')[0].Trim();
                S.Save();
            }
        }
    }

    private void UpdateAgentData()
    {
        string AmnData = GetFormString("ServicesData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
        string[] Lines = AmnData.Split('~');
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i] == "")
                return;

            string[] temp = Lines[i].Split('^');
            var check = Service.CheckByConact(ContactNo: temp[2]);

            if (check.Count == 0)
            {
                Agent A = new Agent();
                A.ID = 0;
                //A.Type = 0;
                A.AgentName = temp[0];
                A.Address = temp[1];
                A.Mobile1 = temp[2];
                A.Varified = 0;
                A.Deleted = 0;
                //A.City = temp[3].Split('-')[0].Trim();
                //A.Save();
            }
        }
    }

    private void UpdateAminityData()
    {
        string AmnData = GetFormString("AminityData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
        string[] Lines = AmnData.Split('~');
        for (int i = 0; i < Lines.Length; i++)
        {
            string[] temp = Lines[i].Split('^');
            Landmark Al = new Landmark();
            //string count = Al.GetByName(temp[3].ToString());
            //if (count != "")
            //    return;
            Al.Lat = Cmn.ToDbl(temp[0]);
            Al.Lng = Cmn.ToDbl(temp[1]);
            Al.Address = temp[2].ToString();
            Al.LandmarkName = temp[3].ToString();
            Al.LandmarkType = "petrol";
            Al.ContNo = temp[4].ToString();
            Al.Save();
        }
    }

    private void UpdateCurrentProject()//from MagicBrick
    {
        int BuilderId = 0;
        string PropertyData = GetFormString("PropertyData").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
        string[] Lines = PropertyData.Split('~');

        for (int i = 0; i <= 1; i++)
        {
            string[] DataCol = Lines[i].Split('^');
            string Link = DataCol[0];
            string ProjName = DataCol[1].Trim();
            string BuilderName = DataCol[2].Replace(".", "").Trim();
            string PriceRange = DataCol[3].Split(':')[1];
            string Address = DataCol[4].Split(':')[0].Replace("Possession by", "").Trim();
            string PassDate = DataCol[5].Trim();

            Agent Ag = Agent.GetByName(BuilderName);
            if (Ag == null)
            {
                Agent A = new Agent();
                A.ID = 0;
                A.AgentName = BuilderName;
                A.UserType = 2;
                A.Save();
                DatabaseCE db = new DatabaseCE();
                string Error = "";
                BuilderId = Cmn.ToInt(db.GetFieldValue("Select MAX(ID) from Agent", ref Error));
            }
            else
            {
                BuilderId = Ag.ID;
            }

            Society S = new Society()
            {
                ID = 0,
                SocietyName = ProjName,
                Address = Address,
                City = "Ghaziabad",
                State = "Uttar Pradesh",
                Country = "India",
                BuilderID = BuilderId
            }.Save();
        }
    }

    private void GetClietIDByMobileNo(string mobileno)
    {
        AgentClient AC = AgentClient.GetClientByMobile(mobileno);
        if (AC != null)
        {
            str.Append(AC.ID);
        }
    }

    private void GetLatLng(int SocietyID)
    {
        Society S = Society.GetByID(SocietyID);
        string Lat = S.Lat.ToString();
        string Lng = S.Lng.ToString();
        str.Append(Lat + "^" + Lng + "^");
    }

    //void GetAvalByClientID(int ClientID)
    //{
    //    List<AsignClient> asgnClient = AsignClient.GetByClientID(ClientID);
    //    List<int> AvlIDList=new List<int>();
    //    foreach (AsignClient ac in asgnClient)
    //    {
    //        AvlIDList.Add(Cmn.ToInt(ac.AvlID));
    //    }
    //    List<Availability> avlList=null;
    //    str.Append("<table style='border-spacing:1px;'><tr><th>SocietyName<th>BHK<th>Rent<th>Avl From<th>Area<th>Bathroom<th>FlorNo<th>Features<th>Features");
    //    foreach (int avlid in AvlIDList)
    //    {
    //        int a = avlid;
    //        avlList=Availability.GetAvlByID(a);
    //        foreach (Availability avl in avlList)
    //        {
    //            str.Append("<tr><td>" + avl.SocietyName + "<td>" + avl.BHK + "<td>" + avl.Amount + "<td>" + Cmn.ToDate(avl.DateAvailableFrom).ToString("dd-mm-yy") + "<td>" + avl.Area + "<td>" + avl.Bathroom + "<td>" + avl.FloorNo + "<td>" + avl.Facing + "<td>" + Cmn.ToDate(avl.PostedOnDate).ToString("dd-mm-yy")+"</tr>");
    //            //str.Append(avl.SocietyName + "^" + avl.BHK + "^" + avl.Bathroom + "^" + avl.Balcony);
    //            //str.Append("~");
    //        }
    //    }
    //    str.Append("</table>");
    //}

    void GetAsignClient(int ClientID)
    {
        List<AsignClient> AsignClientList = AsignClient.GetByClientID(ClientID);
        foreach (AsignClient S in AsignClientList)
        {
            str.Append(S.ID + "^" //0
                + S.AvlID + "^" //1
                + S.ClientID + "^" //2
                + "~");
        }
    }

    void AssignClientToProperty(int AvailabilityID, int AgentCustomerID, int term)
    {
        if (term == 0)
        {
            int count = 0;
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                List<AsignClient> AC = (from m in context.AsignClients.Where(x => x.AvlID == AvailabilityID && x.ClientID == AgentCustomerID) select m).ToList();
                foreach (AsignClient ac in AC)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                AsignClient AC = new AsignClient()
                {
                    ID = 0,
                    ClientID = AgentCustomerID,
                    AvlID = AvailabilityID
                }.Save();
                str.Append("added");
            }
            else
                str.Append("Already added");
        }
        else
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                context.ExecuteStoreCommand("DELETE FROM AsignClient WHERE AvlID = {0} and ClientID={1}", AvailabilityID, AgentCustomerID);
            }
            str.Append("removed");
        }
    }

    void GetAvlClientList(int SellerID)
    {
        int a = SellerID;
        string Query = "select * from AsignClient where AvlID in (select ID from Availability where SellerID=" + SellerID + ")";

        using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        {
            var client = context.ExecuteStoreQuery<AsignClient>(Query, "");
            foreach (AsignClient ac in client)
            {
                str.Append(ac.AvlID + "^" + ac.ClientID + "~");
            }
        }
    }

    void GetAgentClientList(int SellerID)
    {
        int a = SellerID;
        string Query = "select * from AgentClient where id in(select ClientID from AsignClient where AvlID in (select ID from Availability where SellerID=" + SellerID + "))";

        using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        {
            var client = context.ExecuteStoreQuery<AgentClient>(Query, "");
            str.Append("<table style='border-spacing:0px;'><th>Name</th><th>Phone</th><th>MobileNO</th><th>Email</th>");
            foreach (AgentClient ac in client)
            {
                str.Append("<tr><td>" + ac.Name);
                str.Append("</td><td>" + ac.PhoneNo);
                str.Append("</td><td>" + ac.MobileNo);
                str.Append("</td><td>" + ac.EmailID + "</td></tr>");
            }
            str.Append("</table>");
        }
    }

    void GetClientList(int AgentID)
    {
        List<AgentClient> ClientList = AgentClient.GetListByAgentID(AgentID);

        foreach (AgentClient A in ClientList)
        {
            str.Append(A.ID + "^" + A.Name + "^" + A.MobileNo + "^" + A.EmailID + "~");
        }
    }

    void SearchBuilder(string Arg)
    {
        if (Arg == "")
            return;

        StringBuilder Result = new StringBuilder();

        List<Agent> list = Global.BuilderList.Values.Where(m => m.AgentCompany != null && m.AgentCompany.ToLower().Contains(Arg) && m.UserType != null && m.UserType == 2).ToList();

        int ctr = 0;
        foreach (Agent a in list)
        {
            Result.Append(",{\"id\":\"" + a.ID
                + "\",\"label\":\"" + a.AgentCompany + "," + a.ID
                + "\",\"value\":\"" + a.AgentCompany
                + "\"}");
            if (ctr++ > 10)
                break;
        }
        if (Result.Length > 0)
            str.Append("[" + Result.ToString().Substring(1) + "]");
    }

    void SearchSociety(string Arg, string Data)
    {
        if (Data == "0")
        {
            if (Arg == "")
                return;

            int i;
            string Query = "select * from Society where  " + (int.TryParse(Arg, out i) ? "(ID LIKE '" + Arg + "%')" : "(SocietyName LIKE '" + Arg + "%')");

            StringBuilder Result = new StringBuilder();
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var society = context.ExecuteStoreQuery<Society>(Query, "");
                int ctr = 0;
                foreach (Society s in society)
                {
                    Result.Append(",{\"id\":\"" + s.ID
                        + "\",\"label\":\"" + s.SocietyName + "," + s.ID
                        + "\",\"value\":\"" + s.SocietyName
                        + "\"}");
                    if (ctr++ > 10)
                        break;
                }
                if (Result.Length > 0)
                    str.Append("[" + Result.ToString().Substring(1) + "]");
            }
        }
        else if (Data == "1")
        {
            if (Arg == "")
                return;

            int i;

            string Query = "select * from Agent where  " + (int.TryParse(Arg, out i) ? "(ID LIKE '" + Arg + "%')" : "(AgentName LIKE '" + Arg + "%')");
            StringBuilder Result = new StringBuilder();
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var agent = context.ExecuteStoreQuery<Agent>(Query, "");
                int ctr = 0;
                foreach (Agent a in agent)
                {
                    Result.Append(",{\"id\":\"" + a.ID
                        + "\",\"label\":\"" + a.AgentName + "," + a.ID
                        + "\",\"value\":\"" + a.AgentName
                        + "\"}");
                    if (ctr++ > 10)
                        break;
                }
                if (Result.Length > 0)
                    str.Append("[" + Result.ToString().Substring(1) + "]");
            }
        }
    }

    void ErrorWriter(Exception ex, string Message = "")
    {
        File.AppendAllText(Server.MapPath(@"~\error.txt"), ex.Message + ex.StackTrace);
    }

    void CheckIfPropertyAlreadyDownloaded(string DataSource, string DataSourceID)
    {
        Availability A = Availability.GetByDataSourceAndID(DataSource, DataSourceID);
        str.Append(A != null ? "Found" : "Not Found");

    }
    void UpdatePropertyAvailability()
    {
        if (Data1 == "magicbricks.com")
        {
            UpdateMagicBrickProperty();
        }
        else if (Data1 == "indiaproperty.com")
        {
            UpdateIPProperty();
        }
        else if (Data1 == "99acres.com")
        {
            Update99Property();
        }
        else if (Data1 == "makaan.com")
        {
            UpdateMakaanProperty();
        }
    }

    void UpdateMagicBrickProperty()
    {
        try
        {
            Availability Avl = new Availability();
            string PropertyData = GetFormString("PropertyData");

            if (string.IsNullOrWhiteSpace(PropertyData))
                return;

            string[] Lines = PropertyData.Split('^');

            for (int i = 0; i < Lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(Lines[i]))
                    continue;

                Avl.DataSource = "MB";
                Avl.AvailabilityType = 0;
                Avl.DateAvailableFrom = Cmn.GetIndiaTime();
                string Data = Lines[i].Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                switch (i)
                {
                    case 0: Avl.DataSourceID = Data.Split(':')[1].Trim(); break;
                    //case 1: Avl.Heading = Data.Trim(); break;
                    case 2: Avl.Amount = Cmn.ToInt(Data == "Call for Price" ? "" : Data.Split(':')[1].Split('(')[0].Replace(",", "").Trim()); break;
                    case 4: Avl.SocietyName = Cmn.ProperCase(Data.Split(':')[1].Split('|')[0].Trim()); break;
                    case 5: Avl.SocietyLocation = Data.Split(':')[1].Replace("State", "").Trim(); break;
                    case 7: Avl.Company = Data != "~" ? Data : ""; break;
                    case 8:
                        Avl.SellerName = Data.Split('$')[0].Trim();
                        Avl.Address = Data.Split('$')[1].Split(':')[1].Trim();
                        break;
                    case 9:
                        string d = Data.Split(':')[1].Trim();
                        Avl.PostedOnDate = Cmn.ToDate(d.Substring(4, 2) + "-" + d.Substring(0, 3) + "-" + d.Substring(9));
                        break;

                    case 6:
                        string[] F = Data.Split('@');

                        foreach (string s in F)
                        {
                            string[] lbl = s.Split('$');
                            if (lbl.Length < 2)
                                continue;

                            string A = lbl[0].Trim();
                            string B = lbl[1].Trim();

                            if (A.Contains("Bedroom(s)"))
                                Avl.BHK = Cmn.ToInt(B);
                            else if (A.Contains("Bathroom"))
                                Avl.Bathroom = Cmn.ToInt(B);
                            else if (A.Contains("Balconies"))
                                Avl.Balcony = Cmn.ToInt(B);
                            else if (A.Contains("Additional Rooms"))
                                Avl.AdditionalRoom = B;
                            else if (A.Contains("Covered Area"))
                                Avl.Area = Cmn.ToInt(B.Substring(0, 4));
                            else if (A.Contains("Carpet Area"))
                                Avl.CarpetArea = Cmn.ToInt(B.Substring(0, 4));
                            else if (A.Contains("Plot Area"))
                                Avl.PlotArea = Cmn.ToInt(B.Substring(0, 4));
                            else if (A.Contains("Floor Number"))
                                Avl.FloorNo = Cmn.ToInt(B);
                            else if (A.Contains("Total Floors"))
                                Avl.TotalFloors = Cmn.ToInt(B);
                            else if (A.Contains("Furnished"))
                                Avl.Furnishing = B;
                            else if (A.Contains("Directional Facing"))
                                Avl.DirectionalFacing = B;
                            else if (A.Contains("Facing"))
                                Avl.Facing = B;
                            else if (A.Contains("Age of Construction"))
                                Avl.AgeOfConstruction = B;
                            else if (A.Contains("Available Units"))
                                Avl.AvailableUnits = Cmn.ToInt(B);
                            else if (A.Contains("Type of Ownership"))
                                Avl.OwnershipType = B;
                        }
                        break;
                    case 10:
                        string mobiles = Data.Split('$')[0];
                        string[] mobtemp = mobiles.Split(':');
                        if (mobtemp[0] == "Mobile")
                        {
                            string[] len = mobtemp[1].Split(',');
                            switch (len.Length)
                            {
                                case 1:
                                    Avl.SellerMobile1 = len[0].Trim();
                                    break;
                                case 2:
                                    Avl.SellerMobile1 = len[0].Trim();
                                    Avl.SellerMobile2 = len[1].Trim();
                                    break;
                                case 3:
                                    Avl.SellerMobile1 = len[0].Trim();
                                    Avl.SellerMobile2 = len[1].Trim();
                                    Avl.SellerMobile3 = len[2].Trim();
                                    break;
                                case 4:
                                    Avl.SellerMobile1 = len[0].Trim();
                                    Avl.SellerMobile2 = len[1].Trim();
                                    Avl.SellerMobile3 = len[2].Trim();
                                    break;

                                default:
                                    break;

                            }
                        }
                        string[] phone = Data.Split('$')[1].Split(':');
                        if (phone[0] == "Landline")
                        {
                            Avl.SellerPhone = phone[1].Replace("--", "").Trim();
                        }
                        break;
                    case 11:
                        if (Data.Trim() == "Contact Owner")
                            Avl.SellerType = 1;
                        if (Data.Trim() == "Contact Agent")
                            Avl.SellerType = 0;
                        break;
                }
            }

            UpdateAvl(Avl);
        }
        catch (Exception ex)
        {
            ErrorWriter(ex);
        }
    }

    void UpdateIPProperty()
    {
        Availability Avl = new Availability();
        string PropertyData = GetFormString("PropertyData");

        if (string.IsNullOrWhiteSpace(PropertyData))
            return;

        string[] Lines = PropertyData.Split('^');

        for (int i = 0; i < Lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(Lines[i]))
                continue;

            Avl.DataSource = "IP";
            Avl.AvailabilityType = 0;
            Avl.DateAvailableFrom = Cmn.GetIndiaTime();

            string Data = Lines[i].Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
            switch (i)
            {
                case 0:
                    string[] stemp = Data.Split(',');
                    Avl.SocietyName = stemp[0].Trim();
                    Avl.SocietyLocation = Data.Trim();
                    break;
                case 1:
                    string[] temps = Data.Split('|');
                    string[] S1 = temps[0].Split('$');

                    string ABC = temps[1].Trim();
                    Avl.AgeOfConstruction = ABC.Replace("Age of property:", "").Replace("$", "").Trim();

                    foreach (string s in S1)
                    {
                        if (s == "")
                            continue;
                        string A = s.Split(':')[0].Trim();
                        string B = s.Split(':')[1].Trim();

                        if (A.Contains("Rent Per Month"))
                            Avl.Amount = Cmn.ToInt(B.Replace(",", "").Trim());
                        else if (A.Contains("Built-up Area"))
                            Avl.Area = Cmn.ToInt(B.Replace("Sq. Feet", "").Trim());
                        else if (A.Contains("Bedrooms"))
                        {
                            Avl.BHK = Cmn.ToInt(B.Split('|')[0].Replace("More than", "").Trim());
                            //AgeOfConstruction = B.Split('|')[1].Trim();
                        }

                    }
                    break;
                case 2:
                    string[] S2 = Data.Split('$');

                    foreach (string s in S2)
                    {
                        if (s == "")
                            continue;
                        string A = s.Split(':')[0].Trim();
                        string B = s.Split(':')[1].Trim();

                        if (A.Contains("Total No. of Floors"))
                            Avl.TotalFloors = Cmn.ToInt(B.Trim());
                        else if (A.Contains("Property on floor"))
                            Avl.FloorNo = Cmn.ToInt(B.Trim());
                    }
                    break;

                case 3:

                    string X = Data.Split('|')[0];
                    string Y = Data.Split('|')[1];
                    string[] temp = X.Split(':');
                    if (temp[0] == "Agent")
                    {
                        Avl.SellerType = 0;
                        Avl.SellerName = temp[1].Replace("Property ID", "").Trim();
                    }
                    else if (temp[0] == "Individual")
                    {
                        Avl.SellerType = 1;
                    }
                    Avl.SellerName = temp[1].Replace("Property ID", "").Trim();
                    Avl.DataSourceID = temp[2].Trim();
                    string tempdate = Y.Replace("Last updated on", "").Trim();
                    Avl.PostedOnDate = Cmn.ToDate(tempdate.Substring(4, 2) + "-" + tempdate.Substring(0, 3) + "-" + tempdate.Substring(8));
                    break;

                case 4:

                    string[] S3 = Data.Split('$');

                    foreach (string s in S3)
                    {
                        if (s == "")
                            continue;
                        string A = s.Split(':')[0].Trim();
                        string B = s.Split(':')[1].Trim();

                        if (A.Contains("Furnishing"))
                            Avl.Furnishing = B.Trim();
                    }
                    break;
                case 5:
                    Avl.SellerName = Data.Replace("Agent", "").Trim();
                    break;
                case 6:
                    Avl.SellerMobile1 = Data.Replace("Mobile:", "").Replace("91-", "").Trim();
                    break;
                case 7:
                    Avl.SellerPhone = Data.Split(':')[1].Trim();
                    break;
            }
        }
        UpdateAvl(Avl);
    }

    void Update99Property()
    {

        Availability Avl = new Availability();

        string PropertyData = GetFormString("PropertyData");

        if (string.IsNullOrWhiteSpace(PropertyData))
            return;

        string[] Lines = PropertyData.Split('^');

        Avl.DataSource = "99";
        Avl.AvailabilityType = 0;
        Avl.DateAvailableFrom = Cmn.GetIndiaTime();


        for (int i = 0; i < Lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(Lines[i]))
                continue;

            string Data = Lines[i].Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
            switch (i)
            {
                case 0:
                    string d = Data.Trim();
                    Avl.PostedOnDate = Cmn.ToDate(d.Substring(4, 2) + "-" + d.Substring(0, 3) + "-" + d.Substring(8));
                    break;
                //case 1: Avl.Heading = Data.Trim(); break;
                case 2: Avl.SocietyLocation = Data.Split(':')[1].Trim(); break;
                case 3: Avl.Amount = Cmn.ToInt(Data.Split('.')[1].Split('.')[0].Replace(",", "").Trim()); break;
                case 4:
                    string[] F = Data.Split('@');

                    foreach (string s in F)
                    {
                        string[] lbl = s.Split('$');
                        if (lbl.Length < 2)
                            continue;

                        string A = lbl[0].Trim();
                        string B = lbl[1].Trim();

                        if (A.Contains("Built-up Area:"))
                            Avl.Area = Cmn.ToInt(B.Split('.')[0].Trim());
                        else if (A.Contains("Bedrooms:"))
                            Avl.BHK = Cmn.ToInt(B);
                        else if (A.Contains("Bathrooms:"))
                            Avl.Bathroom = Cmn.ToInt(B);
                        else if (A.Contains("Balconies:"))
                            Avl.Balcony = Cmn.ToInt(B);
                        else if (A.Contains("Facing:"))
                            Avl.DirectionalFacing = B;
                    }
                    break;
                case 5:
                    string[] S = Data.Split('@');

                    foreach (string s in S)
                    {
                        string[] lbl = s.Split('$');
                        if (lbl.Length < 2)
                            continue;

                        string A = lbl[0].Trim();
                        string B = lbl[1].Trim();

                        if (A.Contains("Society  Name:"))
                            Avl.SocietyName = B;
                        else if (A.Contains("Total Floors in Complex:"))
                            Avl.TotalFloors = Cmn.ToInt(B.Split('t')[0].Trim());
                        else if (A.Contains("Property on Floor:"))
                        {
                            string input = B.Trim();
                            string result = Regex.Replace(input, "\\D", "");
                            Avl.FloorNo = Cmn.ToInt(result);
                        }
                        else if (A.Contains("Furnishing:"))
                            Avl.Furnishing = B.Split('t')[0];

                    }
                    break;
                case 6:
                    if (Lines[9].Trim() != "Owner")
                    {
                        string[] St = Data.Split('|');
                        Avl.SellerName = Cmn.ProperCase(Data.Split('|')[0]).Trim();
                        Avl.Company = Cmn.ProperCase(Data.Split('|')[1]).Trim();
                        //SellerName = AgentCompany;
                    }
                    else
                    {
                        Avl.SellerName = Data.Trim();
                    }

                    break;
                case 7:
                    if (Lines[9].Trim() != "Owner")
                    {
                        string[] str = Data.Split('$');
                        string mobile = str[0].Trim();
                        string phone = str[1].Trim();
                        string[] mobiles = mobile.Split(':');
                        string[] phones = phone.Split(':');
                        if (mobiles[0] == "Mobile")
                        {
                            string[] tempmob = mobiles[1].Split(',');

                            int b = tempmob.Length;
                            switch (b)
                            {
                                case 1:
                                    Avl.SellerMobile1 = tempmob[0].Trim();
                                    break;
                                case 2:
                                    Avl.SellerMobile1 = tempmob[0].Trim();
                                    Avl.SellerMobile2 = tempmob[1].Trim();
                                    break;
                                case 3:
                                    Avl.SellerMobile1 = tempmob[0].Trim();
                                    Avl.SellerMobile2 = tempmob[1].Trim();
                                    Avl.SellerMobile3 = tempmob[2].Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (phones[0] == "Phone")
                        {
                            Avl.SellerPhone = phones[1].Trim();
                        }
                    }
                    else
                    {
                        Avl.SellerMobile1 = Data.Trim();
                    }
                    break;
                case 8:
                    Avl.DataSourceID = Data.Trim();
                    break;
                case 9:
                    if (Data.Trim() == "Dealer")
                        Avl.SellerType = 0;
                    if (Data.Trim() == "Owner")
                        Avl.SellerType = 1;
                    if (Data.Trim() == "Builder")
                        Avl.SellerType = 2;
                    break;
                case 10:
                    Avl.Address = Data.Trim();
                    break;
            }
        }

        UpdateAvl(Avl);
    }

    void UpdateMakaanProperty()
    {
        Availability Avl = new Availability();

        string PropertyData = GetFormString("PropertyData");

        if (string.IsNullOrWhiteSpace(PropertyData))
            return;

        string[] MainSpl = PropertyData.Split('$');

        Avl.DataSource = "MK";
        Avl.AvailabilityType = 0;
        Avl.DateAvailableFrom = Cmn.GetIndiaTime();

        for (int a = 0; a < MainSpl.Length; a++)
        {
            if (string.IsNullOrWhiteSpace(MainSpl[a]))
                continue;
            switch (a)
            {
                case 0:
                    string[] Lines = MainSpl[a].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('~');

                    for (int i = 0; i < Lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(Lines[i]))
                            continue;

                        string[] Data = Lines[i].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('^');

                        switch (i)
                        {
                            case 0:
                                string temp1 = Data[0].Split('|')[0];
                                string temp2 = Data[0].Split('|')[1];
                                Avl.DataSourceID = temp1.Split(':')[1].Trim();
                                string tempdate = temp2.Split(':')[1].Replace(",", "").Trim();
                                //string tempdate=Regex.Replace(temp, @"\s+", " ");
                                Avl.PostedOnDate = Cmn.ToDate(tempdate.Substring(4, 2) + "-" + tempdate.Substring(0, 3) + "-" + tempdate.Substring(6));
                                break;
                        }
                    }
                    break;
                case 1:
                    string[] Lines1 = MainSpl[a].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('~');

                    for (int i = 0; i < Lines1.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(Lines1[i]))
                            continue;

                        string Data = Lines1[i].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                        switch (i)
                        {
                            case 0:
                                Avl.Amount = Cmn.ToInt(Data.Split('|')[0].Split('(')[0].Replace("Rs.", "").Replace(",", "").Replace("^", "").Trim());
                                break;
                            case 1:
                                string temp = Data.Split('|')[0].Trim();
                                string temp2 = Data.Split('|')[1].Trim();
                                Avl.BHK = Cmn.ToInt(Regex.Replace(temp, "\\D", "").Trim());
                                //BHK = temp.Replace("Bedrooms", "").Trim();
                                Avl.Area = Cmn.ToInt(temp2.Replace(",", "").Replace("Sq.Ft", "").Trim());
                                break;
                        }
                    }
                    break;

                case 2:
                    string[] Lines2 = MainSpl[a].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('~');

                    for (int i = 0; i < Lines2.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(Lines2[i]))
                            continue;

                        string[] Data = Lines2[i].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('^');
                        if (i == 1)
                        {
                            if (Data[0] == "")
                                i = i + 1;
                        }

                        switch (i)
                        {
                            case 0:
                                string seltypetmp = Data[0].Split(':')[1].Split('(')[1].Replace(")", "").Trim();
                                Avl.SellerName = Data[0].Split(':')[1].Split('(')[0].Trim();
                                if (seltypetmp == "Agent")
                                {
                                    Avl.SellerName = Data[0].Split(':')[1].Split('(')[0].Trim();
                                    //AgentName = SellerName;
                                    Avl.SellerType = 0;
                                }
                                if (seltypetmp == "Individual")
                                    Avl.SellerType = 1;
                                if (seltypetmp == "Builder")
                                    Avl.SellerType = 2;
                                break;
                            case 1:
                                Avl.Company = Data[0].Split(':')[1].Trim();
                                //Avl.SellerName = CompanyName;
                                break;
                            case 2:
                                Avl.SellerMobile1 = Data[1].Split('-')[1].Split(',')[0].Trim();
                                break;

                        }
                    }
                    break;

                case 3:
                    string[] Lines3 = MainSpl[a].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('~');

                    for (int i = 0; i < Lines3.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(Lines3[i]))
                            continue;

                        string[] Data = Lines3[i].Replace("\n", "").Replace("\t", " ").Replace("\r", "").Replace("&nbsp;", " ").Trim().Split('^');
                        string b = Data[0].Trim();
                        switch (b)
                        {
                            case "Floor Number":
                                Avl.FloorNo = Cmn.ToInt(Data[1].Split(':')[1].Trim());
                                break;
                            case "Total Floor":
                                Avl.TotalFloors = Cmn.ToInt(Data[1].Split(':')[1].Trim());
                                break;
                            case "Bathrooms":
                                Avl.Bathroom = Cmn.ToInt(Data[1].Split(':')[1].Trim());
                                break;
                            case "Age of Construction":
                                Avl.AgeOfConstruction = Data[1].Split(':')[1].Trim();
                                break;
                            case "Furnished":
                                Avl.Furnishing = Data[1].Split(':')[1].Trim();
                                break;
                            case "Facing":
                                Avl.DirectionalFacing = Data[1].Split(':')[1].Trim();
                                break;
                            case "Ownership Type":
                                Avl.OwnershipType = Data[1].Split(':')[1].Trim();
                                break;
                        }
                    }
                    break;
            }
        }
        UpdateAvl(Avl);
    }

    void UpdateAvl(Availability _Avl)
    {
        try
        {
            Availability Avltemp = Availability.GetByDataSourceAndID(_Avl.DataSource, _Avl.DataSourceID);

            //if (Avltemp != null)
            //    _Avl.ID = Avltemp.ID;

            _Avl.Save();

            Agent A = Agent.UpdateAgent(_Avl);   //Update Agent

            if (A != null)      //refill AgentId from Agent table 
            {
                _Avl.SellerID = A.ID;
                _Avl.Save();
            }
            str.Append("Update - Data Saved - " + _Avl.Message);
        }
        catch (Exception ex)
        {
            str.Append(ex.Message);
        }
    }

    List<Society> GetSocietyList()
    {
        List<Society> SocietyList = Society.GetAvailable();
        str.Append("<table id='societytable' cellpadding='0' cellspacing='1'>");
        str.Append("<tr><th><th>Society Name<th>1 BHK<th>2 BHK<th>3 BHK<th>4 BHK<th>5 BHK");
        int ctr = 0;

        List<Availability> AvailabilityList = Availability.GetAvailableBySociety();

        foreach (Society S in SocietyList)
        {
            ctr++;

            List<Availability> tempAvailabilityList = AvailabilityList.FindAll(m => m.SocietyID == S.ID);

            S.ForSale1BHK = 0;
            S.ForSale2BHK = 0;
            S.ForSale3BHK = 0;
            S.ForSale4BHK = 0;
            S.ForSale5BHK = 0;

            foreach (Availability A in tempAvailabilityList)
            {
                if (A.BHK == 1) S.ForSale1BHK++;
                if (A.BHK == 2) S.ForSale2BHK++;
                if (A.BHK == 3) S.ForSale3BHK++;
                if (A.BHK == 4) S.ForSale4BHK++;
                if (A.BHK == 5) S.ForSale5BHK++;
            }
            str.Append("<tr style='text-align:left'><td>" + ctr + "<td onclick='GetSocietyAvailability(" + S.ID + ",-1)'><a href='#' onclick='return ShowSocietyDetail(" + S.ID + ")'>" + S.SocietyName + "</a>");
            str.Append("<td class='tablecell' onclick='GetSocietyAvailability(" + S.ID + ",1)'>" + (S.ForSale1BHK != 0 ? S.ForSale1BHK.ToString() : "") + "</td>");
            str.Append("<td class='tablecell' onclick='GetSocietyAvailability(" + S.ID + ",2)'>" + (S.ForSale2BHK != 0 ? S.ForSale2BHK.ToString() : "") + "</td>");
            str.Append("<td class='tablecell' onclick='GetSocietyAvailability(" + S.ID + ",3)'>" + (S.ForSale3BHK != 0 ? S.ForSale3BHK.ToString() : "") + "</td>");
            str.Append("<td class='tablecell' onclick='GetSocietyAvailability(" + S.ID + ",4)'>" + (S.ForSale4BHK != 0 ? S.ForSale4BHK.ToString() : "") + "</td>");
            str.Append("<td class='tablecell' onclick='GetSocietyAvailability(" + S.ID + ",5)'>" + (S.ForSale5BHK != 0 ? S.ForSale5BHK.ToString() : "") + "</td>");
        }
        str.Append("<table>");
        return SocietyList;
    }

    void GetAvailibilityText(int SocietyID, int BHK, int SellerID, int SellerType, int Avl_ID, int ViewType = 0)
    {
        List<Availability> AvailabilityList = Availability.GetList(SocietyID, BHK, SellerID, Avl_ID, SellerType);
        foreach (Availability S in AvailabilityList)
        {
            str.Append(S.ID + "^" //0
                + S.SocietyID + "^" //1
                + S.SocietyName + "^" //2
                + S.SellerID + "^" //3
                + S.SellerName + "^" //4
                + S.BHK + "^" //5
                + S.Amount + "^"//6 
                + Cmn.ToDate(S.DateAvailableFrom).ToString("dd-MMM-yy") + "^" //7
                + S.Area + "^" //8
                + S.Bathroom + "^"//9 
                + S.FloorNo + "^" //10
                + S.Facing + "^"//11
                //+ Cmn.ToDate(S.PostedOnDate).ToString("dd-MMM-yy") + "^" //12
                + Convert.ToInt32(NoOfDays(Cmn.ToDate(S.PostedOnDate))) + "^"
                + "~");
        }
    }

    double NoOfDays(DateTime d1)
    {
        DateTime d2 = Cmn.GetIndiaTime();
        TimeSpan days = d2 - d1;
        return days.TotalDays;
    }

    List<Availability> GetSocietyAvailability(int SocietyID, int BHK, int SellerID, int SellerType, int AvlID, int ViewType = 0)
    {
        List<Availability> AvailabilityList = Availability.GetList(SocietyID, BHK, SellerID, SellerType);   //Get List Societyvise and BHKvise

        str.Append("<table id='AvlTable' style='border-spacing:1px;'><tr>");

        switch (ViewType)
        {
            case 1:
                str.Append("<th><th>Society");
                break;
            default:
                str.Append("<th><th>Agent<th>Phone");
                break;
        }

        str.Append("<th>BHK<th>Rent<th>Avail From<th>Area<th>Bath<th>Floor<th>Features<th>Posted On");

        int ctr = 0;

        if (SellerID != 0)
        {
            foreach (Availability S in AvailabilityList)
            {
                //str.Append("<tr onclick='return ShowAvailabilityDetail(" + S.ID + "," + S.SellerID + ")'><td>" + (++ctr));
                switch (ViewType)
                {
                    case 1:
                        str.Append("<tr onclick='return ShowAvailabilityDetail(" + S.ID + "," + S.SellerID + ")'><td>" + (++ctr));
                        //Society Soc = Society.GetByID(Cmn.ToInt(S.SocietyID));
                        //string SocietyName = Soc.SocietyName!=null?Soc.SocietyName:"";
                        str.Append("<td>" + S.SocietyName);
                        break;

                    default:
                        str.Append("<tr><td>" + (++ctr));
                        str.Append("<td><a href='#' onclick='return ShowAgentDetail(" + S.SellerID + "," + S.ID + ")'>" + (string.IsNullOrWhiteSpace(S.SellerName) ? "&lt;no name&gt;" : Cmn.ProperCase(S.SellerName)) + "</a><td>" + S.SellerMobile1);
                        break;
                }

                str.Append("<td>" + S.BHK);
                str.Append("<td><a href='#' onclick='return ShowAvailabilityDetail(" + S.ID + ")'>" + S.Amount.ToString() + "</a>");
                str.Append("<td>" + (S.DateAvailableFrom != null ? Cmn.ToDate(S.DateAvailableFrom).ToString("dd-MMM-yy") : ""));
                str.Append("<td>" + S.Area);
                str.Append("<td>" + S.Bathroom);
                str.Append("<td>" + S.FloorNo);
                str.Append("<td>" + S.Facing);
                str.Append("<td>" + Cmn.ToDate(S.PostedOnDate).ToString("dd-MMM-yy"));
                switch (ViewType)
                {
                    case 1:
                        str.Append("<tr><td><td colspan='9'><div id='divAvl" + S.ID + "'></div>"); break;
                }
            }
        }
        str.Append("</table>");
        return AvailabilityList;
    }

    //List<Availability> GetSocietyAvailability(int SocietyID, int BHK, int SellerID, int SellerType, int ViewType = 0)
    //{
    //    List<Availability> AvailabilityList = Availability.GetList(SocietyID, BHK, SellerID, SellerType);   //Get List Societyvise and BHKvise

    //    str.Append("<table id='AvlTable' cellpadding='0' cellspacing='1'>");
    //    str.Append("<tr id=''><th><th>Agent<th>Phone<th>BHK<th>Rent<th>Available From<th>Area<th>Bath<th>Floor No<th>Facing<th>Power Backup<th>Posted On");
    //    int ctr = 0;

    //    foreach (Availability S in AvailabilityList)
    //    {
    //        ctr++;
    //        str.Append("<tr style='text-align:left'><td>" + ctr + "<td><a href='#' onclick='return ShowAgentDetail(" + S.SellerID + "," + S.ID + ")'>" + (string.IsNullOrWhiteSpace(S.SellerName) ? "&lt;no name&gt;" : Cmn.ProperCase(S.SellerName)) + "</a>");
    //        str.Append("<td>" + S.SellerMobile1);
    //        str.Append("<td>" + S.BHK);
    //        str.Append("<td><a href='#' onclick='return ShowAvailabilityDetail(" + S.ID + ")'>" + S.Amount.ToString() + "</a>");

    //        DateTime datetime = Cmn.ToDate(S.DateAvailableFrom.ToString());
    //        string date = "";


    //        date = datetime.ToString("dd-MMM-yy");

    //        str.Append("<td>" + date);
    //        str.Append("<td>" + S.Area);
    //        str.Append("<td>" + S.Bathroom);
    //        str.Append("<td>" + S.FloorNo);
    //        str.Append("<td>" + S.Facing);
    //        str.Append("<td>");

    //        DateTime datetime1 = Cmn.ToDate(S.PostedOnDate.ToString());
    //        string postedondate = datetime1.ToString("dd-MMM-yy");
    //        str.Append("<td>" + postedondate);
    //    }
    //    str.Append("<table>");
    //    return AvailabilityList;
    //}

    void GetAgentDetail(int ID, int IsHTML)
    {
        Agent A = Agent.GetByID(ID);

        if (A != null)
        {
            if (IsHTML == 1)
            {
                str.Append("<table class='table-4' cellpadding='0' cellspacing='1'>");
                str.Append("<tr><td>Name<td>" + A.AgentCompany);
                str.Append("<tr><td>Landline<td><a href='tel:" + A.PhoneNo1 + "'>" + A.PhoneNo1);
                str.Append("</a><tr><td>Mobile 1<td><a href='tel:" + A.Mobile1 + "'>" + A.Mobile1);
                str.Append("<a><tr><td>Mobile 2<td><a href='tel:" + A.Mobile2 + "'>" + A.Mobile2);
                str.Append("<a><tr><td>Contact Person<td>" + A.AgentName);
                str.Append("<tr><td>Address <td>" + A.Address);
                str.Append("<table>");
            }
            else
            {
                str.Append(A.ID + "^" + A.AgentName + "^" + A.AgentCompany + "^" + A.Mobile1 + "^" + A.Mobile2 + "^" + A.Address);
            }
        }
    }

    void GetProjectDetail(int ID)
    {
        Society A = Society.GetByID(ID);

        if (A != null)
        {
            str.Append(A.SocietyName + "^"
            + A.Address + "^"
            + A.Pin + "^"
            + A.City + "^"
            + A.BuiltYear + "^"
            + A.Country + "^"
            + A.DistanceAirport + "^"
            + A.DistaneRailStation + "^"
            + A.DistanceBusStation + "^"
            + A.DistanceSchool + "^"
            + A.DistanceHospital + "^"
            + A.DistanceShopping + "^"
            + A.DistaneRailStation + "^"
            + A.PowerBackup + "^"
            + A.Lat + "^"
            + A.Lng + "^"
            + A.Subcity + "^"
            + "~");
        }
    }

    void GetProjectDetailJson(int ID)
    {
        Society A = Society.GetByID(ID);
        str.Append(new JavaScriptSerializer().Serialize(A));
        AppendError = false;
    }

    void GetAllProjectDetailJson(int ID)
    {
        Society A = Society.GetByID(ID);
        List<Society> S = Society.GetBySubCity(Cmn.ToInt(A.SubCityID));
        str.Append(new JavaScriptSerializer().Serialize(S));
        AppendError = false;
    }

    void GetAvailabilityDetail(int ID)
    {
        Availability A = Availability.GetByID(ID);

        if (A != null)
        {
            //str.Append("<table cellpadding='0' cellspacing='1'>");
            //str.Append("<tr><td>Name<td>" + A.SellerName);
            //str.Append("<tr><td>BHK<td>" + A.BHK);
            //str.Append("<table>");

            str.Append(A.ID + "^"
                       + A.SocietyName + "^"
                       + A.BHK + "^"
                       + A.Amount + "^"
                        + A.Area + "^"
                        + A.Bathroom + "^"
                        + A.FloorNo + "^"
                        + A.SocietyID + "^"
                       + "~");
        }
    }

    void UpdateAvailibility() //update availability by mobile post
    {
        Availability A = new Availability();
        A.SocietyID = Cmn.ToInt(GetFormString("societyId"));
        A.SellerName = GetFormString("name");
        A.SellerMobile1 = GetFormString("mobile");
        A.Amount = Cmn.ToInt(GetFormString("rent"));
        A.Area = Cmn.ToInt(GetFormString("area"));
        A.BHK = Cmn.ToInt(GetFormString("bhk"));
        A.Bathroom = Cmn.ToInt(GetFormString("bath"));
        A.SellerID = Cmn.ToInt(GetFormString("sellerId"));
        A.SellerType = Cmn.ToInt(GetFormString("sellerType"));
        A.AvailabilityType = Cmn.ToInt(GetFormString("availType"));
        A.Save();
    }


    void GetPDF()
    {
        NameValueCollection nvc = Request.Form;
        string n = nvc["_HTML"];
        string HTML = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'>";
        HTML += "<body><b>" + n + "</b></body></html>";
        HTML = Server.HtmlDecode(HTML);
        HTML = HTML.Replace("\\", "'");
        Global.MenuPDFData = HTML;
    }
    void ShowPDF()
    {
        new PDFPrinter(this).CreateMenuHTML(Global.MenuPDFData);
    }

    public static string SendEmail(string Subject, string EmailTo, string Name, string Usermail, string Description)
    {
        SmtpClient client = new SmtpClient();

        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;

        // setup Smtp authentication
        NetworkCredential credentials = new NetworkCredential("rraprop@gmail.com", "rra12345");
        client.UseDefaultCredentials = false;
        client.Credentials = credentials;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("rraprop@gmail.com");
        msg.To.Add(new MailAddress(EmailTo));
        msg.CC.Add(new MailAddress("rraprop@gmail.com"));
        //msg.Bcc.Add(new MailAddress("uam2014.oc@gmail.com"));
        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = string.Format("<html><head>PropertyMap Feedback</head><body><p>" + Description +
        "<p>" + Usermail + "</p>" +
        "<p><b>" + Name + "<b/></p>" +
        "</body></html>");
        try
        {
            client.Send(msg);
            return "";
        }
        catch (Exception ex)
        {
            return "Error occured while sending your message." + ex.Message;
        }
    }
}


