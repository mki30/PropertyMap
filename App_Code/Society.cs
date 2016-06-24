using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;
using System.IO;
using KMLGenerator;

namespace PropertyListModel
{
    public partial class Society
    {
        public List<ApartmentShortInfo>[] ApartmentListShortInfo = new List<ApartmentShortInfo>[7];
        public List<ApartmentType> ApartmentList = new List<ApartmentType>();
        public Agent Builder;
        public List<Agent> AgentList = new List<Agent>();
        public string SearchName = "";

        public string Message = "";

        public Society()
        {
            BuilderID = 0;
        }

        public string CreateKML()
        {
            string folder = HttpContext.Current.Server.MapPath(@"~/Data/KML/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string FileName = folder + SocietyName + ".kml";

            if (Lat == null || Lng == null)
                return "";

            Generator kml = new Generator();                            //generate kml usingkml gen

            if (!string.IsNullOrWhiteSpace(PolyPoints))
            {
                string[] pts = PolyPoints.Trim('^').Split('^');
                foreach (string p in pts)
                {
                    string[] pt = p.Split(',');
                    kml.AddPathPoint(Cmn.ToDbl(pt[0]), Cmn.ToDbl(pt[1]), 0);
                }

                if (pts.Length > 0)
                {
                    string[] pt = pts[0].Split(',');
                    kml.AddPathPoint(Cmn.ToDbl(pt[0]), Cmn.ToDbl(pt[1]), 0);
                }
            }

            kml.AddSight(SocietyName, (double)Lat, (double)Lng, 0, 500);
            kml.WriteKMLFile(FileName);

            //return Global.GetRootPathVirtual + @"/Data/KML/" + SocietyName + ".kml";
            return @"~/Data/KML/" + SocietyName + ".kml";
        }

        public static Society GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static Society GetByURLName(string URLName)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.FirstOrDefault(m => m.URLName == URLName);
            }
        }

