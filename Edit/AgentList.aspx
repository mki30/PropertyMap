<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentList.aspx.cs" Inherits="Edit_AgentList" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/Common.js"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('#lstAgent').val(LoadFromLocalStorage("AgentID"));
            parent.LoadData($('#lstAgent').val(), 1);

            $("#txtFilter").keyup(function () { ListBoxFilter(this, "#lstAgent"); });

            $('#lstAgent').bind("change", function ()
            {
                SaveInLocalStorage("AgentID", $(this).val());
                parent.LoadData($(this).val(), 1);
            });
        });
    </script>
</head>
<body>
   <form id="form1" runat="server">
    <div>
    <div>
        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="ddCity" runat="server" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="ddCity_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtFilter" runat="server" style="width:175px;" ToolTip="Type To Search in List" autocomplete="off"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="lstAgent" runat="server"  style="outline:none; width:180px; height:600px;"></asp:ListBox>
                </td>
            </tr>
        </table>
    </div>
    </div>
   </form>
</body>
</html>
