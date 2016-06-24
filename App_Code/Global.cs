using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using PropertyListModel;


public enum ProjectPriceType
{
    BSP_Fexi_Plan = 0,
    BSP_SP_Plan = 1,
    Location_PLC = 2,
    //Pool_PLC = 3,
    //Road_PLC = 4,
    //Corner_PLC = 5,
    Lease_Rent = 6,
    Club_Membership = 7,
    Fire_Fighting_Charges = 8,
    External_Elictrical_Charges = 9,
    Power_Backup = 10,
    Parking = 11
}

public enum ImagesLocations
{
    demo = 0,
    Projects_Images = 1,
    Project_Layout = 2,
    Project_Logo = 3,
    Apartment_Type = 4
}

public enum SellerType
{
    Agent,
    Owner,
    Builder
}

public enum Amenities : int
{
    Sec = 0,
    PwrBck = 1,
    Lift = 2,
    Swimpool = 3,
    Park = 4,
    Gym = 5,
    Club = 6,
    RWH = 7,
    Vastu = 8,
}

public enum AreaUnit
{
    sqft = 0,
    sqyd = 1,
    sqmt = 2
}

public enum PageType
{
    PAGE_CLIENT_MAIN = 0,
    Agent_Main = 1
}

public enum PropertyTypes
{
    Apartment = 0,
    Duplex = 1,
    Plot = 2,
    Villa = 3,
    Independent_Floor = 4,
    Penthouse = 5
}

public enum Floor
{
    Basement = -1,
    Ground = 0,
    First = 1,
    Second = 2,
    Terace = 3
}

public enum Status
{
    Announced = 1,
    Upcoming = 2,
    Ongoing = 3,
    Complete = 4
}

public class Global
{
    public static Dictionary<int, City> CityList = new Dictionary<int, City>();
    public static Dictionary<int, Society> ProjectList = new Dictionary<int, Society>();
    public static Dictionary<int, Agent> BuilderList = new Dictionary<int, Agent>();
    public static Dictionary<int, Agent> AgentList = new Dictionary<int, Agent>();
    public static Dictionary<int, ApartmentType> ApartmentTypeList = new Dictionary<int, ApartmentType>();
    public static Dictionary<int, PriceTrend> PriceTrendList = new Dictionary<int, PriceTrend>();
    public static Dictionary<int, Availability> AvailabilityList = new Dictionary<int, Availability>();
    public static List<ImagesDetail> ImageDetailsList = new List<ImagesDetail>();

    public Global()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetText(object obj)
    {
        if (obj is ProjectPriceType)
        {
            switch ((ProjectPriceType)obj)
            {
            case ProjectPriceType.BSP_Fexi_Plan: return "Flexi Plan";
            case ProjectPriceType.BSP_SP_Plan: return "SP Plan";
            case ProjectPriceType.Location_PLC: return "Location PLC";
            //case ProjectPriceType.Pool_PLC:return "Pool PLC";
            //case ProjectPriceType.Road_PLC:return "Road PLC";
            //case ProjectPriceType.Corner_PLC:return "Corner PLC";
            case ProjectPriceType.Lease_Rent:return "Lease Rent";
            case ProjectPriceType.Club_Membership:return "Club Membership";
            case ProjectPriceType.Fire_Fighting_Charges:return "Fire Fighting Charges";
            case ProjectPriceType.External_Elictrical_Charges:return "External Elictrical Charges";
            case ProjectPriceType.Power_Backup:return "Power Backup";
            case ProjectPriceType.Parking:return "Parking";
            }
        }

        //else if (obj is ClassType)
        //{
        //    switch ((ClassType)obj)
        //    {
        //        case ClassType.Class2: return "Class 2";
        //        case ClassType.Class3: return "Class 3";
        //        case ClassType.DGFT: return "DGFT";
        //    }
        //}
        return "";
    }

