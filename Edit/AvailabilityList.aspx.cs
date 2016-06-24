using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_AvailabilityList : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        int SocietyID = Cmn.ToInt(QueryString("SocietyID"));
        List<Availability> AvailabilityList = Availability.GetList(SocietyID,0);

        foreach (Availability A in AvailabilityList)
        {
            lstAvailability.Items.Add(new ListItem( A.SellerName + "-" + A.BHK ,A.ID.ToString()));
        }

        int AvlID = Cmn.ToInt(QueryString("AvlID"));
        if (AvlID != 0)
            lstAvailability.SelectedValue = AvlID.ToString();

        lstAvailability.Attributes.Add("onchange", "parent.ShowAvl($('#lstAvailability').val())");
    }
}