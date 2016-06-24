<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuilderProjectList.aspx.cs" Inherits="Edit_BuilderProjectList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.min.js"></script>
    <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet"/>
    <script src="../js/jquery-ui-1.10.2.custom.min.js"></script>
    <script src="../js/TableSearch.js"></script>
    <%--<script src="../js/PropertyList.js"></script>--%>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        .table-condensed th, .table-condensed td
        {
            padding:0px 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div style="float:left;"><asp:TextBox ID="txtName" placeholder="search..." runat="server" style="border-radius:0px;"></asp:TextBox></div>
        </div>
        <asp:Literal ID="ltProjectList" runat="server"></asp:Literal>
     </div>
    </form>
</body>
</html>
