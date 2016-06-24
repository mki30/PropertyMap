<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCity.aspx.cs" Inherits="Edit_EditCity" %>

<%@ Register Src="~/UserControl/Map.ascx" TagPrefix="uc1" TagName="Map" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script  type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    
    <script type="text/javascript">

        $(document).ready(function ()
        {
            //$("#DivDialog").dialog({ width: 800, height: 600, autoOpen: false, show: "fade", hide: "fade", title: "Enlarge" });
            $("#frameCity")[0].src = "EditCityDetail.aspx?ID=" + $("#ddCity").val();
        });

        function ShowImageDialog(imgPath)
        {
            $("#DivDialog").dialog('open');
            $("#frameMap").hide();
            $("#aptimgtag").remove();
            $("#DivDialog").append("<img id='aptimgtag' src='" + imgPath + "' height='540' width='770'/>");
        }

        function ShowCity()
        {
            $("#txtAddress").val($("#lstSubCity1").find(":selected").text() + ", " + $("#ddCity").find(":selected").text() + ", India");

            $("#frameCity")[0].src = "EditCityDetail.aspx?ID=" + $("#lstSubCity1").val();

            $("#lstSubCity1")[0]


            var IndexFound = -1;
            var txt=$("#lstSubCity1").find(":selected").text();
            $(Labels).each(function (Index) {

                if (this == txt)
                    IndexFound = Index;
            });

            if(IndexFound!=-1)
                ShowPoly(IndexFound, true);
        }

        function SelectPoly(Index)
        {
            $("#lstSubCity1")[0].selectedIndex=Index;
            ShowCity();
        }
    </script>
</head>
<body style="background-color:#eff3f6;">
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table>
                <tr>
                    <td style="width:250px; vertical-align: top; border: 1px solid gainsboro;">
                        <asp:DropDownList ID="ddCity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCity_SelectedIndexChanged" width="100%"></asp:DropDownList>
                        <asp:ListBox ID="lstSubCity1" runat="server" onclick="ShowCity()" Height="800px" width="100%" ></asp:ListBox>
                        <asp:CheckBox ID="chkNotVerified" runat="server" AutoPostBack="True" OnCheckedChanged="chkNotVerified_CheckedChanged" Text="Show Not Verified" />
                        <asp:CheckBox ID="chkHasPolygon" runat="server" AutoPostBack="True" OnCheckedChanged="chkHasPolygon_CheckedChanged" Text="Has Polygon" />
                    </td>
                    <td style="vertical-align: top; width: 300px; border: 1px solid gainsboro;">
                      <iframe id="frameCity" style="width:100%;height:700px;border:0px;" ></iframe>
                    </td>
                    <td style="border: 1px solid gainsboro; vertical-align:top;">
                        <uc1:Map runat="server" ID="Map"/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
     <div id="DivDialog">
        <iframe id="frameMap" style="width: 99%; height: 99%; border: 0px;"></iframe>
    </div>
</body>
</html>
