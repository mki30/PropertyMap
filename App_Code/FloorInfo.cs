using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace PropertyListModel
{
    /// <summary>
    /// Summary description for FloorInfo
    /// </summary>
    public partial class FloorInfo
    {
        string Message = "";
        public FloorInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static FloorInfo GetByAptIDandFloorNo(int AptID, int FloorNo)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.FloorInfoes.FirstOrDefault(m => m.AptTypeID == AptID && m.FloorNo == FloorNo);
            }
        }

        public static Dictionary<string, string> GetFloorList(PropertyTypes PropertyTypes)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            switch (PropertyTypes)
            {
                case PropertyTypes.Apartment:
                    list.Add("Apartment", "");
                    break;
                case PropertyTypes.Duplex:
                    list.Add("Ground", "0");
                    list.Add("First", "1");
                    break;
                case PropertyTypes.Plot:
                    list.Add("Plot", "");
                    break;
                case PropertyTypes.Villa:
                    list.Add("Basement", "-1");
                    list.Add("Ground", "0");
                    list.Add("First", "1");
                    list.Add("Second", "2");
                    break;
                case PropertyTypes.Independent_Floor:
                    list.Add("Basement", "-1");
                    list.Add("Ground", "0");
                    list.Add("First", "1");
                    list.Add("Second", "2");
                    list.Add("Terrace", "3");
                    break;
                case PropertyTypes.Penthouse:
                    list.Add("Terrace", "0");
                    list.Add("Upper", "1");
                    list.Add("Lower", "2");
                    break;
            }

            return list;
        }

        public FloorInfo Save()                                 
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    FloorInfo tempFloorInfo = context.FloorInfoes.FirstOrDefault(m => m.AptTypeID == AptTypeID && m.FloorNo == FloorNo);
                    Boolean IsNew = tempFloorInfo == null;

                    LUDate = DateTime.Now;

                    if (IsNew)
                        context.AddToFloorInfoes(this);
                    else
                    {
                        if (tempFloorInfo != null)
                            context.CreateObjectSet<FloorInfo>().Detach(tempFloorInfo);

                        context.CreateObjectSet<FloorInfo>().Attach(this);
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

        public static List<FloorInfo> GetByAptID(int ApartmentID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.FloorInfoes.Where(m => m.AptTypeID == ApartmentID).ToList();
            }
        }
        public FloorInfo Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<FloorInfo>().Attach(this);
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
    }
}