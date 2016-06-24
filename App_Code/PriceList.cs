using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace PropertyListModel
{
    public partial class PriceList
    {
        public string Message = "";
        public PriceList()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public PriceList Load()
        {
            PriceList PL = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                PL = context.PriceLists.FirstOrDefault(m => m.ID == ID);
            }

            return PL;
        }

        public static PriceList GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceLists.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static PriceList GetByAptTypeIDandSocID(int AptTypeID ,int SocID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceLists.FirstOrDefault(m => m.ApartmentTypeID == AptTypeID && m.SocietyID == SocID);
            }
        }


        public PriceList LoadBySocIDandAptTypeID()
        {
            PriceList PL = null;

            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                PL = context.PriceLists.FirstOrDefault(m => m.SocietyID == SocietyID && m.ApartmentTypeID == ApartmentTypeID);
            }

            return PL;
        }

        public PriceList Save()
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
                            ID = context.PriceLists.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;
                    
                    if (IsNew)
                        context.AddToPriceLists(this);
                    else
                    {
                        context.CreateObjectSet<PriceList>().Attach(this);
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
        
        //public static List<PriceList> GetList(int ID)
        //{
        //    using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        //    {
        //        return context.PriceLists.Where(m => m.SocietyID == ID).ToList();
        //    }
        //}

        //public static List<PriceList> GetList(int SocietyID)
        //{
        //    using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
        //    {
        //        return context.PriceLists.Where(m => m.SocietyID == SocietyID).ToList();
        //    }
        //}

        public static PriceList GetByAptID(int AprtID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.PriceLists.FirstOrDefault(m => m.ApartmentTypeID == AprtID);
            }
        }
    }
}