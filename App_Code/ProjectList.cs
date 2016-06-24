using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using PropertyListModel;

/// <summary>
/// Summary description for ProjectList
/// </summary>

public class ProjectList
{
    public ProjectList()
    {
       // TODO: Add constructor logic here
    }
    
    public static string GetProjectList()
    {
        StringBuilder str = new StringBuilder("");
        var list = Society.GetCityList();
        if (list != null)
        {
            foreach (var O in list)
            {
                Type type = O.GetType();

                var City = type.GetProperty("City").GetValue(O, null);
                var SubCity = type.GetProperty("Subcity").GetValue(O, null);

                if (string.IsNullOrWhiteSpace(City))
                    continue;

                //if (ctr > 0)
                str.Append(GetListByCitySubcity(City, SubCity));
            }
        }
        return str.ToString();
    }

    public static string GetListByCitySubcity(string City,string SubCity)
    {
        List<Society> list = Society.GetBySubCity(2);
        StringBuilder str = new StringBuilder("<table>");
        foreach (Society S in list)
        {
            str.Append("<tr><td><a href='" + Global.GetRootPathVirtual + @"\project\" + S.SocietyName.Replace(" ", "_") + "-" + S.ID + "'>" + S.SocietyName + "</a>");
        }
        return  str.ToString() + "</table>";

        //List<Society> SocList = Society.Get(City, SubCity);
        
        //StringBuilder str2 = new StringBuilder("<h2>" + City + "-" + Cmn.ProperCase(SubCity) + "</h2><table class='ProjectList'>");
        //int ctr = 0;
        //foreach (Society S in SocList)
        //{
        //    string LayoutFile = HttpContext.Current.Server.MapPath(@"~\Data\Images_LayoutPlan\" + S.ID + ".jpg");
            
        //    if (File.Exists(LayoutFile))
        //    {
        //        List<ApartmentType> at = ApartmentType.GetList(S.ID);
        //        str2.Append((ctr % 3 == 0 ? ctr > 0 ? "</tr><tr>" : "<tr>" : "") + "<td><a href='" + Global.GetRootPathVirtual + "/project/" + S.ID + "'>" + S.SocietyName + "</a></td>");
        //        ctr++;
        //    }
        //}
        //str2.Append("</tr></table>");
        //return str2.ToString();
    }
}