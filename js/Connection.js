var geocoder;
var map;
var marker;
var Overlays = new Array();
var CityList = new Array();
var ConnectionList = new Array();
var PointInMapBounds = new Array();
var LastSelectedCity = 0;

var SelectedControlID;

$(document).ready(function ()
{
    initialize();

    $("#lstCityFrom option").each(function (index, item)  //Taking Option value of Listbox
    {
        CityList.push(item);
    });

    $("#txtName").keyup(function ()  //Taking value of TextBox
    {
        ListBoxFilter();
    });

    $(":input").focus(function ()
    {
        SelectedControlID = $(this).attr("id");
    });

});
    
function MoveNext()
{
    var i = $("#lstCityFrom")[0].selectedIndex;
    $("#lstCityFrom")[0].selectedIndex = ++i;
    onCitySelection();
}

function ListBoxFilter()
{
    var Search = $("#txtName").val().toLowerCase();

    for (i = 0; i < CityList.length; i++)
    {
        var text = $(CityList[i]).text().toLowerCase();

        if ($("#radStartWith").prop("checked")) // search for text starting with search
        {
            if (text.substr(0, Search.length) == Search)
            {
                $("#lstCityFrom").get(0).selectedIndex = i;
                break;
            }
        }
        else // search for text matching at any place
        {
            if (text.indexOf(Search) > -1)
            {
                $("#lstCityFrom").get(0).selectedIndex = i;
                break;
            }
        }
    }
}

function ShowAddress()
{
    var txt = $("#lstCityTo option:selected").text();
    GetlatLngFromDatabase();
}

function initialize()
{
    geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(15.133333, 78.716667);
    var myOptions = { zoom: 10, center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP };
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    marker = new google.maps.Marker({ map: map, position: latlng, icon: '../Images/icon_blue_dot.png' });

    google.maps.event.addListener(map, 'idle', function ()
    {
        if ($("#chkConectionsMapBound").prop("checked"))
        {
            GetPointsInBounds();
        }
    });
}

function ClearOverlays()
{
    for (var i = 0; i < Overlays.length; i++)
        Overlays[i].setMap(null);
    Overlays.length = 0;
}

function onCitySelection()
{
    var FromID = $("#lstCityFrom").val();
    GetPointsInBounds();

    $("#frmCityEdit")[0].src = "eRoadCityEdit.aspx?ID=" + FromID;
}

function GetPointsInBounds()  //1
{
    ClearOverlays();
    var bounds = map.getBounds();
   
    $.ajax({ url: "../Data.aspx?Action=GET_POINTS_IN_BOUNDS&Data=" + $("#lstCityFrom").val()
            + "&Data1=" + bounds.getNorthEast().lat()
            + "&Data2=" + bounds.getNorthEast().lng()
            + "&Data3=" + bounds.getSouthWest().lat()
            + "&Data4=" + bounds.getSouthWest().lng(), cache: false, success: GetPointsInBoundsDone
    });

    GetConnectionsInMapBounds();
}

var NB_ID = 0, NB_CityName = 1, NB_StateName = 2, NB_Lat = 3, NB_Lng = 4, NB_Distance = 5;

function GetPointsInBoundsDone(Data)  //2 Showing NearBy Cities in Table
{
    PointInMapBounds.length = 0; // clear the array

    var Lines = Data.split('~');

    var str = "<table cellpadding='0' cellspacing='0'><tr><td>Name<td>km";
    for (var i = 0; i < Lines.length; i++)
    {
        if (Lines[i] == "")
            continue;

        var F = Lines[i].split('^');

        if (i == 1) //details of the from city
        {
            var pt = new google.maps.LatLng(parseFloat(F[NB_Lat]), parseFloat(F[NB_Lng]));

            marker.setPosition(pt);
            marker.setTitle(F[NB_CityName] + "," + F[NB_StateName]);

            if ($("#chkShowCityInCenter").prop("checked"))
                map.setCenter(pt);
        }

        PointInMapBounds.push(F);
        str += "<tr><td><a href='#' onclick='return ConnectCity(" + F[NB_ID] + "," + F[NB_Distance] + ")'>" + F[NB_CityName] + "</a><td>" + F[NB_Distance];
    }

    str += "</table>";
    str += "<a href='#' onclick='return ZoomToConnectedPoints(PointInMapBounds)'>Zoom</a>";

    $("#divNearByCity").html(str);

    ShowPointInMapBounds();
    GetCityConnection();
}

function ShowMarker(Index)  //2 
{
    var F = PointInMapBounds[Index];
    var pt = new google.maps.LatLng(parseFloat(F[3]), parseFloat(F[4]));
    marker.setPosition(pt);
}

function ConnectCity(ToCityID, Distance)  //3
{
    var FromID = $("#lstCityFrom").val();
    $.ajax({ url: "../Data.aspx?Action=SAVE_CONNECTION&Data1=" + FromID + "&Data2=" + ToCityID + "&Data3=" + Distance, cache: false, success: GetPointsInBounds });
}

function GetCityConnection()  //3 Getting NearBy Connected Cities
{
    $.ajax({ url: "../Data.aspx?Action=GET_CITY_CONNECTION&Data1=" + $("#lstCityFrom").val(), cache: false, success: GetCityConnectionDone });
}

