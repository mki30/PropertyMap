using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;


namespace PropertyListModel
{
    public partial class AsignClient
    {
        public string Message = "";
        public AsignClient()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public AsignClient Load()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.FirstOrDefault(m => m.ID == ID);
            }
        }
        public AsignClient Save()
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
                            ID = context.AsignClients.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                   
                    if (IsNew)
                        context.AddToAsignClients(this);
                    else
                    {
                        context.CreateObjectSet<AsignClient>().Attach(this);
                        context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message + "-" + ex.StackTrace + "-" + ex.InnerException.Message;
            }
            return this;
        }

        public AsignClient Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<AsignClient>().Attach(this);
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

        public static AsignClient GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<AsignClient> GetByClientID(int ClientID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.Where(m =>m.ClientID==ClientID).ToList();
            }
        }

        public static AsignClient GetByAllIDs(int AvlID,int AgentID,int ClientID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.FirstOrDefault(m => m.AvlID == AvlID&&m.AgentID==AgentID&&m.ClientID==ClientID);
            }
        }
        public static List<AsignClient> GetAssigned(int AgentID,int ClientID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.Where(m => m.ClientID == ClientID&&m.AgentID==AgentID).ToList();
            }
        }

        public static int GetAssignedCount(int ClientID, int AgentID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AsignClients.Where(m => m.ClientID == ClientID && m.AgentID == AgentID).Count();
            }
        }
    }
}