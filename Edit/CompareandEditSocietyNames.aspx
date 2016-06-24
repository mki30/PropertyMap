<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompareandEditSocietyNames.aspx.cs" Inherits="Edit_CompareandEditSocietyNames" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="1">
             <tr >
                <td colspan="2">
                    <asp:DropDownList ID="ddlLocality" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Our Societies:
                    </td>
                
                <td>
                   Others:
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="ListBox1" runat="server" Height="485px" Width="252px"></asp:ListBox>
                    </td>
                <td>
                    <asp:ListBox ID="ListBox2" runat="server" Height="480px" Width="276px"></asp:ListBox>
                </td>
            </tr>
     </table>
    </div>
    <asp:Button ID="btnUpdate" runat="server" Text="Auto Update" OnClick="btnUpdate_Click" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    <asp:Button ID="btnUpdateSociety" runat="server" OnClick="btnUpdateSociety_Click" Text="Update Society" />
    </form>
</body>
</html>
