using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class Edit_AgentClientEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!IsPostBack)
        {
            //lblAvlID.Text = Request.QueryString["AvlID"] != null ? Request.QueryString["AvlID"].ToString() : "";
            ShowData(0);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }
    void Update()
    {
        AgentClient A = new AgentClient()
        {
            ID = Cmn.ToInt(lblID.Text),
            Name = txtName.Text,
            PhoneNo = txtPhone.Text,
            MobileNo = txtMobile.Text,
            EmailID = txtEmail.Text,
            City = txtCity.Text,
            Address = txtAddress.Text,
            AgentID = Cmn.ToInt(lblAgentID.Text),
        }.Save();
        lblStatus.Text = "Data Saved";
    }
    void ShowData(int ID)
    {
        lblID.Text = Request.QueryString["ClientID"] != null ? Request.QueryString["ClientID"].ToString() : "0";
        ID = Cmn.ToInt(lblID.Text);
        lblAgentID.Text = Request.QueryString["AgentID"] != null ? Request.QueryString["AgentID"].ToString() : "";
        AgentClient A = new AgentClient() { ID = ID }.Load();
        if (A != null)
        {
            lblID.Text = A.ID.ToString();
            txtName.Text = A.Name != null ? A.Name.ToString() : "";
            txtPhone.Text = A.PhoneNo != null ? A.PhoneNo.ToString() : "";
            txtMobile.Text = A.MobileNo != null ? A.MobileNo.ToString() : "";
            txtEmail.Text = A.EmailID != null ? A.EmailID : "";
            txtCity.Text = A.City != null ? A.City : "";
            txtAddress.Text = A.Address != null ? A.Address.ToString() : "";
        }
    }
}