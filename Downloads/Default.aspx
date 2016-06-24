<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="DataDownload_Kohler_Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script src="../js/jquery-1.8.0.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function CheckAction()
    {
        if ($("#txtData").val() != "")
        {
            var decoded = htmlDecode($("#txtData").val());
            $("#txtResult").val(htmlEncode(Process(decoded)));
            //$("#txtData").val("");
        }
    }

    function htmlEncode(value) { return $('<div/>').text(value).html(); }
    function htmlDecode(value) { return $('<div/>').html(value).text(); }

    function Process(data)
    {
        var str = "";

        $("table.propDetailWrapper", $(data)).each(function ()
        {
            $("div.propertyBrifInfo h1", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            $("div.priceDetail", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            $("span.pricePerSqFt", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            $("div.propertyBrifInfoSecond", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            $("div.propertyBrifInfoThird", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            $("div.propertyBaseDetail", $(this)).each(function ()
            {
                $("table", $(this)).each(function ()
                {

                    $("tr", $(this)).each(function ()
                    {


                        str += $(this).text() + "^";

                    });
                });

            });


            $("div.agentComName", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });
            $("div.agentName", $(this)).each(function ()
            {
                str += $(this).text() + "^";
            });

            str += "~";

        });

        return str;
    }


    function GetCleanTable(obj, replacetxt, replacewith)
    {
        var str = "";
        $("tr", $(obj)).each(function (index)
        {
            $("th", $(this)).each(function (index)
            {
                str += $(this).text() + "^";


            });

            $("td", $(this)).each(function (index)
            {
                str += $(this).text() + "^";
            });

            str += "~";
        });

        return str;
    }
</script>
<body onload="CheckAction()">
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
        <asp:TextBox ID="txtPageNumber" runat="server">1</asp:TextBox>
        &nbsp;ProductList Name<asp:TextBox ID="txtProductListName" runat="server"></asp:TextBox>
        &nbsp;URL<asp:TextBox ID="txtURL" runat="server" Width="692px"></asp:TextBox>
        <br />
        <br />
        <asp:TextBox ID="txtData" runat="server" Height="519px" TextMode="MultiLine" Width="468px"></asp:TextBox>
        <asp:TextBox ID="txtResult" runat="server" Height="519px" TextMode="MultiLine" Width="468px"></asp:TextBox>
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />
        <%-- <asp:Button ID="btnInsert" runat="server"  Text="Insert" onclick="btnInsert_Click" />--%>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div id="divData">
    </div>
    </form>
</body>
</html>
