using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_EditCityDetail : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        int ID = Cmn.ToInt(QueryString("ID"));
        if (!IsPostBack)
            ShowData(ID);
    }

    void ShowData(int CityID)
    {
        City C = City.GetByID(CityID);
        if (C != null)
        {
            lblID.Text = C.ID.ToString();
            txtParentID.Text = C.ParentID.ToString();
            txtName.Text = C.Name;
            txtSortName.Text = C.SortName;
            txtLat.Text = C.Lat.ToString();
            txtLng.Text = C.Lng.ToString();
            txtPolyPoints.Text = C.PolyPoints;
            chkVerified.Checked= C.Verified ==1 ;

            int? ParentID = C.ParentID;

            lblParentName.Text = "";
            while (ParentID != null && ParentID != 0)
            {
                City parent = City.GetByID((int)ParentID);
                if (parent != null)
                {
                    lblParentName.Text += " " + parent.Name;
                    ParentID = parent.ParentID;
                }
                else
                    break;
            }
            lblParentName.Text = txtName.Text.Replace("Sector 00", "Sector ").Replace("Sector 0", "Sector ") + ", " + lblParentName.Text.Trim();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        City C = City.GetByID(Cmn.ToInt(lblID.Text));

        if (C == null)
            C = new City();

        C.Name = txtName.Text;
        C.SortName = txtSortName.Text;
        C.ParentID = Cmn.ToInt(txtParentID.Text);
        C.Lat = Cmn.ToDbl(txtLat.Text);
        C.Lng = Cmn.ToDbl(txtLng.Text);
        C.PolyPoints = txtPolyPoints.Text;
        C.Verified = chkVerified.Checked ? 1 : 0;
        C.Save();
        Response.Write(C.Message);
    }

    protected void btnCreateSubCity_Click(object sender, EventArgs e)
    {
        int ParentID = Cmn.ToInt(lblID.Text);
        if (txtSubCity.Text != "")
        {

            string[] Lines = txtSubCity.Text.Split('\n');
            foreach (string S in Lines)
            {
                if (S.Trim() != "")
                {
                    City c = City.GetByName(ParentID, S.Trim());
                    if (c == null)
                        c = new City();

                    c.ParentID = ParentID;
                    c.Name = S.Trim();
                    c.SortName = S.Trim();
                    c.Save();
                }
            }
        }
        else if (txtFrom.Text != "" && txtTo.Text != "")
        {
            int From = Cmn.ToInt(txtFrom.Text);
            int To = Cmn.ToInt(txtTo.Text);
            for (int i = From; i <= To; i++)
            {
                string CityName="Sector " + i;
                string SortCityName="Sector " + (To > 100 ? i.ToString("000") : To > 10 ? i.ToString("00") : i.ToString());

                City c = City.GetByName(ParentID, CityName);
                    if (c == null)
                        c = new City();

                    c.ParentID = ParentID;
                    c.Name = CityName;
                    c.SortName = SortCityName;
                    c.Save();
            }
        }
    }
    protected void btnUpdateChildCount_Click(object sender, EventArgs e)
    {

        List<City> list = City.GetAll();

        foreach (City C in list)
        {
            //if (C.ID == 475)
            {

                List<City> childlist = City.GetByParentID(C.ID);
                C.ChildCount = childlist == null ? 0 : childlist.Count;

                if (C.ChildCount == 0 || C.ChildCount == null)
                {
                    List<Society> Societylist = Society.GetByArea(C.ID);
                    if (Societylist.Count == 0)
                        Societylist = Society.GetBySubCity(C.ID);

                    C.ChildSocietyCount = Societylist == null ? 0 : Societylist.Count;
                }

                if (C.ChildCount != 0 || C.ChildSocietyCount != 0)
                    C.Save();
            }
        }

    }
    protected void btnUpdateSortName_Click(object sender, EventArgs e)
    {
        List<City> list = City.GetByParentID(Cmn.ToInt(txtParentID.Text));

        Regex regex = new Regex(@"^[0-9]+$");
        foreach (City C in list)
        {
            string[] data = C.Name.Split(' ');
            string SortName = C.Name;

            if (data.Length == 2)
            {
                if (regex.IsMatch(data[1]))
                {
                    SortName = data[0] + " " + data[1].PadLeft(Cmn.ToInt(txtDigits), '0');
                }
            }

            if (SortName != C.SortName)
            {
                C.SortName = SortName;
                C.Save();
            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        City C = City.GetByID(Cmn.ToInt(lblID.Text));
        C.Delete();
    }
}