var DataFromList = new Array(),
DataToList = new Array(),
LastLat = "", LastLng = "",
geocoder,
map,
myPoints = new Array(),
marker,
directionsRenderer,
directionsService,
RunOnAuto=true;
var BasePath = 'http://localhost:4566/PropertyMap/';
var D_ID = 0,
    D_NAME = 1,
    D_LAT = 2,
    D_LNG = 3;

var LandmarkTypeText = "airport";
var LandmarkTypeVal = 0;

$(document).ready(function ()
{
    LandmarkTypeVal = $("#SelectLandm").val();                    // set and chane landmark values
    LandmarkTypeText = $("#SelectLandm option:selected").text();
    $('#SelectLandm').change(function ()
    {
        LandmarkTypeVal = $("#SelectLandm").val();
        LandmarkTypeText=$("#SelectLandm option:selected").text();
    });
    
    initialize();
    FillFromList();
});

function htmlEncode(value) { return $('<div/>').text(value).html(); }
function htmlDecode(value) { return $('<div/>').html(value).text(); }

function FillFromList()
{
    var CityName = "Ghaziabad";
    $.ajax({
        url: "Data.aspx?Action=GetDistanceFromList&Data1=" + CityName, success: function (data)
        {
            FillList(data, DataFromList, "#lstFromList");
        }
    });
    FillToList();
}


function FillToList()
{
    var index = 0;
    $.ajax({
        url: "Data.aspx?Action=GetDistanceToList&Data1=" + DataFromList[index][D_ID]+"&Data2="+LandmarkTypeText, success: function (data)
        {
            //alert(data);
            FillList(data, DataToList, "#lstToList");
            $("#lstToList")[0].selectedIndex = 0;
            FindRoute();
        }
});
}

var ctr = 0;
function UpdateDistance(Instruction, LatLng, Time)
{
    var indexFrom = $("#lstFromList").val();
    var indexTo = $("#lstToList").val();
    var dist = $("#txtRoadDistance").val();

    if (indexFrom == -1 || indexTo == -1)
        return;
    //console.log(Instruction + "-" + LatLng);
    //alert(LandmarkTypeVal);
    $.post(BasePath + "Data.aspx?Action=UpdateDistance&Data1=" + DataFromList[indexFrom][D_ID]
                    + "&Data2=" + DataToList[indexTo][D_ID]
                    + "&Data3=" + dist
                    + "&Data4=" + Time
                    + "&Data5="+LandmarkTypeVal,
                    { "Instructions": Instruction, "LatLng": LatLng },
            function (data)
            {
                $("#lblUpdated").html(ctr + " - Hotels Updated");
                if (RunOnAuto)
                {
                    $("#lstToList")[0].selectedIndex++;
                    if ($("#lstToList")[0].selectedIndex == -1)
                    {
                        ctr++;
                        $("#lstFromList")[0].selectedIndex++;
                        //FillToList();
                        $("#lstToList")[0].selectedIndex = 0;
                        setTimeout("FindRoute()", 2000);
                    }
                    else
                        setTimeout("FindRoute()", 2000);
                }
            });
}

function FindRoute()
{
    var indexFrom = $("#lstFromList").val();
    var indexTo = $("#lstToList").val();
    GenerateRoute(DataFromList[indexFrom][D_LAT], DataFromList[indexFrom][D_LNG], DataToList[indexTo][D_LAT], DataToList[indexTo][D_LNG]);
}

function FillList(Data, Arr, ctlID)
{
    var O = "";
    Arr.length = 0;
    var Lines = Data.split('~');
    console.log(Lines);
    for (i = 0; i < Lines.length; i++)
    {
        if (Lines[i] == "")
            continue;

        var Fields = Lines[i].split('^');
        Arr.push(Fields);
        O += "<option value='" + (Arr.length - 1) + "'>" + Fields[D_NAME] + "</option>";
    }
    $(ctlID).html(O);
}

function initialize()
{
    geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(LastLat == "" ? 28.58 : LastLat, LastLng == "" ? 77.33 : LastLng);
    var myOptions = {
        zoom: 13,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    google.maps.event.addListener(map, 'click', function (e)
    {
        $("#txtLat").val(e.latLng.Xa);
        $("#txtLng").val(e.latLng.Ya);
        marker.setPosition(e.latLng);
    });
    marker = new google.maps.Marker({ map: map, position: latlng });
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    directionsRenderer.setMap(map);
}

function codeAddress(address)
{
    geocoder.geocode({ 'address': address }, function (results, status)
    {
        if (status == google.maps.GeocoderStatus.OK)
        {
            map.setCenter(results[0].geometry.location);
            var pt = results[0].geometry.location;
            marker.setPosition(pt);
            $("#txtLat").val(pt.lat());
            $("#txtLng").val(pt.lng());
        }
    });
}

function GenerateRoute(FromLat, FromLng, ToLat, ToLng)
{
    console.log(FromLat + ", " +  FromLng + ", " +  ToLat + ", " +  ToLng);
    var end = new google.maps.LatLng(FromLat, FromLng);
    var address= new google.maps.LatLng(ToLat, ToLng);

    var Instructions = "";
    var str = "<table style='width:100%'>", ctr = 1;
    $("#txtRoadDistance").val("").css("background-color", "white");
    
    if (address)
    {
        var start = address;
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };
        var LatLng = "";
        directionsService.route(request, function (result, status)
        {
            if (status == google.maps.DirectionsStatus.OK)
            {
                directionsRenderer.setDirections(result);
                var meters = 0, Seconds = 0, route = result.routes[0];

                for (ii = 0; ii < route.legs.length; ii++)
                {
                    //For Displaying Route
                    $(route.legs[ii].steps).each(function (index)
                    {
                        Instructions += htmlDecode(this.instructions) + "^";
                        str += "<tr style='background:" + (ctr % 2 ? "#FBF8EB" : "#FFFFFF") + "'><td>" + ctr + ". <td>" + htmlDecode(this.instructions);
                        ctr++;

                        $(this.lat_lngs).each(function (ik)
                        {
                            LatLng += this + "^";
                        });
                    });

                    meters += route.legs[ii].distance.value;
                    Seconds += route.legs[ii].duration.value;
                }
                var distance = Math.round(meters / 1000), duration = Math.round(Seconds / 60);
                Instructions += "~" + distance + "~" + duration;
                $("#txtRoadDistance").val(distance);
                
                UpdateDistance(Instructions, LatLng, duration);
                $("#txtRoadDistance").css("background-color", "Yellow");
                str += "</table>";
                $("#divInstruction").html(str);
            }
        });
    }
    return false;
}
