<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAvailability.aspx.cs" Inherits="Edit_EditAvailability" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <link href="../css/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-ui-1.9.2.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/AutoComplete.css" />
    <script type="text/javascript" src="../js/QueryString.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <script type="text/javascript" src="../js/less-1.3.0.min.js"></script>

    <script type="text/javascript">
        var q = new QueryString();
        q.read();
        var a = q.getQueryString('CSS');

        if (a == 1)
            AddCSS();
        function AddCSS()
        {
            $('#less').remove();
            $('head').append('<link rel="stylesheet" href="../css/dynstyle.css" type="text/css" />');
        }

        $(function ()
        {
            $("#txtDateAvailableFrom").datepicker({ dateFormat: "dd-M-yy", changeMonth: true, changeYear: true });

            $("#txtSociety").autocomplete({
                minLength: 2, select: function (event, ui)
                {
                    $("#lblSocietyID").html(ui.item.id);
                    $("#hdSocietyID").val(ui.item.id);
                    fillTypeDropdown();
                }, source: SendRequest
            });
        });

        var Data1 = 0;
        $(document).ready(function ()
        {
            $("#lblSellerID").text(q.getQueryString('AgentID'));
            //$('#txtSociety').focus(function ()
            //{
            //    Data1 = 0;
            //});
            //$('#txtSellerName').focus(function ()
            //{
            //    Data1 = 1;
            //})
            fillTypeDropdown(function ()
            {
                $("#ddApartmentType").val($("#hdApartmentTypeID").val());
            });

            $("#ddApartmentType").change(function ()
            {
                $("#hdApartmentTypeID").val($(this).val());
            });
        })

        function fillTypeDropdown(CallBack)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetApartmentTypeJSON&Data1=" + $("#hdSocietyID").val(), cache: false, success: function (data)
                {
                    data = data.replace('~', '');
                    var json = $.parseJSON(data);
                    console.log(json);
                    var s = "<option value='0'>---Select---</option>";
                    $(json).each(function ()
                    {
                        s += "<option value='" + this.ID + "'>" + this.TypeName + " (" + this.SuperArea + ")</option>";
                    });
                    $('#ddApartmentType').html(s);
                },
                complete: function ()
                {
                    if (CallBack)
                        CallBack();
                },
                error: function (xhr, ajaxOptions, thrownError)
                {
                    alert(xhr.status);
                }
            });
        }

        var cache = {}, lastXhr;
        function SendRequest(request, response)
        {
            var term = request.term;
            if (term in cache)
            {
                response(cache[term]); return;
            }
            lastXhr = $.getJSON("/Data.aspx?Action=SearchSociety&Data1=" + Data1, request, function (data, status, xhr)
            {
                cache[term] = data;
                if (xhr === lastXhr)
                {
                    response(data);
                }
            });
        }

        function DeleteImage(Path)   //Delete Images using ajax call
        {
            alert(Path);
            $.ajax({
                url: "/Data.aspx?Action=DelteImages&Data1=" + Path.replace("../", ""), cache: false, success: function (data)
                {
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

    </script>
</head>
<body class="bodyAvailability">
     <form id="form1" runat="server">
        <div style="padding-left: 5px; float: left;">
            <table style="border-collapse: collapse;">
                <tr>
                    <td>ID&nbsp;&nbsp;<asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                    </td>
                    <td>Agent ID&nbsp;&nbsp;<asp:Label ID="lblSellerID" runat="server"></asp:Label>
                        <asp:HiddenField ID="hidSellerID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Society-<asp:Label ID="lblSocietyID" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:HiddenField ID="hdSocietyID" runat="server" />
                        <asp:TextBox ID="txtSociety" runat="server" Width="189px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Type<asp:HiddenField ID="hdApartmentTypeID" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddApartmentType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>For</td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonListAvlFor" runat="server" RepeatDirection="Horizontal" Style="border-spacing: 0px;" OnSelectedIndexChanged="RadioButtonListAvlFor_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0" Selected="true">Rent</asp:ListItem>
                            <asp:ListItem Value="1">Sale</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trSellerType" runat="server">
                    <td>
                        <asp:Label ID="lblSellerType" runat="server" Text="Seller Type"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radioButonListSellerType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" Style="border-spacing: 0px;">
                            <asp:ListItem Value="0" Selected="True">Agent</asp:ListItem>
                            <asp:ListItem Value="1">Owner</asp:ListItem>
                            <asp:ListItem Value="2">Builder</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>Amount</td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:Label ID="lblPricePerSqf" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Available From</td>
                    <td>
                        <asp:TextBox ID="txtDateAvailableFrom" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Floor No</td>
                    <td>
                        <asp:DropDownList ID="ddlFlorNo" runat="server" CssClass="PostPropDrp">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">Ground</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Facing Direction</td>
                    <td>
                        <asp:DropDownList ID="ddlFacingDir" runat="server" CssClass="PostPropDrp">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>East</asp:ListItem>
                            <asp:ListItem>West</asp:ListItem>
                            <asp:ListItem>North</asp:ListItem>
                            <asp:ListItem>South</asp:ListItem>
                            <asp:ListItem>South-East</asp:ListItem>
                            <asp:ListItem>North-East</asp:ListItem>
                            <asp:ListItem>Norht-west</asp:ListItem>
                            <asp:ListItem>South-West</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trRentFor" runat="server" visible="false">
                    <td>Rent For</td>
                    <td>
                        <asp:DropDownList ID="ddrentFor" runat="server">
                            <asp:ListItem Value="0">No Restriction</asp:ListItem>
                            <asp:ListItem Value="1">Family Only</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Height="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:FileUpload ID="FileUpload1" class="multi max-6" runat="server" ToolTip="Socity Images" />
                    <div id="imageDiv" runat="server"></div>
                </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="margin-left: 40px">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ToolTip="Save Availibility" />
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" ToolTip="Add New Availability" />
                        <asp:Button ID="btnDelete" runat="server" OnClientClick="return confirm('Delete?')" Text="Delete" OnClick="btnDelete_Click" ToolTip="Delete Availability " />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="margin-left: 40px">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblStatus" runat="server" ForeColor="#FF5050"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
