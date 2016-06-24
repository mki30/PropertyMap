using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Web.Script.Serialization;

namespace PropertyListModel
{
    public partial class PriceTrend
    {
        public string Message = "";
        public PriceTrend()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public PriceTrend Load()
        {
            PriceTrend B = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                B = context.PriceTrends.FirstOrDefault(m => m.ID == ID);
            }

            return B;
        }

        public PriceTrend Save()
        {

            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    Boolean IsNew = false;

                    if (ID == 0)
                    {
                        ID = 1;
                        try
                        {
                            ID = context.PriceTrends.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToPriceTrends(this);
                    else
                    {
                        context.CreateObjectSet<PriceTrend>().Attach(this);
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

        public PriceTrend Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<PriceTrend>().Attach(this);
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
        
        public static PriceTrend GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<PriceTrend> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.ToList();
            }
        }

        public static List<PriceTrend> GetAll(string SubCity)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.Where(m => m.Subcity == SubCity).ToList();
            }
        }
        
        public static List<CustomPriceTrend> GetAllSubityInfo(int cityId)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                //PriceTrend PT = new PriceTrend();
                var newList = context.PriceTrends.Where(m => m.CityID == cityId && m.Year == DateTime.Today.Year).GroupBy(row => new { Subcity = row.Subcity })
                   .Select(g => new CustomPriceTrend()
                   {
                       SubCity = g.Key.Subcity,
                       MaxRate = (int)(g.Max(i => i.Max)),
                       MinRate = (int)(g.Min(i => i.Min)),
                       SubCityID = g.Max(i => i.SubCityID),
                   }).ToList();
                return newList;
            }
        }
        
        public class CustomPriceTrend
        {
            public string SubCity { get; set;}
            public int MaxRate { get; set; }
            public int MinRate { get; set; }
            public int? SubCityID { get; set;}
        }
        
        public static int GetIdByYearandMonth(string city, string subcity, string year, string month)
        {
            int id = 0;
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var record = context.PriceTrends.AsEnumerable().FirstOrDefault(m => m.PriceDate.Value.Year.ToString() == year && m.PriceDate.Value.Month.ToString() == month && m.City == city && m.Subcity == subcity);
                if (record != null)
                    id = Cmn.ToInt(record.ID);
            }
            return id;
        }

        //public static void GetSubcity(string city)
        public static List<string> GetSubcity(string city)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.Select(m => m.Subcity).Distinct().ToList();
                //List<PriceTrend> PT = context.PriceTrends.GroupBy(x => x.Subcity).Seect(grp => grp.FirstOrDefault()).Where(b => b.City == city).ToList();
            }
        }

        public static List<string> GetAllCity()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.Select(m => m.City).Distinct().ToList();

            }
        }

        public static PriceTrend GetBySubCityQuarterYear(string City ,string SubCity, int Quarter, int Year)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.FirstOrDefault(m => m.City == City && m.Subcity == SubCity && m.Quarter == Quarter && m.Year == Year);
            }
        }

        public static void SubcityList(ListBox lstSubCity)
        {
            List<PriceTrend> list = PriceTrend.GetByCity("Noida");

            if (list != null)
            {
                lstSubCity.Items.Clear();
                foreach (PriceTrend PT in list)
                {
                    lstSubCity.Items.Add(new System.Web.UI.WebControls.ListItem(PT.Subcity, PT.CityID.ToString()));
                }
            }  
        }
        
        public static List<PriceTrend> GetByCity(string  city)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
               return context.PriceTrends.GroupBy(x => x.Subcity).Select(grp => grp.FirstOrDefault()).Where(b=>b.City==city).ToList();
            }
        }
       
        public static List<PriceTrend> GetBySubCityID(int SubCityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceTrends.Where(m => m.SubCityID == SubCityID).OrderByDescending(m => m.Year).ThenByDescending(m => m.Quarter).ToList();
            }
        }
    }
}