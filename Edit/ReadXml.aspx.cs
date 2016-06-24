using PropertyListModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public partial class ReadXml : BasePageEdit
{
    class Node
    {
        public double lat;
        public double lng;
    }
    
    class Way
    {
        public string Name;
        public List<Node> NodeList = new List<Node>();
        public Dictionary<string, string> Tags = new Dictionary<string, string>();
    }

    Dictionary<int, Node> Nodes = new Dictionary<int, Node>();
    Dictionary<int, Way> Ways = new Dictionary<int, Way>();

    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
       if (!IsPostBack)
        {
            City.CityList(ddCity);

           string Folder=Server.MapPath(@"~\OSM\");
           if(!Directory.Exists(Folder))
               Directory.CreateDirectory(Folder);

            string[] files=Directory.GetFiles(Folder);
            foreach(string f in files)
            {
                ddCityOSM.Items.Add(Path.GetFileNameWithoutExtension(f));
            }
        }
    }

    public void ReadXML(string FileName )
    {
        try
        {
            using (var file = new StreamReader(Server.MapPath(@"~\OSM\" + FileName), Encoding.UTF8))
            {
                using (XmlReader reader = XmlReader.Create(file))
                {
                    //reader.MoveToContent();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:

                                switch (reader.Name)
                                {
                                    case "node":
                                        {
                                            XElement el = XElement.ReadFrom(reader) as XElement;
                                            int id = Cmn.ToInt(el.Attribute("id").Value);
                                            double lat = Cmn.ToDbl(el.Attribute("lat").Value);
                                            double lon = Cmn.ToDbl(el.Attribute("lon").Value);

                                            if (!Nodes.ContainsKey(id))
                                                Nodes.Add(id, new Node() { lat = lat, lng = lon });
                                        }
                                        break;
                                    case "way":
                                        {
                                            XElement el = XElement.ReadFrom(reader) as XElement;
                                            int id = Cmn.ToInt(el.Attribute("id").Value);

                                            Way W = null;

                                            if (!Ways.ContainsKey(id))
                                            {
                                                W = new Way();
                                                Ways.Add(id, W);
                                            }
                                            else
                                                W = Ways[id];

                                            foreach (var tag in el.Elements("nd"))
                                            {
                                                int refid = Cmn.ToInt(tag.Attribute("ref").Value);
                                                Node n;
                                                if (Nodes.TryGetValue(refid, out n))
                                                    W.NodeList.Add(n);
                                            }

                                            foreach (var tag in el.Elements("tag"))
                                            {
                                                var k = tag.Attribute("k");


                                                var v = tag.Attribute("v");
                                                if (k != null && v != null)
                                                {
                                                    W.Tags.Add(k.Value, v.Value);
                                                    if (k.Value == "name")
                                                        W.Name = v.Value;

                                                }
                                            }
                                        }
                                        break;
                                }
                                
                                //if (el != null)
                                //    if (el.Element("tag") != null)
                                //        yield return el;
                                break;
                        }
                    }
                }
            }

            StringBuilder str = new StringBuilder("<table class='DataTable'><tr><td>Name</td><td>Is In</td><td>Land Use</td><td>Points</td><td></td>");
            StringBuilder strPoints = new StringBuilder("");
            StringBuilder strLabels = new StringBuilder("");
            var list = Ways.Values.OrderBy(m => m.Name).ToList();

            int ctr = 0;
            foreach (Way W in list)
            {
                string name;
                W.Tags.TryGetValue("name", out name);

                string is_in;
                W.Tags.TryGetValue("is_in", out is_in);

                string landuse;
                W.Tags.TryGetValue("landuse", out landuse);


                if (name != null && landuse != null)
                {
                    str.Append("<tr onmouseover='ShowPoly(" + ctr + ")'><td>" + name + "<td>" + (is_in!=null?is_in:"") + "<td>" + landuse + "<td>" + W.NodeList.Count 
                        + "<td><a href='#' onclick='AutoImport(" + ctr + ")'>Map</a>");

                    string p = "";
                    foreach (Node N in W.NodeList)
                        p += N.lat + "," + N.lng + ",";

                    strPoints.Append("Points[" + ctr + "]=[" + p.TrimEnd(',') + "];" + Environment.NewLine);
                    strLabels.Append("Labels[" + ctr + "]=['" + name.Trim().Replace("'","") + "'];" + Environment.NewLine);

                    ctr++;
                }
            }

            ltObjectLlist.Text = str.ToString() + "</table>";

            Cmn.WriteClientScript(this, strPoints.ToString() + strLabels.ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    void DoMapping(int ParentID, Boolean Import = false)
    {

        City ParentCity = City.GetByID(ParentID);

        if (ParentCity == null)
            return;

        List<City> city = City.GetByParentID(ParentID);

        string str = "";

        foreach (Way W in Ways.Values)
        {
            string name;
            W.Tags.TryGetValue("name", out name);

            string is_in;
            W.Tags.TryGetValue("is_in", out is_in);

            string landuse;
            W.Tags.TryGetValue("landuse", out landuse);

            if (name != null && is_in != null && landuse != null && landuse == "residential")
            {

                if (Import)
                {
                    City C = City.GetByName(ParentID, name);
                    if (C == null)
                    {
                        new City()
                        {
                            ParentID=ParentID,
                            Name=name
                        }.Save();
                    }
                }
                else
                {
                    foreach (City C in city)
                    {
                        if (!string.IsNullOrWhiteSpace(C.PolyPoints)) // skip the cities which have some data
                            continue;

                        if (C.Name == W.Name && is_in == ParentCity.Name)
                        {
                            str += "<br/>" + C.Name;
                            string p = "";
                            foreach (Node N in W.NodeList)
                                p += N.lat + "," + N.lng + "^";

                            p.TrimEnd('^');
                            
                            C.PolyPoints = p;
                            C.Save();
                        }
                    }
                }
            }
        }
        Response.Write(str);
    }

    protected void btnAutoMap_Click(object sender, EventArgs e)
    {
        DoMapping(Cmn.ToInt(ddCity.SelectedValue));
    }
   
    protected void btnImport_Click(object sender, EventArgs e)
    {
        DoMapping(Cmn.ToInt(ddCity.SelectedValue), true);
    }

    protected void ddCityOSM_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReadXML(ddCityOSM.Text + ".osm");
        
        ListItem li = ddCity.Items.FindByText(ddCityOSM.Text);

        if (li != null)
        {
            ddCity.SelectedValue = li.Value;
        }
    }
    protected void btnProjectPolyImport_Click(object sender, EventArgs e)
    {
        int city = Cmn.ToInt(ddCity.SelectedValue);
        List<Society> S = Society.GetByCity(city);
        int i=0;
        foreach (Society s in S)
        {
            string Folder = Server.MapPath(@"~\Data\WikiMapia\PolyData\");
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            string FileName = Folder + s.SocietyName + ".txt";

            if (!File.Exists(FileName))
            {
                using (var client = new WebClient())
                {
                    try
                    {
                        string URL = @"http://api.wikimapia.org/?function=place.search"
                        + "&key=5354BD14-C520BF6C-2BD56B87-F6969939-082E0C10-B1DC41D6-7A21EE39-348EC9A7"
                        + "&q=" + s.SocietyName
                        + "&lat=" + s.Lat
                        + "&lon=" + s.Lng
                        + "&format=json"
                        + "&data_blocks=geometry";
                        // +"&pack=gzip";
                        client.DownloadFile(URL, FileName);
                        File.SetCreationTime(FileName, DateTime.Now);
                    }
                    catch
                    {
                    }
                }
            }
            i++;
        }

    }
}