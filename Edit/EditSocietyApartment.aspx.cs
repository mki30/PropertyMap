using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditSocietyApartment :BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!IsPostBack)
        {
            lblSocietyID.Text = QueryString("SocietyID");
            //Utility.GetApartmentType(ddlApartmentType, Cmn.ToInt(lblSocietyID.Text));
            ShowData(Cmn.ToInt(QueryString("ID")));

        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        UpdateApartment(Cmn.ToInt(lblID.Text));
    }

    void UpdateApartment(int ID)
    {
        try
        {
            //using (SocietyApartmentTableAdapter ta = new SocietyApartmentTableAdapter())
            //{
            //    PropertyMap.SocietyApartmentDataTable dt = ta.GetDataByID(ID);
            //    PropertyMap.SocietyApartmentRow dr = dt.Rows.Count == 0 ? dt.NewSocietyApartmentRow() : dt[0];
            //    dr.SocietyID = Cmn.ToInt(lblSocietyID.Text);
            //    dr.ApartmentTypeID = Cmn.ToInt(ddlApartmentType.SelectedValue);
            //    dr.ApartmentNumber = txtAptNumber.Text;
            //    dr.Block = txtBlock.Text;
            //    dr.Floor = Cmn.ToInt(txtFloor.Text);
            //    dr.Facing = ddlFacing.SelectedItem.Text;
            //    dr.Lifts = Cmn.ToInt(txtLifts.Text);
            //    dr.Parking1 = ddlParking1.SelectedItem.Text;
            //    dr.Parking2 = ddlParking2.SelectedItem.Text;
            //    dr.ServantRoom = chkServentRoom.Checked ? 1 : 0;
            //    dr.PowerBackup = Cmn.ToInt(txtPowerBackup.Text);

            //    if (dt.Rows.Count == 0)
            //        dt.Rows.Add(dr);

            //    ta.Update(dt);

            //    if (Cmn.ToInt(lblID.Text) == 0)
            //        lblID.Text = ta.MaxID().ToString();

            //    WriteClientScript("parent.RefreshApartment(" + lblSocietyID.Text + "," + lblID.Text + ")");
            //}

            lblStatus.Text = "Data Saved";

        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }

    void ShowData(int ID)
    {
        try
        {
            //using (SocietyApartmentTableAdapter ta = new SocietyApartmentTableAdapter())
            //{
            //    PropertyMap.SocietyApartmentDataTable dt = ta.GetDataByID(ID);
            //    PropertyMap.SocietyApartmentRow dr = dt.Rows.Count == 0 ? dt.NewSocietyApartmentRow() : dt[0];

            //    if (dt.Rows.Count > 0)
            //    {
            //        lblID.Text = dr.ID.ToString();
            //        lblSocietyID.Text = dr.IsSocietyIDNull() ? " " : dr.SocietyID.ToString();
            //    }
            //    ddlApartmentType.SelectedValue = dr.IsApartmentTypeIDNull() ? "0" : dr.ApartmentTypeID.ToString();
            //    txtAptNumber.Text = dr.IsApartmentNumberNull() ? " " : dr.ApartmentNumber.ToString();
            //    txtBlock.Text = dr.IsBlockNull() ? " " : dr.Block.ToString();
            //    txtFloor.Text = dr.IsFloorNull() ? " " : dr.Floor.ToString();
            //    ddlFacing.SelectedItem.Text = dr.IsFacingNull() ? "East" : dr.Facing.ToString();
            //    txtLifts.Text = dr.IsLiftsNull() ? " " : dr.Lifts.ToString();
            //    ddlParking1.SelectedValue = dr.IsParking1Null() ? "0" : dr.Parking1.ToString();
            //    ddlParking2.SelectedValue = dr.IsParking2Null() ? "0" : dr.Parking2.ToString();

            //    chkServentRoom.Checked = dr.IsServantRoomNull() ? false : (dr.ServantRoom == 1 ? true : false);
            //    txtPowerBackup.Text = dr.IsPowerBackupNull() ? "" : dr.PowerBackup.ToString();

            //}
        }

        catch (Exception ex)
        {
            Response.Write(ex);
        }

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        ShowData(0);
    }

    protected void btnPlus_Click(object sender, EventArgs e)
    {
        int from = Cmn.ToInt(txtFrom.Text);
        int to = Cmn.ToInt(txtTo.Text);

        for (int a = from; a <= to; a++)
        {
            txtAptNumber.Text = a.ToString();
            UpdateApartment(0);
        }

        txtFloor.Text = (Cmn.ToInt(txtFloor.Text) + 1).ToString();
        txtFrom.Text = (from + Cmn.ToInt(txtAdd.Text)).ToString();
        txtTo.Text = (to + Cmn.ToInt(txtAdd.Text)).ToString();
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
}