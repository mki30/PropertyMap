<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditApartmentType.aspx.cs" Inherits="Edit_EditApartmentType" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.2.custom.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function ()
        {
            $("#Image1").click(function ()
            {
                parent.ShowImageDialog($("#Image1").prop('src'));
            });

            //$("#imgFrame")[0].src = "ImageUploader.aspx?AptID=" + $('#lblID').text()+"&ProjectID=" + $('#lblSocietyID').text();
            $("#ddPropertyType").change(function (e)
            {
                $("#imgFrame")[0].src = "ImageUploader.aspx?AptID=" + $('#lblID').text() + "&ProjectID=" + $('#lblSocietyID').text() + "&PropertyType=" + $("#ddPropertyType").val();
            });
            RefreshFloorInfoGrid();
        });

        function ShowFloor(AptID, FloorNo)
        {
            $("#imgFrame")[0].src = "ImageUploader.aspx?AptID=" + AptID + "&FloorNo=" + FloorNo;
        }

        function DeleteFloors(AptID, FloorNo)
        {
            $.ajax({
                url: "../Data.aspx?Action=DeleteFloors&Data1=" + AptID + "&Data2=" + FloorNo, cache: false, success: function (data)
                {
                    if (data == "")
                        RefreshFloorInfoGrid();
                    else
                    {
                        alert("Image Not delted Some error Occured!");
                    }
                }
            });
        }

        function UpdateTextName()
        {
            //if ($("#txtAptType").val().length == 0)
            //{
                var bed = $("#ddlBedroom").val() != "" ? $("#ddlBedroom").val() + "BR" : "";
                var bath = $("#ddlBathroom").val() != "" ? "-" + $("#ddlBathroom").val() + "T" : "";
                $("#txtAptType").val(bed + bath);
            //}
        }

        function RefreshImages()
        {
            $.ajax({
                url: "../data.aspx?action=GetImages&Data1=" + $("#lblID").html(), cache: false, success: function (data)
                {
                    $("#divImages").html(data);
                }
            });
        }

        function DeleteImage(Path,ImageID)   //Delete Images using ajax call
        {
            //alert(Path);
            $.ajax({
                url: "../Data.aspx?Action=DelteImages&Data1=" + Path.replace("../", "")+"&Data2="+ImageID, cache: false, success: function (data)
                {
                    if (data.replace('~','') == "")
                        RefreshImages();
                    else
                    {
                        alert("Image Not delted Some error Occured!");
                    }
                }
            });
        }

        function RefreshFloorInfoGrid()
        {
            //alert($("#lblID").text());
            $.ajax({
                url: "../Data.aspx?Action=GetFloorInfo&Data1=" + $("#lblID").text(), cache: false, success: function (data)
                {
                    $("#divFloors").html(data);
                }
            });
        }

</script>
    <style type="text/css">
        .txtwidth
        {
            width: 80px;
        }

        .txtTaxwidth
        {
            width: 40px;
        }

        img
        {
            border: 1px solid gainsboro;
        }

            img:hover
            {
                -moz-box-shadow: 0 0 10px #ef7474;
                -webkit-box-shadow: 0 0 10px #ef7474;
                box-shadow: 0 0 10px #ef7474;
            }

        #floorInfo tr:nth-child(odd)
        {
            background: gainsboro;
        }

        #floorInfo tr:hover
        {
            cursor: pointer;
            background-color: pink;
        }

        #floorInfo tr th, td
        {
            text-align: left;
        }
    </style>
