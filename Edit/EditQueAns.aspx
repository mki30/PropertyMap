<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditQueAns.aspx.cs" Inherits="Edit_EditQueAns" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/bootstrap/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
     <form id="form1" runat="server" style="margin-bottom:0px;padding:10px;height:200px;">
        <div>
            <table>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
               <tr>
                    <td>Ans</td>
                    <td>
                        <asp:TextBox ID="txtAns" runat="server" Height="123px" Width="500px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:200px;">
                        <asp:Label ID="lblStatus" runat="server" Text="Label" ForeColor="#FF3300" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:Button CssClass="btn" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button CssClass="btn" ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"  />
                    </td>
                </tr>
            </table>
        </div>
         <p></p>
    </form>
</body>
</html>
