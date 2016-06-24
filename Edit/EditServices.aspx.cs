using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditServices : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
                             
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }
    
    public void ShowData(int ID)
    {
        Service S = new Service() { ID = ID }.Load();
        if (S != null)
        {
            lblID.Text = S.ID.ToString();
        }
    }

    private void Update()
    {
        Service S= new Service()
        {
            ID = Cmn.ToInt(lblID.Text),
            Type = Cmn.ToInt(ddlServiceType.SelectedValue),
            Name=txtName.Text,
            Contact=txtContact.Text,
            City = ddlCity.SelectedItem.Text,
            Address=txtAdress.Text,
            Lat=Cmn.ToDbl(txtLat.Text),
            Lng=Cmn.ToDbl(txtLng.Text),
        }.Save();

        lblStatus.Text = "Data Saved-" + S.Message;
    }
   
}