<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditBlog.aspx.cs" Inherits="Edit_EditBlog" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
     <%--<link rel="stylesheet" href="/css/redmond/jquery-ui-1.7.1.custom.css" />--%>
    <style>
        table, input[type='text'] 
        {
            width: 90%;
        }
        .fr 
        {
            float: right;
        }
        .auto 
        {
            width: auto !important;
        }
        table tr td:nth-child(2n+1) 
        {
            width: 70px;
        }
        .mtp10 {
            margin-top: 10px;
        }
        #ui-datepicker-div {
            display: none;
        }
    </style>
      <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
      <link href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet"/>
</head>
<body>
   <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>ID</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                        <%--<span class="fr">
                            <asp:CheckBox ID="chkOffline" runat="server" Text="Offline"/>
                            <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" />
                        </span>--%>
                    </td>
                </tr>
                <tr>
                    <td>Title</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>UrlName</td>
                    <td>
                        <asp:TextBox ID="txtUrlName" runat="server" /></td>
                </tr>
                <tr>
                    <td></td><td><asp:DropDownList ID="ddCity" runat="server" AutoPostBack="true"></asp:DropDownList></td></tr>
                    <tr><td></td></tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtShortDesc" runat="server" TextMode="MultiLine"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Description: (Full Screen: Alt+Ctrl+F)&nbsp;&nbsp;&nbsp;<span><span><asp:FileUpload ID="imgUpload" runat="server" /></span><span><asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" Style="height:22px;" /></span></span><asp:Label ID="lblImgs" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr style="padding-top:10px;">
                    <td>Post Date</td>
                    <td>
                        <asp:TextBox ID="txtdate" CssClass="auto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Reference</td>
                    <td>
                        <asp:TextBox ID="txtRefUrl" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblStatus" ForeColor="Red" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="mtp10">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Style="height: 26px" />
            <%--<asp:Button ID="btnNew" runat="server" Text="New Blog" />--%>
            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
        </div>
    </form>
</body>

<script src="../js/jquery-1.9.1.min.js"></script>
<script src="../js/jquery-ui-1.10.2.custom.min.js"></script>
<script src="../tinymce/tinymce.min.js"></script>
<script src="../tinymce/createedit.js"></script>
<script src="../bootstrap/js/bootstrap.min.js"></script>

<script>
    createEditor("#txtDescription");
    createEditor("#txtShortDesc");
    $(document).ready(function ()
    {
        $('#txtdate').datepicker({
            "dateFormat": "dd-M-yy",
            changeMonth: true,
            changeYear: true
        });
    });
</script>
</html>
