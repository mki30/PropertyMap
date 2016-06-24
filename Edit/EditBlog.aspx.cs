using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_EditBlog : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        int id = Request.QueryString["ID"] != null ? Cmn.ToInt(Request.QueryString["ID"]) : 0;

        if (!IsPostBack)
        {
            FillCity(0, ddCity);
            showData(id);
        }
    }
    
    void FillCity(int cityId, DropDownList dd)
    {
        List<City> C = Global.CityList.Values.Where(m => m.ParentID == cityId).ToList();
        foreach (City c in C)
        {
            dd.Items.Add(new ListItem(c.Name, c.ID.ToString()));
        }
    }

    protected void ddCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillCity(Cmn.ToInt(ddCity.SelectedValue), ddSubCity);
        //txtUrlName.Text=ddCity.SelectedItem.Text+"-"+txtTitle.Text;
    }
    
    void showData(int ID)
    {
        lblID.Text = ID.ToString();
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            string vError = string.Empty;
            IDataReader dr = db.GetDataReader("Select * from Post where ID= " + ID, ref vError);
            while (dr.Read())
            {
                txtTitle.Text = dr["Title"].ToString();
                txtUrlName.Text = dr["UrlName"].ToString();
                txtdate.Text = Cmn.ToDate(dr["PostDate"]).ToString("dd-MMM-yyyy");
                txtRefUrl.Text = dr["ReferenceUrl"].ToString();
                txtShortDesc.Text = dr["SortData"].ToString();
                txtDescription.Text = Cmn.GetUnCompressed(((byte[])dr["Data"]), Cmn.ToInt(dr["DataSize"]));
                ddCity.SelectedValue = dr["CityID"].ToString();
                //FillCity(Cmn.ToInt(ddCity.SelectedValue), ddSubCity);
                //ddSubCity.SelectedValue = dr["SubcityID"].ToString();
                //chkOffline.Checked = dr["Offline"] == "1" ? true : false;
                //chkOffline.Checked = dr["IsDelete"] == "1" ? true : false;
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "<b>Error: </b>" + ex.Message;
        }
        finally
        {
            db.Close();
        }
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            int id = Cmn.ToInt(lblID.Text);
            string title = Cmn.ProperCase(txtTitle.Text), vError = string.Empty;
            int i = (id == 0 ? db.GetCount("Select * from Post where title='" + title + "'", ref vError) : 0);
            if (id == 0 && i > 0)
            {
                lblStatus.Text = "Blog already exist with same title";
                return;
            }
            int ID = (id == 0 ? db.GetMax("Post", "ID", "", ref vError) + 1 : id);
            string slug = Cmn.GenerateSlug(txtTitle.Text);

            string insertQuery1 = "insert into Post (ID,Title,UrlName,CityID,Data,DataSize,PostDate,ReferenceUrl,SortData)"
                         + "VALUES(@id, @title, @urlname,@CityID, @data,@datasize,@postdate,@referenceurl,@SortData);";
            if (id != 0)
                insertQuery1 = "UPDATE Post SET "
            + "Title=@title,UrlName=@urlname,SortData=@SortData,Data=@data,CityID=@CityID,DataSize=@datasize,PostDate=@postdate,ReferenceUrl=@referenceurl" +
            " WHERE ID=@ID";

            SqlCeCommand cmd = new SqlCeCommand(insertQuery1, (SqlCeConnection)db.myconnection);
            cmd.Parameters.Add("@ID", DbType.Int32).Value = ID;
            cmd.Parameters.Add("@title", DbType.String).Value = title;
            cmd.Parameters.Add("@urlname", DbType.String).Value = slug;
            cmd.Parameters.Add("@data", DbType.Byte).Value = Cmn.GetCompressed(txtDescription.Text);
            cmd.Parameters.Add("@SortData", DbType.String).Value = txtShortDesc.Text;
            cmd.Parameters.Add("@datasize", DbType.Int32).Value = txtDescription.Text.Length;
            cmd.Parameters.Add("@postdate", DbType.Date).Value = Cmn.ToDate(txtdate.Text);
            cmd.Parameters.Add("@referenceurl", DbType.String).Value = txtRefUrl.Text;
            cmd.Parameters.Add("@CityID", DbType.Int32).Value = ddCity.SelectedValue;
            //cmd.Parameters.Add("@subcityID", DbType.Int32).Value = ddSubCity.SelectedValue;
            //cmd.Parameters.Add("@offline", DbType.Int32).Value = (chkOffline.Checked ? 1 : 0);
            //cmd.Parameters.Add("@isdelete", DbType.Int32).Value = (chkDelete.Checked ? 1 : 0);
            
            int svd = cmd.ExecuteNonQuery();
            if (svd == 1)
            {
                lblStatus.Text = "Saved " + DateTime.Now;
                lblID.Text = ID.ToString();
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "<b>Error:</b>" + ex.Message;
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (imgUpload.HasFile)
        {
            FolderCheck("~/data/blog");   //should be removed after one blog added(Images uploaded)
            string fileName = Cmn.GetImageFileName(lblID.Text, "~/data/blog/", Path.GetExtension(imgUpload.FileName));
            imgUpload.SaveAs(Server.MapPath("~/data/blog/") + fileName);
        }
        getBlogImages();
    }
    void getBlogImages()
    {
        string Folder = HttpContext.Current.Server.MapPath("~/data/blog/"),
                          productKey = lblID.Text + "_*", svdFiles = string.Empty;

        string[] ImageFiles = Directory.GetFiles(Folder, productKey);
        foreach (string im in ImageFiles)
        {
            svdFiles += "/data/blog/" + Path.GetFileName(im) + "&nbsp;<a class='rmv-img' href='#' title='Delete Image'>X</a>, ";
        }
        lblImgs.Text = svdFiles;
    }
    public string FolderCheck(string Path)
    {
        bool IsExists = Directory.Exists(Server.MapPath(Path));
        if (!IsExists)
            Directory.CreateDirectory(Server.MapPath(Path));
        return Path;
    }
    
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE(Global.BlogConnection);
        try
        {
            db.RunQuery("delete from post where Id=" + Cmn.ToInt(lblID.Text));
            //WriteClientScript("opener.document.location.reload(true);");
        }
        catch (Exception ex)
        {
            lblStatus.Text = "<b>Error: </b>" + ex.Message;
        }
        finally
        {
            db.Close();
        }
     }
}