using System;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class CreatePDF
{
    Page page;
    StringBuilder HTML = new StringBuilder("");
    MultiColumnText columns = new MultiColumnText();
    Document doc = new Document(PageSize.A4, 50f, 15f, 30f, 30f);

    static string Hotel="",City = "";
    public static string PDFHeader ="";
    public static string PDFKeywords="";
    public static string PDFSubject = "";
    public static string PDFTitle="";
    public static string PDFOf = "Hotel";

    public void SetPageInfo(string Header, string Keywords, string Subject, string Title, string pdfOf="Hotel")
    {
        PDFHeader = Header;
        PDFKeywords = Keywords;
        PDFSubject = Subject;
        PDFTitle = Title;
        PDFOf = pdfOf;
    }
    
    public CreatePDF(Page page, int Columns, string HotelName, string CityName)
    {
        this.page = page;
        Hotel = HotelName;
        City = CityName;
        
        doc.SetPageSize(iTextSharp.text.PageSize.A4);
        columns.AddRegularColumns(20f, doc.PageSize.Width - 36f, 10f, Columns);
        HTML.Clear();
    }

    public void SetPageSize(float Width, float Height)
    {
        doc.SetPageSize(new Rectangle(Width, Height));
    }

    public void PrintPDf()
    {
        new PDFPrinter(page).CreatePDFInvoice(HTML.ToString());
    }

    public void PrintInvoice(int Columns = 3)
    {
        ConvertHTMLToPDF(HTML.ToString(), doc);
        page.Response.ContentType = "Application/pdf";
        page.Response.End();
    }

    public void AddText(string[] Data)
    {
        foreach (string s in Data)
        {
            String result = Regex.Replace(s, @"<[^>]*>", String.Empty);
            Paragraph para = new Paragraph(result, new Font(Font.FontFamily.HELVETICA, 9f));
            para.Alignment = 3;
            para.SetLeading(1f, 2f);
            columns.AddElement(para);
        }
    }

    public void AddHeading(string Data)
    {
         Paragraph para = new Paragraph(Data, FontFactory.GetFont(FontFactory.TIMES, 14, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY));
         para.SpacingAfter = 8;
         para.SpacingBefore = 3;
         columns.AddElement(para);
    }

    public void AddSubHeading(string Data)
    {
        Paragraph para = new Paragraph(Data, FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLDITALIC, BaseColor.GRAY));
        para.SpacingAfter = 8;
        para.SpacingBefore = 3;
        columns.AddElement(para);
    }

    public void AddTable(List<string[]> Rows,float[] widths)
    {
        PdfPTable Tbl = new PdfPTable(widths);
        Tbl.WidthPercentage = 100;

        int i=0;
        foreach(string[] Data in Rows)
        {
            foreach (string td in Data)
            {
                PdfPCell cell1 = new PdfPCell(new Paragraph(td));
                cell1.Border = 1;
                cell1.BackgroundColor = i%2==0 ? BaseColor.WHITE : BaseColor.WHITE;
                Tbl.AddCell(cell1);
            }
            i++;
        }
        columns.AddElement(Tbl);
        
    }

    public void AddImagesInTable(List<string> Images, float width = 0f, float height = 0f)
    {
        PdfPTable Tbl = new PdfPTable(Images.Count);
        Tbl.WidthPercentage = 100;
        foreach (string ImageURL in Images)
        {
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ImageURL);
            //img.ScaleToFit(width, height);
            img.ScaleAbsolute(width, height);
            //if(width!=0||height!=0)
            //img.ScaleToFit(width, height);
            //else
            //    img.ScalePercent(50f);
            PdfPCell cell1 = new PdfPCell(img);
            cell1.Border = 0;
            Tbl.AddCell(cell1);
        }
        columns.AddElement(Tbl);
    }
    
    public void AddTableToColumn(PdfPTable Tbl)
    {
        columns.AddElement(Tbl);
    }

    //public void AddImageAndTextInTable(string ImageURL, string text1, string text2)
    //{
    //    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ImageURL);
    //    img.ScaleAbsolute(80, 50);
    //    PdfPTable Tbl = new PdfPTable(3);
    //    Tbl.WidthPercentage = 100;
    //    for (int i = 1; i <= 3; i++)
    //    {
    //        if (i == 1)
    //        {
    //            PdfPCell cell1 = new PdfPCell(img);
    //            cell1.Border = 0;
    //            Tbl.AddCell(cell1);
    //        }
    //        else if (i == 2)
    //        {
    //            Paragraph para = new Paragraph(text1, new Font(Font.FontFamily.HELVETICA, 9f));
    //            PdfPCell cell2 = new PdfPCell(para);
    //            cell2.Border = 0;
    //            Tbl.AddCell(cell2);
    //        }
    //        else
    //        {
    //            Paragraph para = new Paragraph(text2, new Font(Font.FontFamily.HELVETICA, 9f));
    //            PdfPCell cell3 = new PdfPCell(para);
    //            cell3.Border = 0;
    //            Tbl.AddCell(cell3);
    //        }
    //    }
    //   columns.AddElement(Tbl);
    //}

    public void AddImageWithDetails(string ImageURL, string[] Details)
    {
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ImageURL);
        img.ScalePercent(35f);
        PdfPTable Tbl = new PdfPTable(new float[] { 100f,  150f });
        PdfPCell cell1 = new PdfPCell(img);
        cell1.Border = 0;
        string T = "";
        foreach (string s in Details)
        {
            T += s + Environment.NewLine;
        }

        PdfPCell cell2 = new PdfPCell(new Paragraph(T, FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell2.Border = 0;
        Tbl.AddCell(cell1);
        Tbl.AddCell(cell2);
        columns.AddElement(Tbl);
    }

    public void AddNewLine(int NumberofLine=1)
    {
        for(int i=1;i<=NumberofLine;i++)
            columns.AddElement(new Paragraph(Environment.NewLine, FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
    }

    public void AddHeadingWithDetails(string Heading, string Details, float HeadingColumnWidth=100, float TextCoumnWidth=200, string Alignment="Left")
    {
        PdfPTable Tbl = new PdfPTable(new float[] { HeadingColumnWidth, 10f, TextCoumnWidth });
        Tbl.TotalWidth = 800;
        Tbl.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell cell1 = new PdfPCell(new Paragraph(Heading, FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD, BaseColor.GRAY)));
        cell1.Border = 0;
        cell1.HorizontalAlignment = 2;
        PdfPCell cell2 = new PdfPCell(new Paragraph(":"));
        cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Paragraph(Details, FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell3.Border = 0;
        cell3.HorizontalAlignment = Alignment == "Left" ? 0 : 2;
        Tbl.AddCell(cell1);
        Tbl.AddCell(cell2);
        Tbl.AddCell(cell3);
        columns.AddElement(Tbl);
    }

    public void AddImage(string[] Data)
    {
        iTextSharp.text.Image img;
        foreach (string s in Data)
        {
            if (s.Trim() != "")
            {
                img = iTextSharp.text.Image.GetInstance(s);
                img.SpacingAfter = 20f;
                img.SpacingBefore = 20f;
                columns.AddElement(img);
            }
        }
    }

    public void AddRow(List<List<string>> table, string[] Data)
    {
        List<string> Row = new List<string>();

        foreach (string s in Data)
            Row.Add(s);
        table.Add(Row);
    }

    public class itsEvents : PdfPageEventHelper
    {
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            if (PDFHeader == "")
            {
                PDFHeader = Hotel + "eStay.in - Hotels in " + City;
                PDFKeywords = "Hotels, eStay.in, Hotels in " + City + "," + Hotel;
                PDFSubject = "eStay.in -" + Hotel + ",  Hotels in " + City;
                PDFTitle = Hotel + " - Hotels in New Delhi ";
            }

            //Header
            PdfPTable headerTbl = new PdfPTable(1);
            headerTbl.TotalWidth =800;
            headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfPCell cell1 = new PdfPCell(new Paragraph(Hotel, FontFactory.GetFont(FontFactory.TIMES, 14, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY)));
            cell1.Border = 0;
            cell1.PaddingLeft = 10;
            headerTbl.AddCell(cell1);

            switch(PDFOf)
            {
                case "Hotel": 
                    headerTbl.WriteSelectedRows(0, 10, 50, 590, writer.DirectContent);
                    break;
                case "Booking":
                    headerTbl.WriteSelectedRows(0, 10, 10, 290, writer.DirectContent);
                    break;
            }

            if (PDFOf == "Hotel")
            {
                //Footer
                Paragraph footer = new Paragraph("Page - " + writer.PageNumber, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                footer.Alignment = Element.ALIGN_RIGHT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = 300;
                footerTbl.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.PaddingLeft = 10;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, 715, 25, writer.DirectContent);

                PdfPTable footerURL = new PdfPTable(1);
                footerURL.TotalWidth = 500;
                PdfPCell cellURL = new PdfPCell(new Paragraph(HttpContext.Current.Request.Url.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.LIGHT_GRAY)));
                cellURL.Border = 0;
                footerURL.AddCell(cellURL);

                footerURL.WriteSelectedRows(0, -1, 15, 25, writer.DirectContent);
            }

            //Page Border

            //var content = writer.DirectContent;
            //var pageBorderRect = new Rectangle(document.PageSize);
           
            //pageBorderRect.Left += document.LeftMargin - 40;
            //pageBorderRect.Right -= document.RightMargin - 5;
            //pageBorderRect.Top -= document.TopMargin - 18;
            //pageBorderRect.Bottom += document.BottomMargin - 5 ;

            //content.SetColorStroke(BaseColor.DARK_GRAY);
            //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            //content.Stroke();

            document.AddAuthor("eStay.in");
            document.AddCreator("eStay.in");
            document.AddHeader(Hotel, "eStay.in - Hotels in " + City);
            document.AddKeywords("Hotels, eStay.in, Hotels in " + City + "," + Hotel);
            document.AddSubject("eStay.in -" + Hotel + ",  Hotels in " + City);
            document.AddTitle(Hotel + " - Hotels in New Delhi ");
        }
    }
    
    public void ConvertHTMLToPDF(string HTML, Document doc, Stream stream = null)
    {
        if (stream != null)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false;
        }
        else
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);
            itsEvents ev = new itsEvents();
            writer.PageEvent = ev;
        }
        doc.Open();
        doc.Add(columns);
        if (doc.IsOpen())
            doc.Close();
    }
}