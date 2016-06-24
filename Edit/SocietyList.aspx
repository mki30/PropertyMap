<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SocietyList.aspx.cs" Inherits="Edit_SocietyList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Common.js"></script>
    <script src="../js/Cookie.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
   
     <script type="text/javascript">
        $(function ()
        {
            var SocietyID = LoadFromLocalStorage("SocietyID");
            $('#lstSociety').val(SocietyID);
        });

        $(document).ready(function ()
        {
            parent.LoadData($('#lstSociety').val());
            $('#lstSociety').bind("change", function ()
            {
                SaveInLocalStorage("SocietyID", $(this).val());
                parent.LoadData($(this).val());
            });
        });
</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="border">
            <table style="border-spacing: 0px;width:100%">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCity" runat="server" Width="98%" AutoPostBack="True" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddSubCity" runat="server" Width="98%" AutoPostBack="True" OnSelectedIndexChanged="ddSubCity_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkVerified" runat="server" AutoPostBack="True" OnCheckedChanged="chkVerified_CheckedChanged" Text="Verified" Checked="True"/>
                        <asp:CheckBox ID="chkDeleted" runat="server" AutoPostBack="True" OnCheckedChanged="chkDeleted_CheckedChanged" Text="Deleted" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtFilter" runat="server" Width="98%" ToolTip="Type To Search in List"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstSociety" runat="server" Height="600px" Width="98%" style="outline:none;"></asp:ListBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
