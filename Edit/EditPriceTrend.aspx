<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPriceTrend.aspx.cs" Inherits="Edit_EditPriceTrend" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Min</td>
                    <td>
                        <asp:TextBox ID="txtMin" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Max</td>
                    <td>
                        <asp:TextBox ID="txtMax" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Quarter</td>
                    <td>
                        <asp:TextBox ID="txtQuarter" runat="server"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td>Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:200px;">
                        <asp:Label ID="lblStatus" runat="server" Text="Label" ForeColor="#FF3300" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"/>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
