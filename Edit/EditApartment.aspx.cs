using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditApartment : BasePageEdit

{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!IsPostBack)
        {
            lblSocietyID.Text = QueryString("SocietyID");
            ApartmentType.GetApartmentTypeName(ddlApartmentType, Cmn.ToInt(lblSocietyID.Text));
            for (int i = 1; i <= 6; i++)
            {
                ApartmentType.GetApartmentTypeName((FindControl("ddAptType" + i) as DropDownList), Cmn.ToInt(lblSocietyID.Text));
            }
            ShowData(Cmn.ToInt(QueryString("ID")));
       }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }

    void Update()
    {
        Apartment A = new Apartment()
        {
            ID = Cmn.ToInt(lblID.Text),
            SocietyID = Cmn.ToInt(lblSocietyID.Text),
            ApartmentTypeID = Cmn.ToInt(ddlApartmentType.SelectedValue),
            ApartmentNumber = Cmn.ToInt(txtAptNumber.Text),
            Block = txtBlock.Text,
            Floor = Cmn.ToInt(ddlFloor.SelectedValue),
            Facing = ddlFacing.SelectedItem.Text,
            Lifts = Cmn.ToInt(ddlLifts.SelectedValue),
            Parking1 = Cmn.ToInt(ddlParking1.SelectedValue),
            Parking2 = Cmn.ToInt(ddlParking2.SelectedValue),
            ServantRoom = chkServentRoom.Checked ? 1 : 0,
            PowerBackup = ChkPowerBackup.Checked?1:0
        }.Save();

        lblID.Text = A.ID.ToString(); ;
        lblStatus.Text = "Data Saved" + A.Message;
        WriteClientScript("parent.RefreshApartment(" + lblSocietyID.Text + "," + lblID.Text + ")");
    }

    void ShowData(int ID)
    {
        for (int i = 0; i <= 50; i++)
        {
            ddlFloor.Items.Add(i.ToString());
        }
        
        Apartment A = new Apartment() { ID = ID }.Load();
        if (A != null)
        {

            lblID.Text = A.ID.ToString();
            lblSocietyID.Text = A.SocietyID.ToString();
            ApartmentType.GetApartmentTypeName(ddlApartmentType,(int) A.SocietyID);
            ddlApartmentType.SelectedValue = A.ApartmentTypeID.ToString();
            txtAptNumber.Text = A.ApartmentNumber.ToString();
            txtBlock.Text = A.Block.ToString();
            ddlFloor.SelectedValue = A.Floor.ToString();
            ddlFacing.SelectedItem.Text = A.Facing.ToString();
            ddlLifts.SelectedValue = A.Lifts.ToString();
            ddlParking1.SelectedValue = A.Parking1.ToString();
            ddlParking2.SelectedValue = A.Parking2.ToString();
            chkServentRoom.Checked = A.ServantRoom == 1 ? true : false;
            ChkPowerBackup.Checked = A.PowerBackup==1?true:false;
        }
            
}

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        Update();
    }

    protected void btnPlus_Click(object sender, EventArgs e)
    {
        for (int i = 1; i <= 6; i++)
        {
            TextBox txt = FindControl("txtAptNo" + i) as TextBox;
            if (txt.Text != "")
            {
                lblID.Text = "0";
                txtAptNumber.Text = txt.Text;
                ddlApartmentType.SelectedValue = (FindControl("ddAptType" + i) as DropDownList).SelectedValue;
                Update();
                txt.Text = (Cmn.ToInt(txt.Text) + Cmn.ToInt(txtAdd.Text)).ToString();
            }
        }

        ddlFloor.SelectedValue = (Cmn.ToInt(ddlFloor.SelectedValue) + 1).ToString();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //using (SocietyApartmentTableAdapter ta = new SocietyApartmentTableAdapter())
            //{
            //    PropertyMap.SocietyApartmentDataTable dt = ta.GetDataByID(Cmn.ToInt(lblID.Text));
            //    if (dt.Rows.Count > 0)
            //        dt[0].Delete();
            //    ta.Update(dt);
            //}
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
        WriteClientScript("parent.RefreshApartment(" + lblSocietyID.Text + "," + lblID.Text + ")");
        ShowData(0);
    }
    protected void btnMultiAdd_Click(object sender, EventArgs e)
    {
        panelMultiAdd.Visible = true;
    }
}