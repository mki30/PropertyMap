using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace PropertyListModel
{
    public partial  class Apartment
    {
        public string Message = "";

        public Apartment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Apartment Load()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.Apartments.FirstOrDefault(m => m.ID == ID);
            }
        }

        public Apartment Save()
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
                            ID = context.Apartments.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToApartments(this);
                    else
                    {
                        context.CreateObjectSet<Apartment>().Attach(this);
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

        public static List<Apartment> GetList(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {

                return context.Apartments.Where(m => m.SocietyID == ID).OrderBy(m => m.Block).ThenBy(m => m.ApartmentNumber).ToList();
               
            }
        }
        public static void GetAptNo(ListControl dd,int SocietyID)
        {
            dd.Items.Clear();
            List<Apartment> ApartmentList = GetList(SocietyID);

            foreach (Apartment S in ApartmentList)
            {
                dd.Items.Add(new ListItem(S.Block + "-" + S.ApartmentNumber.ToString(), S.ID.ToString()));
            }
        }
    }
}