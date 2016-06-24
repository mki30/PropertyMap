<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectList.aspx.cs" Inherits="Edit_ProjectList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <link href="../css/PropertyMap.css" rel="stylesheet" />
    <script src="../js/Common.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
           $("#ProjectReportTable").HighLightRows();
           $("#ProjectReportTable td").css("padding-right","5")

        })
        function ShowSociety(ID)
        {
            $("#framePage")[0].src = "EditSociety.aspx?ID=" + ID;
           
        }
        function ShowBuilder(ID)
        {
            $("#framePage")[0].src = "EditAgent.aspx?ID=" + ID;
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
            <td style="width:1000px;vertical-align:top;">
                <div style="overflow:auto;height:875px;width:96%">
                <asp:Literal ID="ltList" runat="server"></asp:Literal>
                </div>
            </td>
            <td style="height:875px;">
                <iframe id="framePage" style="width:400px; height: 100%; border:1px solid gainsboro;"></iframe>
            </td>
        </tr>
    </table>
</div>
   </form>
</body>
</html>
