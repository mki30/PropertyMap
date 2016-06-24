<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AvailabilityList.aspx.cs" Inherits="Edit_AvailabilityList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/Simple.css" />
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="edit-head">Availability List</div>
    <form id="form1" runat="server">
    <div style="width:100%">
        <asp:ListBox ID="lstAvailability" runat="server" Height="800px" Width="100%"></asp:ListBox>
    </div>
    </form>
</body>
</html>
