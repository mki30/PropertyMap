using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
namespace PropertyListModel
{
    public partial class Service
    {
        public string Message = "";
        public Service()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Service Load()
        {
            Service S = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                S = context.Services.FirstOrDefault(m => m.ID == ID);
            }

            return S;
        }

        public Service Save()
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
                            ID = context.Services.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToServices(this);
                    else
                    {
                        context.CreateObjectSet<Service>().Attach(this);
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

        public static List<Service> GetAllServices()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Services.OrderBy(m => m.ID).ToList();
            }
        }
        public static List<Service> CheckByConact(string ContactNo)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Services.Where(m => m.Contact ==ContactNo).Take(1).ToList();
            }
        }

    }

}