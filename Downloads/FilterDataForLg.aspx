<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FilterDataForLg.aspx.cs" Inherits="GeneratingHTML" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 735px;
        }
    </style>
</head>
<body onload="CheckAction()">
    <form id="form1" runat="server">
    <div>
            Filter & Insert Information For Kohler
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtData" runat="server" Height="400px" TextMode="MultiLine" Width="84px"></asp:TextBox>
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtPrcessedData" runat="server" Height="628px" Width="1336px" TextMode="MultiLine"></asp:TextBox>
                </td>
                            </tr>
        </table>
    </div>
    <asp:Button ID="btnProcess" runat="server" Text="Start Process" OnClick="btnProcess_Click" />
    Index:
    <asp:TextBox ID="txtLastIndex" runat="server"></asp:TextBox>
    DataName:
    <asp:TextBox ID="txtProductURL" runat="server" Width="617px"></asp:TextBox>
    <br />
    </form>
    <script src="../../js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function CheckAction()
        {
            var decoded = htmlDecode($("#txtData").val());
            $("#txtPrcessedData").val(Process(decoded));

       if ($("#txtData").val() != "")
            $("#btnProcess").click();
        }

        function OnSubmit()
        {
            var decoded = htmlDecode($("#txtData").val());
            $("#txtPrcessedData").val(Process(decoded));
        }

        function htmlEncode(value)
        {
            return $('<div/>').text(value).html();
        }

        function htmlDecode(value)
        {
            return $('<div/>').html(value).text();
        }

        function Process(data)
        {
            var str = "";
            $("div.overview", $(data)).each(function ()
            {
              $("h2.modelNum", $(this)).each(function ()
                {
                    str += $(this).html();
                });
            });
            str += "~";
            $("div.features", $(data)).each(function ()
            {
                $("img", $(this)).each(function ()
                {
                    str += $(this).attr("tppabs") + "\n"; ;
                });
                str += "~";
                $("ul.icon-list li", $(this)).each(function ()
                {
                    str += $(this).html() + ",";
                });
                str += "&&";
                $("dl", $(this)).each(function ()
                {
                    str += $(this).text() + ",";
                });

            });
            str += "~";

            $("div.specifications *", $(data)).each(function ()
            {
                if (this.tagName == "H4")
                    str += $(this).text() + ":\n";

                if (this.tagName == "DT")
                    str += $(this).text().replace('~', '-') + ":";

                if (this.tagName == "DD")
                    str += $(this).text().replace('~','-') + "\n";

                //                $("h4", $(this)).each(function ()
                //                {
                //                    str += $(this).html() + ",";

                //                });
                //                str += "~";
                //                $("dl", $(this)).each(function ()
                //                {
                //                    str += $(this).text() + ",";
                //                });

            });
            str += "~";

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
                    var obj = $("a.a_bookticket", this);
                    var ServiceID = "";
                    if (obj && $(obj).attr("href"))
                    {
                        ServiceID = $(obj).attr("href") + "^";
                    }

                    str += $(this).text() + "^" + ServiceID;
                });

                str += "~";
            });

            return str;
        }
   
    </script>
</body>
</html>
