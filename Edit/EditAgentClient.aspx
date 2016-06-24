<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAgentClient.aspx.cs" Inherits="Edit_EditAgentClient" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link rel="stylesheet/less" type="text/css" href="../css/StyleSheet.less">
    <script type="text/javascript" src="../js/less-1.3.0.min.js"></script>
</head>

<body class="bodyAgentClient">
    <form id="form1" runat="server">
    <div>
        <table style="border-spacing: 0px;">
            <tr>
                <td>ID:
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Agent Id:
                </td>
                <td>
                    <asp:Label ID="lblAgentID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Name
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Phone</td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Mobile</td>
                <td>
                    <asp:TextBox ID="txtMobile" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Email Id</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>City</td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Address</td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" style="width:150px; height:50px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="#FF3300"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save Client" OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="Delete Client" />
                    <asp:Label ID="lblAvlID" runat="server"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
