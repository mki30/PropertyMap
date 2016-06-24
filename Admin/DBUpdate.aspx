<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBUpdate.aspx.cs" Inherits="Admin_DBUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:Button ID="btnUpdateAgentID" runat="server" OnClick="btnUpdateAgentID_Click" Text="Update Agent ID in Availability" />
        <br/>
        <asp:Button ID="btnUserType" runat="server" Text="Update User Type" OnClick="btnUserType_Click"  />
        <br />
        <asp:Button ID="btnAddOwnerData" runat="server" Text="Add Data Owner To Agent" OnClick="btnAddOwnerData_Click"/>
        <br/>
        <asp:Button ID="btnAddBuilderData" runat="server" Text="Add ADta Builder To  Agent" onclick="btnAddBuilderData_Click" />
        <br />
        <asp:Button ID="btnAddDataToClient" runat="server" Text="Add Data to Client" OnClick="btnAddDataToClient_Click" />
        <br />
        <asp:Button ID="btnUpdateSocietyIDinAvl" runat="server" Text="UpdateSellerID in Avl" OnClick="btnUpdateSocietyIDinAvl_Click" />
        <br />
        <asp:Button ID="btnUpdateLandmark" runat="server" Text="UpdateLandmark" OnClick="btnUpdateSocietyIDinAvl0_Click" />
        <br/>
        <asp:Button ID="btnSocietyUpdate" runat="server" Text="Update Subcity In Society" OnClick="btnSocietyUpdate_Click" /><br />
        <asp:Button ID="btnGeneratePriceTrend" runat="server" Text="GenPriceTrend" OnClick="btnGeneratePriceTrend_Click" />
        <br />
        <asp:Button ID="btnUpdateURLName" runat="server" OnClick="btnUpdateURLName_Click" Text="Update Society URL Name" />
        <br />
        <asp:Button ID="btnUpdateGlobal" runat="server" Text="Update Global" OnClick="btnUpdateGlobal_Click" />
        <br />
        <asp:Button ID="btnUpdtePTCityID" runat="server" OnClick="btnUpdtePTCityID_Click" Text="btnUpdatePriceTrendCityID" />
        <br />
        <asp:Button ID="btnUpdateBuilderName" runat="server" OnClick="btnUpdateBuilderName_Click" Text="Update BuilderName" />
        <br />
        <asp:Button ID="btnCityUrlNameUpdate" runat="server"  Text="Update CityUrlName" OnClick="btnCityUrlNameUpdate_Click" />
        <br />
        <asp:Button ID="btnUpdateImageName" runat="server"  Text="Update ImageName" OnClick="btnUpdateImageName_Click" />
        <br/>
        <asp:Button ID="btnUptateFromCsv" runat="server" Text="CSV Update" OnClick="btnUptateFromCsv_Click" />
        <br />
        <asp:Button ID="btnApplyWaterMark" runat="server" Text="Apply WaterMark" OnClick="btnApplyWaterMark_Click"/>
        <br />
        <asp:Button ID="btnUpdateAgentUrlName" runat="server" Text="Update AgentUrlName" OnClick="btnUpdateAgentUrlName_Click"/>
        <br />
        <asp:Button ID="btnImageTable" runat="server" Text="Fill Image Table" OnClick="btnImageTable_Click"/>
        <br/>
        <asp:Button ID="btnUpdateImageUrlNames" runat="server" Text="Update Image UrlName in Image Table" OnClick="btnUpdateImageUrlNames_Click" />
        <br />
        <asp:Button ID="btnRenameFiles" runat="server" Text="Rename Files in Image flolders" OnClick="btnRenameFiles_Click"/>
        <br />
        <asp:Button ID="CreatePolyPoints" runat="server" Text="CreatePolyPoints" OnClick="CreatePolyPoints_Click"/>
        <br />
        <asp:Button ID="CreateSocietyText" runat="server" Text="CreateSocietyText" OnClick="CreateSocietyText_Click"/>
    </div>
    </form>
</body>
</html>
