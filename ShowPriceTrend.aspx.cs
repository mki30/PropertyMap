using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.Web.Script.Serialization;

public partial class ShowPriceTrend : BasePage
{
    public string City = "";
    public string SubCity = "";
    public string SubCityUrlName = "";

    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title = "India Property Price Trends " + Global.AppTitle;
        MetaDescription = "Property Price Trend Graph";
        MetaKeywords = "property Price Trend Graph";

        StringBuilder str = new StringBuilder("<table>");
        //City C = Global.CityList.Values.FirstOrDefault(m => m.UrlName.ToLower() == Data1);
        //if (C != null)
        //{
        //City = C.UrlName;
        
        try
        {
            City SelectedSubCity = Global.CityList.Values.FirstOrDefault(m => !object.Equals(m.UrlName, null) && m.UrlName.ToLower() == Data1.ToLower());
            if (SelectedSubCity != null)
            {
                SubCity = SelectedSubCity.Name;
                SubCityUrlName = SelectedSubCity.UrlName;
                ltHeading.Text = "Price Trend in " + SelectedSubCity.Name + "," + SelectedSubCity.Parent.Name;
                MetaDescription += " of " + SelectedSubCity.Name;
                MetaKeywords += " " + SelectedSubCity.Name+" "+SelectedSubCity.Parent.Name  ;
            }
            int i = 0;
            City C = Global.CityList.Values.FirstOrDefault(m => !object.Equals(m.UrlName, null) &&  m.UrlName.ToLower() == SelectedSubCity.Parent.Name.ToLower());
            if (C != null)
            {
                foreach (City subcity in C.ChildCityList.OrderBy(m => m.SortName))
                {
                    if (subcity.PriceMin == 0)
                        continue;
                    str.Append("<tr><td><input data-index='" + (i++) + "' data-urlname='" + subcity.UrlName + "' data-name='" + subcity.Name + "' type='checkbox' /></td><td>&nbsp;" + subcity.Name + "</td></tr>");
                }
            }
            lblCheckBox.Text = str.ToString() + "</table>";
        }
        catch (Exception ex)
        {
        }
    }
}