<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditServices.aspx.cs" Inherits="Edit_EditServices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table style="border-spacing:0px;  table-layout: fixed; width:250px;" >
            <tr>
                <td>ID:</td>
                <td >
                    <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Type</td>
                <td>
                    <asp:DropDownList ID="ddlServiceType" runat="server">
                         <asp:ListItem Value="0">Pakers and Movers</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Name</td>
                <td>
                     <asp:TextBox ID="txtName" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr >
                <td>Contact&nbsp; No</td>
                <td >
                    <asp:TextBox ID="txtContact" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Address</td>
                <td>
                    <asp:TextBox ID="txtAdress" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>City</td>
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server">
                        <asp:ListItem Value="0">Ghaziabd</asp:ListItem>
                        <asp:ListItem Value="1">Noida</asp:ListItem>
                        <asp:ListItem Value="2">New Delhi</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Lat</td>
                <td>
                    <asp:TextBox ID="txtLat" runat="server" ></asp:TextBox>
                </td>
            </tr>
          <tr>
                <td>Lng</td>
                <td>
                    <asp:TextBox ID="txtLng" runat="server" ></asp:TextBox>
                </td>
            </tr>
        <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
