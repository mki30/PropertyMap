using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text.RegularExpressions;

public class PDFPrinter
{
    Page page;
    
    public PDFPrinter(Page page)
    {
        this.page = page;
    }

    public static int FileLength(string HTML)
    {
        int Length = 0;
        try
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("PDFFile.pdf", 500000))
            {
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    Document doc = new Document(PageSize.A4, 15f, 15f, 10f, 10f);
                    ConvertHTMLToPDF(HTML.ToString(), doc, stream);
                    Length = (int)stream.Position;
                }
            }
        }
        catch (Exception ex)
        {
            Cmn.LogError(ex, "PrintInvoice_PrintPDf");
        }
        return Length;
    }

    public void CreatePDFInvoice(string HTML)
    {
        Document doc;
        doc = new Document(PageSize.A4, 15f, 15f, 10f, 10f);
        ConvertHTMLToPDF(HTML.ToString(), doc);
        page.Response.ContentType = "Application/pdf";
        //page.Response.AddHeader("content-disposition", "attachment; filename= PDFFile.pdf");
        page.Response.End();
    }


    public void CreateMenuHTML(string HTML)
    {
        Document doc;
        doc = new Document(PageSize.A4, 35f, 35f, 20f, 20f);
        ConvertHTMLToPDF(HTML, doc);
        page.Response.Write(doc);
        page.Response.ContentType = "Application/pdf";
        page.Response.End();
    }

    public class itsEvents : PdfPageEventHelper
    {
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            PdfPTable footerURL = new PdfPTable(3);
            footerURL.TotalWidth = 750;
            PdfPCell cellURL = new PdfPCell(new Paragraph("Page - " + writer.PageNumber, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.LIGHT_GRAY)));
            cellURL.Border = 0;
            footerURL.AddCell(cellURL);

            cellURL = new PdfPCell(new Paragraph("eRail.in Wishes you Happy Journey", FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.RED)));
            cellURL.Border = 0;
            footerURL.AddCell(cellURL);

            cellURL = new PdfPCell(new Paragraph(DateTime.Now.ToString("dd-MMM-yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.LIGHT_GRAY)));
            cellURL.Border = 0;
            footerURL.AddCell(cellURL);

            footerURL.WriteSelectedRows(0, -1, 15, 25, writer.DirectContent);

            
        }
    }
   public static void ConvertHTMLToPDF(string HTML, Document doc, Stream stream=null)
    {
        if(doc==null)
            doc = new Document(PageSize.A4, 35f, 35f, 20f, 20f);

        //HTML = Regex.Replace(HTML, "</?(a|A).*?>", "");
        //HTML = HTML.Replace("\r\n", "<br>");
        
        StringReader reader = new StringReader(HTML);

        HTMLWorker parser = new HTMLWorker(doc);
        if (stream != null)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false;
        }
        else
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);
            //itsEvents ev = new itsEvents();
            //writer.PageEvent = ev;
        }
        doc.Open();
        doc.AddAuthor("eRail.in");
        doc.AddTitle("Train List - eRail.in");
        doc.AddKeywords("Train List - eRail.in, List of trains between two station");
        
        try
        {
            parser.Parse(reader);
        }
        catch (Exception ex) { }
        finally 
        {
             doc.Close();
        }
       
    }
}