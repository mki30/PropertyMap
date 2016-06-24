using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace PropertyListModel
{
    public partial class NewsArticle
    {
        public string Message = "";
        public NewsArticle()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public NewsArticle Save()
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
                            ID = context.NewsArticles.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToNewsArticles(this);
                    else
                    {
                        context.CreateObjectSet<NewsArticle>().Attach(this);
                        context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return this;
        }

        public static NewsArticle GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.NewsArticles.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static List<NewsArticle> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.NewsArticles.OrderBy(m => m.LUDate).ToList();
            }
        }
    }
}