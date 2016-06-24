using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace PropertyListModel
{
    public partial class Availability
    {
        public Society Society;
        public string Message = "";

        public Availability()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static List<Availability> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.ToList();
            }
        }

        public static List<Availability> GetList(int SocietyID, int BHK = 0, int SellerID = 0, int SellerType = 0, int Avl_ID = 0)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.Where(m => (m.SocietyID == SocietyID || SocietyID == 0)
                    && (m.BHK == BHK || BHK == 0)
                    && (m.SellerID == SellerID || SellerID == 0)
                    && (m.SellerType == SellerType)
                    //&&(m.ID == Avl_ID)
                    ).OrderBy(m => m.BHK).ThenBy(m => m.Area).ThenBy(m => m.Amount).ThenBy(m => m.SellerName).ToList();
            }
        }

        //public static List<Availability> GetListAgentvise(int AgentID)       //get AgentviseList used in Edit>Agent
        //{
        //    using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        //    {
        //        return context.Availabilities.Where(m => m.SellerID == AgentID && (m.SellerType == 0)).OrderBy(m => m.Area).ThenBy(m => m.Amount).ToList();
        //    }
        //}

        public static Availability GetByDataSourceAndID(string DataSource, string DataSourceID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.FirstOrDefault(m => m.DataSource == DataSource && m.DataSourceID == DataSourceID);

            }
        }

        public static Availability GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<int?> GetAvailableSocietyList()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.Select(m => m.SocietyID).Distinct().ToList();
            }
        }

        public static List<Availability> GetAvailableBySociety()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.Where(m => m.SocietyID != 0).ToList();
            }
        }

        public Availability Save()
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
                            ID = context.Availabilities.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    if (IsNew)
                        context.AddToAvailabilities(this);
                    else
                    {
                        context.CreateObjectSet<Availability>().Attach(this);
                        context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message + ex.InnerException;
            }
            return this;
        }


        public Availability Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<Availability>().Attach(this);
                    context.DeleteObject(this);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message + "-" + ex.StackTrace + "-" + ex.InnerException.Message;
            }
            return this;
        }

        public static void GetList(ListControl dd)
        {
            dd.Items.Clear();
            List<Availability> SocietyList = GetAll().OrderBy(m => m.SocietyName).ToList();

            foreach (Availability A in SocietyList)
            {
                if (A.SocietyID == 0 && !string.IsNullOrWhiteSpace(A.SocietyName))
                    dd.Items.Add(new ListItem((A.SocietyName), A.ID.ToString()));
            }
        }

        public static Availability GetAvlByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<Availability> GetAvlByAgentID(int AgentID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.Where(m => m.SellerID == AgentID).ToList();
            }
        }
        public static List<Availability> GetAvlByAgentIDandProjectID(int AgentID, int ProjectID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Availabilities.Where(m => m.SellerID == AgentID && m.SocietyID == ProjectID).ToList();
            }
        }
    }
}