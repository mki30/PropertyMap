<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentAvailability.aspx.cs" Inherits="AgentAvailability" enableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/js/jquery-1.9.1.min.js"></script>
    <script src="/js/jquery-ui-1.10.2.custom.min.js"></script>
    <link href="/css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" />
    <link href="/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="/bootstrap/js/bootstrap.min.js"></script>
    <script src="/js/bootstrap-datepicker.js"></script>
    <link href="/css/datepicker.css" rel="stylesheet" />
    <script src="js/jquery.MultiFile.js"></script>
    <%--link to main css removed--%>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#txtAvlFrom").datepicker({
                startView: 2,
                orientation: "auto",
                format: 'dd-MM-yyyy'
            });

            fillTypeDropdown(function ()
            {
                $("#ddApartmentType").val($("#hdApartmentTypeID").val());
            });

            $("#ddApartmentType").change(function ()
            {
                $("#hdApartmentTypeID").val($(this).val());
            });
       });

        function fillTypeDropdown(CallBack)
        {
            $.ajax({
                url: "Data.aspx?Action=GetApartmentTypeJSON&Data1=" + $("#hdSocietyID").val(), cache: false, success: function (data)
                {
                    data = data.replace('~', '');

                    var json = $.parseJSON(data);
                    console.log(json);
                    var s = "<option value='0'>---Select---</option>";
                    $(json).each(function ()
                    {
                        s += "<option value='" + this.ID + "'>" + this.TypeName +" ("+this.SuperArea+ ")</option>";
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

        $(function ()
        {
            $("#txtSociety").autocomplete({
                minLength: 2, select: function (event, ui)
                {
                    $("#hdSocietyID").val(ui.item.id);
                    fillTypeDropdown();
                }, source: SendRequest
            });
        });

        var cache = {}, lastXhr;
        function SendRequest(request, response)
        {
            var term = request.term;
            if (term in cache)
            {
                response(cache[term]); return;
            }
            lastXhr = $.getJSON("../Data.aspx?Action=SearchSociety&Data1=" + 0, request, function (data, status, xhr)
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
    <style type="text/css">
        /*jQuery Autocomplete CSS*/
.ui-corner-all
{
    -moz-border-radius: 0;
    -webkit-border-radius: 0;
    border-radius: 0;
}

.ui-menu
{
    border: 2px solid #c4e9fe;
    font-family: Verdana, Arial, Helvetica, sans-serif;
    font-size: 12px;
    background: #edeeee;
}

    .ui-menu .ui-menu-item a
    {
        color: #888;
    }

    .ui-menu .ui-menu-item:hover
    {
        display: block;
        text-decoration: none;
        color: #3D3D3D;
        cursor: pointer;
        background-color: lightgray;
        background-image: none;
        border: 1px solid lightgray;
    }

.ui-widget-content .ui-state-hover,
.ui-widget-content .ui-state-focus
{
    border: 1px solid lightgray;
    background-image: none;
    background-color: lightgray;
    font-weight: bold;
    color: #3D3D3D;
}
/**/
</style>
</head>
<body>
    <form style="margin-bottom:0px;padding:10px; background-color:#BDC2CE; height:550px;" runat="server">
        <table style="margin-bottom:0px;">
            <tr style="font-weight:bold;">
                <td>|<asp:Literal Text="0" ID="ltID" runat="server" />|
                    <asp:Literal Text="0" ID="ltAgentID" runat="server"/>
                </td>
                <td>
                    <span style="text-align: right;">
                        <asp:HiddenField ID="hdSocietyID" runat="server" />
                        <asp:HiddenField ID="hdApartmentTypeID" runat="server" />
                    </span>
                </td>
            </tr>
            <tr>
                <td>Society
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSociety" />
                </td>
            </tr>
            <tr>
                <td>Type</td>
                <td>
                    <asp:DropDownList ID="ddApartmentType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server">
                <td>For</td>
                <td>
                   <asp:DropDownList ID="ddFor" runat="server" OnSelectedIndexChanged="ddFor_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="1">Sale</asp:ListItem>
                        <asp:ListItem Value="0">Rent</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" visible="false" id="trrentfor">
                <td>Rent For</td>
                <td>
                   <asp:DropDownList ID="ddrentFor" runat="server">
                        <asp:ListItem Value="0">No Restriction</asp:ListItem>
                        <asp:ListItem Value="1">Family Only</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Floor No</td>
                <td>
                   <asp:DropDownList ID="ddFloorNo" runat="server">
                       <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        <asp:ListItem Value="0">Ground</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="6">6</asp:ListItem>
                        <asp:ListItem Value="7">7</asp:ListItem>
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="9">9</asp:ListItem>
                        <asp:ListItem Value="10">10</asp:ListItem>
                        <asp:ListItem Value="11">11</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td>Amount</td>
                <td>
                    <asp:TextBox runat="server" ID="txtAmount" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>AvlFrom</td>
                <td>
                    <asp:TextBox runat="server" ID="txtAvlFrom" />
                </td>
            </tr>
            <tr>
                <td>Description</td>
                <td>
                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="80px" Width="205px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:FileUpload ID="FileUpload1" class="multi max-6" runat="server" ToolTip="Socity Images" />
                    <div id="imageDiv" runat="server"></div>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input id="chkDeleted" type="checkbox" runat="server"/>&nbsp;Delete
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" class="btn" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                    <%--<asp:Button ID="btnDelete" class="btn" runat="server" Text="Delete" OnClick="btnDelete_Click"/>--%>
                    <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Green"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
