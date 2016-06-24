using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_ApartmentList : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Apartment.GetAptNo(lstAprtmentList,Cmn.ToInt(QueryString("SocietyID")));
        int ApartmentID = Cmn.ToInt(QueryString("ApartmentID"));
        if (ApartmentID != 0)
            lstAprtmentList.SelectedValue = ApartmentID.ToString();
            lstAprtmentList.Attributes.Add("onchange", "parent.ShowApartment($('#lstAprtmentList').val())");
    }
}