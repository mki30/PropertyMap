<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReadXml.aspx.cs" Inherits="ReadXml" %>

<%@ Register Src="~/UserControl/Map.ascx" TagPrefix="uc1" TagName="Map" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/PropertyMap.css" rel="stylesheet" />
    <title></title>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/QueryString.js"></script>
    <script type="text/javascript" src="../js/Cookie.js"></script>

    <script type="text/javascript">

        var ObjectList = null;
        var AllObjects = [];
        var counter = 0;
        var page = 0;

        $(document).ready(function ()
        {
            var Cityval = $.cookie("CityID");  // Get Cookie Val
            $("#ddCity").val(Cityval);
            GetCityList();
        });

        function AddObject(obj)
        {
            for (var i = 0; i < AllObjects.length; i++)
            {

                if (AllObjects[i].id == obj.id)
                    return false;
            }

            obj.name = obj.name.replace("Sector-", "Sector ").replace("Sector -", "Sector ").replace("  ", " ");

            AllObjects.push(obj);
            return true;
        }

        function GetBoundsData(lngMin, latMin, lngMax, latMax, FileIndex)
        {
            $("#divRequestCount").html(FileIndex);
            var StartsWith = $("#txtStartsWith").val().toLowerCase();
            var CityName = $("#ddCity").find(":selected").text();
            var Tile = $("#selectDivision").find(":selected").text();
            $.ajax({
                async: false,
                url: "../Data.aspx?Action=WikimapiaBoxImport&Data1=" + CityName + "-" + Tile + "-" + FileIndex + "&Data2=" + lngMin
                + "&Data3=" + latMin
                + "&Data4=" + lngMax
                + "&Data5=" + latMax
                + "&Data6=" + page
                , cache: false, success: function (Data)
                {
                    ObjectList = JSON.parse(Data);

                    $(ObjectList.folder).each(function (Index)
                    {
                        if (StartsWith != "" && this.name.toLowerCase().indexOf(StartsWith) != 0)
                            return;
                        AddObject(this);
                    });
                }
            });
        }
        function Erida()
        {
        }

        function ShowObjectList()
        {
            var str = "<table class='DataTable'>";
            Points.length = 0;
            Labels.length = 0;
            $(AllObjects).each(function (Index)
            {
                str += "<tr onmouseover='ShowPoly(" + Index + ")'><td><a href='#' onclick='return ShowObject(" + Index + ")'>" + this.name + "</a>";
                str += "<td><a href='#' onclick='GetChild(" + this.id + ")'>Get Info</a>&nbsp;&nbsp;"
                str += "<td><a href='#' onclick='AutoImport(" + Index + ")'>Import</a>"

                Labels[Index] = this.name;
                Points[Index] = [];
                $(this.polygon).each(function ()
                {
                    Points[Index].push(this.y);
                    Points[Index].push(this.x);
                });
            });

            str += "</table>";

            $("#WikiData").html(str);
            CreateLabel();
            CreatePoly();
        }

        function GetWikiMapiaBox(p)
        {
            if (!p)
                p = 1;
            var Bounds = map.getBounds();
            var sw = Bounds.getSouthWest();
            var ne = Bounds.getNorthEast();

            var Cells = parseInt($("#selectDivision").val());
            var dlng = (ne.lng() - sw.lng()) / Cells;
            var dlat = (ne.lat() - sw.lat()) / Cells;

            var lng = sw.lng(), lat = sw.lat();
            var ctr = 0;
            for (i = 0; i < Cells; i++)
            {
                lng = sw.lng()
                for (j = 0; j < Cells; j++)
                {
                    var lngmin = lng;
                    var lngmax = lng + dlng;

                    var latmin = lat;
                    var lmax = lng + dlng;

                    GetBoundsData(lng, lat, lng + dlng, lat + dlat, p);

                    lng += dlng;
                }

                lat += dlat;
            }
            ShowObjectList();
        }

        function GetChild(ParentID)
        {
            $.ajax({
                url: "../Data.aspx?Action=WikimapiaBoxObjectInfo&Data1=" + ParentID, cache: false, success: function (Data)
                {
                    var ObjectInfo = JSON.parse(Data);
                    console.log(ObjectInfo)
                    $("#WikiDescription").html(ObjectInfo.description);
                }
            });
        }

        function ShowObject(Index)
        {
            var Obj = ObjectList.folder[Index];
            console.log(Obj);

            return false;
        }

        function GetCityList()
        {
            var ParentID = $("#ddCity").val();
            var chkNonVerified = $("#chkNonVerified").prop("checked") ? 1 : 0;
            $.ajax({
                url: "../Data.aspx?Action=CityList&Data1=" + ParentID + "&Data2=" + chkNonVerified, cache: false, success: function (Data)
                {
                    var list = JSON.parse(Data);
                    var str = "<table class='DataTable'>";
                    $(list).each(function ()
                    {
                         str += "<tr><td>" + this.Name + "<td><a href='#' onclick='FindSimilar(\"" + this.Name + "\")'>Find</a><td><a href='#' onclick='ImportPoly(" + this.ID + ")'>Map</a>";
                    });
                    str += "</table>";

                    $("#divCityList").html(str);
                }
            });
        }

        function FindSimilar(Name)
        {
            for (var i = 0; i < AllObjects.length; i++)
                if (AllObjects[i].name.indexOf(Name) == 0)
                {
                    ShowPoly(i);
                    break;
                }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <asp:DropDownList ID="ddCityOSM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCityOSM_SelectedIndexChanged" Style="width: 100%;">
                        </asp:DropDownList>
                        <div style="width: 100%; height: 800px; overflow: auto">
                            <asp:Literal ID="ltObjectLlist" runat="server"></asp:Literal>
                            <div id="WikiData"></div>
                            <div id="WikiDescription"></div>
                            <asp:Button ID="btnProjectPolyImport" runat="server" Text="Import Proj Poly"/>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 400px;">
                        <asp:Button ID="btnAutoMap" runat="server" Text="Auto Map" OnClick="btnAutoMap_Click" />
                        <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
                        <asp:CheckBox ID="chkNonVerified" runat="server" Text="Show Not Verified" onclick="GetCityList()" />
                        <asp:DropDownList ID="ddCity" runat="server" onchange="GetCityList()" Style="width: 100%;"></asp:DropDownList>
                        <div id="divCityList" style="height: 750px; overflow: auto; width: 100%;"></div>
                        <br />
                        <input id="btnImportWikimapia" type="button" value="Wikimapia" onclick="GetWikiMapiaBox()" />
                        <span>Starts With<input type="text" id="txtStartsWith" value="Sector" style="width: 100px;" /></span>
                        <span>Map Divisions
                        <select id="selectDivision">
                            <option>1</option>
                            <option>5</option>
                            <option>8</option>
                            <option selected="selected">16</option>
                        </select></span>
                        <input id="Button1" type="button" value="Get Next Page" onclick="GetWikiMapiaBox(page++)" />
                        <div id="divRequestCount">
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <uc1:Map runat="server" ID="Map" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
        </div>
    </form>
</body>
</html>
