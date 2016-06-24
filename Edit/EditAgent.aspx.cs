using System;
using System.Web.UI;
using PropertyListModel;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;

public partial class Edit_EditAgent : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        HiddenField1.Value = QueryString("UserType").ToString();

        //City.CityList(ddCity);

        if (Cmn.ToInt(QueryString("UserType").ToString()) == 1)
        {
            //txtAgentCopany.Visible = false;
            //txtDealsIn.Visible = false;
            //txtPhone.Visible = false;
            //Control ContGrp = FindControl(".trhide");
            //ContGrp.Visible = false;
        }

        if (!IsPostBack)
        {
            City.CityList(ddCity);
            if (QueryString("ReqType") == "user")
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }

            lblID.Text = QueryString("ID").ToString();

            if (Cmn.ToInt(QueryString("ID")) != 0)
                ShowData(Cmn.ToInt(QueryString("ID")));

            if (Cmn.ToInt(QueryString("ClientID")) != 0)
                ShowData(Cmn.ToInt(QueryString("ClientID")));
         }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }
    void Update()
    {
        Agent A = new Agent();
        A.ID = Cmn.ToInt(lblID.Text);
        A.UserType = Cmn.ToInt(HiddenField1.Value);
        A.URL = txtURL.Text;

        txtURLName.Text = txtURLName.Text == "" ? txtAgentName.Text.Replace(" ", "-").ToLower() : txtURLName.Text;
        
        if (!Agent.UrlNameExist(txtURLName.Text))
            A.URLName = txtURLName.Text;

        A.AgentCompany = txtAgentCopany.Text.Trim();
        A.AgentName = txtAgentName.Text.Trim();
        A.PhoneNo1 = txtPhone.Text.Trim();
        A.Mobile1 = txtMobile.Text.Trim();
        A.EmailID = txtEmail.Text.Trim();
        A.Password = txtPass.Text;
        A.City = Cmn.ToInt(ddCity.SelectedValue);
        A.OperatingIn = ddlOperatingIn.SelectedIndex != 0 ? ddlOperatingIn.SelectedItem.Text : "";
        A.OperatingSince = Cmn.ToInt(ddlOperatingSince.SelectedValue);
        A.Address = txtAddress.Text.Trim();
        A.DealsIn = ddlDealsIn.SelectedIndex != 0 ? ddlDealsIn.SelectedItem.Text.Trim() : "";
        A.BuilderDescription = txtBuilderDetail.Text.Trim();
        A.Varified = chkVarified.Checked ? 1 : 0;
        A.Deleted = chkDeleted.Checked ? 1 : 0;
        A.Save();
        if (FileUpload1.HasFile)
        {
            string Save_Location = Server.MapPath("~/Data/Images_BuilderLogo/") + A.ID + FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf("."));
            string Save_Location2 = Server.MapPath("~/Data/Images_Agent/" + A.URLName + ".jpg");
            FileUpload1.SaveAs(Save_Location);
            FileUpload1.SaveAs(Save_Location2);
        }
        lblStatus.Text = "Data Saved-" + A.Message;
        ShowData(A.ID);
        WriteClientScript("parent.RefreshSocietyList()");
        
        //Exit:
        //{
        //    lblStatus.Text = "Not Saved-UrlName already Exist";
        //}
    }    

    void ShowData(int ID)
    {
        int currentYear = Cmn.GetIndiaTime().Year;
        
        ddlOperatingSince.Items.Clear();
        ddlOperatingSince.Items.Add(new ListItem("--Select--", "0"));
        for (int i = currentYear; i >= currentYear - 50; i--)
        {
            ddlOperatingSince.Items.Add(i.ToString());
        }

        Agent A = Agent.GetByID(ID);
        if (A != null)
        {
            lblID.Text = A.ID.ToString();
            txtURLName.Text = A.URLName;
            txtAgentCopany.Text = A.AgentCompany != null ? A.AgentCompany : "";
            txtAgentName.Text = A.AgentName != null ? A.AgentName : "";
            txtPhone.Text = A.PhoneNo1 != null ? A.PhoneNo1 : "";
            txtMobile.Text = A.Mobile1 != null ? A.Mobile1 : "";
            txtURL.Text = A.URL != null ? A.URL : "";
            txtEmail.Text = A.EmailID != null ? A.EmailID : "";
            txtPass.Text = A.Password != null ? A.Password : "";
            ddCity.SelectedValue = A.City.ToString();
            ddlOperatingIn.SelectedValue = A.OperatingIn != null ? A.OperatingIn : "";
            ddlOperatingSince.SelectedValue = A.OperatingSince.ToString();
            txtAddress.Text = A.Address != null ? A.Address : "";
            ddlDealsIn.SelectedValue = A.DealsIn != null ? A.DealsIn : "";
            txtBuilderDetail.Text = A.BuilderDescription != null ? A.BuilderDescription : "";
            chkVarified.Checked = (A.Varified == 1 ? true : false);
            chkDeleted.Checked=(A.Deleted==1?true:false);
            if (A.SourceURL != null)
                ltrSource.Text = "<a href=" + A.SourceURL + " target='_blank'>Source</a>";
            
            ltWeb.Text = "<a href='" + A.URL + "' target='_blank'>Web</a>";

            string path = Server.MapPath("~/Data/Images_BuilderLogo/");

            if (File.Exists(path + A.ID + ".jpg"))
                Image1.ImageUrl = "../Data/Images_BuilderLogo/" + A.ID + ".jpg";
            else if (File.Exists(path + A.ID + ".gif"))
                Image1.ImageUrl = "../Data/Images_BuilderLogo/" + A.ID + ".gif";
            else if (File.Exists(path + A.ID + ".png"))
                Image1.ImageUrl = "../Data/Images_BuilderLogo/" + A.ID + ".png";
            lblBuilderURL.Text = "<a href='" + Global.GetRootPathVirtual + "/" + A.AgentName +"' target='_blank'>Microsite</a>";
         }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblID.Text = "0";
        Cmn.ClearTextBoxes(Page);
        Image1.ImageUrl = "";
        //Update();
        //WriteClientScript("parent.RefreshSocietyList()");
        //ShowData(0);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Agent A = new Agent() { ID = Cmn.ToInt(lblID.Text) }.Delete();
        lblStatus.Text = "Deleted" + A.Message;
    }
}