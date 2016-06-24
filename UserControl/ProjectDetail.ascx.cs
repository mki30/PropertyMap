using iTextSharp.text;
using iTextSharp.text.pdf;
using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

public partial class UserControl_ProjectDetail : System.Web.UI.UserControl
{
    Society project;
    Agent agent;
    
    public string MetaDescription = "", MetaKeywords = "", Title = "", ID = "0";
    System.Web.UI.Page ParentPage;
    CreatePDF CP;
    public void InitCtl(System.Web.UI.Page _page, Society _Project, Agent _Agent)
    {
        ParentPage = _page;
        project = _Project;
        agent = _Agent;

        if (Request.Browser.IsMobileDevice)
        {
            Response.Clear();
            string txt = File.ReadAllText(Server.MapPath("~/mobile.htm"));

            txt = txt.Replace(@"[PROJECT_TITLE]", "<title>" + project.SocietyName + "</title>").Replace("Loading Project Detail...", project.ProjectDetailMobile()).Replace("//customscript", "isChangePage=true; changeTo='#Details';");
            txt = txt.Replace("//BasePath", "var BasePath='" + Global.GetRootPathVirtual + "/';");
            
            Response.Write(txt);
            Response.End();
        }
        Title = ShowData();
    }

    void CreatePDF()
    {
        CreatePDF CP = new CreatePDF(this.Page, 1, "", "");
        
        List<string> LogoImgs = new List<string>();
        if (File.Exists(Server.MapPath(@"~/Data/Images_SocietyLogo/") + project.ID + ".jpg"))
            LogoImgs.Add(Server.MapPath("/Data/Images_SocietyLogo/" + project.ID + ".jpg"));
        else
            LogoImgs.Add(Server.MapPath("/Images/ImageNotAvl.jpg'>"));
        
        string societyName = project.SocietyName;
        string address = (string.IsNullOrEmpty(project.Subcity) ? "" : project.Subcity + ",") + project.City + ", " + project.State + (!string.IsNullOrEmpty(project.Pin) ? ("-" + project.Pin) : (""));

        string builder = "";
        if (project.Builder != null)
        {
            builder = project.Builder.AgentCompany;
        }
        int months = 0;
        string possDate = "Expected Possession-" + (project.Status != 4 ? (Cmn.ToDate(project.EndDate).Year == 1900 ? "NA" : Cmn.ToDate(project.EndDate).ToString("MMM-yyyy")
                            + (months != 0 ? "<span style='font-size:12px;'> (&nbsp;in&nbsp;" + months + " months&nbsp;)" : "")) : "Compleated");

        float[] tablewidths = new float[] { 150f, 500f, 300f };
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("/Data/Images_SocietyLogo/" + project.ID + ".jpg"));
        img.ScaleAbsolute(80, 40);
        PdfPTable Tbl = new PdfPTable(tablewidths);
        Tbl.WidthPercentage = 100;

