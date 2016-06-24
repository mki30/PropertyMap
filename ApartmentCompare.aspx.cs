using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class ApartmentCompare : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
            base.Page_Load(sender, e);

            string[] Lines = QueryString("ID").Split(',');

            List<int> AptIDs = new List<int>();

            foreach (string id in Lines)
            {
                if (string.IsNullOrWhiteSpace(id))
                    continue;

                AptIDs.Add(Cmn.ToInt(id));
            }

            //ltCompare.Text= Society.GetAptDetails(AptIDs,true);
    }
}