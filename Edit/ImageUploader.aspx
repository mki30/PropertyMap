<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageUploader.aspx.cs" Inherits="Edit_ImageUploader" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <link href="../css/PropertyMap.css" rel="stylesheet" />
    <script>
        $(function ()
        {
            $("#FileUpload1").change(function ()
            {
                $(this).closest("form").submit();
            });
        });
    </script>

</head>
<body onload="parent.RefreshImages()">
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    PI:<asp:Label ID="lblProjectID" runat="server"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Floor&nbsp;
                    <asp:Label ID="lblApartmentID" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddFloor" runat="server">
                        <asp:ListItem Value="">Apartment</asp:ListItem>
                    </asp:DropDownList>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <table style="width: 100%;">
                    <tr>
                        <td>Total Area</td>
                        <td><asp:TextBox ID="txtTotalArea" runat="server" Width="82px"></asp:TextBox></td>
                        <td>Built Up</td>
                        <td> <asp:TextBox ID="txtBuiltUpArea" runat="server" Width="82px"></asp:TextBox></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Lawn</td>
                        <td><asp:TextBox ID="txtLawn" runat="server" Width="82px"></asp:TextBox></td>
                        <td>Terrace</td>
                        <td> <asp:TextBox ID="txtTerrace" runat="server" Width="82px"></asp:TextBox></td>
                        <td> <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
                    </tr>
                </table>
               </td>
            </tr>
        </table>
    </form>
</body>
</html>
