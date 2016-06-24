using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI.WebControls;

namespace PropertyListModel
{
    public partial class Agent
    {
        public List<Society> ProjectList = new List<Society>();

        public string Message = "";
        public Agent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Agent LoadByCompanyName()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.FirstOrDefault(m => m.AgentCompany == AgentCompany);
            }
        }

        public Agent Save()
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
                            ID = context.Agents.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToAgents(this);
                    else
                    {
                        context.CreateObjectSet<Agent>().Attach(this);
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

        public Agent Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<Agent>().Attach(this);
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

        public static List<Agent> GetList(int CityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.OrderBy(m => m.AgentCompany).ToList();
            }
        }

        public static Agent UpdateAgent(Availability _Availability)
        {
            if (_Availability.SellerType != (int)SellerType.Agent)
                return null;

            Agent A = null;
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                A = context.Agents.FirstOrDefault(m => m.Mobile1 == _Availability.SellerMobile1);
                if (A == null)
                    A = context.Agents.FirstOrDefault(m => m.Mobile2 == _Availability.SellerMobile1);
                if (A == null)
                    A = context.Agents.FirstOrDefault(m => m.Mobile3 == _Availability.SellerMobile1);

                if (A == null)
                {
                    A = new Agent()
                    {
                        AgentName = _Availability.SellerName,
                        PhoneNo1 = _Availability.SellerPhone,
                        Mobile1 = _Availability.SellerMobile1,
                        Mobile2 = _Availability.SellerMobile2,
                        Mobile3 = _Availability.SellerMobile3,
                        AgentCompany = _Availability.Company,
                        Address = _Availability.Address,

                    }.Save();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(A.AgentName))
                    {
                        A.AgentName = _Availability.SellerName;
                        A.Save();
                    }
                }
            }
            return A;
        }

        static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {

            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        public static List<Agent> GetListByScociety(int SocietyID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var agents = context.Availabilities.Where(m => m.SocietyID == SocietyID).Select(m => m.SellerID).ToArray();
                return context.Agents.Where(x => agents.Contains(x.ID)).ToList();
            }
        }

        public static Agent Validate(string UserID, string Password)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                var a = context.Agents.FirstOrDefault(m => m.UserID == UserID && m.Password == Password);
                if (a==null)
                    a = context.Agents.FirstOrDefault(m => m.EmailID == UserID && m.Password == Password);
                if (a==null)
                    a = context.Agents.FirstOrDefault(m => m.Mobile1 == UserID && m.Password == Password);
                return a;
            }
            //return message;
        }

        public static void GetUserList(ListControl lc, SellerType UserType, int varified, int cityid)
        {
            lc.Items.Clear();
            List<Agent> UserList = GetAll(UserType, varified, cityid);

            foreach (Agent S in UserList)
            {
                if (S.AgentName != null)
                    lc.Items.Add(new ListItem(S.AgentName, S.ID.ToString()));
            }
        }

        public static List<Agent> GetAll(SellerType UserType, int Varified, int CityID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.Where(m => m.UserType == (int)UserType && m.Varified == Varified && m.City == CityID).OrderBy(m => m.AgentName).ToList();
            }
        }
        
        public static List<Agent> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.OrderBy(m => m.AgentName).ToList();
            }
        }

        public static Agent GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static bool UrlNameExist(string UrlName)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.Any(m => m.URLName == UrlName);
            }
        }

        public static Agent GetByName(string Name)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.FirstOrDefault(m => m.AgentName == Name && m.UserType == 2);
            }
        }

        public static List<Agent> GetAllDeleted(int UserType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.Where(m => m.Deleted == 1 && m.UserType == UserType).OrderBy(m => m.AgentName).ToList();
            }
        }

        public static List<Agent> GetByCity(int CityId, int UserType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Agents.Where(m => m.City == CityId && m.UserType == UserType).OrderBy(m => m.AgentName).ToList();
            }
        }

       public string BuilderDetailMobile()
       {
                StringBuilder str = new StringBuilder();
                string ext = "jpg";
                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(@"~/Data/Images_BuilderLogo/") + ID + ".gif"))
                    ext = "gif";
                str.Append("<a href='#' class='ui-btn'><img src='/Data/Images_BuilderLogo/" +ID + "." + ext + "' style='height:100%;'>" +
                "<h1 ID='buil_Name'>" + AgentName + "</h1>");
                if (Address != "")
                    str.Append("<p>Add: " + Address + "</p>");
                if (Mobile1 != "")
                    str.Append("<p>Cont: " +Mobile1 + "</p>");
                str.Append("</a>" +
                "<p style='text-align:justify;padding:1em;text-indent:2em;'>" + BuilderDescription + "</p>");
                return str.ToString();
        }
    }
}