<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApartmentList.aspx.cs" Inherits="Edit_ApartmentList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Common.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#txtFilter").keyup(function () { ListBoxFilter(this, "#lstAprtmentList"); });
        });
    </script>

</head>
<body>

    <form id="form1" runat="server">
        <asp:TextBox ID="txtFilter" runat="server" Width="95%" ToolTip="Type to Search in List"></asp:TextBox>
        <br />
      <asp:ListBox ID="lstAprtmentList" runat="server" Height="700px" Width="98%"></asp:ListBox>
</form>
</body>
</html>
