using System;
using PropertyListModel;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web;

public partial class Edit_EditApartmentType : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        lblStatus.Text = "";

        if (!IsPostBack)
        {
            ShowAptsList();
            lblSocietyID.Text = QueryString("SocietyID");
            lblID.Text = QueryString("AptTypeID");
            //lstApartmentType.SelectedIndex = lstApartmentType.Items.Count - 1;  //select last item in list
            lstApartmentType.SelectedValue = lblID.Text;
            CreateChkBoxList();
            ShowData(Cmn.ToInt(lstApartmentType.SelectedValue));
            //ShowData(Cmn.ToInt(QueryString("ID")));
        }
        else if (IsPostBack)
            CreateChkBoxList();
    }

    private void CreateChkBoxList()
    {
        foreach (var features in Enum.GetValues(typeof(OtherFeatures)))
        {
            CheckBox chk = new CheckBox();
            chk.Text = features.ToString().Replace('_', ' ');
            chk.ID = "chkFeatures" + (int)features;
            lblCheckbox.Controls.Add(new Literal() { Text = "<span style=''padding-bottom:1px;></span>" });
            lblCheckbox.Controls.Add(chk);
            lblCheckbox.Controls.Add(new Literal() { Text = "</br>" });
        }
    }
    
    void ShowAptsList()
    {
        ApartmentType.GetApartmentTypeName(lstApartmentType, Cmn.ToInt(QueryString("SocietyID")));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
        ShowAptsList();
    }

    void Update()
    {
        string featurevalues = "";

        foreach (var BA in Enum.GetValues(typeof(OtherFeatures)))
        {
            featurevalues += (lblCheckbox.FindControl("chkFeatures" + (int)BA) as CheckBox).Checked ? "1" : "0";
        }
        
        ApartmentType A = new ApartmentType();
        A.ID = Cmn.ToInt(lblID.Text);
        A.SocietyID = Cmn.ToInt(lblSocietyID.Text);
        A.TypeName = txtAptType.Text;
        A.Bedroom = Cmn.ToInt(ddlBedroom.SelectedValue);
        A.Bathroom = Cmn.ToInt(ddlBathroom.SelectedValue);
        A.Balcony = Cmn.ToInt(ddlBalcony.SelectedValue);
        //A.Area = Cmn.ToInt(txtArea.Text);
        A.PlotArea = Cmn.ToInt(txtPlotArea.Text);
        A.SuperArea = Cmn.ToInt(txtSuperArea);
        A.Unit = Cmn.ToInt(ddUnit.SelectedItem.Value);
        A.OtherFeatures = featurevalues;
        A.Description = txtDescription.Text;
        A.BSP = Cmn.ToInt(txtBSP.Text);
        A.BSP_Installments = Cmn.ToInt(txtBSPInstall.Text);
        A.MaintenanceDep = Cmn.ToInt(txtMaintDep.Text);
        A.ParkingDep = Cmn.ToInt(txtParkingDep.Text);
        A.PowerBackupDep = Cmn.ToInt(txtPB.Text);
        A.ClubDep = Cmn.ToInt(txtClub.Text);
        A.PLC = Cmn.ToInt(txtPLC.Text);
        A.UseType = Cmn.ToInt(ddUseType.SelectedItem.Value);
        A.PropertyType = Cmn.ToInt(ddPropertyType.SelectedValue);
        A.Save();
        
        if (A.Message.Length == 0)
        {
            //if (FileUpload1.HasFile)
            //{
            //    if (ddPropertyType.SelectedItem.Text == "Villa")
            //    {
            //        HttpFileCollection hfc = Request.Files;
            //        for (int img = 0; img < hfc.Count; img++)
            //        {
            //            HttpPostedFile hpf = hfc[img];
            //            if (hpf.ContentLength > 0)
            //            {
            //                hpf.SaveAs(GetSocietyImageFileName(A.ID));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        string Save_Location = Server.MapPath("~/Data/Images_ApartmentType/") + A.ID + FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf("."));
            //        FileUpload1.SaveAs(Save_Location);
            //    }
            //}

            if (ddPropertyType.SelectedItem.Value == "4")
            {
                //Independent I = Independent.GetByAptID(Cmn.ToInt(lblID.Text));
                //if (I == null)
                //    I = new Independent();
                //I.ProjID = Cmn.ToInt(lblSocietyID.Text);
                //I.AptTypeID = Cmn.ToInt(lblID.Text);
                //I.Floor = txtFloor.Text;
                //I.BuiltupArea = Cmn.ToInt(txtBuiltUprea.Text);
                //I.Terrace = Cmn.ToInt(txtTerrace.Text);
                //I.Lawn = Cmn.ToInt(txtLawn.Text);
                //I.Save();
            }

            UpdatePriceList();

            Society S = Society.GetByID(Cmn.ToInt(A.SocietyID));
            S.Save();

            lblStatus.Text = "Data Saved -";
            ShowData(A.ID);
            WriteClientScript("parent.RefreshApartmentType(" + lblSocietyID.Text + "," + lblID.Text + ")");      //call for refresh page after update
        }
        else
            lblStatus.Text = A.Message;
        ShowData(A.ID);
    }

    string GetSocietyImageFileName(int ID)    //get underscore names.
    {
        string FileName = "";

        var Folder = "~/Data/Images_ApartmentType/";
        for (int i = 1; ; i++)
        {
            FileName = Server.MapPath(Folder + ID + "_" + i) + ".jpg";
            if (!File.Exists(FileName))
                break;
        }
        return FileName;
    }

    void UpdatePriceList()
    {
        PriceList PL = new PriceList()
        {
            ID = Cmn.ToInt(lblPriceListID.Text),
            SocietyID = Cmn.ToInt(lblSocietyID.Text),
            ApartmentTypeID = Cmn.ToInt(lblID.Text),
            BSP = Cmn.ToInt(txtBSP.Text),
            BSP_Installments = Cmn.ToInt(txtBSPInstall.Text),
            MaintenanceDep = Cmn.ToInt(txtMaintDep.Text),
            ParkingDep = Cmn.ToInt(txtParkingDep.Text),
            PowerBackupDep = Cmn.ToInt(txtPB.Text),
            PLC = Cmn.ToInt(txtPLC.Text)
        }.Save();
    }
    void ShowData(int ID)
    {
        ApartmentType A = ApartmentType.GetByID(ID);
        if (A != null)
        {
            lblID.Text = A.ID.ToString();
            txtAptType.Text = A.TypeName;
            lblSocietyID.Text = A.SocietyID.ToString();
            ddlBedroom.SelectedValue = A.Bedroom.ToString();
            ddlBathroom.SelectedValue = A.Bathroom.ToString();
            ddlBalcony.SelectedValue = A.Balcony.ToString();

            string features = A.OtherFeatures != null ? A.OtherFeatures.ToString().PadRight(10, '0') : "00000000000";
            foreach (var BA in Enum.GetValues(typeof(OtherFeatures)))
            {
                try
                {
                    (lblCheckbox.FindControl("chkFeatures" + (int)BA) as CheckBox).Checked = features[(int)BA] == '1' ? true : false;
                }
                catch
                {

                }
            }
            
            //txtArea.Text = A.Area.ToString();
            txtPlotArea.Text = A.PlotArea.ToString();
            txtSuperArea.Text = A.SuperArea.ToString();
            txtDescription.Text = A.Description;

            txtBSP.Text = A.BSP.ToString();
            txtBSPInstall.Text = A.BSP_Installments.ToString();
            txtMaintDep.Text = A.MaintenanceDep.ToString();
            txtParkingDep.Text = A.ParkingDep.ToString();
            txtPB.Text = A.PowerBackupDep.ToString();
            txtBSPInstall.Text = A.ClubDep.ToString();
            txtPLC.Text = A.PLC.ToString();

            ddUnit.SelectedValue = A.Unit.ToString();
            ddUseType.SelectedIndex = Cmn.ToInt(A.UseType);
            ddPropertyType.SelectedIndex = Cmn.ToInt(A.PropertyType);

            ltImageUploader.Text = "<iframe id='imgFrame' src='ImageUploader.aspx?AptID=" + ID + "&ProjectID="+lblSocietyID.Text+"' style='height:120px;width:400px;border:1px solid gainsboro;'></iframe>";

            ShowPriceList(Cmn.ToInt(lblSocietyID.Text), Cmn.ToInt(lblID.Text));   //Show price List from PriceList table 
        }
    }

    void ShowPriceList(int SocID, int AprtTypeID)
    {
        PriceList PL = new PriceList() { SocietyID = SocID, ApartmentTypeID = AprtTypeID }.LoadBySocIDandAptTypeID();
        
        if (PL != null)
        {
            lblPriceListID.Text = PL.ID.ToString();
            txtBSP.Text = PL.BSP != null ? PL.BSP.ToString() : "";

            txtBSPInstall.Text = PL.BSP_Installments != null ? PL.BSP_Installments.ToString() : "";
            txtMaintDep.Text = PL.MaintenanceDep != null ? PL.MaintenanceDep.ToString() : "";
            txtParkingDep.Text = PL.ParkingDep != null ? PL.ParkingDep.ToString() : "";

            txtPB.Text = PL.PowerBackupDep != null ? PL.PowerBackupDep.ToString() : "";
            txtPLC.Text = PL.PLC != null ? PL.PLC.ToString() : "";
        }
        else if (PL == null)
        {
            lblPriceListID.Text = "0";
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        //Image1.ImageUrl = "";
        Cmn.ClearTextBoxes(Page);
        //ShowData(0);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string[] Lines = TextBox1.Text.Split('\n');
        foreach (string s in Lines)
        {
            string[] F = s.Split('\t');

            new ApartmentType()
            {
                TypeName = F[0].Trim(),
                Bedroom = Cmn.ToInt(F[2].Trim()),
                Bathroom = Cmn.ToInt(F[3].Trim()),
                Area = Cmn.ToInt(F[4].Trim()),
                SuperArea = Cmn.ToInt(F[5].Trim()),
                Balcony = Cmn.ToInt(F[6].Trim())
            }.Save();
        }
    }

    protected void lstApartmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowData(Cmn.ToInt(lstApartmentType.SelectedValue));
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ApartmentType A = new ApartmentType()
        {
            ID = Cmn.ToInt(lblID.Text)
        }.Delete();

        if (A.Message == "")
        {
            string path = Server.MapPath("~/Data/Images_ApartmentType/" + A.ID + ".jpg");
            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            lblStatus.Text = "Deleted" + A.Message;
            ShowData(Cmn.ToInt(lblSocietyID.Text));
            
            WriteClientScript("parent.RefreshApartmentType(" + lblSocietyID.Text + "," + lblID.Text + ")");   //call refresh page after delete.
        }
        else
            lblStatus.Text = "Something Wrong Not Deleted!" + A.Message;
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string[] Lines = txtImportText.Text.Split('\n');

        foreach (string L in Lines)
        {
            if (L.Trim() == "")
                continue;

            string[] F = L.Split('\t');

            if (F.Length >= 4)
            {
                string type = F[0].Trim();

                ApartmentType A = ApartmentType.GetByName(Cmn.ToInt(lblSocietyID.Text), type);
                if (A == null)
                    A = new ApartmentType();

                A.SocietyID = Cmn.ToInt(lblSocietyID.Text);
                A.TypeName = type;
                A.Bedroom = Cmn.ToInt(F[1].Trim());
                A.Bathroom = Cmn.ToInt(F[2].Trim());
                A.SuperArea = Cmn.ToInt(F[3].Trim());
                A.Save();
            }
        }
        ShowAptsList();
    }

    protected void btnBulkBSP_Click(object sender, EventArgs e)      //Bulk BSP Update
    {
        List<ApartmentType> ApartmentTypeList = ApartmentType.GetList(Cmn.ToInt(lblSocietyID.Text));
        foreach (ApartmentType S in ApartmentTypeList)
        {
           S.BSP = Cmn.ToInt(txtBSP.Text);
           S.Save();
           PriceList PL = PriceList.GetByAptID(S.ID);
           if (PL == null)
               PL = new PriceList();
           PL.SocietyID = Cmn.ToInt(S.SocietyID);
           PL.ApartmentTypeID = S.ID;
           PL.BSP = Cmn.ToInt(txtBSP.Text);
           PL.Save();
        }
    }
}


