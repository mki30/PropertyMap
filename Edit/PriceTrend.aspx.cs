using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;

public partial class Edit_PriceTrend : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        GetTableData(4);
    }
     
    private void GetTableData(int CityID)
    {
        StringBuilder sb = new StringBuilder("<table class='table table-striped table-condensed' style='width:100%'><tr><th>City</th></tr>");

        DateTime date=DateTime.Today;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);

        int ShowQuaters = 14;

        DateTime dt = new DateTime(firstDayOfQuarter.Ticks);
        for (int i = 0; i < ShowQuaters; i++)
        {
            quarterNumber = (dt.Month - 1) / 3 + 1;

            sb.Append("<td>" + "Q"  + quarterNumber + dt.ToString("-yy"));
            dt = dt.AddMonths(-3);
        }

        City city;
        if (Global.CityList.TryGetValue(CityID, out city))
        {
            foreach (City subcity in city.ChildCityList.OrderByDescending(m=>m.SortName))
            {
                sb.Append("<tr><td>" + subcity.Name);

                List<PriceTrend> list = PriceTrend.GetBySubCityID(subcity.ID);

                dt = new DateTime(firstDayOfQuarter.Ticks);
                for (int i = 0; i < ShowQuaters; i++)
                {
                    quarterNumber = (dt.Month - 1) / 3 + 1;
                    PriceTrend PT =list.FirstOrDefault(m=>m.Quarter==quarterNumber  && m.Year==dt.Year);

                    sb.Append("<td>" + (PT!=null?"<a  class='fancybox fancybox.iframe' href='EditPriceTrend.aspx?ID=" + PT.ID + "'>" +  PT.Min + "-" + PT.Max:"" ));
                    dt = dt.AddMonths(-3);
                }
            }
        }
        sb.Append("</table>");
        ltDetail.Text = sb.ToString();
    }
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTableData(Cmn.ToInt(ddlCity.SelectedValue));
    }
}