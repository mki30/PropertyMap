<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditApartment.aspx.cs" Inherits="Edit_EditApartment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
</head>
<body style="padding-left:5px;">
    <form id="form1" runat="server">
    <div style="padding-top:5px;">
        <table style="border-spacing:0px;">
            <tr>
                <td>
                    ID:
                    <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                </td>
                <td>
                    Society :<asp:Label ID="lblSocietyID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Type
                </td>
                <td>
                    <asp:DropDownList ID="ddlApartmentType" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Block No
                </td>
                <td>
                    <asp:TextBox ID="txtBlock" runat="server" Width="40px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Apt. No
                </td>
                <td>
                    <asp:TextBox ID="txtAptNumber" runat="server" Width="40px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Floor
                </td>
                <td>
                    <asp:DropDownList ID="ddlFloor" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Facing
                </td>
                <td>
                    <asp:DropDownList ID="ddlFacing" runat="server">
                        <asp:ListItem>East</asp:ListItem>
                        <asp:ListItem>West</asp:ListItem>
                        <asp:ListItem>North</asp:ListItem>
                        <asp:ListItem>South</asp:ListItem>
                        <asp:ListItem>South-East</asp:ListItem>
                        <asp:ListItem>North-East</asp:ListItem>
                        <asp:ListItem>Norht-west</asp:ListItem>
                        <asp:ListItem>South-West</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Lifts
                </td>
                <td>
                     <asp:DropDownList ID="ddlLifts" runat="server">
                         <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Parking1
                </td>
                <td>
                    <asp:DropDownList ID="ddlParking1" runat="server">
                        <asp:ListItem Value="0">Uncovered</asp:ListItem>
                        <asp:ListItem Value="1">Covered</asp:ListItem>
                        <asp:ListItem Value="2">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Parking2
                </td>
                <td>
                    <asp:DropDownList ID="ddlParking2" runat="server">
                        <asp:ListItem Value="2">No</asp:ListItem>
                        <asp:ListItem Value="0">Uncovered</asp:ListItem>
                        <asp:ListItem Value="1">Covered</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:CheckBox ID="chkServentRoom" runat="server" Text="Servant Room" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:CheckBox ID="ChkPowerBackup" runat="server" Text="Power Backup" />
                </td>
            </tr>
            <tr>
                <td  style="vertical-align:bottom;">
                    <asp:Label ID="lblStatus" runat="server" ForeColor="#FF3300"></asp:Label><br/>
                    <asp:Button ID="Button1" runat="server" Text="Multi Add" onclick="btnMultiAdd_Click" title="Add Multiple Apartments" />
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" Style="width: 50px;" OnClick="btnSave_Click" ToolTip="Save Apartment"/>
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add"  ToolTip="Add New Apartment" />
                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" ToolTip="Delete Apartment"/>
                </td>
            </tr>
         
            <tr>
                <td colspan="2">

                    <asp:Panel ID="panelMultiAdd" runat="server" Visible="false">
                    <table style="border-spacing:0px;">
                        <tr>
                            <td>
                                Apt No
                            </td>
                            <td>
                                <asp:TextBox ID="txtAptNo1" runat="server" Width="50px">101</asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddAptType1" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
             <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtAptNo2" runat="server" Width="50px">102</asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddAptType2" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtAptNo3" runat="server" Width="50px">103</asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddAptType3" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtAptNo4" runat="server" Width="50px">104</asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddAptType4" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtAptNo5" runat="server" Width="50px"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddAptType5" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtAptNo6" runat="server" Width="50px"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddAptType6" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
                    Increment<asp:TextBox ID="txtAdd" runat="server" Width="31px">100</asp:TextBox>
                        <asp:Button ID="btnPlus" runat="server" Text="Add" OnClick="btnPlus_Click" />
                        </asp:Panel>
        </td> 
        
        </tr> </table>
    </div>
    </form>
</body>
</html>