    public static void LoadGlobalData()
    {
        using (PropertyListEntities context = new PropertyListEntities(ConnectionStringEntity))
        {
            CityList.Clear();
            foreach (City C in context.Cities.OrderBy(m => m.Name))
            {
                CityList.Add(C.ID, C);
            }

            foreach (City C in CityList.Values)
            {
                City parent;

                if (C.PolyPoints == "0")
                {
                    C.PolyPoints = Cmn.GetUnCompressed(C.Polygon, (int)C.PolygonDataSize);
                }

                if (CityList.TryGetValue((int)C.ParentID, out parent))
                {
                    C.Parent = parent;
                    if (parent.ID != 0)
                        C.FullName += C.Name + ", " + parent.Name;
                    parent.ChildCityList.Add(C);
                }
            }

            PriceTrendList.Clear();
            foreach (PriceTrend P in context.PriceTrends)
            {
                if (P.Year < 2009)
                    continue;
                PriceTrendList.Add(P.ID, P);

                if (P.SubCityID != null)
                {
                    City C;
                    if (CityList.TryGetValue((int)P.SubCityID, out C))
                    {
                        C.PriceTrendList.Add(P);
                    }
                }
            }

            foreach (City c in CityList.Values)
            {
                PriceTrend pt = c.PriceTrendList.OrderByDescending(m => m.Year).ThenByDescending(m => m.Quarter).FirstOrDefault();
                if (pt != null)
                {
                    c.PriceMin = (int)pt.Min;
                    c.PriceMax = (int)pt.Max;
                }
            }

            AgentList.Clear();
            BuilderList.Clear();
            foreach (Agent A in context.Agents)
            {
                if (A.UserType == 2)
                    BuilderList.Add(A.ID, A);
                else
                    AgentList.Add(A.ID, A);
            }

            ProjectList.Clear();
            foreach (Society S in context.Societies.OrderBy(m => m.SocietyName))
            {
                ProjectList.Add(S.ID, S);
                S.SearchName = S.SocietyName;
                City C;

                S.SearchName += ",";
                if (S.SubCityID != null)
                    if (CityList.TryGetValue((int)S.SubCityID, out C))
                    {
                        C.SocietyList.Add(S);
                        S.SearchName += C.Name;
                    }

                S.SearchName += ",";
                if (S.CityID != null)
                    if (CityList.TryGetValue((int)S.CityID, out C))
                    {
                        C.SocietyList.Add(S);
                        S.SearchName += C.Name;
                    }

                if (S.AreaID != null)
                    if (CityList.TryGetValue((int)S.AreaID, out C))
                    {
                        C.SocietyList.Add(S);
                    }

                if (S.BuilderID != null)
                {
                    Agent A;
                    if (BuilderList.TryGetValue((int)S.BuilderID, out A))
                    {
                        S.Builder = A;
                        A.ProjectList.Add(S);
                    }
                }
            }

            ApartmentTypeList.Clear();
            foreach (ApartmentType A in context.ApartmentTypes)
            {
                Society S;
                if (ProjectList.TryGetValue((int)A.SocietyID, out S))
                {
                    A.Parent = S;
                    int n = (int)A.Bedroom;
                    S.ApartmentList.Add(A);
                }
                ApartmentTypeList.Add(A.ID, A);
            }

            foreach (FloorInfo FI in context.FloorInfoes)
            {
                ApartmentType A;
                if (ApartmentTypeList.TryGetValue((int)FI.AptTypeID, out A))
                {
                    A.FloorInfoList.Add(FI);
                }
            }

            AvailabilityList.Clear();
            foreach (Availability A in context.Availabilities)
            {
                AvailabilityList.Add(A.ID, A);

                if (A.SocietyID != null)
                {
                    Society S;
                    if (ProjectList.TryGetValue((int)A.SocietyID, out S))
                    {
                        A.Society = S;
                    }
                }
            }

            ImageDetailsList.Clear();
            List<ImagesDetail> ids = ImagesDetail.GetList();
            ImageDetailsList = ids;
        }
    }

    public static string BlogID { get; set; }
    public static void LogoutReset()
    {
        Global.UserType = -1;
        Global.UserName = "";
        Global.UserID = 0;
        Global.LogInDone = false;
    }

    public static string AppTitle
    {
        get
        {
            return " - PropertyMap.info";
        }
    }

    public static string ConnectionStringEntity
    {
        get
        {
            return @"metadata=res://*/App_Code.PropertyList.csdl|res://*/App_Code.PropertyList.ssdl|res://*/App_Code.PropertyList.msl;provider=System.Data.SqlServerCe.4.0;provider connection string=""Data Source=|DataDirectory|\PropertyList.sdf;""";
        }
    }

