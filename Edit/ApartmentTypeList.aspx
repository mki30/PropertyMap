<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApartmentTypeList.aspx.cs" Inherits="Edit_ApartmentTypeList" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <link href="../css/PropertyMap.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $(".trhead").hover(function ()
            {
                $(this).css("cursor", "pointer");
            })
        })
        function ShowApartments(ID)
        {
           $("#framePage")[0].src = "EditApartmentType.aspx?SocietyID=" + ID;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td style="vertical-align:top; width:200px;">
                <asp:CheckBox ID="chkShowDeleted" runat="server" Text="Show Deleted" />
                <asp:Button ID="btnReload" runat="server" Text="ReLoad" OnClick="btnReload_Click" />
                <asp:ListBox ID="lstProjects" runat="server" AutoPostBack="True" Height="785px" OnSelectedIndexChanged="lstProjects_SelectedIndexChanged" Width="197px"></asp:ListBox>
            </td>
            <td style="width:800px;">
                <div style="overflow:auto;height:810px;">
                <asp:Literal ID="ltList" runat="server"></asp:Literal>
                </div>
            </td>
            <td style="height:800px;">
                <iframe id="framePage" style="width:400px; height: 100%; border:1px solid gainsboro;"></iframe>
            </td>
        </tr>
    </table>
</div>
   </form>
</body>
</html>
