using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Net;
using System.IO;

public partial class Edit_CompareandEditSocietyNames : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Society.GetList(ListBox1,Cmn.ToInt( ddlLocality.SelectedValue));
        Availability.GetList(ListBox2);
    }
   
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        List<Availability> AvailabilityList = Availability.GetAll();
        List<Society> SocietyList = Society.GetAll();

        Society.GetList(ListBox1,Cmn.ToInt( ddlLocality.SelectedValue));

        ListBox2.Items.Clear();

        foreach (Availability A in AvailabilityList)
        {
            if (A.SocietyID != 0)
                continue;

            if (!string.IsNullOrWhiteSpace(A.SocietyName))
            {
                ListBox2.Items.Add(A.SocietyName);
                foreach (Society S in SocietyList)
                {
                    if (A.SocietyName == S.SocietyName)
                    {
                        A.SocietyID = S.ID;
                        A.Save();
                        break;
                    }
                    if (A.SocietyName.ToLower().Split(' ')[0] == S.SocietyName.ToLower().Split(' ')[0])
                    {
                        A.SocietyID = S.ID;
                        A.Save();
                        break;
                    }
                }
            }
         }
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        for (int i = 'a'; i <= 'z'; i++)
        {
            for (int j = 'a'; j <= 'z'; j++)
            {
                new WebClient().DownloadFile("http://www.commonfloor.com/direct.php?action=gcffc&c=Ghaziabad&str=" + (char)i + (char)j, Server.MapPath(@"~\Data\" + (char)i + (char)j + ".txt"));
                Response.Write("<br/>" + (char)i + (char)j);
                Response.Flush();
            }
        }
    }


    protected void btnUpdateSociety_Click(object sender, EventArgs e)
    {
        string []Files=Directory.GetFiles(Server.MapPath(@"~\Data\"));

        foreach(string f in Files)
        {
            string [] Data=File.ReadAllText(f).Replace("\",\"","^").Replace("[\"","").Split('^');
            
            foreach(string s in Data)
            {
                string societyName = s.Split('~')[0];
                //Response.Write("<br/>" + societyName);
                Society S = Society.GetByName(societyName);
                if(S==null)
                S = new Society() {SocietyName=societyName}.Save();
            }
        }

    }
}