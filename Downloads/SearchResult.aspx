<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchResult.aspx.cs" Inherits="Downloads_Availibility" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            $("#txtData").val("");
        }
    }

    function htmlEncode(value) { return $('<div/>').text(value).html(); }
    function htmlDecode(value) { return $('<div/>').html(value).text(); }
    
    function Process(data)
    {
        var str = "";

        $("div.propSearchMainContent", $(data)).each(function ()
        {
            $("div.propSearchDtlHead a", $(this)).each(function ()
            {
                str += $.trim($(this).attr('href'));
            });

            str += "^"

            $("div.searchDetailPanelRgt", $(this)).each(function ()
            {
                var t = $.trim($(this).text());
                var n = t.indexOf('|');
                if (n > 0)
                {
                    t = $.trim(t.substring(4, n));
                }
                str += t;
            });

            str += "~"
        });
        return str;
    }
</script>
<body onload="CheckAction()">
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtData" runat="server" Height="519px" TextMode="MultiLine" Width="468px"></asp:TextBox>
        <asp:TextBox ID="txtResult" runat="server" Height="519px" TextMode="MultiLine" Width="468px"></asp:TextBox>
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:Label ID="lblID" runat="server"></asp:Label>
    </div>
    <div id="divData">
    </div>
    </form>
</body>
</html>
