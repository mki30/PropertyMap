using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
namespace PropertyListModel
{
    public partial class ProjectPricing
    {
        public string Message = "";
        public ProjectPricing()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ProjectPricing Save()
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
                            ID = context.ProjectPricings.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToProjectPricings(this);
                    else
                    {
                        context.CreateObjectSet<ProjectPricing>().Attach(this);
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

        public static ProjectPricing GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ProjectPricings.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<ProjectPricing> GetByProjectID(int projectID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ProjectPricings.Where(m => m.ProjectID == projectID).ToList();
            }
        }
    }

}