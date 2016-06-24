var geocoder, map, marker;
var _ZoomToRoute = false;

var ViewType = "FULL";
var overlay;

USGSOverlay.prototype = new google.maps.OverlayView();

//$(window).resize(Resized);

var CurrentBounds = null,
MapMinX = 180,
MapMaxX = 0,
MapMinY = 180,
MapMaxY = 0;


// Create your map and overlay

MyOverlay.prototype = new google.maps.OverlayView();
MyOverlay.prototype.onAdd = function () { }
MyOverlay.prototype.onRemove = function () { }
MyOverlay.prototype.draw = function () { }
function MyOverlay(map) { this.setMap(map); }
var projection;

function initialize(Lat,Lng)
{
    console.log($("#map_canvas"));

    if ($("#map_canvas") == undefined)
        return;
    geocoder = new google.maps.Geocoder();
 
    if(Lat==0)
        Lat = 28.615640724032573;

    if(Lng==0)
        Lng= 77.24547417924191;
    
   var Zoom = 17;

    if (isNaN(Zoom))
        Zoom = 10;

    var latlng = new google.maps.LatLng(Lat, Lng);
    var myOptions = { zoom: Zoom, center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP, scaleControl: true, disableDefaultUI: true, }
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    marker = new google.maps.Marker({ map: map, position: latlng });
    
    google.maps.MapTypeControlStyle.DROPDOWN_MENU

    var overlay = new MyOverlay(map);
    // Wait for idle map
    google.maps.event.addListener(map, 'idle', function ()
    {
        // Get projection
        projection = overlay.getProjection();

        CurrentBounds = map.getBounds();

        MapMinX = CurrentBounds.getSouthWest().lng()
        MapMaxX = CurrentBounds.getNorthEast().lng();

        MapMinY = CurrentBounds.getSouthWest().lat()
        MapMaxY = CurrentBounds.getNorthEast().lat();

        MapIdle();
		
    })
       
}

var MapMouseDown, MapMouseUp, MapMouseMove,LastMapType=false;


function ToggleMap()
{
    LastMapType = !LastMapType;
    if(LastMapType)
        map.setMapTypeId(google.maps.MapTypeId.HYBRID);
    else
        map.setMapTypeId(google.maps.MapTypeId.ROADMAP);
}


function MapPan(X, Y)
{
    map.panBy(X, Y);
}

function MapPanTo(Lat, Lng)
{
    map.panTo(new google.maps.LatLng(Lat, Lng));
}

function GetMapCenter()
{
    var pt = map.getCenter();
    return ({ Lat: pt.lat(), Lng: pt.lng() });
}

function GetXYFromLatLng(Lat, Lng)
{
    //var point = overlay.getProjection().fromLatLngToDivPixel(new google.maps.Point(Lat, Lng));
    var point = projection.fromLatLngToDivPixel(new google.maps.LatLng(Lat, Lng));
    return { X: point.x, Y: point.y };
}
function GetLatLng(X, Y)
{
    if(!projection)
        return { Lng: 0, Lat: 0 };
    var coordinates = projection.fromContainerPixelToLatLng(new google.maps.Point(X, Y));
    return { Lng: coordinates.lng(), Lat: coordinates.lat() };
}


function ZoomMap(InOut)
{
    var zoomLevel = map.getZoom();
    map.setZoom(zoomLevel + InOut);
}


function ZoomToRoute()
{
    if (bounds != null)
        map.fitBounds(bounds);
}

function ShowMarker(Lat, Lng)
{
    var pt = new google.maps.LatLng(parseFloat(Lat), parseFloat(Lng));
    map.panTo(pt);
    marker.setPosition(pt);
}

function SetCenter(Lat, Lng)
{
    var pt = new google.maps.LatLng(parseFloat(Lat), parseFloat(Lng));
    map.setCenter(pt);
}

function USGSOverlay(bounds, image, map)
{
    this.bounds_ = bounds;
    this.image_ = image;
    this.map_ = map;
    this.div_ = null;

    // Explicitly call setMap on this overlay
    this.setMap(map);
}

USGSOverlay.prototype.onAdd = function ()
{
    this.div_ = document.createElement('div');
    $(this.div_).css({ "background-color": "yellow", position: "absolute" });

    var panes = this.getPanes();
    panes.overlayLayer.appendChild(this.div_);
}



