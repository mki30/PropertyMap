using PropertyListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMI_EquatedMonthlyInstallment :BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        Title ="EMI Calculator" + Global.AppTitle;
        MetaDescription = "simple calculator for calculating loan emi";
        MetaKeywords="emi calculator,loan emi calculator,easy emi calculation,loan emi calculation,loan emi calculation for property";

        int SocID = Cmn.ToInt(QueryString("SocID"));
        int PlanID = Cmn.ToInt(QueryString("AptTypeID"));

        ShowData(PlanID, SocID);
    }

    void ShowData(int PlanID, int SocID)
    {
        if (PlanID == 0 || SocID == 0)
            return;

        PriceList PL = PriceList.GetByAptTypeIDandSocID(PlanID, SocID);
        if (PL != null)
        {
            ApartmentType A = ApartmentType.GetByID(PlanID);
            if (A != null)
            {
                txtArea.Text = A.SuperArea.ToString();
            }
            BSPRate.Text = PL.BSP.ToString();
            MaintenanceDeposit.Text = PL.MaintenanceDep.ToString();
            PowerBack.Text = PL.PowerBackupDep.ToString();
            Parking.Text = PL.ParkingDep.ToString();
            PLCRate.Text = PL.PLC.ToString();
            //BSPServiceTaxRate.Text = PL.ServiceTax_BSP.ToString();
            //PowerBackServiceTaxRate.Text = PL.ServiceTax_PowBak.ToString();
            //ParkingServiceTax.Text = PL.ServiceTax_Parking.ToString();
            //AgentRate.Text = "1";
            // RegistryRate.Text = "7";
            //AmtPerc.Text = "80";
            IntrestRate.Text = "12";
            Year.Text = "10";
        }
    }
}