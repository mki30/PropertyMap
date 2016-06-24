using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
namespace PropertyListModel
{
    public partial class Landmark
    {
        public string Message = "";
        public double Distance = 0;
        public Landmark()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Landmark GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Landmarks.FirstOrDefault(m => m.ID == ID);
            }

        }
        
        public static List<Landmark> GetByDistance(double lat,double Lng,int Distance)
        {
            List<Landmark> LandmarkList=new List<Landmark>();

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                foreach (Landmark L in context.Landmarks)
                {
                    LandmarkList.Add(L);
                    L.Distance = Cmn.CalcDistance(lat, Lng, (double)L.Lat, (double)L.Lng);
                }
            }
           return LandmarkList.Where(m=>m.Distance<Distance).OrderBy(m=>m.LandmarkType).ThenBy(m => m.Distance).ToList();
        }

        public Landmark Save()
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
                            ID = context.Landmarks.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;

                    if (IsNew)
                        context.AddToLandmarks(this);
                    else
                    {
                        context.CreateObjectSet<Landmark>().Attach(this);
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

        public string GetByName(string LandmName)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Landmarks.FirstOrDefault(m => m.LandmarkName == LandmName).ToString();
            }
        }

        public static List<Landmark> GetAll(string LandmarkType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Landmarks.Where(m=>m.LandmarkType==LandmarkType).OrderBy(m => m.LandmarkType).ToList();
            }
        }
    }
}