</head>
<body style="padding-left: 5px;">
    <form id="form1" runat="server">
        <div id="maindiv" style="padding-top: 5px;">
            <table>
                <tr>
                    <td>
                        <table style="border-spacing: 0px;">
                            <tr>
                                <td rowspan="9" style="vertical-align: top;">
                                    <div>
                                        <asp:ListBox ID="lstApartmentType" runat="server" OnSelectedIndexChanged="lstApartmentType_SelectedIndexChanged" AutoPostBack="true" Height="163px" Width="152px"></asp:ListBox>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtImportText" runat="server" Height="54px" TextMode="MultiLine" Width="145px" ToolTip="Copy from excel - Type, Bedroom, Bathroom, SuperArea"></asp:TextBox>
                                    </div>
                                    <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" Text="Import" />
                                </td>
                            </tr>
                            <tr>
                                <td>Society:<asp:Label ID="lblSocietyID" runat="server"></asp:Label>
                                    ID:
                    <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddUseType" runat="server">
                                        <asp:ListItem Value="0">Residential</asp:ListItem>
                                        <asp:ListItem Value="1">Commercial</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddPropertyType" runat="server">
                                        <asp:ListItem Value="0">Apartment</asp:ListItem>
                                        <asp:ListItem Value="1">Duplex</asp:ListItem>
                                        <asp:ListItem Value="2">Plot</asp:ListItem>
                                        <asp:ListItem Value="3">Villa</asp:ListItem>
                                        <asp:ListItem Value="4">Independent Floor</asp:ListItem>
                                        <asp:ListItem Value="5">Penthouse</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="Delete Apartment Type" OnClientClick="return confirm('Confirm Delete?')" OnClick="btnDelete_Click" />
                                    <asp:Button ID="btnAdd" runat="server" Text="Clear" OnClick="btnAdd_Click" ToolTip="Add New Apartment Type" />
                                <td>
                                <td rowspan="4" style="vertical-align: top;min-width:80px;">
                                    <asp:Label ID="lblCheckbox" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtAptType" runat="server" Width="100px" AccessKey="N"></asp:TextBox>
                                    <asp:DropDownList ID="ddlBedroom" runat="server" AccessKey="B" onchange="UpdateTextName()">
                                        <asp:ListItem Value="0">Bed </asp:ListItem>
                                        <asp:ListItem Value="1">Bed 1</asp:ListItem>
                                        <asp:ListItem Value="2">Bed 2</asp:ListItem>
                                        <asp:ListItem Value="3">Bed 3</asp:ListItem>
                                        <asp:ListItem Value="4">Bed 4</asp:ListItem>
                                        <asp:ListItem Value="5">Bed 5</asp:ListItem>
                                        <asp:ListItem Value="6">Bed 6</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlBathroom" AccessKey="T" runat="server" onchange="UpdateTextName()">
                                        <asp:ListItem Value="0">Bath </asp:ListItem>
                                        <asp:ListItem Value="1">Bath 1</asp:ListItem>
                                        <asp:ListItem Value="2">Bath 2</asp:ListItem>
                                        <asp:ListItem Value="3">Bath 3</asp:ListItem>
                                        <asp:ListItem Value="4">Bath 4</asp:ListItem>
                                        <asp:ListItem Value="5">Bath 5</asp:ListItem>
                                        <asp:ListItem Value="6">Bath 6</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlBalcony" AccessKey="U" runat="server" ToolTip="Balcony">
                                        <asp:ListItem Value="0">Bal </asp:ListItem>
                                        <asp:ListItem Value="1">Bal 1</asp:ListItem>
                                        <asp:ListItem Value="2">Bal 2</asp:ListItem>
                                        <asp:ListItem Value="3">Bal 3</asp:ListItem>
                                        <asp:ListItem Value="4">Bal 4</asp:ListItem>
                                        <asp:ListItem Value="5">Bal 5</asp:ListItem>
                                        <asp:ListItem Value="6">Bal 6</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Super Area</td>
                                <td>
                                    <asp:TextBox ID="txtSuperArea" runat="server" AccessKey="A" Width="50px"></asp:TextBox>
                                    &nbsp;Plot Area&nbsp;
                                    <asp:TextBox ID="txtPlotArea" runat="server" Width="50px"></asp:TextBox>
                                    <asp:DropDownList ID="ddUnit" runat="server">
                                        <asp:ListItem Value="0">sqft</asp:ListItem>
                                        <asp:ListItem Value="1">sqyd</asp:ListItem>
                                        <asp:ListItem Value="2">sqmt</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Description</td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Height="52px" Width="98%"></asp:TextBox>
                                     <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ToolTip="Save Apartment Type" Width="100px" Height="30px" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Literal ID="ltImageUploader" runat="server"></asp:Literal>
                                    <div id="divFloors"></div>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                Floor :

                                <td>BUA :
                                    <asp:TextBox ID="txtBuiltUprea" runat="server" Width="60px"></asp:TextBox>
                                    Terr :
                                    <asp:TextBox ID="txtTerrace" runat="server" Width="60px"></asp:TextBox>
                                    Lawn :
                                    <asp:TextBox ID="txtLawn" runat="server" Width="60px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox1" runat="server" Height="167px" TextMode="MultiLine" Visible="False" Width="142px"></asp:TextBox>
                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                            </tr>

                            <%--<tr>
                                <td></td>
                                <td colspan="2" style="text-align: right;">
                                   
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="vertical-align: top; width: 200px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnBulkBSP" runat="server" Text="Update All" ToolTip="Update BSP of all Types" Width="100px" OnClick="btnBulkBSP_Click" />
                            </tr>
                            <tr>
                                <td>BSP:
                         <asp:Label ID="lblPriceListID" runat="server" Text="0"></asp:Label>

                                </td>
                                <td>

                                    <asp:TextBox ID="txtBSP" runat="server" class="txtwidth"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>BSP Installments:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtBSPInstall" runat="server" class="txtwidth"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Maintinance Dep:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMaintDep" runat="server" class="txtwidth"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Parking Deposit:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtParkingDep" runat="server" class="txtwidth"></asp:TextBox>
                                </td>

                            </tr>

                            <tr>
                                <td>Power Backup Dep:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPB" runat="server" class="txtwidth"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td>Club Deposit:</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtClub" runat="server" class="txtwidth"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td>PLC:
                                </td>

                                <td colspan="2">
                                    <asp:TextBox ID="txtPLC" runat="server" class="txtwidth"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <div style="width: 100%; overflow: auto">
                            <div id="divImages">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
