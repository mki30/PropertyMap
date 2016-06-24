<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditSociety.aspx.cs" Inherits="Edit_EditSociety" ValidateRequest="false" ClientIDMode="Static" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <link rel="stylesheet" type="text/css" href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <%--<script type="text/javascript" src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>--%>
    <script type="text/javascript" src="../js/jquery-ui-1.10.2.custom.min.js"></script>
    <script type="text/javascript" src="../js/jquery.MultiFile.js"></script>
    <link href="../css/AutoComplete.css" rel="stylesheet" />
    <%--<script src="../js/Common.js"></script>--%>

    <style type="text/css">
        .ui-widget
        {
            font-size: 11px;
        }
    </style>

    <script type="text/javascript">
        $(function ()
        {
            $("#txtStartDate").datepicker({ dateFormat: "dd-M-yy", changeMonth: true, changeYear: true });
            $("#txtCompDate").datepicker({ dateFormat: "dd-M-yy", changeMonth: true, changeYear: true });

            $("#txtBuilder").autocomplete({
                minLength: 2, select: function (event, ui)
                {
                    $("#hidBuilderID").val(ui.item.id);
                }, source: SendRequest
            });
            ShowProjectPricingTable();
        });

        var cache = {}, lastXhr;
        function SendRequest(request, response)
        {
            var term = request.term;
            if (term in cache)
            {
                response(cache[term]); return;
            }
            lastXhr = $.getJSON("../Data.aspx?Action=SearchBuilder", request, function (data, status, xhr)
            {
                cache[term] = data;
                if (xhr === lastXhr)
                {
                    response(data);
                }
            });
        }

        $(function ()               // uncheck varified on delete ckeck
        {
            $("#chkDeleted").click(function ()
            {
                if ($(this).is(':checked'))
                    $("#chkVerified").prop('checked', false);
            })
        });

        function UpdateLatLng(Lat, Lng)
        {
            $("#txtLat").val(Lat);
            $("#txtLng").val(Lng);
        }

        function UpdatePolyPoints(data)
        {
            $("#txtPoly").val(data);
        }

        function btnFindAddress_onclick()
        {
            //alert($("#lblID").text());
            var QueryString = "Zoom=17&ID=" + $("#lblID").text() + "&Lat=" + $("#txtLat").val() + "&Lng=" + $("#txtLng").val() + "&Address=" + $("#txtSocietyName").val() + "," + $("#ddlSubCity option:selected").text()
            + ", " + $("#ddlCity option:selected").text()
            + ", " + $("#ddlState option:selected").text()
            + ", " + $("#ddlCountry option:selected").text();
            parent.ShowMap(QueryString);
        }

        function DeleteImage(Path, ImageID)   //Delete Images using ajax call
        {
            //alert(Path);
            $.ajax({
                url: "../Data.aspx?Action=DelteImages&Data1=" + Path.replace("../", "") + "&Data2=" + ImageID, cache: false, success: function (data)
                {
                    //alert(data);
                    if (data.replace('~', '') == "")
                        ClearClientImage();
                    else
                    {
                        alert("Image Not delted! Error:" + data);
                    }
                }
            });

            function ClearClientImage()
            {
                $("#imageDiv img").each(function ()
                {
                    if ($(this).attr("src") === Path)
                    {
                        $(this).parent().remove();
                    }
                });

                if ($("#LogoImgDiv img").attr("src") === Path)
                {
                    $("#LogoImgDiv img").parent().remove();
                }

                if ($("#LayoutImgDiv img").attr("src") === Path)
                {
                    $("#LayoutImgDiv").remove();
                }
            }
            return false;
        }
        function DoValidation()
        {
            if ($("#ddlState").val() == "-1")
                return false;
            else
                return true;
        }

        function SavePrice()
        {
            var formdata = $("#priceForm input,#priceForm select").serialize();
            $.post("/Data.aspx?Action=SaveProjectPriceing&Data1=" + $("#lblProjectPriceID").text() + "&Data2=" + $("#lblID").text(), formdata, function (data)
            {
                if (data == "")
                    ShowProjectPricingTable();
                else
                    alert(data);
            });
        }

        function ShowProjectPricingTable()
        {
            $.ajax({
                url: "/Data.aspx?Action=GetProjectPricingTable&Data1=" + $("#lblID").text() + "&Data2=", cache: false, success: function (data)
                {
                    $("#lblPriceTable").html(data.replace('~', ''))
                }
            });
        }

        function GetPricingRecord(pricingId)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetProjectPricingRecord&Data1=" + pricingId + "&Data2=", cache: false, success: function (data)
                {
                    var obj = $.parseJSON(data);
                    $("#lblProjectPriceID").text(obj.ID);
                    $("#txtPriceName").val(obj.Name);
                    $("#txtPriceValue").val(obj.Value);
                    $("#ddProjectPriceType").val(obj.Type);
                }
            });
        }

        function ResetPriceForm()
        {
            $("#lblProjectPriceID").text("0");
            $("#txtPriceName").val("");
            $("#txtPriceValue").val("");
            $("#ddProjectPriceType").find('option:first').attr('selected', 'selected');
        }

        $.fn.serializeObject = function ()
        {
            var o = {};
            $("input,select,checkbox,password,radio,textarea,input:hidden", this).each(function ()
            {
                var id = $(this).attr("id");
                var val = $.trim($(this).val())
                if (this.type == 'checkbox' || this.type == 'radio')
                {
                    val = $(this).prop('checked') ? "1" : "0";
                }
                o[id] = val;
            });
            return o;
        };
    </script>

