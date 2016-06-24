using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Blog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = Request.QueryString["Data5"] != null ? Cmn.ToInt(Request.QueryString["Data5"]) : 0;
        if (id == 10)
        {
            Response.Clear();
            buildList();
            Response.End();
        }
    }
    
    void buildList()
    {
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            Dictionary<string, Dictionary<string, List<SearchSite>>> DicBlog = new Dictionary<string, Dictionary<string, List<SearchSite>>>();
            string vError = string.Empty;
            IDataReader dr = db.GetDataReader("Select * from Post order by postdate desc", ref vError);
            while (dr.Read())
            {
                DateTime date = Cmn.ToDate(dr["PostDate"]);    // Cmn.ToDate(dr["PostDate"]).ToString("dd-MMM-yyyy")
                string year = date.Year.ToString(),
                    mth = date.ToString("MMM");
                if (!DicBlog.ContainsKey(year))
                    DicBlog.Add(year, new Dictionary<string, List<SearchSite>>());

                if (!DicBlog[year].ContainsKey(mth))
                    DicBlog[year].Add(mth, new List<SearchSite>());

                DicBlog[year][mth].Add(new SearchSite()
                {
                    ID = Cmn.ToInt(dr["ID"]),
                    Name = dr["title"].ToString()
                });
            }

            var newList = DicBlog.Select(a => new
            {
                data = a.Key + " (" + a.Value.Count() + ")",
                state = "closed",
                children = a.Value.Select(x => new
                {
                    data = x.Key + " (" + x.Value.Count() + ")",
                    state = "closed",
                    children = x.Value.Select(y => new
                    {
                        attr = new { id = y.ID },
                        data = y.Name
                    })
                })
            }).ToList<object>();
            
            Response.Write(new JavaScriptSerializer().Serialize(newList));
            Console.Write(new JavaScriptSerializer().Serialize(newList));
            Response.AddHeader("Content-Type", "application/json");
        }
        catch (Exception ex)
        {
            Response.Write("<b>Error: </b>" + ex.Message);
        }
        finally
        {
            db.Close();
        }
    }
}


