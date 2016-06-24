using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace PropertyListModel
{
    public enum OtherFeatures : int
    {
        Store = 0,
        Study = 1,
        Servant = 2,
        Pooja = 3,
        Dress=4,
        Terrace=5,
        Lobby=6,
        Foyer=7,
        Utility=8,
        Last
    }
    
    public class ApartmentShortInfo
    {
        public int? Price;
        public int? Area;
        public int? BHK;
    }

    public partial class ApartmentType
    {
        public string Message = "";
        public string  SocietyName = "";
        public Society Parent;
        public List<FloorInfo> FloorInfoList=new List<FloorInfo>();

        public string  SetOtherFeatures(OtherFeatures Feature,Boolean Value)
        {
            if (string.IsNullOrWhiteSpace(OtherFeatures))
                OtherFeatures = "";

            OtherFeatures = OtherFeatures.PadRight((int)PropertyListModel.OtherFeatures.Last, '0');

            StringBuilder str = new StringBuilder(OtherFeatures);
            str[(int)Feature] = Value ? '1' : '0';
            OtherFeatures = str.ToString();
            return OtherFeatures;
        }

        public ApartmentType()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static List<ApartmentType> GetAll()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.ToList();
            }
        }
        
        public static ApartmentType GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static ApartmentType GetByName(int SocietyID,string Name)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.FirstOrDefault(m => m.SocietyID == SocietyID && m.TypeName==Name);
            }
        }

        public static List<ApartmentType> GetBySocietyID(int SocietyID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.Where(m => m.SocietyID == SocietyID).ToList();
            }
        }

        public ApartmentType Check()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.FirstOrDefault(m => m.SocietyID == SocietyID
                    && m.Bedroom == Bedroom
                    && m.Balcony == m.Balcony
                    && m.SuperArea == SuperArea
                    && m.TypeName == TypeName
                    );
            }
        }

        public ApartmentType Save()
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
                            ID = context.ApartmentTypes.Max(m => m.ID) + 1;
                        }
                        catch { }
                        IsNew = true;
                    }

                    LUDate = DateTime.Now;
                    //if (LUBy == null) LUBy = _Global.Employee.FirstName;

                    if (IsNew)
                        context.AddToApartmentTypes(this);
                    else
                    {
                        context.CreateObjectSet<ApartmentType>().Attach(this);
                        context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                    }
                    context.SaveChanges();
                    //Parent.Save();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message;
            }
            return this;
        }

        public ApartmentType Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    context.CreateObjectSet<ApartmentType>().Attach(this);
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

        public static List<ApartmentType> GetList(int SocietyID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ApartmentTypes.Where(m => m.SocietyID == SocietyID).OrderBy(m => m.TypeName).ToList();
            }
        }

        public static void GetApartmentTypeName(ListControl dd,int SocietyID)
        {
            dd.Items.Clear();
            List<ApartmentType> ApartmentTypeList = GetList(SocietyID);
            
            foreach (ApartmentType S in ApartmentTypeList)
            {
                dd.Items.Add(new ListItem(S.TypeName, S.ID.ToString()));
            }
        }
    }
}