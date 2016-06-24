using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;


namespace PropertyListModel
{
    public partial class AgentClient
    {
        public string Message = "";
        public AgentClient()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        public AgentClient Load()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AgentClients.FirstOrDefault(m => m.ID == ID);
            }
        }
        
        public AgentClient Save()
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
                            ID = context.AgentClients.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToAgentClients(this);
                    else
                    {
                        context.CreateObjectSet<AgentClient>().Attach(this);
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

        public static List<AgentClient> GetList()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AgentClients.OrderBy(m => m.Name).ToList();
            }
        }

        public static List<AgentClient> GetListByAgentID(int AgentID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AgentClients.Where(m => m.AgentID==AgentID).ToList();
            }
        }

        public static AgentClient GetClientByMobile(string Mobile)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.AgentClients.FirstOrDefault(m => m.MobileNo == Mobile);
            }
        }
        
        //public static void GetClientList(ListControl lc)
        //{
        //    lc.Items.Clear();
        //    List<AgentClient> ClientList = GetList();
        //    foreach (AgentClient S in ClientList)
        //    {
        //        lc.Items.Add(new ListItem(S.Name, S.ID.ToString()));
        //    }
        //}

        //public static List<AgentClient> GetList()
        //{
        //    using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        //    {
        //        return context.AgentClients.OrderBy(m => m.Name).ToList();
        //    }
        //}
    }
}