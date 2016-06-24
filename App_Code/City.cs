using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace PropertyListModel
{
    /// <summary>
    /// Summary description for City
    /// </summary>
    public partial class City
    {
        public City Parent;
        public string FullName;
        public List<City> ChildCityList = new List<City>();
        public List<Society> SocietyList = new List<Society>();
        public List<PriceTrend> PriceTrendList = new List<PriceTrend>();
        public int PriceMin;
        public int PriceMax;
        
        public string Message = "";
        public City()
        {
            ParentID = 0;
            //
            // TODO: Add constructor logic here
            //
        }
        
        public static List<City> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Cities.OrderBy(m => m.SortName).ToList();
            }
        }

        public City Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<City>().Attach(this);
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

        public static City GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Cities.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static City GetByName(int ParentID, string SubCityName)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Cities.FirstOrDefault(m => m.ParentID == ParentID && m.Name == SubCityName);
            }
        }

        public static City GetByName(string CityName)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Cities.FirstOrDefault(m => m.Name == CityName);
            }
        }

        public City Save()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    Boolean IsNew = false;

                    if (ID == 0)
                    {

                        try
                        {
                            ID = 1;
                            ID = context.Cities.Max(m => m.ID) + 1;
                            IsNew = true;
                        }
                        catch { }
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToCities(this);
                    else
                    {
                        context.CreateObjectSet<City>().Attach(this);
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

        public static List<City> GetByParentID(int ParentCityID = 0, Boolean ShowNotVerified = false, Boolean ShowHasPlygon = false)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Cities.Where(m => m.ParentID == ParentCityID
                    && (ShowNotVerified == false || m.Verified == 0 || m.Verified == null)
                    && (ShowHasPlygon == false || (m.PolyPoints != null && m.PolyPoints.Length > 0))).OrderBy(m => m.SortName).ToList();
            }
        }
        
        public static List<City> CityList(System.Web.UI.WebControls.ListControl lst = null, int ParentID = -1, Boolean ShowNotVerified = false, Boolean ShowHasPlygon = false,Boolean OnlysomeCity=false)
        {
            int a = ParentID;
            List<City> AllCities = new List<City>();

            List<City> list = City.GetByParentID(ParentID, ShowNotVerified, ShowHasPlygon);
            AllCities.AddRange(list);

            if (ParentID == -1)
            {
                list = City.GetByParentID(0);
                AllCities.AddRange(list);
            }
            
            foreach (City c in list)
            {
                if (c.CityGroup == 1)
                {
                    List<City> sublist = City.GetByParentID(c.ID, ShowNotVerified, ShowHasPlygon);
                    foreach (City sc in sublist)
                    {
                        sc.Parent = c;
                        //sc.Name += ", " + c.Name;
                    }
                    AllCities.AddRange(sublist);
               }
            }

            if (OnlysomeCity)
            {
                List<int> cityList = new List<int>() { 1, 2, 4, 392, 173, 335, 645};    //include only these cities
                list = AllCities.Where(m=>cityList.Contains(m.ID)).OrderBy(m => m.Name).ToList();
            }
            
            foreach (City C in list)
            {
                if (C.Polygon != null)
                    C.PolyPoints = Cmn.GetUnCompressed(C.Polygon, (int)C.PolygonDataSize);
            }

            if (lst != null)
            {
                lst.Items.Clear();
                lst.Items.Add(new System.Web.UI.WebControls.ListItem("---Select City---", "-1"));

                foreach (City C in list)
                {
                    //if (ShowNotVerified && C.Verified == 1)
                    //    continue;
                    //if (ShowHasPlygon && string.IsNullOrWhiteSpace(C.PolyPoints))
                    //    continue;
                    lst.Items.Add(new System.Web.UI.WebControls.ListItem(C.Name, C.ID.ToString()));
                }
            }
            return list;
         }
         public static void GetByCityIDSubCityID(int cityId, int subCityId)
         {
         }
      }
}