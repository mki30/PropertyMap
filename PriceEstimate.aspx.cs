using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Text;

public partial class PriceEstimate : BasePage
{
    List<ProjectPricing> PTL;
    RadioButtonList radioBspPlan;
    protected new void Page_Load(object sender, EventArgs e)
    {
        PTL = ProjectPricing.GetByProjectID(1670);
        if (PTL.Count > 0)
        {
            radioBspPlan = new RadioButtonList();
            radioBspPlan.ID = "radioBspPlan";
            radioBspPlan.RepeatDirection = RepeatDirection.Horizontal;
            foreach (ProjectPricing PP in PTL)
            {
                if ((PP.Type == (int)ProjectPriceType.BSP_Fexi_Plan || PP.Type == (int)ProjectPriceType.BSP_SP_Plan))
                {
                    if (!radioBspPlan.Items.Contains(new ListItem(Global.GetText((ProjectPriceType)PP.Type), PP.Type.ToString())))
                        radioBspPlan.Items.Add(new ListItem(Global.GetText((ProjectPriceType)PP.Type), PP.Type.ToString()));
                }
            }
            radioBspPlan.SelectedIndexChanged += new EventHandler(BSPSelectionChanged);
            radioBspPlan.AutoPostBack = true;
            lblPlan.Controls.Add(radioBspPlan);
        }
        if (!IsPostBack)
        {
            int id = QueryStringInt("id");
            ApartmentType AT = Global.ApartmentTypeList.FirstOrDefault(m => m.Key == id).Value;
            if (AT != null)
            {
                lblTypeName.Text = AT.TypeName;
                lblArea.Text = AT.SuperArea.ToString();
                lblSociety.Text = AT.Parent.SocietyName;
                radioBspPlan.SelectedIndex = 0;
                ShowPriceEstimate();
            }
        }
    }
    private void ShowPriceEstimate(int BspSelected = (int)ProjectPriceType.BSP_Fexi_Plan)
    {
        List<ProjectPricing> BSPList = PTL.Where(m => m.Type == BspSelected).ToList();
        StringBuilder sb = new StringBuilder();
        
        if (BSPList.Count > 0)
        {
            ddBSPPlan.Items.Clear();
            foreach (ProjectPricing PP in BSPList)
                ddBSPPlan.Items.Add(new ListItem(PP.Name + "-" + PP.Value, PP.Value));
        }
        List<ProjectPricing> PLCPriceList = PTL.Where(m => m.Type == (int)ProjectPriceType.Location_PLC).ToList();
        if (PLCPriceList.Count > 0)
        {
            ddLocationPLC.Items.Clear();
            foreach (ProjectPricing PP in PLCPriceList)
                ddLocationPLC.Items.Add(new ListItem(PP.Name + "-" + PP.Value, PP.Value));
        }
        txtLeaseRent.Text = PTL.FirstOrDefault(m => m.Type == (int)ProjectPriceType.Lease_Rent).Value;
        txtClubMembrship.Text = PTL.FirstOrDefault(m => m.Type == (int)ProjectPriceType.Club_Membership).Value;
        txtFFC.Text = PTL.FirstOrDefault(m => m.Type == (int)ProjectPriceType.Fire_Fighting_Charges).Value;
        txtEEC.Text = PTL.FirstOrDefault(m => m.Type == (int)ProjectPriceType.External_Elictrical_Charges).Value;
        txtPowerBackup.Text = PTL.FirstOrDefault(m => m.Type == (int)ProjectPriceType.Power_Backup).Value;
        List<ProjectPricing> ParkingList = PTL.Where(m => m.Type == (int)ProjectPriceType.Parking).ToList();
        if (ParkingList.Count > 0)
        {
            ddCarParking.Items.Clear();
            foreach (ProjectPricing PP in ParkingList)
                ddCarParking.Items.Add(new ListItem(PP.Name, PP.Value));
        }
        lblSalePrice.Text = sb.ToString();
    }

    private void BSPSelectionChanged(object sender, EventArgs e)
    {
        RadioButtonList radioBSPSelect = (RadioButtonList)sender;
        int a = radioBSPSelect.SelectedIndex;
        ShowPriceEstimate(radioBSPSelect.SelectedIndex);
    }

}