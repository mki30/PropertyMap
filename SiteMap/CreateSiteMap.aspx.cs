using System;
using System.Text;
using System.Xml;
using PropertyListModel;

using System.Collections.Generic;
using System.Linq;
using System.IO;

public partial class Search : System.Web.UI.Page
{
    string Domain = "http://propertymap.info";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PublishSitemap();
    }
    public void PublishSitemap()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = Encoding.UTF8;
        XmlWriter writer = XmlWriter.Create(Server.MapPath("~/sitemap.xml"));

        writer.WriteStartDocument();
        writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
       
        //projects
        foreach (Society S in Global.ProjectList.Values.Where(m => m.Verified == 1 && m.Verified!=null))
        {
            WriteTag(Domain + "/" +S.URLName.ToLower(), true, "monthly", "0.8", writer);
        }
        //Builders
        foreach (Agent B in Global.BuilderList.Values)
        {
            if(B.AgentCompany!=null)
            WriteTag(Domain + "/" +B.AgentCompany.Replace(" ","_").ToLower(), true, "monthly", "0.8", writer);
        }
        DirectoryInfo di = new DirectoryInfo(Server.MapPath(@"~/Data/PDF"));
        
        //pdfs
        foreach (FileInfo fi in di.GetFiles())
        {
            WriteTag(Domain + "/data/pdf/" + fi.Name.ToLower(), true, "monthly", "0.6", writer);
        }

        //projects Images
        foreach (Society S in Global.ProjectList.Values.Where(m => m.Verified == 1 && m.Verified != null))
        {
            List<ImagesDetail> ids = Global.ImageDetailsList.Where(m => m.ReferenceID == S.ID && m.ImageReferenceType == (int)ImagesLocations.Projects_Images).ToList();
            string location = "";
            List<string> imgUrl = new List<string>(); 
            if (ids.Count > 0)
            {
                location = Domain + "/" + S.URLName.ToLower();
                foreach (ImagesDetail id in ids)
                {
                    imgUrl.Add(Domain + "/" + id.UrlName);
                }
                WriteImgTag(location, imgUrl, writer);
            }
        }
        writer.WriteEndDocument();  
        writer.Close();
    }

    private void WriteImgTag(string loc, List<string> imgUrl, XmlWriter MyWriter)
    {
        MyWriter.WriteStartElement("url");
        MyWriter.WriteStartElement("loc");
        MyWriter.WriteValue(loc);
        MyWriter.WriteEndElement();
        foreach (string img in imgUrl)
        {
            MyWriter.WriteStartElement("image","image","ns:xyz");
            MyWriter.WriteStartElement("image", "loc","ns:dsfdsgf");
            MyWriter.WriteValue(img);
            MyWriter.WriteEndElement();
            MyWriter.WriteEndElement();
        }
        MyWriter.WriteEndElement();
    }
    private void WriteTag(string Navigation, bool date, string freq, string Priority, XmlWriter MyWriter)
    {
        MyWriter.WriteStartElement("url");

        if (Navigation != "")
        {
            MyWriter.WriteStartElement("loc");
            MyWriter.WriteValue(Navigation);
            MyWriter.WriteEndElement();
        }
        
        if (date)
        {
            MyWriter.WriteStartElement("lastmod");
            MyWriter.WriteValue(String.Format("{0:yyyy-MM-dd}", DateTime.Now));
            MyWriter.WriteEndElement();
        }

        if (freq != "")
        {
            MyWriter.WriteStartElement("changefreq");
            MyWriter.WriteValue(freq);
            MyWriter.WriteEndElement();
        }

        if (Priority != "")
        {
            MyWriter.WriteStartElement("priority");
            MyWriter.WriteValue(Priority);
            MyWriter.WriteEndElement();
        }
        MyWriter.WriteEndElement();
    }
}