using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace PropertyListModel
{
    public partial class Distance
    {
        public string Message = "";
        public Distance()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Distance Load()
        {
            Distance D = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                D = context.Distances.FirstOrDefault(m => m.ID == ID);
            }

            return D;
        }

        public Distance Save()
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
                            ID = context.Distances.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToDistances(this);
                    else
                    {
                        context.CreateObjectSet<Distance>().Attach(this);
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

        public static List<Distance> GetAllDistances()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Distances.OrderBy(m => m.ID).ToList();
            }
        }

        public static List<Distance> GetDistanceBySocIDandLandMrkID(int SocietyID,int LandMrkID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Distances.Where(m=>m.SocietyID==SocietyID && m.LandmarkID==LandMrkID).Take(1).ToList();
            }
        }

        public static int GetRoadDistance(int SocietyID, int LandmarkID)
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    return Cmn.ToInt(context.Distances.FirstOrDefault(m => m.SocietyID == SocietyID && m.LandmarkID == LandmarkID).RoadDistance);
                }
            }
            catch
            {
                return 0;
            }
        }
    }

}