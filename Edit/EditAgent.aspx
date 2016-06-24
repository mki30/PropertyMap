<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAgent.aspx.cs" Inherits="Edit_EditAgent" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/QueryString.js"></script>
    <script type="text/javascript">

        $(document).ready(function ()
        {
            var q = new QueryString();
            q.read();
            var a = q.getQueryString('CSS');

            if (a == 1)
                AddCSS()
            function AddCSS()
            {
                $('head').append('<link rel="stylesheet" href="../css/dynstyle.css" type="text/css" />');
            }
            var usertype = q.getQueryString('UserType');  //hide some unnecessary rows for owner
            if (usertype == 1)
                $('.trhide').hide();

            if (q.getQueryString('ShowList') == 1)
            {
                $("#frameProjList").hide();
            }
            else
                $("#frameProjList")[0].src = "BuilderProjectList.aspx?builderID=" + q.getQueryString('ID');
        });

        function UpdateName(obj)
        {
            $("#txtAgentName").val($(obj).val());
        }
    </script>
    <style type="text/css">
        textarea, input:not([type=checkbox])
        {
            width: 95%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <div id="editAgent" style="padding-left: 5px; float: left;">
            <table>
                <tr>
                    <td style="width: 80px">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Clear" OnClick="btnAdd_Click" ToolTip="Add New Agent" Width="47%" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="Delete Agent" OnClick="btnDelete_Click" Width="47%" />
                    </td>
                </tr>
                <tr class="trhide">
                    <td>Company</td>
                    <td>
                        <asp:TextBox ID="txtAgentCopany" runat="server" CssClass="txtsyle" onchange="UpdateName(this)"></asp:TextBox>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>URL Name</td>
                    <td>
                        <asp:TextBox ID="txtURLName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>
                        <asp:Literal ID="ltWeb" runat="server" Text="Web"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>Phone</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Mobile</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Email Id</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtPass" runat="server" CssClass="txtsyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>City
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCity" runat="server" CssClass="dropdn">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>Operating In
                    </td>
                    <td class="auto-style1">
                        <asp:DropDownList ID="ddlOperatingIn" runat="server" CssClass="dropdn">
                            <asp:ListItem>--Select--</asp:ListItem>
                            <asp:ListItem>Delhi/NCR</asp:ListItem>
                            <asp:ListItem>Delhi</asp:ListItem>
                            <asp:ListItem>Gaziabad</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>Operating Since</td>
                    <td class="auto-style1">
                        <asp:DropDownList ID="ddlOperatingSince" runat="server" CssClass="dropdn">
                            <asp:ListItem>2012</asp:ListItem>
                            <asp:ListItem>2011</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="multiTxtSyle" Height="80px"></asp:TextBox>
                    </td>
                </tr>

                <tr class="trhide">
                    <td>Deals In</td>
                    <td class="auto-style1">
                        <asp:DropDownList ID="ddlDealsIn" runat="server" CssClass="dropdn">
                            <asp:ListItem>-Select-</asp:ListItem>
                            <asp:ListItem>Multistory Apartment</asp:ListItem>
                            <asp:ListItem>Plot</asp:ListItem>
                            <asp:ListItem>Shop</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr class="trhide">
                    <td>Builder Detail</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtBuilderDetail" runat="server" TextMode="MultiLine" Height="80px"></asp:TextBox>
                    </td>
                </tr>

                <tr class="trhide">
                    <td><span>Builder Logo</span></td>
                    <td class="auto-style1">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </td>
                </tr>
                <tr class="trhide">
                    <td></td>
                    <td>
                        <span>
                            <asp:CheckBox ID="chkVarified" runat="server" />Varified</span>
                        <span>
                            <asp:CheckBox ID="chkDeleted" runat="server" />Deleted </span>
                    </td>
                </tr>
                <tr class="trhide">
                    <td>
                        <asp:Literal ID="ltrSource" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Image ID="Image1" runat="server" Style="height: 40px; width: 90px; padding-left: 2px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ToolTip="Save Agent" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style1">&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style1">
                        <asp:Label ID="lblBuilderURL" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe id="frameProjList" style="height: 800px; width: 50%; border: 1px solid gainsboro;"></iframe>
        </div>
    </form>
</body>
</html>
