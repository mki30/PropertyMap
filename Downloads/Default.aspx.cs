using System;
using System.IO;
using System.Net;
using PropertyListModel;

public partial class DataDownload_Kohler_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ProcessData();

        if (!IsPostBack)
        {
            txtData.Text = Server.HtmlEncode(File.ReadAllText(Server.MapPath(@"~\Downloads\Availibility.htm")));
        }
    }

    void ProcessData()
    {
        if (txtResult.Text == "")
            return;

        string ProductListName = txtProductListName.Text;
        string[] Lines = txtResult.Text.Split('~');

        foreach (string s in Lines)
        {
            if (string.IsNullOrWhiteSpace(s))
                continue;

            string[] Fields = s.Split('^');
            
            // string ProductCode = Fields[0];//.Trim();
         }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string URL = txtURL.Text;
        string PageNumber = txtPageNumber.Text;
        string FileName = Server.MapPath(@"~\Downloads\Data\" + PageNumber + ".htm");

        if (!File.Exists(FileName))
        {
            new WebClient().DownloadFile(@"" + URL, FileName);
        }
        else
        {
            txtData.Text = Server.HtmlEncode(File.ReadAllText(FileName));
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtResult.Text == "")
            return;

        string[] Lines = txtResult.Text.Split('~');

        DatabaseCE db = new DatabaseCE();

        try
        {
            foreach (string s in Lines)
            {
                //string AgentName = "";
                //string Mobile = "";
                if (string.IsNullOrWhiteSpace(s))
                    continue;

                string[] Fields = s.Replace("\r\n\t", "").Replace("\t", "").Replace("\r", "").Split('^');

                //continue;

                //string AgentCompanyName = Fields[0].Trim();
                //if (string.IsNullOrWhiteSpace(Fields[6]))
                //{
                //    AgentName = Fields[7].Replace("Agent:", "").Trim();
                //    Mobile = Fields[9].Trim();
                //}
                //else
                //{
                //    AgentName = Fields[6].Replace("Agent:", "").Trim();
                //    Mobile = Fields[8].Trim();
                //}
                //string DealsIn = (Fields[3].Trim().Replace("\t\t\n", "").Replace("Dealing In: ", "").Replace("\r", "")).Trim();
                //string PropertyHandled = Fields[4].Trim().Replace("Properties Handled: ", "");
                //string OperatingIn = Fields[5].Trim().Replace("Operating In: ", "");
                //string MazicBrickURL = Fields[1].Trim();
                //Mobile = Mobile.Substring(Mobile.IndexOf(", '") + 3);
                //Mobile = Mobile.Substring(0, Mobile.IndexOf("'"));

                //string[] mob = Mobile.Split(',');
                //if (mob.Length > 0)
                //{
                //    string mobile1 = mob[0];
                //    string mobile2 = mob[1];
                //    string mobile3 = mob[2];
                //}

                //string Error = "";
                //if (AgentCompanyName != "")// insert the agent company name in the database
                //{
                //    string sql = "select ID from agent where AgentCompany='" + AgentCompanyName + "' and OperatingIn='" + OperatingIn + "'";

                //    string ID = db.GetFieldValue(sql, ref Error);

                //    if (ID == "")
                //    {

                //        Error = db.RunQuery("insert into agent (agentCompany,mobile1,agentname,Dealsin,PropertiesHandled,OperatingIn,MagicBrickURL) values('" + AgentCompanyName + "','" + Mobile + "','" + AgentName + "','" + DealsIn + "','" + PropertyHandled + "','" + OperatingIn + "','" + MazicBrickURL + "')");
                //    }
                //    else
                //    {
                //        string SQL = "update  agent set agentCompany='" + AgentCompanyName + "', mobile1='" + Mobile + "', agentname='" + AgentName + "', Dealsin='" + DealsIn + "', PropertiesHandled='" + PropertyHandled + "', OperatingIn='" + OperatingIn + "', MagicBrickURL='" + MazicBrickURL + "' where ID=" + ID;
                //        Error = db.RunQuery(SQL);
                //    }
                //}

            }
        }
        catch
        {
        }
        finally
        {
            db.Close();
        }

    }
    //protected void btnInsert_Click(object sender, EventArgs e)
    //{

    //    string[] Lines = File.ReadAllLines(Server.MapPath(@"~\App_Data\PropertyDealers.txt"));

    //    int ctr = 0;
    //    foreach (string s in Lines)
    //    {
    //        string[] F = s.Split('^');
    //        ctr++;

    //        if (F[0] != "")
    //        {
    //            Agent currentAgent = new Agent() { AgentCompany = F[0].Trim() }.LoadByCompanyName();
    //            if (currentAgent != null)
    //            {
    //                Response.Write(ctr + "-" + F[0] + "<br/>");
    //                Response.Flush();
    //                continue;
    //            }

    //            Agent A = new Agent()
    //            {
    //                ID = currentAgent != null ? currentAgent.ID : 0,
    //                AgentCompany = Cmn.ProperCase(F[0].Trim()),
    //                Mobile1 = (F[2].Replace("+(91)-", "").Trim()),
    //                Address = F[1].Trim(),
    //                Lat = Cmn.ToDbl(F[4].Trim()),
    //                Lng = Cmn.ToDbl(F[5].Trim())
    //            }.Save();

    //            Response.Write(F[0] + A.Message + "<br/>");
    //            Response.Flush();

    //            if (A.Message != "")
    //                break;
    //        }

    //    }
    //    lblMessage.Text = "Saved";
    //}
}