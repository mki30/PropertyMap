using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.Web.Script.Serialization;

public partial class PropList : System.Web.UI.UserControl
{
    City city;
    Agent agent;

    const int AVL_RENT = 0;
    const int AVL_SALE = 1;

    public string MetaDescription = "", MetaKeywords = "";

    public void InitCtlPropList(City _city)
    {
        city = _city;
        MetaDescription = "Loking For New Property? Welcome to our site. Here Find Thousands of Properties ,View Complete information of new Projects-coplete price list,floor plans,layout plans,location in map,property price-trend information graph,calculate emi.";
        MetaKeywords = "india property,property sites,India Real Estate,residential properties in india,residential projects in india,buy property in india,Apartment for Sale,studio apartment,investment in india property," +
        "builder information, apartment list, projects in gurgaon,projects in noida,noida extension projects,noida extension," +
        "projects in noida,projects in ghaziabad,2 bhk flats in noida,2 bhk flats in noida extention,2 bhk flats in gurgaon";

        CreateCityList();
    }
    
    public void InitCtlAgentList(Agent _Agent, int ListType)
    {
        agent = _Agent;
        MetaDescription = "Agent description";
        MetaKeywords = "Places where agent is dealing in";

        ltCity.Text = ListType==AVL_RENT? "For Rent":"For Sale";
        ltCityList.Text += "<li><a href='/" + agent.URLName.ToLower() + "/rent'>For Rent</a></li>";
        ltCityList.Text += "<li><a href='/" + agent.URLName.ToLower() + "/sale'>For Sale</a></li>";
        
        CreateBudgetDropdown(ListType);
        ShowAgentLising(_Agent.ID, ListType);
    }

    void CreateCityList()
    {
        int cityId = 0;
        string[] cityList ={
                            "ghaziabad","Ghaziabad","1",
                            "ghaziabad-indirapuram","Ghaziabad-Indirapuram","2",
                            "noida","Noida","4",
                            "greater-noida","Greater Noida+Noida Extn","392",
                            "gurgaon","Gurgaon","173",
                            "yamuna-expressway","Yamuna Expressway","335",
                           };

        ltCity.Text = Cmn.ProperCase(city.Name);

        for (int i = 0; i < cityList.Length; i += 3)
        {
            ltCityList.Text += "<li><a href='/" + cityList[i] + "'>" + cityList[i + 1] + "</a></li>";
            if (city.UrlName == cityList[i])
                cityId = Cmn.ToInt(cityList[i + 2]);
        }
        
        CreateBudgetDropdown(AVL_SALE);
        ShowAllCities(cityId != 0 ? cityId : 4);
    }

    public void CreateBudgetDropdown(int ListType)
    {
        if (ListType == AVL_RENT)
        {
            ltBudgetList.Text = "<li><a href='#' onclick='return SelectBudget(0,100000000,this)'>All</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(0,5000,this)'>0-5K</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(5000,10000,this)'>5K-10K</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(10001,20000,this)'>10K-20K</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(20001,40000,this)'>20K-40K</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(40001,100000000,this)'>>50K</a></li>";
        }
        else
        {
            ltBudgetList.Text = "<li><a href='#' onclick='return SelectBudget(0,100000,this)'>All</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(20,40,this)'>20-40L</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(41,60,this)'>41-60L</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(61,80,this)'>61-80L</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(81,100,this)'>81-1Crore</a></li>" +
                        "<li><a href='#' onclick='return SelectBudget(100,3000,this)'>1Crore</a></li>";
        }
    }

    int GetQuarter(int month)
    {
        return 1;
        //return Math.Floor(month / 4) + 1;
    }

    public void ShowAgentLising(int AgentID, int AvlType = AVL_SALE)
    {
        List<Society> slist = new List<Society>();// create a temporary list of the societies for agent listing
        List<Availability> avlList = Availability.GetAvlByAgentID(AgentID).Where(m => m.AvailabilityType == AvlType).ToList();

        foreach (Availability avl in avlList)
        {
            if (avl.SocietyID == null || avl.ApartmentTypeID == null)
                continue;

            Society sc = slist.FirstOrDefault(m => m.ID == (int)avl.SocietyID); // check if the project has been added

            if (sc == null && Global.ProjectList.TryGetValue((int)avl.SocietyID, out sc)) // get the project from the global list
            {
                sc = new Society() { ID = sc.ID, SocietyName = sc.SocietyName, URLName = sc.URLName, Subcity = sc.Subcity };// create a new object for the temporary list, do not use the global object for agent listing.
                slist.Add(sc);
            }
            ApartmentType apt;

            if (Global.ApartmentTypeList.TryGetValue((int)avl.ApartmentTypeID, out apt))// if the apartment type exists in the global list
            {
                if (avl.Amount != null)
                {
                    if (agent != null && AvlType == AVL_SALE)
                        avl.Amount = avl.Amount / 100000;

                    AddProperty(sc, apt, (int)avl.Amount);
                }
            }
        }
        ShowList(slist);
    }

