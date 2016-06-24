using PropertyListModel;
using System;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;

public partial class PropertyMap : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "India Property Map" + Global.AppTitle;
        MetaDescription = "view property location in map";
        MetaKeywords = "map, property location map,prime location property,city loaction map,properties on one map";
        
        Data1 = Data1.ToLower();
        if(Data1!="")
        ltHeading.Text = Data1;
        Society S=null;
        if(Data1!="")
        S = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == Data1);

        string load="";
        switch (Action)
        {
            case "map":
                load = "2D";
                  break;
            case "map3d":
                  load ="3D";
                break;
        }
        string str = "var SelectedProjectID=" + (S != null ? S.ID : 0) + ", " + Environment.NewLine;
        str += " SelectedCityID=" + (S != null && S.CityID != null ? S.CityID : 0) + ", " + Environment.NewLine;
        str += " SelectedSubCityID=" + (S != null && S.SubCityID != null ? S.SubCityID : 0) + ", " + Environment.NewLine;
        str += " SelectedAreaID=" + (S != null && S.AreaID != null ? S.AreaID : 0) + ", " + Environment.NewLine;
        str += " LoadMap='" + load + "' ;" + Environment.NewLine;
        Cmn.WriteClientScript(this, str);
    }
}