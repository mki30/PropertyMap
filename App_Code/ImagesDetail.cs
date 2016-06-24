using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using ImageResizer;
using System.IO;

namespace PropertyListModel
{
    public partial class ImagesDetail
    {
        public string Message { get; set; }
        public Boolean DeleteObjectFromDb = false;
        public ImagesDetail()
        {
        }
        public static List<ImagesDetail> GetList()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.ToList();
            }
        }

        public static List<ImagesDetail> GetByIDs(int referenceID, int imageReferenceType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.Where(m => m.ReferenceID == referenceID && m.ImageReferenceType == imageReferenceType).ToList();
            }
        }

        public static ImagesDetail GetByIDs(int referenceID, int imageReferenceType, string imageID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.FirstOrDefault(m => m.ReferenceID == referenceID && m.ImageID == imageID && m.ImageReferenceType == imageReferenceType);
            }
        }

        public static ImagesDetail GetByID(int ID)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.FirstOrDefault(m => m.ID == ID);
            }
        }

        public static void Save(List<ImagesDetail> imageList)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                foreach (ImagesDetail im in imageList)
                {
                    if (im.DeleteObjectFromDb)
                        im.Delete();
                    else
                        im.Save(context);
                }
            }
        }

        public ImagesDetail Save()
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return Save(context);
            }
        }


        public static void SaveList(List<ImagesDetail> list)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                foreach (ImagesDetail fp in list)
                {
                    if (fp != null)
                        fp.Save(context);
                }
                context.SaveChanges();
            }
        }

        public ImagesDetail Save(PropertyListEntities context)
        {
            try
            {
                Boolean isNew = false;
                if (ID == 0)
                {
                    ID = 1;
                    try
                    {
                        ID = context.ImagesDetails.Max(m => m.ID) + 1;
                    }
                    catch
                    { }
                    isNew = true;
                }
                LUDate = DateTime.Now;
                LUBy = Global.UserName;
                if (isNew)
                {
                    context.AddToImagesDetails(this);
                }
                else
                {
                    context.CreateObjectSet<PropertyListModel.ImagesDetail>().Attach(this);
                    context.ObjectStateManager.ChangeObjectState(this, EntityState.Modified);
                }
                context.SaveChanges();
                Message = "Updated Successfully";
            }
            catch (Exception ex)
            {
                Message += ex.Message + "," + ex.StackTrace;
            }
            return this;
        }
        public ImagesDetail Delete()
        {
            try
            {
                using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
                {
                    ImagesDetail temp = context.ImagesDetails.FirstOrDefault(m => m.ID == ID);
                    context.DeleteObject(temp);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Message += ex.Message + "," + ex.StackTrace;
            }
            return this;
        }


        public static ImagesDetail GetByImageIDandReferenceType(string imageId, int referenceType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.FirstOrDefault(m => m.ImageID == imageId && m.ImageReferenceType==referenceType);
            }
        }

        public static List<ImagesDetail> GetByReferenceIDandReferenceType(int refId, int referenceType)
        {
            using (PropertyListEntities context = new PropertyListEntities(Global.ConnectionStringEntity))
            {
                return context.ImagesDetails.Where(m => m.ReferenceID == refId && m.ImageReferenceType == referenceType).ToList();
            }
        }
    }
}