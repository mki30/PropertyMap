var map, geocoder, poly = [], mapLabel = [];
var Lat = 28.6355845045388, Lng = 77.364169974778, Zoom = 12, ID = 0;
var LastSelected;

$(document).ready(function ()
{
    
    if (!window.Points) 
        window.Points = [];

    if (!window.Labels)
        window.Labels = [];

    initialize();

    CreateLabel();
    CreatePoly();

    //if (LastSelected)
    //    ShowPoly(LastSelected);
});

function initialize()
{
    var latlng = new google.maps.LatLng(Lat, Lng);
    var myOptions = { zoom: Zoom, center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP, scaleControl: false }
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    geocoder = new google.maps.Geocoder();

    google.maps.event.addListener(map, 'click', addPoint);     //Update Marker on Click
}

function addPoint(event)
{
    var Lat = event.latLng.lat();
    var Lng = event.latLng.lng();
    var Pt = new google.maps.LatLng(Lat, Lng);

    $("#txtLat").val(Lat);
    $("#txtLng").val(Lng);

    if ($("#chkCreateArea").prop("checked"))
        AddPathPoint(Pt);
}

function AddPathPoint(LatLng)
{
    var path = poly[LastSelected].getPath();
    path.insertAt(path.length, LatLng);
}

function Reset()
{
    var path = poly[LastSelected].getPath();
    path.clear();
}

function CreateLabel()
{
    $(mapLabel).each(function () {
        this.setMap(null);
    });

    mapLabel.length = 0;

    $(Labels).each(function (Index)
    {
        var Pts = Points[Index];
        if (!Pts)
            return;

        if (Pts.length < 2)
            return;

        mapLabel[Index] = new MarkerWithLabel({
            position: new google.maps.LatLng(parseFloat(Pts[0]), parseFloat(Pts[1])),
            draggable: false,
            raiseOnDrag: false,
            map: map,
            labelContent: this,
            labelAnchor: new google.maps.Point(30, 20),
            labelClass: "labels", // the CSS class for the label
            labelStyle: { opacity: 1.0 },
            icon: "http://placehold.it/1x1",
            visible: false
        });

    });

    //maps
}

function CreatePoly()
{
    $(poly).each(function (Index)
    {
        this.setMap(null);
    });

    poly.length = 0;

    console.log(map);

    $(Points).each(function (Index)
    {

        var Pts = this;
        var path = new google.maps.MVCArray;
        
        for (var i = 0; i < Pts.length; i += 2)
        {
            var Pt = new google.maps.LatLng(Pts[i], Pts[i + 1]);

            console.log(Pt);
            path.insertAt(path.length, Pt);
        }

        poly[Index] = new google.maps.Polygon({
            strokeWeight: 3,
            fillColor: '#5555FF',
            //editable: true
        });
        poly[Index].setMap(map);
        poly[Index].setPaths(new google.maps.MVCArray([path]));

        google.maps.event.addListener(poly[Index], "click", function (event)
        {
            try
            {
                AutoImport(Index);
            }
            catch (e) { }

            try
            {
                ShowPoly(Index);
                SelectPoly(Index);
            }
            catch (e) { }


        });

        google.maps.event.addListener(poly[Index], "mousemove", function (event)
        {
            if (mapLabel[Index])
                mapLabel[Index].setVisible(true);
        });

        google.maps.event.addListener(poly[Index], "mouseout", function (event)
        {
            if (mapLabel[Index])
                mapLabel[Index].setVisible(false);
        });

        google.maps.event.addListener(path, 'insert_at', function ()
        {
            SendPointToParent();
        });
        google.maps.event.addListener(path, 'remove_at', function ()
        {
            SendPointToParent();
        });
        google.maps.event.addListener(path, 'set_at', function ()
        {
            SendPointToParent();
        });

    });
}

function SendPointToParent()
{
    //try
    {
        $("#frameCity")[0].contentWindow.UpdatePolyPoints(GetPolyData());
    }
    //catch (e)
    {

    }
}

function GetPolyData()
{
    var str = "";
    var path = poly[LastSelected].getPath();
    path.forEach(function (element)
    {
        str += element.lat() + "," + element.lng() + "^";

    });

    return str;
}

function AutoImport(Index)
{
    
    if ($("#chkAutoImport").prop("checked"))
    {
        var ParentCityID = $("#ddCity").val();
        
        var Pts = Points[Index];
        var PolyPoints = "";
        for (var i = 0; i < Pts.length; i += 2)
        {
            PolyPoints += Pts[i] + "," + Pts[i + 1] + "^";
        }

        $.post("../Data.aspx?Action=UpdateCityPoly&Password=2012&Data1=" + ParentCityID + "&Data2=-1&Data3=" + Labels[Index], { LatLng: PolyPoints }, function (data) { alert("Done"); });
    }
    else
        alert("Auto import not selected");
}

function ImportPoly(CityID, ParentCityID) {

    if ($("#txtPolyPoints").val() == "") {

        alert("No Polypoints found");
        return;
    }

    if(!ParentCityID)
        ParentCityID = $("#ddCity").val();

    $.post("../Data.aspx?Action=UpdateCityPoly&Password=2012&Data1=" + ParentCityID + "&Data2=" + CityID, { LatLng: $("#txtPolyPoints").val() }, function (data) { alert("Done"); });
}



function SetCenter(Lat, Lng)
{
    var pt = new google.maps.LatLng(parseFloat(Lat), parseFloat(Lng));
    map.setCenter(pt);
    map.setZoom(16);
    PosMarker.setPosition(pt);
}


function ShowPoly(Index, Editable)
{
    if (LastSelected)
        poly[LastSelected].setOptions({ fillColor: "5555FF" });

    console.log(poly);
    poly[Index].setOptions({ fillColor: "yellow" });

    if (Editable)
    {
        $(poly).each(function ()
        {
            this.setOptions({ editable: false });
        });

        poly[Index].setOptions({ editable: true });
    }

    LastSelected = Index;


    var bounds = new google.maps.LatLngBounds();

    var Pts = Points[Index];
    var PolyPoints = "";
    for (var i = 0; i < Pts.length; i += 2)
    {
        var ToPt = new google.maps.LatLng(Pts[i], Pts[i + 1]);
        PolyPoints += Pts[i] + "," + Pts[i + 1] + "^";
        bounds.extend(ToPt);
    }

    $("#txtPolyPoints").val(PolyPoints);

    if (!$("#chkZoomLock").prop("checked"))
        map.fitBounds(bounds);
    
    if (!$("#chkPanLock").prop("checked"))
        map.panTo(bounds.getCenter());

        return false;

}


function FindAddress()
{
    geocoder.geocode({ 'address': $("#txtAddress").val() }, function (results, status)
    {
        if (status == google.maps.GeocoderStatus.OK)
        {
            console.log(results);
            map.setCenter(results[0].geometry.location);
            var pt = results[0].geometry.location;
            //PosMarker.setPosition(pt);
            map.fitBounds(results[0].geometry.bounds);
            $("#txtLat").val(pt.lat());
            $("#txtLng").val(pt.lng());

        }
    });
}