var CC_FromCityID = 0, CC_ToCityID = 1, CC_CityName = 2, CC_Lat = 3, CC_Lng = 4, CC_Distance = 5;
function GetCityConnectionDone(Data)  //4 Showing Nearby Connected Cities in Table.
{
    ConnectionList.length = 0;
    var Lines = Data.split('~');
    var str = "";

    if (Lines[0] != "")
        $("#divConnectedCity").html(Lines[0]);
    else
    {
        for (var i = 1; i < Lines.length; i++)
        {
            if (Lines[i] == "")
                continue;

            var F = Lines[i].split('^');
            ConnectionList.push(F);
            str += "<tr><td><a href='#' onclick='ShowNextCity(\"" + F[CC_CityName] + "\")'>" + F[CC_CityName] + "</a><td>" + F[CC_Distance] + "<td><a href='#' onclick='RemoveConnection(" + F[CC_FromCityID] + "," + F[CC_ToCityID] + ")'> X </a>";
        }
        str = "<table cellpadding='0' cellspacing='0'><tr><td>ToCityName<td>km<td>" + str + "</table>";
        str += "<a href='#' onclick='return ZoomToConnectedPoints(ConnectionList)'>Zoom</a>";

        $("#divConnectedCity").html(str);
    }
    ShowConnectionsOnMap();
}

function ShowConnectionsOnMap()  //5  Showing Connected NearBy Cities on Map By Line.
{
    if (ConnectionList.length == 0)
        return;

    var FromPt = new google.maps.LatLng(parseFloat(PointInMapBounds[0][NB_Lat]), parseFloat(PointInMapBounds[0][NB_Lng]));

    for (var i = 0; i < ConnectionList.length; i++)
    {
        var Fields = ConnectionList[i];
        var ToPt = new google.maps.LatLng(parseFloat(Fields[CC_Lat]), parseFloat(Fields[CC_Lng]));
        var Connections = new google.maps.Polyline({ path: [FromPt, ToPt], strokeColor: "#ff0000", strokeCapacity: 1.0, strokeWeight: 2 });
        Connections.setMap(map);

        Overlays.push(Connections);
    }
}

function ZoomToConnectedPoints(PointsArray)
{
    var bounds = new google.maps.LatLngBounds();
    for (var i = 1; i < PointsArray.length; i++)
    {
        var Fields = PointsArray[i];
        var ToPt = new google.maps.LatLng(parseFloat(Fields[CC_Lat]) + .01, parseFloat(Fields[CC_Lng]));
        bounds.extend(ToPt);
    }

    map.fitBounds(bounds);
    return false;
}

function RemoveConnection(FromCityID, ToCityID)  //3
{
    $.ajax({ url: "../Data.aspx?Action=REMOVE_CONNECTION&Data1=" + FromCityID + "&Data2=" + ToCityID, cache: false, success: GetCityConnection });
}

function ShowNextCity(CityName)  //6
{
    $("#txtName").val(CityName);
    ListBoxFilter();
    GetPointsInBounds();
}

function ShowPointInMapBounds()  //3  Showing NearBy Cities on Map.
{
    var FromPt = new google.maps.LatLng(parseFloat(PointInMapBounds[0][NB_Lat]), parseFloat(PointInMapBounds[0][NB_Lng]));

    for (var i = 1; i < PointInMapBounds.length; i++)
    {
        var Fields = PointInMapBounds[i];
        var ToPt = new google.maps.LatLng(parseFloat(Fields[NB_Lat]) + .005, parseFloat(Fields[NB_Lng]));
        var marker = new google.maps.Marker({ map: map, position: ToPt, title: Fields[NB_CityName] + "," + Fields[NB_StateName], icon: '../Images/icon_red_dot.png' });
        GetID(marker, Fields[NB_ID], Fields[NB_CityName]);
        Overlays.push(marker);
    }
}

function GetID(marker, ToCityID, CityName)
{
    google.maps.event.addListener(marker, 'click', function ()
    {
        LastSelectedCity = $("#spanCityID").html();

        $("#spanCityID").html(ToCityID);
        $("#spanCityName").html(CityName);

        GetPointsInBounds();
       
        if ($("#chkAutoConnect").prop("checked"))
        {
            $("#lstCityFrom").val($("#spanCityID").html());
            ConnectCity(LastSelectedCity, 0);
        }
    });
}

function GetConnectionsInMapBounds()
{
    var bounds = map.getBounds();
    
    $.ajax({ url: "../Data.aspx?Action=GET_CONNECTIONS_IN_BOUNDS"
            + "&Data1=" + bounds.getNorthEast().lat()
            + "&Data2=" + bounds.getNorthEast().lng()
            + "&Data3=" + bounds.getSouthWest().lat()
            + "&Data4=" + bounds.getSouthWest().lng()
            , cache: false, success: ShowConnectedCities
    });
}

function ShowConnectedCities(Data)
{
    var Lines = Data.split('~');
    var bounds = new google.maps.LatLngBounds();

    for (var i = 0; i < Lines.length; i++)
    {
        if (Lines[i] == "")
            continue;

        var F = Lines[i].split('^');
        var frompt = new google.maps.LatLng(parseFloat(F[0]), parseFloat(F[1]));
        var topt = new google.maps.LatLng(parseFloat(F[2]), parseFloat(F[3]));
        var myPoints = new Array();
        myPoints.push(frompt);
        myPoints.push(topt);

        var Connections = new google.maps.Polyline({ path: myPoints, strokeColor: "#ff0000", strokeCapacity: 1.0, strokeWeight: 2 });

        Connections.setMap(map);
        Overlays.push(Connections);
    }
}

function btnClearMap_onclick()
{
    ClearOverlays();
}

$(window).keypress(function (e)
{
    if (SelectedControlID != "txtName")
        $("#btnClearMap").focus();

    var code = e.which || e.keyCode;

    switch (code)
    {
        case 99: //C
            ConnectCity($("#spanCityID").html(), 0);
            break;
        case 115: //S
            $("#lstCityFrom").val($("#spanCityID").html());
            GetPointsInBounds();
            break;
    }
});


