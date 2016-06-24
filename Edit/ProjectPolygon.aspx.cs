using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_ProjectPolygon : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (Society S in Global.ProjectList.Values)
            {
                if (S.City == "Ghaziabad" && (S.Deleted==0||S.Deleted==null))
                {
                    if (S.Verified == 0) continue;
                    ddProject.Items.Add(new ListItem(S.SocietyName +" ("+S.Subcity +") -" + (S.PolyPoints!=null  && S.PolyPoints.Length>0?"Y":""),S.ID.ToString()));
                }
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        Society S = Global.ProjectList.Values.FirstOrDefault(m => m.ID == Cmn.ToInt(ddProject.SelectedValue));
        if (S == null)
            return;

        using (var client = new WebClient())
        {
            try
            {
                string URL = @"http://api.wikimapia.org/?function=place.search"
                + "&key=5354BD14-C520BF6C-2BD56B87-F6969939-082E0C10-B1DC41D6-7A21EE39-348EC9A7"
                + "&q=" + S.SocietyName
                + "&lat=" + S.Lat
                + "&lon=" + S.Lng
                + "&format=json"
                + "&data_blocks=geometry";
                // +"&pack=gzip";
                string Data=client.DownloadString(URL);
                
                WriteClientScript("var data=" + Data + ";");
            }
            catch
            {
            }
        }
    }
}