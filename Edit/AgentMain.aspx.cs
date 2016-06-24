using System;

public partial class Edit_AgentMain : BasePageEdit
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        string sellerType=Request.QueryString["sellerType"]!=null?Request.QueryString["sellerType"].ToString():"";
        Cmn.WriteClientScript(this, "var sellerType=" + sellerType);
        ltEditType.Text = "Edit " + (sellerType == "0" ? "Agent" : sellerType == "1"?"Owner":"Builder");
        ltSellerHead.Text=(sellerType=="0"?"Agent":sellerType=="1"?"Owner":"Builder");
    }
}