        public static List<Society> GetBySubCity(int SubCityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.SubCityID == SubCityID).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> GetByCity(int CityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.CityID == CityID).OrderBy(m => m.SocietyName).ToList();
            }
        }
        
        public static List<Society> GetByArea(int AreaID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.AreaID == AreaID).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> GetByBuilder(int BuilderID, Boolean RemoveDeleted = true)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.BuilderID == BuilderID && m.Deleted != 1).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static Society GetByName(string Name)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.FirstOrDefault(m => m.SocietyName == Name);
            }
        }

        public static Society GetByName(string Name, int CityID, int SubCityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.FirstOrDefault(m => m.SocietyName == Name && m.CityID == CityID && m.SubCityID == SubCityID);
            }
        }

        public static List<ApartmentType> GetCompareList(List<int> AppartmentTypeIDList)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var d = context.ApartmentTypes.Where(m => AppartmentTypeIDList.Contains(m.ID)).ToList();

                return d.Join(context.Societies,
                        a => a.SocietyID,
                        s => s.ID,
                        (a, s) => new ApartmentType()
                        {
                            ID = a.ID,
                            TypeName = a.TypeName,
                            SocietyID = s.ID,
                            SocietyName = s.SocietyName,
                            Bedroom = a.Bedroom,
                            SuperArea = a.SuperArea,
                            Bathroom = a.Bathroom,
                            Balcony = a.Balcony
                       }).ToList();
            }
        }

        

        public static List< string> GetSocietyImageList(int dataId)     //Society Image List to be used while creating PDF
        {
            List<string> ImagesList = new List<string>();
            var path = "/Data/Watermark/Images_Society/";
            string[] imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + "_*.*");
            if (imageFiles.Length == 0)
            {
                path = "/Data/Images_Society/";
                imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + "_*.*");
            }
            foreach (var image in imageFiles)
            {
                ImagesList.Add(HttpContext.Current.Server.MapPath(path + Path.GetFileName(image)));
            }
            return ImagesList;
        }

       
        public static string GetSocietyImage(int dataId,string urlName)     //Society Image
        {
            List<ImagesDetail> imgDetails = Global.ImageDetailsList.Where(m => m.ReferenceID == dataId && m.ImageReferenceType == (int)ImagesLocations.Projects_Images).ToList();
            var img = "";
            var path = "/Data/Watermark/Images_Society/";
            List<string> imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + "_*.*").ToList();
            if (imageFiles.Count == 0)
            {
                if (imgDetails != null)
                {
                    path = "/Data/Images_Society/";
                    foreach (ImagesDetail id in imgDetails)
                    {
                        if(File.Exists(HttpContext.Current.Server.MapPath(path)+id.UrlName+".jpg"))
                        {
                            imageFiles.Add(HttpContext.Current.Server.MapPath(path) + id.UrlName + ".jpg");
                        }
                    }
                }
            }

            int ctr = 1;
            var isfirst = true;
            foreach (var image in imageFiles)
            {
                if (ctr == 1 && isfirst)
                {
                    img += "<div class='row-fluid' id='gallery'>";
                    isfirst = false;
                }
                string imgAlt = urlName.Replace("-", " ") + " " + Path.GetFileNameWithoutExtension(image);

                string pathLargeImg = path.Replace("Images_Society", "Images_Society_Large");
                string hrefProjectImgLarge = path + Path.GetFileName(image);
                if (File.Exists(HttpContext.Current.Server.MapPath(pathLargeImg + Path.GetFileName(image))))
                    hrefProjectImgLarge = pathLargeImg+Path.GetFileName(image);
                img += "<div class='span6' style='margin-top:5px;'><a href='" + hrefProjectImgLarge + "' target='_blank'><img class='img-polaroid' style='border:1px solid #eee; width:98%; height:230px;' src='" + path + Path.GetFileName(image) + "' alt='" + imgAlt + "'/><a></div>";

                if (ctr == 2)
                {
                    img += "</div>";
                    ctr = 1;
                    isfirst = true;
                }
                if (ctr == 1 && isfirst != true)
                {
                    ctr = 2;
                }
            }
            return img;
        }
        
        //public string GetLayoutImage(int dataId)
        //{
        //    string[] imageFiles = null;
        //    string path = "~/Data/Watermark/Images_LayoutPlan/";
        //    imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        //    if (imageFiles.Length == 0)
        //    {
        //        path = "~/Data/Images_LayoutPlan/";
        //        imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        //    }
        //    return imageFiles.Length > 0 ? "<span style='margin:0px 3px 0px 3px;'><img id='layout' class='img-polaroid' style='border:1px solid #eee; width:98.5%;'  src='" + ResolveClientUrl(path) + Path.GetFileName(imageFiles[0]) + "'  alt='image'/></span>" : "";
        //}

        public List<string[]> GetAptTypeImage()
        {
            List<string[]> Typtable = new List<string[]>();
            foreach (ApartmentType A in ApartmentList.OrderBy(m => m.PropertyType).ThenBy(m => m.TypeName))
            {
                string area = A.SuperArea != 0 ? A.SuperArea.ToString() : (A.PlotArea != 0 ? A.PlotArea.ToString() + "" : "");
                string typeName = A.TypeName;
                string name = typeName +" ("+ area+")";
                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("/Data/Images_ApartmentType/" + A.ID + ".jpg")))
                {
                 Typtable.Add(new string[] { A.ID.ToString(), name});
                }
            }
            return Typtable;
        }

        public List<string[]> GetAptDetailsPDF()
        {
            List<string[]> tableData=new List<string[]>();
            tableData.Add(new string[]{"Name","Type","Bed","Bath","Area","BSP","Price"});
            string unit = "";
            foreach (ApartmentType A in ApartmentList.OrderBy(m => m.PropertyType).ThenBy(m => m.TypeName))
            {

                if (A.BSP == null)
                    A.BSP = 0;

                string area = A.SuperArea != 0 ? A.SuperArea.ToString() : (A.PlotArea != 0 ? A.PlotArea.ToString() + "" : "");
                string bsp = A.BSP == 0 ? "-" : A.BSP.ToString();
                string price = "";

                if (A.BSP != 0)
                {
                    float Area = (float)A.SuperArea;       //When SuperArea is not Available
                    if (A.SuperArea == null || A.SuperArea == 0)
                        if (A.PlotArea != null)
                            Area = (float)A.PlotArea;

                    price = ((int)A.BSP * Area / 100000).ToString("0") + " L";

                    string temp = price.Replace("L", "").Trim();
                    int z = (int)Cmn.ToDbl(temp) / 100;
                    if (((int)Cmn.ToDbl(temp) / 100 > 0))
                    {
                        price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
                    }
                }
                unit = ((AreaUnit)(A.Unit != null ? A.Unit : 0)).ToString();
                string propertyType=(A.PropertyType != null ? ((PropertyTypes)A.PropertyType).ToString().Replace("_", " ") : "Apartment");
                tableData.Add(new String[]{A.TypeName,propertyType,A.Bedroom.ToString(),A.Bathroom.ToString(),area,bsp,price});
            }
            return tableData;
        }

        public string GetAptDetails(Boolean WithSocietyName = false)
        {
            StringBuilder str = new StringBuilder();

            //if (ApartmentList.Count > 0)
            //    str.Append("<table class='table table-bordered table-hover table-striped table-condensed' style='margin-bottom:0px;'><tr><th>Name</th><th>Type</th><th>Bed</th><th>Bath</th><th>Area</span></th><th title='Basic Selling Price'>BSP</th><th title='Basic Price in Lakhs'>Price<th>Plan</th></tr>");
            
            string unit = "";
            foreach (ApartmentType A in ApartmentList.OrderBy(m => m.PropertyType).ThenBy(m => m.TypeName))
            {
                if (A.BSP == null)
                    A.BSP = 0;

                string area = A.SuperArea != 0 ? A.SuperArea.ToString() : (A.PlotArea != 0 ? A.PlotArea.ToString() + "" : "");
                string bsp = A.BSP == 0 ? "-" : A.BSP.ToString();
                string price = "";

                if (A.BSP != 0)
                {
                    float Area = (float)A.SuperArea;       //When SuperArea is not Available
                    if (A.SuperArea == null || A.SuperArea == 0)
                        if (A.PlotArea != null)
                            Area = (float)A.PlotArea;

                    price = ((int)A.BSP * Area / 100000).ToString("0") + " L";

                    string temp = price.Replace("L", "").Trim();
                    int z = (int)Cmn.ToDbl(temp) / 100;
                    if (((int)Cmn.ToDbl(temp) / 100 > 0))
                    {
                        price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
                    }
                }

                unit = ((AreaUnit)(A.Unit != null ? A.Unit : 0)).ToString();

                str.Append("<tr><td>" + A.TypeName + "</td><td>" + (A.PropertyType != null ? ((PropertyTypes)A.PropertyType).ToString().Replace("_", " ") : "Apartment") + "</td>"
                    + "<td>" + A.Bedroom + "</td>"
                    + "<td>" + A.Bathroom + "</td>"
                    + "<td style='text-align:center;'>" + area + "</td>"//(A.Unit!=0?(" "+(AreaUnit)(A.Unit!=null?A.Unit:0)):"")+ "</td>"
                    + "<td>" + bsp + "</td>"
                    + "<td>" + (price == "" ? "-" : price)
                    + "</td>");
                //<td><a href='#divForm' class='fancyBox' onclick='FillFields("+A.BSP+","+A.SuperArea+")'><img style='height:16px;'  src='" + Global.GetRootPathVirtual + "/Images/calculator-icon.png' title='Calculate'/></a></td>");

                string AptimgPath = @"/Data/Images_ApartmentType/";
                string AptimgPathWater = @"/Data/Watermark/Images_ApartmentType/";

                ImagesDetail imgLayout = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == A.ID && m.ImageReferenceType == (int)ImagesLocations.Apartment_Type);
                str.Append("<td>");
                
                //string[] Files = Directory.GetFiles(HttpContext.Current.Server.MapPath(AptimgPathWater), A.ID + ".*");
                //if (Files.Length == 0)
                //    Files = Directory.GetFiles(HttpContext.Current.Server.MapPath(AptimgPath), A.ID + ".*");
                //str.Append("<td>");

                //string anchor = "<a class='fancyBox' rel='group'  href='" + Global.GetRootPathVirtual + AptimgPath;
                //string anchor = "<a  href='" + Global.GetRootPathVirtual + AptimgPath;
                string anchor = "<a  href='" + Global.GetRootPathVirtual;

                if (A.FloorInfoList.Count == 0)
                {
                    //if (Files.Length > 0)
                    //    str.Append(anchor + Path.GetFileName(Files[0]) + "' title='" + A.Bedroom + "B-" + A.Bathroom + "T (" + A.SuperArea + " sqft.) flat' data-alt='alt for fancybox one'>"+
                    //        "<img alt='FP' src='" + Global.GetRootPathVirtual + "/Images/fp-an.gif' title='Floor Plan'/></a>");
                    //else
                    //    str.Append("  -</td>");

                    if (imgLayout!=null)
                        //str.Append(anchor + imgLayout.UrlName + ".jpg' title='" + A.Bedroom + "B-" + A.Bathroom + "T (" + A.SuperArea + " sqft.) flat' data-alt='alt for fancybox one'>" +
                        //    "<img alt='FP' src='" + Global.GetRootPathVirtual + "/Images/fp-an.gif' title='Floor Plan'/></a>");
                        str.Append(anchor +"/"+ this.URLName.ToLower() + "/floor-plans#"+A.ID+"' target='_blank' title='" + A.Bedroom + "B-" + A.Bathroom + "T (" + A.SuperArea + " sqft.) flat'>view</a>");
                    else
                        str.Append("  -</td>");
                }
                else
                {
                    foreach (FloorInfo FI in A.FloorInfoList)
                    {
                        str.Append(anchor + A.ID + "_" + FI.FloorNo + ".jpg' title='" + FI.FloorName + "'>" + FI.FloorName + (FI.FloorName == null ? "-" + FI.FloorNo : "") + "</a> &nbsp;&nbsp;");
                    }
                }
                str.Append("</td></tr>");
            }

            StringBuilder str1 = new StringBuilder();
            if (ApartmentList.Count > 0)
            {
                str1.Append("<h2>Units</h2>");
                str1.Append("<table class='table table-bordered table-hover table-striped table-condensed' style='margin-bottom:0px;' id='aptTypeTable'><tr><th>Name</th><th>Type</th><th>Bed</th><th>Bath</th><th style='width:100px;text-align:center;'>Area (" + unit.Trim() + ")</th><th title='Basic Selling Price'>BSP</th><th title='Basic Price in Lakhs'>Price<th>Plan</th></tr>");
                str1.Append(str.ToString());
                str1.Append("</table>");
            }
            //if (ApartmentList.Count >0)
            //    str.Append("</table>");
            return str1.ToString();
        }

        public static IEnumerable<dynamic> GetCompareList(int SocietyID, int AppartmentTypeID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.Where(m => m.SocietyID == SocietyID && m.ID == AppartmentTypeID)
               .DefaultIfEmpty()
               .Join(context.Societies,
                     a => a.SocietyID,
                     s => s.ID,
                     (a, s) => new
                     {
                         //SocityID = s.ID,
                         SocietyName = s.SocietyName,
                         Bedroom = a.Bedroom,
                         SuperArea = a.SuperArea,
                         Bathroom = a.Bathroom,
                         Balcony = a.Balcony
                     }).ToList();
            }
        }

        public static IEnumerable<dynamic> GetCityList()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Select(i => new { i.City, i.Subcity }).Distinct().OrderByDescending(i => i.City).ToArray();
            }
        }

        public static List<Society> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> GetAllVerified()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.Verified == 1).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> GetIfLocAvl()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.Lat != null && m.Lat != 0).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> Get(string City, string SubCity)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.City == City & m.Subcity == SubCity).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static List<Society> GetAvailable()
        {
            List<int?> AvailableSocietyList = Availability.GetAvailableSocietyList();

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(x => AvailableSocietyList.Contains(x.ID)).OrderBy(m => m.SocietyName).ToList();
            }
        }

        public static int GetPropertyIDByName(string Name)
        {
            int ID = 0;
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                ID = context.Societies.Where(m => m.SocietyName.Contains(Name)).Select(x => x.ID).Single();
            }
            return ID;
        }

        public Society Save()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {

                    Society tempSociety = context.Societies.FirstOrDefault(m => m.ID == ID);

                    Boolean IsNew = tempSociety == null;

                    if (IsNew)
                    {
                        ID = 1;
                        try
                        {
                            ID = context.Societies.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToSocieties(this);
                    else
                    {
                        if (tempSociety != null)
                            context.CreateObjectSet<Society>().Detach(tempSociety);
                        context.CreateObjectSet<Society>().Attach(this);
                        context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message;
            }
            return this;
        }
        public Society Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<Society>().Attach(this);
                    context.DeleteObject(this);
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                Message += ex.Message;
            }
            return this;
        }

        public static void GetList(ListControl dd, int SubCityID)     //fill Society List 
        {
            dd.Items.Clear();
            List<Society> SocietyList = GetBySubCity(SubCityID);

            foreach (Society S in SocietyList)
            {
                dd.Items.Add(new ListItem(S.SocietyName, S.ID.ToString()));
            }
        }

        public static void GetSocietyList(DropDownList dd)           //fill Society DropDown
        {
            dd.Items.Clear();
            List<Society> SocietyList = GetAll();

            foreach (Society S in SocietyList)
            {
                dd.Items.Add(new ListItem(S.SocietyName, S.ID.ToString()));
            }
        }

        public static void GetLocality(DropDownList ddlLocality)
        {
            DatabaseCE db = new DatabaseCE();
            string Error = "";
            try
            {
                ddlLocality.Items.Clear();

                DataSet ds = db.GetDataSet("Select distinct Subcity from Society", ref Error);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Subcity"].ToString() != "")
                        ddlLocality.Items.Add(new ListItem(dr["Subcity"].ToString()));
                }
            }

            catch (Exception)
            {
            }

            finally
            {
                db.Close();
            }
        }

        public static List<string> GetLocalityNames()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Select(m => m.Subcity).Distinct().ToList();
            }
        }

        public string ProjectDetailMobile()
        {
            StringBuilder str = new StringBuilder();
            str.Append("<a href='#' class='ui-btn'><img src='/Data/Images_societyLogo/" + ID + ".jpg' style='width:8em;'><h1 id='Soc_Name'>" + SocietyName + "</h1>");
            str.Append("<p><span id='Soc_SubCity'>" + (string.IsNullOrEmpty(Subcity) ? "" : Subcity + ",") + "</span>" +
                "<span id='Soc_City'>" + City + ", " + State + (!string.IsNullOrEmpty(Pin) ? ("-" + Pin) : ("")) + "</span></p></a>");
            string aptTable = GetApartmentTable(ID);
            str.Append("<div>" + aptTable + "</div>");
            str.Append("<p style='text-align:justify;padding:1em;text-indent:2em;'>" + Description + "</p>");
            str.Append("<a id='btnFav' class='ui-btn' onclick='AddFavouriteProjects(\"" + ID + "\",\"" + SocietyName + "\",\"" + URLName + "\",\"" + City + "\",\"" + Subcity + "\")' href='#'>Add To Fav</a>");
            str.Append("<a class='ui-btn ui-shadow ui-corner-all ui-btn-icon-right ui-icon-action' target='_blank' href=" + URL + ">Official Website</a>" +
            "<a class='ui-btn ui-btn-icon-right ui-icon-location' href='https://maps.google.com/maps?q=" + Lat + "," + Lng + "'>Map</a>" +
            "<a class='ui-btn ui-shadow ui-corner-all ui-btn-icon-right ui-icon-user' id='BuilderID' onclick='RunCommand(CMD_FindBuilderDetail)' href='/"+Builder.URLName.ToLower()+"' data-BuilderID='" + Builder.ID + "' data-ajax='false'>" + Builder.AgentCompany + "</a>");
            //str.Append("<div id='Gallery'>");
            //str.Append("<a class='Swipe' rel='external' href='/Data/Images_societyLogo/" + ID + ".jpg'><img src='/Data/Images_societyLogo/" + ID + ".jpg' alt='Image 01' /></a></div>");
            return str.ToString();
        }
        
        string GetApartmentTable(int Id)    //Apatrment table for project detail page
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<table style='width:100%;'  class='ui-body-d ui-shadow table-stripe'><tr><th>Type</th><th>Area</th><th>BSP</th><th>Price</th></tr>");
                List<ApartmentType> list = Global.ApartmentTypeList.Values.Where(m => m.SocietyID == Id).OrderBy(m => m.Bedroom).ThenBy(m => m.SuperArea).ToList();
                if (list.Count == 0)
                    return "";
                foreach (ApartmentType a in list)
                {
                    string price = "";
                    string BSP = "";
                    string Area = "-";
                    if ((a.SuperArea != null || a.SuperArea != 0))
                        Area = a.SuperArea.ToString();
                    if (a.BSP != null && a.SuperArea != null)
                    {
                        price = ConvertPrice((((int)a.BSP) * ((int)a.SuperArea)).ToString());
                        BSP = a.BSP.ToString();
                    }
                    else
                    {
                        price = "-";
                        BSP = "-";
                    }
                    sb.Append("<tr style='text-align:center;'><td>" + a.Bedroom + "B-" + a.Bathroom + "T </td><td>" + Area + "</td><td>" + BSP + "</td><td>" + price + "</td></tr>");
                }
                sb.Append("</table>");
                //sb.Append("<div id='Gallery'>");
                //sb.Append("<a class='Swipe' href='/Data/Images_societyLogo/" + Id + ".jpg'><img src='/Data/Images_societyLogo/" + Id + ".jpg' alt='Image 01'></a></div>");
            }
            catch
            {
            }
            return sb.ToString();
        }

        string ConvertPrice(string price)      //Convert price to Lacks and Crores 
        {
            price = (Cmn.ToInt(price) / 100000).ToString("0") + " L";

            string temp = price.Replace("L", "").Trim();
            int z = (int)Cmn.ToDbl(temp) / 100;
            if (((int)Cmn.ToDbl(temp) / 100 > 0))
            {
                price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
            }
            return price;
        }

        public static List<Society> GetByCityandSubCity(int cityid, int subcityid)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Societies.Where(m => m.CityID == cityid&&m.SubCityID==subcityid&& m.Verified==1).ToList();
                //return context.Societies.Where(m => m.CityID == subcityid).ToList();
            }
        }
    }
}