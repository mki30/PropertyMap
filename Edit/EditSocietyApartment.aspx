<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditSocietyApartment.aspx.cs" Inherits="Edit_EditSocietyApartment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/Simple.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                    <asp:DropDownList ID="ddlApartmentType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Apartment No:
                </td>
                <td>
                    <asp:TextBox ID="txtAptNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Block No:
                </td>
                <td>
                    <asp:TextBox ID="txtBlock" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Floor
                </td>
                <td>
                    <asp:TextBox ID="txtFloor" runat="server"></asp:TextBox>
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
                    <asp:TextBox ID="txtLifts" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Parking1
                </td>
                <td>
                    <asp:DropDownList ID="ddlParking1" runat="server">
                        <asp:ListItem>Covered</asp:ListItem>
                        <asp:ListItem>Uncovered</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Parking2
                </td>
                <td>
                    <asp:DropDownList ID="ddlParking2" runat="server">
                        <asp:ListItem>Covered</asp:ListItem>
                        <asp:ListItem>Uncovered</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Servent Room
                </td>
                <td>
                    <asp:CheckBox ID="chkServentRoom" runat="server" Text="Yes" />
                </td>
            </tr>
            <tr>
                <td>
                    Power Backup
                </td>
                <td>
                    <asp:TextBox ID="txtPowerBackup" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="#FF3300"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" Style="width: 50px;" OnClick="btnSave_Click" />
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" Text="Delete" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    ---------------------------------</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    From<asp:TextBox ID="txtFrom" runat="server" Width="31px"></asp:TextBox>
                    To<asp:TextBox ID="txtTo" runat="server" Width="31px"></asp:TextBox>
                    Increment<asp:TextBox ID="txtAdd" runat="server" Width="31px"></asp:TextBox>

                    <asp:Button ID="btnPlus" runat="server"  Text="+" onclick="btnPlus_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
