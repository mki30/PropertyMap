using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Blogs : BasePage
{
    int LastBlogID = 0;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!string.IsNullOrEmpty(Data1))
        {
            if (ShowDetails(Data1) == 1)
                return;
        }
        Title = "Blogs";
        ShowBlogs();
    }
    
    void ShowBlogs()
    {
        string vError = string.Empty;
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            StringBuilder sb = new StringBuilder("<ol id='items' class='items'>");
            IDataReader dr = db.GetDataReader("Select * from Post order by postdate desc", ref vError);
            while (dr.Read())
            {
                sb.Append("<li class='item box-shadow'><div class='article' itemscope itemtype='http://schema.org/BlogPosting'>");

                DateTime date = Cmn.ToDate(dr["PostDate"]);
                string year = date.Year.ToString(), 
                        mth = date.ToString("MMM"),
                        day = date.Day.ToString(), 
                        urlname = dr["UrlName"].ToString();
                sb.Append("<div class='article-header'>");
                sb.Append("<a class='ribbon date ' title='Today' href='#'>");
                sb.Append("<div class='top ribbon-piece'>" + mth + "</div>");
                sb.Append("<div class='bottom ribbon-piece'>" + day + "</div>");
                sb.Append("<div class='tail'>");
                sb.Append("<div class='left ribbon-piece'></div>");
                sb.Append("<div class='right ribbon-piece'></div>");
                sb.Append("</div></a>");
                sb.Append("<h2 class='title entry-title' itemprop='name'><a href='/blogs/" + urlname + "' itemprop='url'>" + dr["Title"] + "</a></h2>");
                sb.Append("</div>");
                sb.Append("<div class='article-content' itemprop='articleBody'>");
                //string desc = Cmn.GetUnCompressed(((byte[])dr["data"]), Cmn.ToInt(dr["datasize"]));
                string desc = dr["Sortdata"].ToString();
                if (desc.Length > 500)
                    desc = desc.Substring(0, 500);
                    desc = "<div>" +desc+ " <a class='rdfl' href='/blogs/" + urlname + "'>read full...</a></div>";
                sb.Append(desc);
                sb.Append("</div>");
                sb.Append("<div class='article-footer'></div>");
                sb.Append("</div></li>");
            }
            sb.Append("</ol>");
            blog.InnerHtml = sb.ToString();
            
        }
        catch (Exception ex)
        {
            vError = ex.Message;
        }
        finally
        {
            db.Close();
        }
    }

    int ShowDetails(string urlName)
    {
        string vError = string.Empty;
        int i = 0;
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            StringBuilder sb = new StringBuilder();
            IDataReader dr = db.GetDataReader("Select * from Post where urlname='" + urlName.ToLower().Trim() + "'", ref vError);
            int visits = 0, ID = 0;
            while (dr.Read())
            {
                pnlscripts.Visible = true;
                string title = dr["title"].ToString(), url = dr["UrlName"].ToString();
                Title = title.Replace("?", "");
                visits = Cmn.ToInt(dr["VisitCount"]);
                ID = Cmn.ToInt(dr["ID"]);
                i++;

                sb.Append("<div class='entry-wrap box-shadow'>");
                sb.Append("<h1 class='new-single-title'>" + title + "</h1>");
                sb.Append("<div class='entry-sb-info'><span id='pdt'>" + Cmn.ToDate(dr["PostDate"]).ToString("MMM dd, yyyy"));
                sb.Append(" | </span>");
                sb.Append("<a href='https://twitter.com/share' class='twitter-share-button' data-url='https://propertymap.info/blogs/"+url+"' data-via='propertymap.info' data-lang='en'>Tweet</a>");
                sb.Append("<div class='g-plusone' data-size='medium'></div>");
                sb.Append("<script src='//platform.linkedin.com/in.js' type='text/javascript'>lang: en_US</script><script type='IN/Share' data-counter='right'></script>");
                sb.Append("</div>");
                sb.Append("<div class='entry-content'>");
                sb.Append(Cmn.GetUnCompressed(((byte[])dr["data"]), Cmn.ToInt(dr["datasize"])));
                sb.Append("</div></div>");
            }
            blog.InnerHtml = sb.ToString();

            if (i == 1 && ID != 0 && Global.BlogID != ID.ToString())
            {
                visits = visits + 1;
                db.RunQuery("Update Post set VisitCount=" + visits + " where ID=" + ID);
                Global.BlogID = ID.ToString();
            }
        }
        catch (Exception ex)
        {
            //Cmn.LogError(ex, "Error:Blog View");
        }
        finally
        {
            db.Close();
        }
        return i;
    }
}