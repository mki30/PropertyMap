<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectPolygon.aspx.cs" Inherits="Edit_ProjectPolygon" %>

<%@ Register Src="~/UserControl/Map.ascx" TagPrefix="uc1" TagName="Map" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script>
        var Points = [];
        $(document).ready(function ()
        {
            var str = "<table>";
            //console.log(data);
            Points.length=0;
            $(data.places).each(function (Index)
            {
                //console.log("Asd");
                var Pts = new Array();
                $(this.polygon).each(function ()
                {
                    Pts.push(this.y);
                    Pts.push(this.x);
                });

                str += "<tr><td><a href='#' onclick='ShowPoly(" + Index + ")'>" + this.title + "</a><td><a href='#' onclick='Import(" + Index + ")'>Import</a>";
                Points.push(Pts);
            });
            str += "</table>";

            setTimeout("CreatePoly()", 2000);

            $("#divData").html(str);
        });

        function Import(index)
        {
            console.log(Points[index], $("#ddProject").val());

            $.post("../Data.aspx?Action=UpdatePolyPointsWiki", { PolyPoints: JSON.stringify(Points[index]), ProjId: $("#ddProject").val() }, function (data)
            {
               if (data == "")
                {
                    $("#DivMessage").text("Saved!");
                    $("#DivMessage").css("color", "green");
                    MoveNext();
                }
                else
                {
                    $("#DivMessage").text("Nor Saved! Somme Error Occurred!");
                    $("#DivMessage").css("color", "red");
                }
            });
        }

        function MoveNext()
        {
            var i = $("#ddProject")[0].selectedIndex;
            $("#ddProject")[0].selectedIndex = ++i;
            $("#btnSearch").click();
        }
        
</script>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="vertical-align:top;">
                        <input type="button" value="12" onclick="map.setZoom(12); $('#chkZoomLock').prop('checked', true);" />
                        <input type="button" value="13" onclick="map.setZoom(13); $('#chkZoomLock').prop('checked', true);" />
                        <input type="button" value="14" onclick="map.setZoom(14); $('#chkZoomLock').prop('checked', true);" />
                        <input type="button" value="16" onclick="map.setZoom(16); $('#chkZoomLock').prop('checked', true);"/>
                        <input type="button" value="17" onclick="map.setZoom(17); $('#chkZoomLock').prop('checked', true);"/>
                        <br />
                        <asp:DropDownList ID="ddProject" runat="server"></asp:DropDownList>
                        <br />
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        <input type="button" value="Next" onclick="MoveNext()" />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        <div id="divData"></div>
                        <br/>
                        <div id="DivMessage"></div>
                    </td>
                    <td style="vertical-align: top">&nbsp;</td>
                    <td style="vertical-align: top;">
                        <uc1:Map runat="server" ID="Map"/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
