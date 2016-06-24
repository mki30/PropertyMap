<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageImageSelect.aspx.cs" Inherits="Edit_ManageImageSelect" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#lstProject").change(function ()
                {
                    parent.showData($("#lstProject").val(), $("#ddImageType").val());
                });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddImageType" runat="server" width="98%">
            <asp:ListItem Value="1">Project Images</asp:ListItem>
            <asp:ListItem Value="2">Project Layout</asp:ListItem>
            <asp:ListItem Value="3">Project Logo</asp:ListItem>
            <asp:ListItem Value="4">Apartment Type</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div>
          <asp:DropDownList ID="ddlCity" runat="server" Width="98%" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
         </div>

        <div>
            <asp:DropDownList ID="ddSubCity" runat="server" Width="98%" AutoPostBack="True">
            </asp:DropDownList>
        </div>
         <div>
        <asp:ListBox ID="lstProject" runat="server" Height="580px" Width="98%" style="outline:none;"></asp:ListBox>
    </div>
       <%-- <div>
            <asp:Button ID="btnUpdateImageUrlNames" runat="server" Text="Update Image UrlName" OnClick="btnUpdateImageUrlNames_Click" /></div>
        <div style="margin-top:5px;">
            <asp:Button ID="btnRenameFiles" runat="server" Text="Rename Files" OnClick="btnRenameFiles_Click"/>
        </div>--%>
    </form>
</body>
</html>
