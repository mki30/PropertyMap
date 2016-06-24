using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class UserControl_AvlDetail : System.Web.UI.UserControl
{
    public string MetaDescription = "", MetaKeywords = "", Title = "", ID = "0";
    System.Web.UI.Page ParentPage;
    Availability avl = null;
    Agent AG = null;
    public void InitCtl(System.Web.UI.Page _page, Availability _avl)
    {
        avl = _avl;
        ShowDetials(avl);
    }

    Society project = null;
    List<string[]> AvltableData = new List<string[]>();
    private void ShowDetials(Availability avl)
    {
        Title = avl.URL;
        MetaKeywords = "";
        MetaDescription = "";
        project = avl.Society;
        
        StringBuilder sb = new StringBuilder("<table class='table table-striped table-bordered'>");
        int days = (DateTime.Now - Cmn.ToDate(avl.PostedOnDate)).Days;
        sb.Append("<tr><td><span class='bold'>Added On : </span>" + days + " days ago </td><td><span class='bold'>Avl From : </span>" + Cmn.ToDate(avl.DateAvailableFrom).ToString("dd-MMM-yy") + "</td></tr>");
        sb.Append("<tr><td><span class='bold'>Society : </span><a href='/" + avl.Society.URLName.ToLower() + "'>" + avl.Society.SocietyName + "<a></td><td><span class='bold'>Type : </span>" + GetAptType(Cmn.ToInt(avl.ApartmentTypeID)) + " </td></tr>");
        ApartmentType AT = Global.ApartmentTypeList.Values.FirstOrDefault(m => m.ID == avl.ApartmentTypeID);
        ltOverviewHead.Text = AT.Bedroom + " BHK-" + avl.Society.SocietyName;
        AG = Global.AgentList.Values.FirstOrDefault(m => m.ID == avl.SellerID);
        sb.Append("<tr><td><span class='bold'>Area : </span>" + AT.SuperArea + " (sqft.)</td><td><span class='bold'>For : </span>" + (avl.AvailabilityType.ToString() == "1" ? "Sale" : "Rent") + "</td></tr>");
        sb.Append("<tr><td><span class='bold'>Floor No : </span>" + avl.FloorNo + "</td><td><span class='bold'>" + (avl.AvailabilityType.ToString() == "1" ? "Price : " : "Rent : ") + "</span>" + Cmn.ConvertPriceToLandCr(avl.Amount.ToString()) + "</td></tr>");
        if (!string.IsNullOrEmpty(avl.Description))
        {
            sb.Append("<tr><td colspan='2'><span class='bold'>Other Detail : </span>" + avl.Description + "</td></tr>");
        }
        sb.Append("</table>");

        societyImg.Src = "/Data/Images_SocietyLogo/" + avl.Society.ID+".jpg";
        societyImg.Alt = "";
        ltAvaldetails.Text = sb.ToString();
        
        int a=(DateTime.Now - Cmn.ToDate(avl.DateAvailableFrom)).Days;
        AvltableData.Add(new string[] { "Status : " +( a<0? Cmn.ToDate(avl.DateAvailableFrom).ToString("dd-MMM-yy").ToString() : "Available"), "" });
        AvltableData.Add(new string[] { "Society : " + avl.Society.SocietyName, "Type : " + GetAptType(Cmn.ToInt(avl.ApartmentTypeID))});
        AvltableData.Add(new string[] { "Area : " + AT.SuperArea, "For : " +(avl.AvailabilityType.ToString() == "1" ? "Sale" : "Rent")});
        AvltableData.Add(new string[] { "Floor No : " + (avl.FloorNo == null ? "Not Avl" : avl.FloorNo.ToString()), "Price : " + Cmn.ConvertPriceToLandCr(avl.Amount.ToString()) });
        //AvltableData.Add(new string[] { "Other Detail : " + avl.Description, ""});

        ltImages.Text = GetImages(avl.ID);
        //ltDescription.Text = avl.Description;
        ltProjDescHead.Text = avl.Society.SocietyName + "- Description";
        ltProjectDesc.Text = avl.Society.Description;
        ltProjectGalleryHead.Text = avl.Society.SocietyName + "-Images";
        ltProjectGallery.Text = Society.GetSocietyImage(avl.Society.ID,avl.Society.URLName);
        ltProjectLayoutHead.Text = avl.Society.SocietyName + "-Layout Plan";
        ltProjectLayout.Text = GetLayoutImage(avl.Society.ID);
        if (AG != null)
            ltContact.Text = GetAgentContact(AG);
        GetOtherAvailabilities(Cmn.ToInt(avl.SellerID));
    }

    private string GetAgentContact(Agent AG)
    {
        StringBuilder sb = new StringBuilder();
        string imgPath = @"/Data/Images_Agent/" + AG.URLName + ".jpg";
        sb.Append("<div class='span2'><img class='img-polaroid' style='border:1px solid #eee; width:98%; height:50px;' src='" + imgPath + "' alt=''/></div>");
        sb.Append("<div class='span10'>" +
            "<table>" +
             "<tr><td><span class='bold'>Contact : " + AG.Mobile1 + " </span></td></tr>" +
             "<tr><td><span class='bold'>Address : " + AG.Address + "</span></td></tr>" +
             "<tr><td><span class='bold'><a href='/" + AG.URLName + "'>View Agent Profile</a></span></td></tr>" +
             "</table>" +
            "</div>");
        return sb.ToString();
    }

    private void GetOtherAvailabilities(int AgentID)
    {
        StringBuilder sb = new StringBuilder("");
        List<Availability> avllist = Global.AvailabilityList.Values.Where(m => m.SellerID == AgentID).ToList();

        Dictionary<string, int> list = new Dictionary<string, int>();

        foreach (Availability A in avllist)
        {
            string projectname = "";
            Society S = Global.ProjectList.Values.FirstOrDefault(m => m.ID == A.SocietyID);
            projectname = S.SocietyName;

            string type = "";

            ApartmentType AT = Global.ApartmentTypeList.Values.FirstOrDefault(m => m.ID == A.ApartmentTypeID);

            type = AT.Bedroom + "BHK apartment in " + projectname;

            if (!string.IsNullOrEmpty(A.URL))
                list.Add("<a href='/" + A.URL + "'>" + type + " (" + Cmn.ConvertPriceToLandCr(A.Amount.ToString()) + ")</a>",(int) AT.Bedroom);
        }

        string LastTxt = "";
        foreach (KeyValuePair<string, int> kvp in list.OrderBy(m => m.Value))
        {
            string BHK = kvp.Value.ToString();
            if (LastTxt != BHK)
                sb.Append("<h5 style='background-color:#ddd;'>" + BHK + " BHK</h5>");

            LastTxt = BHK;
            sb.Append("<div>" + kvp.Key + ",</div>");
        }
        ltOtherPostings.Text = sb.ToString();
    }

    private string GetAptType(int ApartmentTypeID)
    {
        ApartmentType AT = Global.ApartmentTypeList.Values.FirstOrDefault(m => m.ID == ApartmentTypeID);
        string type = "";
        if (AT != null)
            type = (AT.Bedroom + "B - " + AT.Bedroom + "T");
        return type;
    }

    private string GetLayoutImage(int dataId)
    {
        string[] imageFiles = null;
        string path = "~/Data/Watermark/Images_LayoutPlan/";
        imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        if (imageFiles.Length == 0)
        {
            path = "~/Data/Images_LayoutPlan/";
            imageFiles = File.Exists(Server.MapPath(path + dataId + ".jpg")) ? Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".jpg") : Directory.GetFiles(HttpContext.Current.Server.MapPath(path), dataId + ".gif");
        }
        return imageFiles.Length > 0 ? "<span style='margin:0px 3px 0px 3px;'><img id='layout' class='img-polaroid' style='border:1px solid #eee; width:98.5%;'  src='" + ResolveClientUrl(path) + Path.GetFileName(imageFiles[0]) + "'  alt='image'/></span>" : "";
    }

    private string GetImages(int ID)
    {
        var img = "";
        var path = "~/Data/Watermark/Images_Society/";
        string[] imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), ID + "_*.*");
        if (imageFiles.Length == 0)
        {
            path = "~/Data/Images_Availability/";
            imageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(path), ID + "_*.*");
        }
        int ctr = 1;
        var isfirst = true;
        foreach (var image in imageFiles)
        {
            if (ctr == 1 && isfirst)
            {
                img += "<div class='row-fluid'>";
                isfirst = false;
            }
            img += "<div class='span6' style='margin-top:5px;'><img class='img-polaroid' style='border:1px solid #eee; width:98%; height:230px;' src='" + ResolveClientUrl(path) + Path.GetFileName(image) + "' alt=''/></div>";

            if (ctr == 2)
            {
                img += "</div>";
                ctr = 1;
                isfirst = true;
            }
            if (ctr == 1 && isfirst != true)
            {
                ctr = 2;
            }
        }
        return img;
    }

    void CreatePDF()
    {
        CreatePDF CP = new CreatePDF(this.Page, 1, "", "");
        //CP.AddHeading("Heading");

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
        
        //BuilderInfo-top
        float[] tablewidths1 = new float[] { 150f, 500f, 300f};
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("/Data/Images_Agent/" + AG.URLName + ".jpg"));
        img.ScaleAbsolute(80, 40);
        PdfPTable Tbl = new PdfPTable(tablewidths1);
        Tbl.WidthPercentage = 100;
        PdfPCell cell1 = new PdfPCell(img);
        cell1.Border = 0;
        Tbl.AddCell(cell1);

        Paragraph para = new Paragraph(AG.AgentName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15f, iTextSharp.text.Font.BOLD));
        para.Add(Environment.NewLine);
        para.Add(new Paragraph(AG.Address, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f)));
        PdfPCell cell2 = new PdfPCell(para);
        cell2.Border = 0;
        Tbl.AddCell(cell2);

        Paragraph para2 = new Paragraph("Mobile :" + AG.Mobile1, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15f, iTextSharp.text.Font.BOLD));
        PdfPCell cell3 = new PdfPCell(para2);
        cell3.Border = 0;
        Tbl.AddCell(cell3);
        
        CP.AddTableToColumn(Tbl);
        CP.AddNewLine();

        //projecrinfo-top
        float[] tablewidths2 = new float[] { 150f, 500f, 300f };
        iTextSharp.text.Image projLogoImg = iTextSharp.text.Image.GetInstance(Server.MapPath("/Data/Images_SocietyLogo/" + project.ID + ".jpg"));
        projLogoImg.ScaleAbsolute(80, 40);
        PdfPTable Tbl2 = new PdfPTable(tablewidths2);
        Tbl2.WidthPercentage = 100;

        PdfPCell Tbl2cell1 = new PdfPCell(projLogoImg);
        Tbl2cell1.Border = 0;
        Tbl2.AddCell(Tbl2cell1);
        Paragraph Tbl2para = new Paragraph(societyName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15f, iTextSharp.text.Font.BOLD));
        Tbl2para.Add(Environment.NewLine);
        Tbl2para.Add(new Paragraph(address, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f)));
        PdfPCell Tbl2cell2 = new PdfPCell(Tbl2para);
        Tbl2cell2.Border = 0;
        Tbl2.AddCell(Tbl2cell2);

        Paragraph Tbl2para2 = new Paragraph(builder, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f));
        Tbl2para2.Add(Environment.NewLine);
        Tbl2para2.Add(new Paragraph(possDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f)));

        PdfPCell Tbl2cell3 = new PdfPCell(Tbl2para2);
        Tbl2cell3.Border = 0;
        Tbl2.AddCell(Tbl2cell3);
        CP.AddTableToColumn(Tbl2);
        
        CP.AddNewLine();

        //AvlTypeTable
        float[] aptWidths = new float[] { 400f, 400f };
        CP.AddTable(AvltableData, aptWidths);
        CP.AddText(new String[] { "Other Details : "+avl.Description });
        
        //Floor Plan
        CP.AddHeading("Floor Plan :");
        List<string> FPImgs = new List<string>();
        if (File.Exists(Server.MapPath(@"~/Data/Images_ApartmentType/") + avl.ApartmentTypeID + ".jpg"))
        {
            FPImgs.Add(Server.MapPath("/Data/Images_ApartmentType/" + avl.ApartmentTypeID + ".jpg"));
            CP.AddNewLine(1);
            CP.AddImagesInTable(FPImgs, 550f, 400f);
        }
        
        Society society = Global.ProjectList.Values.FirstOrDefault(m => m.ID == project.ID);
        
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
        CP.SetPageInfo(project.URLName, MetaKeywords, "", project.URLName, "");
        CP.PrintInvoice(1);
    }
    protected void DownloadPdf_ServerClick(object sender, EventArgs e)
    {
        CreatePDF();
    }
}