        PdfPCell cell1 = new PdfPCell(img);
        cell1.Border = 0;
        Tbl.AddCell(cell1);
        Paragraph para = new Paragraph(societyName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15f, iTextSharp.text.Font.BOLD));
        para.Add(Environment.NewLine);
        para.Add(new Paragraph(address, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f)));
        PdfPCell cell2 = new PdfPCell(para);
        cell2.Border = 0;
        Tbl.AddCell(cell2);
        
        Paragraph para2 = new Paragraph(builder, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f));
        para2.Add(Environment.NewLine);
        para2.Add(new Paragraph(possDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f)));

        PdfPCell cell3 = new PdfPCell(para2);
        cell3.Border = 0;
        Tbl.AddCell(cell3);
        CP.AddTableToColumn(Tbl);

        CP.AddNewLine();
        Society society = Global.ProjectList.Values.FirstOrDefault(m => m.ID == project.ID); //aptTypeTable
        List<string[]> list = society.GetAptDetailsPDF();
        float[] widths = new float[] { 150f, 150f, 50f, 50f, 50f, 50f, 70f };
        CP.AddTable(list, widths);

        CP.AddHeading("Details :");
        string projdetails = project.Description;
        CP.AddText(new string[] { projdetails });

        CP.AddHeading("Heighlights :");
        List<string> Imgs = Society.GetSocietyImageList(project.ID);
        for (int ctr1 = 0; ctr1 < Imgs.Count; ctr1 += 2)
        {
            List<string> Imgs1 = new List<string>();
            try
            {
                Imgs1.Add(Imgs[ctr1]);
                Imgs1.Add(Imgs[ctr1 + 1]);
            }
            catch (Exception ex)
            {
            }
            CP.AddNewLine();
            CP.AddImagesInTable(Imgs1, 260f, 150f);
        }

        CP.AddHeading("Layout Plan :");
        List<string> LayoutImgs = new List<string>();
        if (File.Exists(Server.MapPath(@"~/Data/Images_LayoutPlan/") + project.ID + ".jpg"))
        {
            LayoutImgs.Add(Server.MapPath("/Data/Images_LayoutPlan/" + project.ID + ".jpg"));
            CP.AddNewLine(1);
            CP.AddImagesInTable(LayoutImgs, 550f, 400f);
        }

        CP.AddHeading("Floor Plans :");
        List<string[]> typeImagelist = society.GetAptTypeImage();

        List<string> TypeImgs = new List<string>();
        foreach (string[] aptType in typeImagelist)
        {
            TypeImgs.Add(Server.MapPath("/Data/Images_ApartmentType/" + aptType[0] + ".jpg"));
            CP.AddSubHeading(aptType[1]);
            CP.AddImagesInTable(TypeImgs, 550f, 300f);
            TypeImgs.Clear();
        }
        CP.SetPageInfo(project.URLName, MetaKeywords, "", project.URLName, "");
        CP.PrintInvoice(1);
    }

    string ShowData()
    {
        ID = project.ID.ToString();

        //Society society = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == urlName.ToLower());
        //if (society == null)
        //{
        //    HttpContext.Current.Response.Redirect(@"~/NotFound.aspx");
        //}
        //ID = project.ID.ToString();
        //meta desc and meta keywords

        MetaDescription = "Find complete and latest details of " + project.URLName.Replace('-', ' ') + "-view floor plans,price list,layout plan,map,site plan and other details,Download brochure in an easy way";
        MetaKeywords = project.SocietyName + " , " + (string.IsNullOrEmpty(project.Subcity) ? "" : project.SocietyName + " " + project.Subcity + " " + project.City) + " , " + project.SocietyName + " " + project.City + " , " + project.SocietyName + " floor plans , " + project.SocietyName + " price list , " + project.SocietyName + " layout plan , " + project.SocietyName + " map ," + project.SocietyName + " Site Plan, " + project.SocietyName + " Brochure download";

        if (File.Exists(Server.MapPath("~/Data/KMZ/" + project.SocietyName + ".kmz")))
            GoogleEarth.Attributes["href"] = "Data/KMZ/" + project.SocietyName + ".kmz";
        else
            GoogleEarth.Attributes["href"] = "/kml/" + project.URLName;   //"project.CreateKML();   

        StringBuilder str = new StringBuilder();

        Cmn.WriteClientScript(ParentPage, "var Lat=" + project.Lat + ", Lng=" + project.Lng);

        
        if (File.Exists(Server.MapPath(@"~/Data/Newspaper_Small/") + project.URLName + ".jpg"))
        {
            AdvSection.Visible = true;
            Ad_Image.InnerHtml = "<a href='/Data/Newspaper/" + project.URLName + ".jpg' target='_blank'><img src ='/Data/Newspaper_Small/" + project.URLName + ".jpg" + "' title='" + project.SocietyName
                  + "' alt='" + project.SocietyName + "' /></a>";
        }

        ImagesDetail imgDetails = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == project.ID && m.ImageReferenceType == (int)ImagesLocations.Project_Logo);
        if (imgDetails != null)
        {
            if (File.Exists(Server.MapPath(@"~/Data/Images_SocietyLogo/") + imgDetails.UrlName + ".jpg"))
                lblLogo.Text = "<img src ='/Data/Images_SocietyLogo/" + imgDetails.UrlName + ".jpg" + "' title='" + project.SocietyName
                  + "' alt='" + project.SocietyName + "' class='builderlogo'/>";
        }
        
        else
            lblLogo.Text = "<img src ='/Images/ImageNotAvl.jpg' title='" + project.SocietyName + "'  alt='" + project.SocietyName
           + "' class='builderlogo'/>";

        ltrProjectName.Text = project.SocietyName;
        ltrAddress.Text = (string.IsNullOrEmpty(project.Subcity) ? "" : project.Subcity + ",") + project.City + ", " + project.State + (!string.IsNullOrEmpty(project.Pin) ? ("-" + project.Pin) : (""));

        string desc = "";                   //Description
        if (project.Description != "")
        {
            desc = "<h2>Description</h2><div class='row-fluid' style='text-align: justify;' id='projdesc'>";
            desc += project.Description;
            desc += "</div>";
        }

        ltrDetail.Text = desc;

        string tempVedioLnk = "";
        if (!string.IsNullOrEmpty(project.VedioLink))
            tempVedioLnk = project.VedioLink.Split('/')[3];
        if (string.IsNullOrEmpty(project.VedioLink))
            fancyBoxVedio.Visible = false;
        else
        fancyBoxVedio.Attributes["href"] = string.IsNullOrEmpty(project.VedioLink) ? "" : ("http://www.youtube.com/embed/" + tempVedioLnk);
        if (File.Exists(Server.MapPath(@"~\Data\PDF\" + project.URLName + ".pdf")))
        {
            FileInfo fi = new FileInfo(Server.MapPath(@"~\Data\PDF\" + project.URLName + ".pdf"));
            double size = (double)fi.Length / (1024 * 1024);

            ltBoucher.Text = "<li><a href='/data/pdf/" + project.URLName.ToLower() + ".pdf' target='_blank'><i class='icon-book'></i>E-Brochure (" + size.ToString("0.00") + " Mb) </a></li>";
        }

        ltPriceList.Text = string.IsNullOrEmpty(project.PricelistURL) ? "" : "<li><a href='" + project.PricelistURL + "' target='_blank'><i class='icon-list-alt'></i>Price List</a></li>";
        ltWebsite.Text = string.IsNullOrEmpty(project.URL) ? "" : "<li><a href='" + project.URL + "' target='_blank'><i class='icon-globe'></i>View Official Site</a></li>";
        ltMap.Text = "<a  href='/map/" + project.URLName + "'><i class=' icon-map-marker'></i>Map</a>";

        //ltfbLike.Text = "<div class='script-div-fb' style='width:100px;'><iframe src='//www.facebook.com/plugins/like.php?href=http://propertymap.info/project/" + project.URLName +
        //     "&send=false&width=250&show_faces=false&font&colorscheme=light&layout=button_count&action=like&height=35&appId=281109892002022'" +
        //     " scrolling='no' frameborder='0' allowtransparency='true' style='border: none; overflow: hidden; width:100%; height:20px;'></iframe></div>";

        //ltGplus.Text = "<div class='script-div'><div class='g-plusone' data-size='medium' data-annotation='bubble' data-width='300'></div>" +
        //"<script type='text/javascript'>(function () {var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;po.src = 'https://apis.google.com/js/plusone.js';var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);})();" +
        //"</script>" +
        //"</div></div>";

        if (project.Builder != null)
        {
            BuilderName.InnerText = project.Builder.AgentName;
            builder.InnerHtml = "<a style='color:gainsboro;' href='/" + project.Builder.AgentName.Replace(" ", "-").ToLower() + "' target='_blank'>by " + project.Builder.AgentName + "</a>";
            if (!string.IsNullOrEmpty(project.Builder.AgentName))
                BuilderName.HRef = "/" + project.Builder.AgentName.Replace(" ", "-").ToLower();
            BuilderName.Target = "_blank";

            StringBuilder sb = new StringBuilder();

            List<Society> list = project.Builder.ProjectList.Where(m => m.Verified == 1 && m.ID != project.ID).ToList();
            if (list.Count == 0)
                OtherProjects.Visible = false;

            int ctr = 0;
            int itemsPerCol = (int)Math.Ceiling(list.Count / 3.0);
            foreach (Society S in list)
            {
                if (ctr % itemsPerCol == 0)
                {
                    if (sb.Length > 0)
                        sb.Append("</ul></div>");
                    sb.Append("<div style='width:33%; float:left'><ul style='list-style-type:square;'>");
                }
                sb.Append("<li><a style='padding:0px 5px 0px 0px;' href='/" + S.URLName.ToLower() + "'>" + S.SocietyName + "</a></li>");
                ctr++;
            }
            sb.Append("</ul></div>");
            ltProjects.Text = sb.ToString();
        }

        ltNearBy.Text = GetNearBy(project);                       // Get NearBy Projects 
        ltAgentList.Text = GetAgentListings(project);
        int months = 0;
        if (DateTime.Now < project.EndDate && project.EndDate != null)
        {
            TimeSpan? dt1 = (project.EndDate - DateTime.Now);
            months = (int)((double)dt1.Value.Days / 30.436875);
        }
        
        ltpossessDate.Text = project.Status != 4 ? (Cmn.ToDate(project.EndDate).Year == 1900 ? "NA" : Cmn.ToDate(project.EndDate).ToString("MMM-yyyy")
                            + (months != 0 ? "<span style='font-size:12px;'> (&nbsp;in&nbsp;" + months + " months&nbsp;)" : "</span>")) : "<span style='color:orange;'>Compleated</span>";

        if (agent == null)    //when agent id not avl
            ltAptTypeTable.Text = project.GetAptDetails() != "" ? project.GetAptDetails() : "Unit plans details coming soon.";    //Apartment Type Table
        else    //In case of agent show Availability
        {
            List<Availability> avlList = Availability.GetAvlByAgentIDandProjectID(agent.ID, project.ID);
            List<Availability> saleList = new List<Availability>();
            List<Availability> rentList = new List<Availability>();
            foreach (Availability avl in avlList)
            {
                if (avl.AvailabilityType == 1)
                    saleList.Add(avl);
                else if (avl.AvailabilityType == 0)
                    rentList.Add(avl);
            }

            StringBuilder str1 = new StringBuilder();
            if (saleList.Count > 0)
            {
                str1.Append("<h2>For Sale By " + agent.AgentName + "</h2>");
                str1.Append("<table class='table table-bordered table-hover table-striped table-condensed'><tr><th>Type</th><th>Area</th><th>price</th><th>For</th><th>Avl From</th><th>Posted On</th><th>Description</th><th>FP</th></tr>");
                foreach (Availability avl in saleList)
                {
                    ApartmentType apt;
                    if (Global.ApartmentTypeList.TryGetValue((int)avl.ApartmentTypeID, out apt))// if the apartment type exists in the global list
                    {
                        string avlUrl = !(string.IsNullOrEmpty(avl.URL)) ? "/" + avl.URL + "" : "#";
                        str1.Append("<tr><td class='tdminwidth'><a href='" + avlUrl + "'>" + apt.Bedroom + "B-" + apt.Bathroom + "T</a></td><td class='tdminwidth'>" + apt.SuperArea + "</td><td class='tdminwidth'>" + ConvertPrice(avl.Amount.ToString()) + "</td><td class='tdminwidth'>" + (avl.AvailabilityType == 1 ? "Sale" : "Rent") + "</td><td class='tdminwidth2'>" + Cmn.ToDate(avl.DateAvailableFrom).ToString("dd-MMM-yy") +
                             "</td><td class='tdminwidth2'>" + Cmn.ToDate(avl.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>" + avl.Description + "</td>" +
                        //"<td><a href='/Data/Images_ApartmentType/" + apt.ID + ".jpg' class='fancyBox' rel='group' title='" + apt.Bedroom + "B-" + apt.Bathroom + "T (" + apt.SuperArea + " sqft.) flat' ><img alt='FP' src='" + Global.GetRootPathVirtual + "/Images/fp-an.gif' title='Floor Plan'/></a></td></tr>");
                        "<td><a href='/Data/Images_ApartmentType/" + apt.ID + ".jpg' title='" + apt.Bedroom + "B-" + apt.Bathroom + "T (" + apt.SuperArea + " sqft.) flat' >view</a></td></tr>");
                    }
                }
                str1.Append("</table>");
            }

            else if (rentList.Count > 0)
            {
                str1.Append("<h2>For Rent By " + agent.AgentName + "</h2>");
                str1.Append("<table class='table table-bordered table-hover table-striped table-condensed'><tr><th>Type</th><th>Area</th><th>price</th><th>For</th><th>Avl From</th><th>Posted On</th><th>Description</th><th>FP</th></tr>");
                foreach (Availability avl in rentList)
                {
                    string avlUrl = !(string.IsNullOrEmpty(avl.URL)) ? "/" + avl.URL + "" : "#";
                    ApartmentType apt;
                    if (Global.ApartmentTypeList.TryGetValue((int)avl.ApartmentTypeID, out apt))// if the apartment type exists in the global list
                    {
                        str1.Append("<tr><td class='tdminwidth'><a href='" + avlUrl + "'>" + apt.Bedroom + "B-" + apt.Bathroom + "T<a></td><td class='tdminwidth'>" + apt.SuperArea + "</td><td class='tdminwidth'>" + avl.Amount.ToString() + "</td><td class='tdminwidth'>" + (avl.AvailabilityType == 1 ? "Sale" : "Rent") + "</td><td class='tdminwidth2'>" + Cmn.ToDate(avl.DateAvailableFrom).ToString("dd-MMM-yy") +
                             "</td><td class='tdminwidth2'>" + Cmn.ToDate(avl.PostedOnDate).ToString("dd-MMM-yy") + "</td><td>" + avl.Description + "</td>" +
                        //"<td><a href='/Data/Images_ApartmentType/" + apt.ID + ".jpg' class='fancyBox' rel='group' title='" + apt.Bedroom + "B-" + apt.Bathroom + "T (" + apt.SuperArea + " sqft.) flat'><img alt='FP' src='" + Global.GetRootPathVirtual + "/Images/fp-an.gif' title='Floor Plan'/></a></td></tr>");
                        "<td><a href='/Data/Images_ApartmentType/" + apt.ID + ".jpg' title='" + apt.Bedroom + "B-" + apt.Bathroom + "T (" + apt.SuperArea + " sqft.) flat' >view</a></td></tr>");
                    }
                }
                str1.Append("</table>");
            }
            ltAptTypeTable.Text = str1.ToString();

            ProjectList.Visible = true;
            PropList.InitCtlAgentList(agent, 1);
            OtherProjects.Visible = false;
            NearbyProjects.Visible = false;
            AgentsList.Visible = false;
        }

        ltrGallary.Text = Society.GetSocietyImage(project.ID,project.URLName);
        if (ltrGallary.Text != "")
            ImagesHead.InnerHtml = "Images";
        ltrLayout.Text = GetLayoutImage(project.ID,project.SocietyName);
        if (ltrLayout.Text != "")
            LayoutHead.InnerHtml = "Site Layout";

        return project.URLName.Replace("-", " ") + " " + (agent != null ? agent.AgentCompany : "");
    }

    private string ConvertPrice(string price)
    {
        price = (Cmn.ToInt(price) / 100000).ToString("0") + " L";
        string temp = price.Replace("L", "").Trim();
        int z = (int)Cmn.ToDbl(temp) / 100;
        if (((int)Cmn.ToDbl(temp) / 100 > 0))
        {
            price = (Cmn.ToDbl(temp) / 100).ToString("0.00") + " Cr";
        }
        return price;
    }

    private string GetAgentListings(Society S)
    {
        List<Availability> avlList = Global.AvailabilityList.Values.Where(m => m.SocietyID == S.ID).ToList();

        List<Agent> temp = new List<Agent>();
        foreach (Availability avl in avlList)
        {
            Agent A = Global.AgentList.Values.FirstOrDefault(m => m.ID == avl.SellerID);
            if (!temp.Contains(A) && A != null)
                temp.Add(A);
        }
        if (temp.Count == 0)
            AgentsList.Visible = false;
        else
            return "";
        int ctr = 0;
        int itemsPerCol = (int)Math.Ceiling(temp.Count / 3.0);
        StringBuilder sb = new StringBuilder();
        foreach (Agent a in temp.OrderBy(m => m.AgentName))
        {
            if (ctr % itemsPerCol == 0)
            {
                if (sb.Length > 0)
                    sb.Append("</ul></div>");
                sb.Append("<div style='width:33%; float:left'><ul style='list-style-type:square;'>");


                if (string.IsNullOrWhiteSpace(a.AgentName))
                    continue;
            }
            sb.Append("<li><a style='padding:0px 5px 0px 0px;' href='/" + a.URLName.ToLower() + "'>" + a.AgentName + "</a></li>");
            ctr++;
        }
        sb.Append("</ul></div>");
        return sb.ToString();
    }

    private string GetNearBy(Society society)
    {
        List<Society> nearBy = Global.ProjectList.Values.Where(m => m.CityID == Cmn.ToInt(project.CityID) && m.Verified == 1).ToList();
        Dictionary<Society, double> dist = new Dictionary<Society, double>();

        foreach (Society S in nearBy)
        {
            try
            {
                if (S == society) continue; //avoid add same project in dict
                if (S.Lat != null && project.Lat != null)
                {
                    if (project.Lat != 0 && S.Lat != 0)
                    {
                        double d = Cmn.CalcDistance((double)project.Lat, (double)project.Lng, (double)S.Lat, (double)S.Lng);
                        if (!dist.ContainsKey(S))
                            dist.Add(S, d);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        if (dist.Count == 0)
        {
            NearbyProjects.Visible = false;
            return "";
        }
        
        StringBuilder sb = new StringBuilder();
        int ctr = 0;
        int take = 10;
        int itemsPerCol = (int)Math.Ceiling(take / 3.0);
        foreach (var d in dist.OrderBy(m => m.Value).Take(take))
        {
            if (ctr % itemsPerCol == 0)
            {
                if (sb.Length > 0)
                    sb.Append("</ul></div>");

                sb.Append("<div style='width:33%; float:left'><ul style='list-style-type:square;'>");
            }
            sb.Append("<li><a style='padding:0px 5px 0px 0px;' href='/" + d.Key.URLName.ToLower() + "'>" + d.Key.SocietyName + "</a><span style='font-size:11px;'>(" + (d.Value).ToString("0.0") + "km)</span></li>");
            ctr++;
        }
        sb.Append("</ul></div>");
        return sb.ToString();
    }
    
    private string GetLayoutImage(int dataId,string ProjectName)
    {
        ImagesDetail imgLayout = Global.ImageDetailsList.FirstOrDefault(m => m.ReferenceID == project.ID && m.ImageReferenceType == (int)ImagesLocations.Project_Layout);
        string layout = "";
        if (imgLayout != null)
        {
            string imgAlt = ProjectName.Replace("-", " ") + " Layout Plan";

            string hrefLargeLyout = "/Data/Images_LayoutPlan/" + imgLayout.UrlName + ".jpg";
            if(File.Exists(Server.MapPath("/Data/Images_LayoutPlan_Large/" + imgLayout.UrlName + ".jpg")))
            hrefLargeLyout = "/Data/Images_LayoutPlan_Large/" + imgLayout.UrlName + ".jpg";
            
            layout = "<span style='margin:0px 3px 0px 3px;'><a href='"+hrefLargeLyout+"' target='_blank'><img id='layout' class='img-polaroid' style='border:1px solid #eee; width:98.5%;'  src='/Data/Images_LayoutPlan/" + imgLayout.UrlName + ".jpg'  alt='" + imgAlt + "'/></a></span>";
        }
        return layout;
        
        //string[] imageFiles = null;
        //string path = "~/Data/Watermark/Images_LayoutPlan/";
        //imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        //if (imageFiles.Length == 0)
        //{
        //    path = "~/Data/Images_LayoutPlan/";
        //    imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        //}
        //string imgAlt = ProjectName.Replace("-", " ") + " Layout Plan";
        //return imageFiles.Length > 0 ? "<span style='margin:0px 3px 0px 3px;'><img id='layout' class='img-polaroid' style='border:1px solid #eee; width:98.5%;'  src='" + ResolveClientUrl(path) + Path.GetFileName(imageFiles[0]) + "'  alt='"+imgAlt+"'/></span>" : "";
    }

    //public string GetSocietyImage(int dataId)     //Society Image
    //{
    //    var img = "";
    //    var path = "/Data/Watermark/Images_Society/";
    //    string[] imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + "_*.*");
    //    if (imageFiles.Length == 0)
    //    {
    //        path = "/Data/Images_Society/";
    //        imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + "_*.*");
    //    }
    //    int ctr = 1;
    //    var isfirst = true;
    //    foreach (var image in imageFiles)
    //    {
    //        if (ctr == 1 && isfirst)
    //        {
    //            img += "<div class='row-fluid'>";
    //            isfirst = false;
    //        }
    //        img += "<div class='span6' style='margin-top:5px;'><img class='img-polaroid' style='border:1px solid #eee; width:98%; height:230px;' src='" + path + Path.GetFileName(image) + "' alt=''/></div>";

    //        if (ctr == 2)
    //        {
    //            img += "</div>";
    //            ctr = 1;
    //            isfirst = true;
    //        }
    //        if (ctr == 1 && isfirst != true)
    //        {
    //            ctr = 2;
    //        }
    //    }
    //    return img;
    //}
    protected void DownloadPdf_ServerClick(object sender, EventArgs e)
    {
        CreatePDF();
    }
}