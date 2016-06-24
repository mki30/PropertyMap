using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
namespace PropertyListModel
{
    public partial class ProjectQuestion
    {
        public string Message = "";
        public ProjectQuestion()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ProjectQuestion Load()
        {
            ProjectQuestion B = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                B = context.ProjectQuestions.FirstOrDefault(m => m.ID == ID);
            }

            return B;
        }

        public ProjectQuestion Save()
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
                            ID = context.ProjectQuestions.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;

                    if (IsNew)
                        context.AddToProjectQuestions(this);
                    else
                    {
                        context.CreateObjectSet<ProjectQuestion>().Attach(this);
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

        public ProjectQuestion Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<ProjectQuestion>().Attach(this);
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


        public static List<ProjectQuestion> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ProjectQuestions.OrderBy(m => m.LUDate).ToList();
            }
        }

        public static ProjectQuestion GetByID(int ID)
        {

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ProjectQuestions.FirstOrDefault(m=>m.ID==ID);
            }
        }

    }
}