<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentClientEdit.aspx.cs" Inherits="Edit_AgentClientEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/js/jquery-1.9.1.min.js"></script>
    <script src="/bootstrap/js/bootstrap.min.js"></script>
    <link href="/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
</head>
<body> 
    <form id="form1" runat="server" style="margin-bottom:0px;padding:10px; background-color:#BDC2CE;">   <%--set margin to 0 to avoid scroll in fancybox--%>
    <div >
     <table style="margin-bottom:0px;">
            <tr style="font-weight:bold;">
                <td>ID : <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                </td>
                <td>
                    Agent Id : <asp:Label ID="lblAgentID" runat="server"></asp:Label>
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
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" style="height:50px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="#FF3300"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="Save" ToolTip="Save Client" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Delete" ToolTip="Delete Client" />
                    <asp:Label ID="lblAvlID" runat="server"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
