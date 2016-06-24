using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditBlog :BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        FillCity(0,ddCity);
        FillTree();
    }

    protected void btnSavePost_Click(object sender, EventArgs e)
    {
        Update(0);
    }
    
    void FillCity(int cityId, DropDownList dd)
    {
        List<City> C = Global.CityList.Values.Where(m => m.ParentID == cityId).ToList();
        foreach (City c in C)
        {
            dd.Items.Add(new ListItem(c.Name,c.ID.ToString()));
        }
    }
    void Update(int id)
    {
        NewsArticle NA = new NewsArticle();
        
        NA.CityID = Cmn.ToInt(ddCity.SelectedValue);
        NA.SubcityID = Cmn.ToInt(ddSubCity.SelectedValue);
        NA.Header = txtHeader.Text;
        NA.Details = Cmn.GetCompressed(txtEditor.Text);
        NA.DetailsSize = txtEditor.Text.Length;
        NA.Save();
    }
    protected void ddCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCity(Cmn.ToInt(ddCity.SelectedValue),ddSubCity);
    }
    void ShowData(int id)
    {
        NewsArticle NA = NewsArticle.GetByID(id);
        if(NA!=null)
        {
            ddCity.SelectedValue = NA.CityID.ToString();
            FillCity(Cmn.ToInt(NA.CityID),ddSubCity);
            ddSubCity.SelectedValue = NA.SubcityID.ToString();
            txtHeader.Text = NA.Header;
            txtEditor.Text = Cmn.GetUnCompressed(NA.Details,Cmn.ToInt(NA.DetailsSize));
        }
    }
    void FillTree()
    {
        List<NewsArticle> list = NewsArticle.GetAll();
        AddNode(list);
    }

    void AddNode(List<NewsArticle> list)
    {
        foreach (NewsArticle na in list)
        {
            string year = Cmn.ToDate(na.LUDate).Year.ToString();
            TreeNode yr = new TreeNode(year);

            if (CheckIfExists(year))
            {
                ArchieveTree.Nodes.Add(yr);
                AddMonths(list,yr, year);
            }
        }
    }
    
    void AddMonths(List<NewsArticle> list, TreeNode  tn, string year)
    {
        foreach (NewsArticle na in list)
        {
            DateTime dt=Cmn.ToDate(na.LUDate);
            if (dt.Year.ToString() == year)
            {
                string month= dt.Month.ToString();
                if (CheckIfExists(month))
                {
                    TreeNode yr = new TreeNode(month);
                    AddUpdates(list, yr, year, month);
                    tn.ChildNodes.Add(yr);
                }
            }
        }
    }
    
    void AddUpdates(List<NewsArticle> list, TreeNode tn, string year, string month)
    {
        foreach (NewsArticle na in list)
        {
            DateTime dt = Cmn.ToDate(na.LUDate);
            if (dt.Month.ToString() == month && dt.Year.ToString() == year)
            {
                TreeNode yr = new TreeNode(na.LUDate.ToString(),na.ID.ToString());
                tn.ChildNodes.Add(yr);
            }
        }
    }

    Boolean CheckIfExists(string text)
    {
        foreach (TreeNode tnn in ArchieveTree.Nodes)
        {
            if (tnn.Text == text)
            {
                return false;
            }
        }
        return true;
    }
    
    protected void ArchieveTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        if(ArchieveTree.SelectedNode.Value!=null)
        ShowData(Cmn.ToInt(ArchieveTree.SelectedNode.Value));
    }
}