    void ShowList(List<Society> slist)
    {
        if (slist.Count == 0)
            return;

        StringBuilder str = new StringBuilder("");
        str.Append("<table class='table table-bordered table-hover table-striped table-condensed ProjectListHome' style='background-color:white'>" +
           "<tr class='skp'><th rowspan='2' style='width:10px'>#</th><th>Project</th><th colspan='2' title='1 BHK'>1 bhk</th>" +
           "<th colspan='2' title='2 BHK'>2 bhk</th><th colspan='2' title='3 BHK'>3 bhk</th><th colspan='2' title='4 BHK'>4 bhk</th><th colspan='2'>Locality</th></tr>");
        str.Append("<tr class='skp'><td></td><td>Area</td><td>Price</td><td>Area</td><td>Price</td><td>Area</td><td>Price</td><td>Area</td><td>Price</td><td></td>");
        ltProjectList.Text = str.ToString();

        int ctr = 0;
        StringBuilder str1 = new StringBuilder("");
        foreach (Society SC in slist)
        {
            ctr++;
            str1.Clear();

            string projUrl = "/" + SC.URLName.ToLower() + (agent != null ? "/" + agent.URLName.ToLower() : "");

            str1.Append("<tr><td>" + ctr + "</td><td><a href='" + projUrl + "' title='" + SC.SocietyName + "'>" + SC.SocietyName + "</a></td>");

            for (int i = 1; i < 5; i++)
            {
                List<ApartmentShortInfo> aptList = SC.ApartmentListShortInfo[i];

                if (aptList == null)
                    str1.Append("<td></td><td></td>");

                else
                {
                    string area = "", price = "";

                    if (aptList.Count > 0)
                    {
                        area = aptList[0].Area.ToString();
                        price = aptList[0].Price.ToString();
                        
                        //if (Action == "agentsite" && Data1 == "rent")
                        //{
                        //    price = (Cmn.ToInt(price) * 100000).ToString();
                        //}
                    }

                    if (aptList.Count > 1)
                    {
                        int n = aptList.Count - 1;

                        if (aptList[n].Area.ToString() != area)
                            area += "-" + aptList[n].Area;

                        if (aptList[n].Price.ToString() != price)
                            price += "-" + aptList[n].Price.ToString();

                        if (price == "0")
                            price = "-";
                    }
                    str1.Append("<td class='area'>" + area + "</td><td class='price'>" + price + "</td>");
                }
            }
            //if (!string.IsNullOrEmpty(SC.Subcity))
            //{
            SC.Subcity = SC.Subcity.Replace("Sector", "Sec").Replace("Expressway", "Exp").Replace("Extension", "Ext").Replace("Extention", "Ext");
            str1.Append("<td><a href='/map/?CityID=" + SC.CityID + "&SubcityID=" + SC.SubCityID + "'>" + (string.IsNullOrEmpty(SC.Subcity) ? "-" : SC.Subcity) + "</a></td>");
            ltProjectList.Text += str1.ToString();
            //}
        }
        str1.Clear();
        str1.Append("</tr></table>");
        ltProjectList.Text += str1.ToString();

        var newList = slist.Select(a => new
        {
            Name = a.SocietyName,
            URLName = a.URLName,
            Apts = a.ApartmentListShortInfo,
            BuilderID = a.BuilderID,
            SubCity = a.Subcity
        }).ToList();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue; //Or any size you want to use, basically int maxValue is 2GB   // to avoid -The length of the string exceeds the value set on the maxJsonLength property exception in Json serialization.
        ltData.Text = "<script> var s=ProjectList=" + serializer.Serialize(newList) + ";</script>";
    }

    void AddProperty(Society sc, ApartmentType apt, int PropertyPrice)
    {
        int BHK = (int)apt.Bedroom;
        if (BHK <= 4)
        {
            if (sc.ApartmentListShortInfo[BHK] == null)
                sc.ApartmentListShortInfo[BHK] = new List<ApartmentShortInfo>();
            sc.ApartmentListShortInfo[BHK].Add(new ApartmentShortInfo() { Price = PropertyPrice, BHK = apt.Bedroom, Area = apt.SuperArea });
        }
    }

    void ShowAllCities(int CityID)
    {
        List<Society> slistTemp = new List<Society>();// create a temporary list of the societies for agent listing

        List<Society> slist = Global.ProjectList.Values.Where(m => m.Verified == 1 && (m.CityID == CityID || m.SubCityID == CityID)).ToList();

        foreach (Society SC in slist)
        {
            List<PriceTrend> ptList = Global.PriceTrendList.Values.Where(m => m.SubCityID == SC.SubCityID && m.Year == DateTime.Now.Year && m.Quarter == GetQuarter(DateTime.Now.Month)).ToList();

            int avgPrice = 0;
            try
            {
                if (ptList.Count > 0)
                    avgPrice = (Cmn.ToInt(ptList.First().Max) + Cmn.ToInt(ptList.First().Min)) / 2;
            }
            catch
            {
            }


            Society scTemp = slistTemp.FirstOrDefault(m => m.ID == (int)SC.ID); // check if the project has been added

            if (scTemp == null && Global.ProjectList.TryGetValue((int)SC.ID, out scTemp)) // get the project from the global list
            {
                scTemp = new Society() { ID = scTemp.ID, SocietyName = scTemp.SocietyName, URLName = scTemp.URLName, Subcity = scTemp.Subcity };// create a new object for the temporary list, do not use the global object for agent listing.
                slistTemp.Add(scTemp);
            }

            List<ApartmentType> list = Global.ApartmentTypeList.Values.Where(m => m.SocietyID == SC.ID).OrderBy(m => m.Bedroom).ThenBy(m => m.SuperArea).ToList();
            foreach (ApartmentType a in list)
            {
                int Price = (int)(a.BSP != null ? a.BSP * a.SuperArea / 100000 : avgPrice * a.SuperArea / 100000);
                AddProperty(scTemp, a, Price);
            }
        }
        ShowList(slistTemp);
    }
}