</head>
<body>

    <form id="form1" runat="server">
        <div style="padding-left: 5px; width: 350px;">
            <table>
                <tr>
                    <td>ID</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Clear" ToolTip="Add New Society" />
                        <input id="btnFindAddress" type="button" value="Map" onclick="btnFindAddress_onclick()" title="View And Edit Lat Lns in map" /><asp:Literal ID="ltSourceUrl" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<asp:Literal ID="ltProjectUrl" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSocietyName" runat="server" MaxLength="100" Width="70%" OnTextChanged="txtSocietyName_TextChanged"></asp:TextBox>
                        &nbsp;<asp:TextBox ID="txtTown" runat="server" Width="25%" placeholder="town."></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>URL Name</td>
                    <td>
                        <asp:TextBox ID="txtURLName" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Lat/Lng&nbsp;<asp:Label ID="lblWiki" runat="server" Text="wiki"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLat" runat="server" MaxLength="32" Width="30%"></asp:TextBox>
                        <asp:TextBox ID="txtLng" runat="server" Width="32%"></asp:TextBox>
                        <asp:TextBox ID="txtPoly" runat="server" Width="31%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>State</td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" Width="99%">
                            <asp:ListItem Value="-1">---------State---------</asp:ListItem>
                            <asp:ListItem>Uttar Pradesh</asp:ListItem>
                            <asp:ListItem>New Delhi</asp:ListItem>
                            <asp:ListItem>Haryana</asp:ListItem>
                            <asp:ListItem>Punjab</asp:ListItem>
                            <asp:ListItem>Tamil Nadu</asp:ListItem>
                            <asp:ListItem>Maharastra</asp:ListItem>
                            <asp:ListItem>Karnatka</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>City</td>
                    <td>
                        <asp:DropDownList ID="ddlCity" runat="server" Width="49%" AutoPostBack="True" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlSubCity" runat="server" Width="49%" AutoPostBack="True" OnSelectedIndexChanged="ddlSubCity_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Area</td>
                    <td>
                        <asp:DropDownList ID="ddArea" runat="server" Width="50%">
                        </asp:DropDownList>
                        Pin
                        <asp:TextBox ID="txtPin" runat="server" MaxLength="6" Width="30%"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Literal ID="ltBuilder" runat="server"></asp:Literal></td>
                    <td>
                        <asp:HiddenField ID="hidBuilderID" runat="server" />
                        <asp:TextBox ID="txtBuilder" runat="server" Width="70%"></asp:TextBox>
                        <asp:Label ID="lblBuilderID" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>

                    <td colspan="2">
                        <asp:Panel ID="checkboxes" runat="server">
                            <asp:Label ID="lblCheckbox" runat="server"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td title="Project start and completion date">Start/Compl</td>
                    <td>
                        <asp:TextBox ID="txtStartDate" runat="server" Width="32%" placeholder="start date..."></asp:TextBox>
                        <asp:TextBox ID="txtCompDate" runat="server" Width="31%" placeholder="pussession date..."></asp:TextBox>
                        <asp:DropDownList ID="ddStatus" runat="server" Width="32%" Height="22px">
                            <asp:ListItem Value="1">Announced</asp:ListItem>
                            <asp:ListItem Value="2">Upcoming</asp:ListItem>
                            <asp:ListItem Value="3" Selected="True">Ongoing</asp:ListItem>
                            <asp:ListItem Value="4">Completed</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td title="Builder Web page for the project">
                        <asp:Literal ID="ltBuilderURL" runat="server"></asp:Literal></td>
                    <td>
                        <asp:TextBox ID="txtURLBuilder" runat="server" Width="98%" placeholder="project url from builders website..."></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td title="Brochure URL of the project">
                        <asp:Literal ID="ltBrochureURL" runat="server"></asp:Literal></td>
                    <td>
                        <asp:TextBox ID="txtBrochureURL" runat="server" Width="98%" placeholder="Brochure url from builders website..."></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Price List</td>
                    <td>
                        <asp:TextBox ID="txtPriceList" runat="server" Width="98%" placeholder="Plicelist url from builders website..."></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Video URL</td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Description<br />
                        <asp:Literal ID="ltGoogleEarthLink" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDetail" runat="server" Height="51px" TextMode="MultiLine" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkVerified" runat="server" Text="Verified" />&nbsp;&nbsp;
                         <asp:CheckBox ID="chkDeleted" runat="server" Text="Delete" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:FileUpload ID="LogoUpload" runat="server" ToolTip="Socity Logo" />
                        <div id="LogoImgDiv" runat="server" style="float: right">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:FileUpload ID="ImagesUpload" class="multi max-6" runat="server" ToolTip="Socity Images" />
                        <div id="imageDiv" runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:FileUpload ID="LayoutUpload" runat="server" ToolTip="Layout Image" />
                        <div id="LayoutImgDiv" runat="server" style="float: right">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="margin-left: 40px">
                        <asp:Button ID="btnSave" OnClientClick="return DoValidation();" runat="server" OnClick="btnSave_Click" Text="Save" ToolTip="Save Form Data" Width="150px" Height="40px" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="margin-left: 40px">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblStatus" runat="server" Style="color: #f00;"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" Height="147px" TextMode="MultiLine" Visible="False" Width="300px"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" Visible="false" Text="Button" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table id="priceForm">
                            <tr>
                                <td colspan="2">
                                    <h3>Pricing</h3>
                                </td>
                                <td>ID:<asp:Label ID="lblProjectPriceID" runat="server" Text="0"></asp:Label></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPriceName" runat="server" style="width:100px;"></asp:TextBox>
                                </td>
                                <td>Value
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPriceValue" runat="server" style="width:100px;"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddProjectPriceType" runat="server"></asp:DropDownList>
                                    </td>
                                <td colspan="2">
                                    <input id="btnSavePrice" type="button" value="Save" onclick="SavePrice()" />
                                    <input id="btnAddNewPriceType" type="button" value="Reset" onclick="ResetPriceForm()" /></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblPriceTable" runat="server" Text=""><img src="../Images/progress2.gif" /></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>


