using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using PropertyListModel;
using System.Text;

public partial class Admin_DBUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();   //Update PropertyMap Database Structute
        try
        {
            int ctr = 0;
            while (DBCheck.UpdateDBStructure(db, ++ctr)) ;
            Response.Write("Update done");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        
        finally
        {
            db.Close();
        }

        DatabaseCE dbBlog = new DatabaseCE(Global.BlogConnection);          //Upadte Blog Database Structure
        try
        {
            int ctr = 0;
            while (DBCheck.UpdateBlogDBStructure(dbBlog, ++ctr)) ;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        finally
        {
            dbBlog.Close();
        }
    }

    protected void btnUpdateAgentID_Click(object sender, EventArgs e)
    {
        //List<Availability> AvailabilityList = Availability.GetAll();
        //foreach (Availability Avl in AvailabilityList)
        //{
        //    Agent A = Agent.UpdateAgent(Avl);
        //    if (A != null)
        //    {
        //        Avl.SellerID = A.ID;
        //        Avl.Save();
        //    }
        //}
    }

    protected void btnUserType_Click(object sender, EventArgs e)
    {
        //List<Agent> AgentList = Agent.GetAllAgents();
        //foreach (Agent Agt in AgentList)
        //{
        //    Agt.UserType = 0;
        //    Agt.Save();
        //}
    }

    protected void btnAddOwnerData_Click(object sender, EventArgs e)
    {
        //List<Owner> OwnerList = Owner.GetAllOwners();
        //foreach (Owner onr in OwnerList)
        //{
        //    Agent A = new Agent()
        //    {
        //        ID = 0,
        //        UserType = 1,
        //        AgentName = onr.OwnerName,
        //        Address = onr.Address,
        //        Mobile1 = onr.ContactNo.ToString(),
        //        EmailID = onr.EmailID
        //    }.Save();
        //}
    }

    protected void btnAddBuilderData_Click(object sender, EventArgs e)
    {
        //List<Builder> BuilderList = Builder.GetAllBuilders();
        //foreach (Builder bldr in BuilderList)
        //{
        //    Agent A = new Agent()
        //    {
        //        ID = 0,
        //        UserType = 2,
        //        AgentName = bldr.BuilderName,
        //        Address = bldr.Address,
        //        Mobile1 = bldr.ContactNo,
        //        EmailID = bldr.EmailID
        //    }.Save();
        //}
    }
    protected void btnAddDataToClient_Click(object sender, EventArgs e)
    {
        //List<Agent> AgentList = Agent.GetAllAgents();
        //foreach (Agent a in AgentList)
        //{
        //    if (a.AgentName != null)
        //    {
        //        AgentClient AC = new AgentClient()
        //        {
        //            ID = 0,
        //            Name = a.AgentName,
        //            MobileNo = a.Mobile1,
        //            PhoneNo = a.PhoneNo1,
        //            City = a.City,
        //            EmailID = a.EmailID,
        //            Address = a.Address,
        //            AgentID=510,
        //        }.Save();
        //    }
        //}
    }
    protected void btnUpdateSocietyIDinAvl_Click(object sender, EventArgs e)
    {
        //List<Availability> AvlList =Availability.GetAll();
        //foreach(Availability avl in AvlList)
        //{
        //    if (avl.ID > 450)
        //    {
        //        avl.SellerID = 510;
        //        avl.Save();
        //    }
        //}
    }
    protected void btnUpdateSocietyIDinAvl0_Click(object sender, EventArgs e)
    {
        //List<Landmark> LandList = Landmark.GetAll();
        //foreach (Landmark L in LandList)
        //{
        //    if (L.LandmarkType == "petrol")
        //    {
        //        L.LandmarkType = "petrol pump";
        //        L.Save();
        //    }
        //}
    }

    protected void btnSocietyUpdate_Click(object sender, EventArgs e)
    {
        //List<Society> SocList = Society.GetAll();
        //foreach (Society S in SocList)
        //{
        //    S.Subcity = "indrapuram";
        //    S.Save();
        //}
    }

    protected void btnGeneratePriceTrend_Click(object sender, EventArgs e)
    {
        string vari = "var Rate = new Array();" + Environment.NewLine + "var locname";
        File.AppendAllText(Server.MapPath(@"~/Js/PriceTrend.js"), vari);
        List<string> LocNames = Society.GetLocalityNames();
        foreach (string locname in LocNames)
        {
            List<PriceTrend> PriceTrendList = PriceTrend.GetAll(locname);
            List<string> PriceDates = new List<string>();

            string strMax = "var MaxRate=[";
            string strMin = "var MinRate=[";
            string strAvg = "var AvgRate=[";

            int ctr = 0;
            DateTime MinDate = new DateTime(1970, 1, 1);
            foreach (PriceTrend pt in PriceTrendList)
            {
                ctr++;
                double Ticks = (Cmn.ToDate(pt.PriceDate) - MinDate).TotalMilliseconds;

                strMax += "[" + Ticks.ToString("0") + "," + pt.Max + "],";
                strAvg += "[" + Ticks.ToString("0") + "," + (pt.Max + pt.Min) / 2 + "],";
                strMin += "[" + Ticks.ToString("0") + "," + pt.Min + "],";
                
                PriceDates.Add((pt.PriceDate).ToString());
            }

            strMax = strMax.TrimEnd(',') + "];";
            strMin = strMin.TrimEnd(',') + "];";
            strAvg = strAvg.TrimEnd(',') + "];";
            string LocName = Environment.NewLine + locname.Replace(" ", "");
            string newvari = Environment.NewLine + "Rate.Push({Name:" + LocName + ",Minrate:MinRate,Maxrate:MaxRate,Avgrate:AvgRate});";
            File.AppendAllText(Server.MapPath(@"~/Js/PriceTrend.js"), Environment.NewLine + strMin + Environment.NewLine + strMax + Environment.NewLine + strAvg + Environment.NewLine + newvari);
        }
    }

    protected void btnUpdateURLName_Click(object sender, EventArgs e)
    {
        //List<Society> list = Society.GetAll();
        //foreach (Society S in list)
        //{
        //    S.URLName = (S.SocietyName + "-" + S.Subcity + "-" + S.City).Replace(" ", "-");
        //    S.Save();
        //}
    }

    protected void btnUpdateGlobal_Click(object sender, EventArgs e)
    {
        Global.LoadGlobalData();
    }

    protected void btnUpdtePTCityID_Click(object sender, EventArgs e)
    {
        //List<PriceTrend> PT = PriceTrend.GetAll();

        //foreach (var P in PT)
        //{
        //    City C = City.GetByName(P.City);
        //    string SubCity = P.Subcity.Contains("-") ? P.Subcity.Replace("-", " ") : P.Subcity;
        //    City c = City.GetByName(C.ID, SubCity.Trim());
        //    if (c != null)
        //    {
        //        P.CityID = c.ParentID;
        //        P.SubCityID = c.ID;
        //    }
        //    else
        //    {
        //        P.CityID = -1;
        //        P.SubCityID = -1;
        //    }
        //    P.Save();
        //}
    }
    
    protected void btnUpdateBuilderName_Click(object sender, EventArgs e)
    {
        //List<Agent> AgntList = Agent.GetAll();
        //int ctra = 0, ctrb = 0;
        //foreach (Agent A in AgntList)
        //{

        //    if (A.AgentCompany != "" && A.AgentCompany != null && A.AgentCompany.Contains('.'))
        //    {
        //        A.AgentCompany = A.AgentCompany.Replace(".", "");
        //        ctra++;
        //    }

        //    if (A.AgentName != "" && A.AgentName != null && A.AgentName.Contains('.'))
        //    {
        //        A.AgentName = A.AgentName.Replace(".", "");
        //        ctrb++;
        //    }
        //    A.Save();
        //}
    }

    protected void btnCityUrlNameUpdate_Click(object sender, EventArgs e)
    {
        //foreach (City c in Global.CityList.Values)
        //{
        //    c.UrlName = c.Name;
        //    City parent = c.Parent;
        //    while (parent != null)
        //    {
        //        c.UrlName += "-" + parent.Name;
        //        parent = parent.Parent;
        //    }
        //    c.UrlName = c.UrlName.Replace(" ", "_").ToLower();
        //    c.Save();
        //}
    }

    protected void btnUpdateImageName_Click(object sender, EventArgs e)
    {
        //DirectoryInfo di = new DirectoryInfo(@"c:\Developement\PropertyMap\Data\Images_Society");
        //FileInfo[] files = di.GetFiles();
        //foreach (FileInfo file in files)
        //{
        //    int id = Cmn.ToInt(file.Name.Split('_')[0]);
        //    Society S = Society.GetByID(id);
        //    string OldPath=file.FullName;
        //    string NewPath=di.FullName+"\\"+S.SocietyName+"-"+file.Name;
        //    string Bakup=di.Parent+"\\Bakup";
        //    File.Replace(OldPath, NewPath, Bakup);
        //}
   }
    protected void btnUptateFromCsv_Click(object sender, EventArgs e)
    {
        //string[] Data = File.ReadAllLines(Server.MapPath(@"~\Data\CSV\Webmaster2.csv"));
        
        //int ctr = 0;
        //foreach (string s in Data)
        //{
        //    ctr++;
        //    if (ctr == 1)
        //        continue;
        //    string[] Fields = s.Split(',');

        //        if (!(Fields[0].Contains("project")))
        //        continue;

        //        string URLName = Fields[0].Split('/').Last();

        //        Society S = Global.ProjectList.Values.Where(m=>m.Verified==1).FirstOrDefault(m => m.URLName == URLName);

        //        //Response.Write(URLName);
        //        try
        //        {
        //            if (S != null)
        //            {
        //                S.Impression = (int)(Cmn.ToDbl(Fields[1]));
        //                S.AvgPos = (int)(Cmn.ToDbl(Fields[4]));
        //                S.Save();
        //                //Response.Write(URLName + "," + Cmn.ToInt(Fields[1]) + "," + Cmn.ToInt(Fields[7]));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write(ex.Message);
        //        }
        // }
    }
    
    protected void btnApplyWaterMark_Click(object sender, EventArgs e)
    {
        //DoWaterMark(@"~\Data\Images_ApartmentType");
        //DoWaterMark(@"~\Data\Images_LayoutPlan");
        //DoWaterMark(@"~\Data\Images_Society");
    }

    private void DoWaterMark(string Path)
    {
        DirectoryInfo di = new DirectoryInfo(Server.MapPath(Path));
        FileInfo[] files = di.GetFiles();

        int ctr = 0;
        foreach (FileInfo fi in files)
        {
            if (ctr > 5)
                break;
            string WaterMarkFile = fi.FullName.Replace("Data", "Data\\Watermark");
            if (!File.Exists(WaterMarkFile))
            {
                WaterMark.CreateImage(fi.FullName, WaterMarkFile, "http://propertymap.info");
            }
            ctr++;
        }
    }
    protected void btnUpdateAgentUrlName_Click(object sender, EventArgs e)
    {
        //List<Agent> AgntList = Agent.GetAll();
        //foreach (Agent A in AgntList)
        //{
        //    if (A.UserType == 2)
        //    {
        //        if (!string.IsNullOrWhiteSpace(A.AgentName))
        //        {
        //            A.URLName = A.AgentName.Replace(" ", "-").Trim().ToLower();
        //            A.Save();
        //        }
        //    }
        //    else 
        //    {
        //        if (!string.IsNullOrWhiteSpace(A.AgentCompany))
        //        {
        //            A.URLName = A.AgentCompany.Replace(" ", "-").Trim().ToLower();
        //            A.Save();
        //        }
        //    }
        //}
    }
    
    protected void btnImageTable_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE(Global.PropertyMapConnnection);
        try
        {
            db.RunQuery("Delete from ImagesDetails");
        }
        catch (Exception ex)
        {
        }
        finally
        {
            db.Close();
        }

        List<Society> AllSocieties = Society.GetAll();
        List<ApartmentType> AT = ApartmentType.GetAll();
        List<ImagesDetail> ImgDetails = new List<ImagesDetail>();

        DirectoryInfo dir_SocietyImg = new DirectoryInfo(Server.MapPath(@"~/Data/Images_Society/"));

        DirectoryInfo dir_ApaType = new DirectoryInfo(Server.MapPath(@"~/Data/Images_ApartmentType/"));
        foreach (Society s in AllSocieties)
        {
            FileInfo[] files = dir_SocietyImg.GetFiles(s.ID + "_*.jpg");
            foreach (FileInfo f in files)
            {
                ImagesDetail imgDetail = new ImagesDetail();
                imgDetail.ReferenceID = s.ID;
                imgDetail.ImageReferenceType = 1;
                imgDetail.ImageID = f.Name;
                ImgDetails.Add(imgDetail);
            }

            string layoutImg = Server.MapPath(@"\Data\Images_LayoutPlan\") + s.ID + ".jpg";
            if (File.Exists(layoutImg))
            {
                ImagesDetail imgDetail = new ImagesDetail();
                imgDetail.ReferenceID = s.ID;
                imgDetail.ImageReferenceType = 2;
                imgDetail.ImageID = s.ID + ".jpg";
                ImgDetails.Add(imgDetail);
            }

            string logoImg = Server.MapPath(@"\Data\Images_SocietyLogo\") + s.ID + ".jpg";
            if (File.Exists(logoImg))
            {
                ImagesDetail imgDetail = new ImagesDetail();
                imgDetail.ReferenceID = s.ID;
                imgDetail.ImageReferenceType = 3;
                imgDetail.ImageID = s.ID + ".jpg";
                ImgDetails.Add(imgDetail);
            }
        }

        foreach (ApartmentType aptType in AT)
        {
            string aptTypeImg = Server.MapPath(@"\Data\Images_ApartmentType\") + aptType.ID + ".jpg";
            if (File.Exists(aptTypeImg))
            {
                ImagesDetail imgDetail = new ImagesDetail();
                imgDetail.ReferenceID = aptType.ID;
                imgDetail.ImageReferenceType = 4;
                imgDetail.ImageID = aptType.ID + ".jpg";
                ImgDetails.Add(imgDetail);
            }
            FileInfo[] files = dir_ApaType.GetFiles(aptType.ID + "_*.jpg");
            foreach (FileInfo f in files)
            {
                ImagesDetail imgDetail = new ImagesDetail();
                imgDetail.ReferenceID = aptType.ID;
                imgDetail.ImageReferenceType = 4;
                imgDetail.ImageID = f.Name;
                ImgDetails.Add(imgDetail);
            }
        }
        UpdateInDatabase(ImgDetails);
    }
    void UpdateInDatabase(List<ImagesDetail> IDS)
    {
        DatabaseCE db = new DatabaseCE(Global.PropertyMapConnnection);
        try
        {
            foreach (ImagesDetail imgd in IDS)
            {
                if (imgd != null)
                {
                    string error = "";
                    int id = db.GetMax("ImagesDetails", "id","",ref error);
                    db.RunQuery("insert into ImagesDetails(ID,ReferenceID,ImageReferenceType,ImageID) values("+(id+1)+"," + imgd.ReferenceID + "," + imgd.ImageReferenceType + ",'" + imgd.ImageID + "')");
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnUpdateImageUrlNames_Click(object sender, EventArgs e)
    {
        List<Society> AllSocieties = Society.GetAll();
        List<ApartmentType> AllApartmentTypes = ApartmentType.GetAll();
        Dictionary<string, ImagesDetail> ImageList = new Dictionary<string, ImagesDetail>();
        List<ImagesDetail> IDS = ImagesDetail.GetList();

        foreach (ImagesDetail imgDetails in IDS)
        {
            try
            {
                Society S = null;

                string urlName = "";
                if (imgDetails.ImageReferenceType != 4)
                {
                    S = AllSocieties.FirstOrDefault(m => m.ID == Cmn.ToInt(imgDetails.ReferenceID));
                    if (S == null)
                        continue;
                }

                switch (imgDetails.ImageReferenceType)
                {
                    case 1:    //Project Images
                        urlName = (Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-" + imgDetails.ImageID.Split('.')[0]).ToLower();
                        urlName = GetUniqueURL(IDS, S, urlName, "");
                        break;

                    case 2:   //Laypot Plan
                        urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-layout-plan";
                        urlName = GetUniqueURL(IDS, S, urlName, "layout-plan");
                        break;

                    case 3:   //Logo Image
                        urlName = Cmn.GenerateSlug(S.SocietyName) + "-" + Cmn.GenerateSlug(S.Subcity) + "-logo";
                        urlName = GetUniqueURL(IDS, S, urlName, "logo");
                        break;

                    case 4:
                        ApartmentType AT = AllApartmentTypes.FirstOrDefault(m => m.ID == Cmn.ToInt(imgDetails.ReferenceID));
                        if (AT != null)
                        {
                            Society Soc = AllSocieties.FirstOrDefault(m => m.ID == Cmn.ToInt(AT.SocietyID));
                            if (Soc != null)
                            {
                                urlName = (Cmn.GenerateSlug(Soc.SocietyName) + "-" + Cmn.GenerateSlug(Soc.Subcity) + "-" + imgDetails.ImageID.Split('.')[0]).ToLower();
                                urlName = urlName = GetUniqueURL(IDS, S, urlName, "");
                            }
                        }
                        break;
                }
                imgDetails.UrlName = urlName;
            }
            catch (Exception ex)
            {
                Response.Write("Error " + ex.Message+"-<br/>"+ex.StackTrace+"<br/>");
            }
        }
        UpdateUrlInDatabase(IDS);
    }
    
    void UpdateUrlInDatabase(List<ImagesDetail> IDS)
    {
        DatabaseCE db = new DatabaseCE(Global.PropertyMapConnnection);
        try
        {
            foreach (ImagesDetail fp in IDS)
            {
                if (fp != null)
                {
                    db.RunQuery("Update ImagesDetails SET URLName='" + fp.UrlName + "' WHERE ID=" + fp.ID);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Database Save Error:"+ex.Message);
        }
        finally
        {
            db.Close();
        }
    }
    
    public List<string> urls = new List<string>();
    string GetUniqueURL(List<ImagesDetail> IDS, Society S, string urlName, string appendText)
    {
        //ImagesDetail id = IDS.FirstOrDefault(m => m.UrlName == urlName);
        string url = urlName;
        if (urls.Contains(urlName) || string.IsNullOrWhiteSpace(urlName))
            url = (S.URLName + (appendText != "" ? "-" + appendText : "")).ToLower().Trim();
        urls.Add(url);
        return url;
    }

    protected void btnRenameFiles_Click(object sender, EventArgs e)
    {
        List<ImagesDetail> IDs = ImagesDetail.GetList();
        string[] paths = { "/Data/Images_Society/", "/Data/Images_LayoutPlan/", "/Data/Images_SocietyLogo/", "/Data/Images_ApartmentType/" };

        foreach (ImagesDetail ID in IDs)
        {
            switch (ID.ImageReferenceType)
            {
                case 1:
                    string sourceFile = Server.MapPath(paths[0] + ID.ImageID);
                    if (File.Exists(sourceFile))
                    {
                        try
                        {
                            string destFile = Server.MapPath(paths[0]) + ID.UrlName + ".jpg";
                            File.Move(sourceFile, destFile);
                        }
                        catch (Exception ex) 
                        {
                        };
                    }
                    break;
                case 2:
                    string sourceFile2 = Server.MapPath(paths[1] + ID.ImageID);
                    if (File.Exists(sourceFile2))
                    {
                        try
                        {
                            string destFile = Server.MapPath(paths[1]) + ID.UrlName + ".jpg";
                            File.Move(sourceFile2, destFile);
                        }
                        catch (Exception ex)
                        {
                        };
                    }
                    break;
                case 3:
                    string sourceFile3 = Server.MapPath(paths[2] + ID.ImageID);
                    if (File.Exists(sourceFile3))
                    {
                        try
                        {
                            string destFile = Server.MapPath(paths[2]) + ID.UrlName + ".jpg";
                            File.Move(sourceFile3, destFile);
                        }
                        catch (Exception ex)
                        {
                        };
                    }
                    break;
                case 4:
                    string sourceFile4 = Server.MapPath(paths[3]) + ID.ImageID;
                    if (File.Exists(sourceFile4))
                    {
                        try
                        {
                            string destFile = Server.MapPath(paths[3]) + ID.UrlName + ".jpg";
                            File.Move(sourceFile4, destFile);
                        }
                        catch (Exception ex)
                        {
                        };
                    }
                    break;
            }
        }
    }

    protected void CreatePolyPoints_Click(object sender, EventArgs e)
    {
        //List<Society> slist = Society.GetAll();
        //foreach (Society s in slist)
        //{ 
              Society s = Society.GetByID(1898);
              string polyPoints = s.PolyPoints;
        //    if (string.IsNullOrEmpty(polyPoints))
        //    {
        //        if (!string.IsNullOrEmpty(s.Lat.ToString())||s.Lat!=0)
        //        {
                    string point1 = (s.Lat + 0.00012232724405).ToString() + "," + (s.Lng - 0.00084757804868).ToString();
                    string point2 = (s.Lat + 0.00076219049697).ToString() + "," + (s.Lng - 0.00026822090146).ToString();
                    string point3 = (s.Lat + 0.00031052272474).ToString() + "," + (s.Lng + 0.00071883201602).ToString();
                    string point4 = (s.Lat - 0.00040462190315).ToString() + "," + (s.Lng + 0.00010728836062).ToString();
                    
                    //string point1 = (s.Lat + 0.0004).ToString() + "," + (s.Lng - 0.0004).ToString();
                    //string point2 = (s.Lat + 0.0004).ToString() + "," + (s.Lng - 0.0004).ToString();
                    //string point3 = (s.Lat + 0.0004).ToString() + "," + (s.Lng + 0.0004).ToString();
                    //string point4 = (s.Lat - 0.0004).ToString() + "," + (s.Lng + 0.0004).ToString();
                    
                    polyPoints = point1 + "^" + point2 + "^" + point3 + "^" + point4;
                    s.PolyPoints = polyPoints;
                    s.Save();
        //     }
        //    }
        //}
    }

    protected void CreateSocietyText_Click(object sender, EventArgs e)
    {
        //List<Society> mainList = new List<Society>();
        //List<Society> tempList = new List<Society>();
        //int[] subcity = { 2, 394, 395 };
        //foreach (int sc in subcity)
        //{
        //    tempList = Society.GetByCityandSubCity(1, sc);
        //    mainList.AddRange(tempList);
        //}

        //StringBuilder sb = new StringBuilder();

        //foreach (Society S in mainList)
        //{
        //    sb.Append(S.SocietyName + "^" + S.Lat + "^" + S.Lng);
        //    sb.Append("&");
        //    sb.Append(Environment.NewLine);
        //}
        
        List<Society> SL = Society.GetByCityandSubCity(1, 395); //2-indrapuram,vaishali-394,vasundhra-395
        StringBuilder sb = new StringBuilder();
        foreach (Society S in SL)
        {
            sb.Append(S.SocietyName + "^" + S.Lat + "^" + S.Lng);
            sb.Append("&");
            sb.Append(Environment.NewLine);
        }
        System.IO.File.WriteAllText(Server.MapPath(@"~\VasundharaApartments.txt"), sb.ToString());
    }
}