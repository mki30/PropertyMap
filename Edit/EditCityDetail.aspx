<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCityDetail.aspx.cs" Inherits="Edit_EditCityDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <link href="../css/PropertyMap.css" rel="stylesheet" />
    <script type="text/javascript">

        function UpdateLatLng(Lat, Lng)
        {
            $("#txtLat").val(Lat);
            $("#txtLng").val(Lng);
        }

        function UpdatePolyPoints(Data)
        {
            $("#txtPolyPoints").val(Data);
        }

        function GetPolyPoints()
        {
            return $("#txtPolyPoints").val();
        }

        
    </script>
</head>
<body  style="padding-left:5px; padding-right:5px;" >
    <form id="form1" runat="server">
        <div style="padding-top:5px;">
            <table style="border-spacing: 0px;">
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Parent ID</td>
                    <td>
                        <asp:TextBox ID="txtParentID" runat="server" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblParentName" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="txtsyle" onchange="UpdateName(this)" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Sort Name</td>
                    <td>
                        <asp:TextBox ID="txtSortName" runat="server" CssClass="txtsyle" onchange="UpdateName(this)" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Lat
                    </td>
                    <td>
                        <asp:TextBox ID="txtLat" runat="server" CssClass="txtsyle" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Lng
                    </td>
                    <td>
                        <asp:TextBox ID="txtLng" runat="server" CssClass="txtsyle" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtPolyPoints" runat="server" Height="142px" TextMode="MultiLine" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkVerified" runat="server" Text="Verified" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id="btnMap" type="button" value="Map" onclick="ShowMap()" />
                        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
                    </td>
                </tr>
            </table>


            <table>
                <tr>
                    <td>From
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>To</td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtSubCity" runat="server" Height="105px" TextMode="MultiLine" ToolTip="Enter the names of the cities to be creates as subcity of the above city" style="width:98%;"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnCreateSubCity" runat="server" Text="Create SubCity" OnClick="btnCreateSubCity_Click" />
                    </td>
                </tr>

                <tr style="border-bottom:1px dotted gray;">
                 <td>&nbsp;</td>
                 <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnUpdateChildCount" runat="server" OnClick="btnUpdateChildCount_Click" Text="Update Child Count" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtDigits" runat="server" Width="20px">2</asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnUpdateSortName" runat="server" OnClick="btnUpdateSortName_Click" Text="Update Sort Name" />
                    </td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
