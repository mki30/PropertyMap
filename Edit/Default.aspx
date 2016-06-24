<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Property Map</title>
    <link id="Favocon" runat="server" rel="icon" href="/Images/shortcut.png"/>
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <link href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css"/>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css"/>
   <script src="../js/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <%--<script src="../js/jquery-ui-1.10.2.custom.min.js"></script>--%>
    <script src="../js/MakeTabs.js" type="text/javascript"></script>
    <script type="text/javascript">
        var BasePath = '<%= ResolveClientUrl(".") %>/';

        TabText = new Array("Society", "Availability", "Agent", "Owner", "Builder", "PriceTrend", "Services", "City", "OSM", "Projects", "Apt List", "Project Polyarea", "Agent Report", "Q&A", "PDF", "Blog", "New Blog", "Manage Images");
        TabLinks = new Array("SocietyMain.aspx", "AvailabilityMain.aspx", "AgentMain.aspx?sellerType=0", "AgentMain.aspx?sellerType=1", "BuilderReport.aspx", "PriceTrend.aspx", "EditServices.aspx", "EditCity.aspx", "ReadXml.aspx", "ProjectReport.aspx", "ApartmentTypeList.aspx", "ProjectPolygon.aspx", "AgentReport.aspx", "QueAnsReport.aspx", "PdfDownload.aspx", "EditBlog_Old.aspx", "Blog.aspx", "ManageImageMain.aspx");

        //tabs persistance
        $(function ()                           
        {
            $('#tabsProfile').tabs({
                selected: (location.hash != "") ? location.hash.replace("#", "") : 0,
                show: function (event, ui)
                {
                    location.hash = $(this).tabs("option", "selected");
                }
            });
        });
</script>
</head>
<body onload="Resized()">
    <form id="form1" runat="server">
        <div style="position:absolute; top:5px; left:1300px; z-index:10000;">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
            <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" />
        </div>
    <div id="tabsProfile">
    </div>
</form>
</body>
</html>