    public static string PropertyMapConnnection
    {
        get
        {
            return @"Data Source=|DataDirectory|\PropertyList.sdf";
        }
    }

    public static string BlogConnection
    {
        get
        {
            return @"Data Source=|DataDirectory|\Blog.sdf";
        }
    }

    public static Boolean LogInDone
    {
        get
        {
            if (HttpContext.Current.Session["LOGINDONE"] == null)
                return false;
            else
                return HttpContext.Current.Session["LOGINDONE"].ToString() == "1" ? true : false;
        }
        set
        {
            HttpContext.Current.Session["LOGINDONE"] = value ? "1" : "0";
        }
    }

    public static string UserName
    {
        get
        {
            if (HttpContext.Current.Session["USERNAME"] == null)
                return "";
            else
                return HttpContext.Current.Session["USERNAME"].ToString();
        }

        set
        {
            HttpContext.Current.Session["USERNAME"] = value;
        }
    }

    public static string MenuPDFData
    {
        get
        {
            if (HttpContext.Current.Session["MENUPDFDATA"] == null)
                return "";
            else
                return HttpContext.Current.Session["MENUPDFDATA"].ToString();
        }

        set
        {
            HttpContext.Current.Session["MENUPDFDATA"] = value;
        }
    }


    public static int UserType
    {
        get
        {
            if (HttpContext.Current.Session["USERTYPE"] == null)
                return -1;
            else
                return Cmn.ToInt(HttpContext.Current.Session["USERTYPE"]);
        }
        set
        {
            HttpContext.Current.Session["USERTYPE"] = value;
        }
    }

    public static int UserID
    {
        get
        {
            if (HttpContext.Current.Session["USERID"] == null)
                return -1;
            else
                return Cmn.ToInt(HttpContext.Current.Session["USERID"]);
        }
        set
        {
            HttpContext.Current.Session["USERID"] = value;
        }
    }

    public static string Culture
    {
        get
        {
            if (HttpContext.Current.Session["Culture"] == null)
                return "en-US";
            else
                return HttpContext.Current.Session["Culture"].ToString();
        }

        set
        {
            HttpContext.Current.Session["Culture"] = value;
        }
    }

    public static string FromPage
    {
        get
        {
            if (HttpContext.Current.Session["FROMPAGE"] == null)
                return "";
            else
                return HttpContext.Current.Session["FROMPAGE"].ToString();
        }
        set
        {
            HttpContext.Current.Session["FROMPAGE"] = value;
        }
    }

    public static string UserEmailID
    {
        get
        {
            if (HttpContext.Current.Session["UserEmailID"] == null)
                return "";
            else
                return HttpContext.Current.Session["UserEmailID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["UserEmailID"] = value;
        }
    }

    public static Boolean IsAdmin
    {
        get
        {
            if (HttpContext.Current.Session["ISADMIN"] == null)
                return false;
            else
                return HttpContext.Current.Session["ISADMIN"].ToString() == "1" ? true : false;
        }
        set
        {
            HttpContext.Current.Session["ISADMIN"] = value ? "1" : "0";
        }
    }

    public static string GetRootPathVirtual       //get vertual path
    {
        get
        {
            string host = (HttpContext.Current.Request.Url.IsDefaultPort) ?
            HttpContext.Current.Request.Url.Host :
            HttpContext.Current.Request.Url.Authority;
            host = String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, host);
            if (HttpContext.Current.Request.ApplicationPath == "/")
                return host;
            else
                return host + HttpContext.Current.Request.ApplicationPath;
        }
    }

    public static void GCCollect()
    {
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
    }

    public static string Captcha
    {
        get
        {
            if (HttpContext.Current.Session["Captcha"] == null)
                return "";
            else
                return (string)HttpContext.Current.Session["Captcha"];
        }
        set
        {
            HttpContext.Current.Session["Captcha"] = value;
        }
    }

    public static string[] ImagesRootLocation = new[] { "data/demo", "data/images_society", "data/images_layoutplan", "data/images_societylogo", "data/images_apartmenttype" };

    public static string GetUniqueURL(Society S, string urlName, string appendText)
    {
        string url = urlName;
        if (Global.ImageDetailsList.FirstOrDefault(m => m.UrlName == urlName) != null || string.IsNullOrWhiteSpace(urlName))
            url = (S.URLName + (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
        return url;
    }

    

}
