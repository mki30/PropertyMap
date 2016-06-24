using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_EditPriceTrend : BasePage
{

    protected new void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String ID = QueryString("ID");
            ShowData(Cmn.ToInt(ID));   
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }

    void ShowData(int ID)
    {
        PriceTrend PT = new PriceTrend() { ID = ID }.Load();
        if (PT != null)
        {
            lblID.Text = PT.ID.ToString();
            txtMin.Text = PT.Min.ToString();
            txtMax.Text = PT.Max.ToString();
            txtQuarter.Text = PT.Quarter.ToString();
            txtYear.Text = PT.Year.ToString();
        }
    }

    private void Update()
    {
        PriceTrend PT = new PriceTrend()
        {
            ID = Cmn.ToInt(lblID.Text),
            Min = Cmn.ToInt(txtMin.Text),
            Max = Cmn.ToInt(txtMax.Text),
            Quarter=Cmn.ToInt(txtQuarter.Text),
            Year=Cmn.ToInt(txtYear.Text)
        }.Save();
        
        lblStatus.Visible = true;
        lblStatus.Text = "Data Saved-" + PT.Message;
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        Cmn.ClearTextBoxes(Page);
    }
    
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        PriceTrend PT = PriceTrend.GetByID(Cmn.ToInt(lblID.Text));
        //PT.Delete();
    }
}
