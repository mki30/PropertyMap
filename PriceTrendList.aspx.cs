using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;
using System.Web.Script.Serialization;

public partial class PriceTrendList : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {

        base.Page_Load(sender, e);

        Title = "India Property Price Trend List" + Global.AppTitle;
        MetaDescription = "property price trend  in noida , guragaon ,grater noiada ,noiada extention,Gahziabad";
        MetaKeywords = "property price trends in noida,property price trends ghaziabad,property price trends greater noida,property pricetrends noida extention,property price trends gurgaon";

        if (!IsPostBack)
        {
            //City.CityList(ddCity, 0);
            string[] cityList ={"Ghaziabad","1","Noida","4","Gurgaon","173","Greater Noida","392"};      //Temporary Add CIties
            for (int i = 0; i < cityList.Length; i += 2)
            {
                ddCity.Items.Add(new ListItem(cityList[i],cityList[i+1]));
            }

            //string previousSelectedCity = Request.Cookies["PriceTrendSelectedCity"].Value;

            Data1 = (Data1 == "" ? "noida" : Data1);
            if (Data1 != "")
            {
                ddCity.SelectedValue = ddCity.Items.FindByText(Cmn.ProperCase(Data1)).Value;
                Response.Cookies["PriceTrendSelectedCity"].Value = ddCity.SelectedItem.Text.ToLower();
            }
         }
         ShowTredTable(Cmn.ToInt(ddCity.SelectedValue));
     }
    
    private void ShowTredTable(int CityID)
    {
        City city;

        StringBuilder str = new StringBuilder("<table class='table-bordered table-hover table-striped table-condensed'>");
        str.Append("<th>Locality</th><th>Lower Range</th><th>Upper Range</th><th>Average</th><th>View Trend</th>");

        if(Global.CityList.TryGetValue(CityID,out city))
        {
            foreach (City subcity in city.ChildCityList.OrderBy(m=>m.SortName))
            {
                if (subcity.PriceMin == 0)
                    continue;
                str.Append("<tr><td>" + subcity.Name + "</td><td>" + subcity.PriceMin + "</td><td>" + subcity.PriceMax + "</td><td>" + (subcity.PriceMax + subcity.PriceMin) / 2 + "</td><td><a href='" + Global.GetRootPathVirtual + "/property-price-trend-graph/" + subcity.UrlName + "'>Graph</a></td></tr>");
            }
        }
        str.Append("</table>");
        ltrTable.Text = str.ToString();
    }
    protected void ddCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ShowTredTable(Cmn.ToInt(ddCity.SelectedValue));
       Response.Redirect(Global.GetRootPathVirtual+"/price-trend/" + ddCity.SelectedItem.ToString().ToLower());
    }
}
