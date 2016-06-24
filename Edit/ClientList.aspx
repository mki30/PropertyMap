<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientList.aspx.cs" Inherits="Edit_ClientList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/Simple.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/QueryString.js"></script>

    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('#lstClient').bind("change", function ()
            {
                parent.LoadData($(this).val());
            });
            //$("#txtFilter").keyup(function () { ListBoxFilter(this, "#lstSociety"); });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ListBox ID="lstClient" Width="120" Height="450" runat="server"></asp:ListBox>
    </div>
    </form>
</body>
</html>
