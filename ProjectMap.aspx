<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectMap.aspx.cs" Inherits="ProjectMap" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript" src="sim/kinetic-v4.0.1.js"></script>
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="sim/CanvasEditor.js"></script>
    <script type="text/javascript" src="sim/canvasmap.js"></script>
    <script type="text/javascript" src="sim/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript">

        var geocoder, map, marker;
        var BasePath = "";

        $(window).resize(Resized);
        $(document).ready(function ()
        {
            Resized();
            initializeLocal();
            Stn = new Station();
            LoadStation("INDIRAPURAM", true);
            setTimeout("DrawPoly()", 2000);
        });

        function DrawPoly()
        {
            $(Stn.ShapeList).each(function (index)
            {
                var pts = new Array();
                // convert from Lat Lng To Pixel
                $(this.Points).each(function ()
                {
                    pts.push(new google.maps.LatLng(this.Lat, this.Lng));
                });

                var poly = new google.maps.Polygon({
                    paths: pts,
                    strokeColor: "#FF0000",
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: "#FF0000",
                    fillOpacity: 0.35
                });
                poly.setMap(map);
            });
        }

        function initializeLocal()
        {
            geocoder = new google.maps.Geocoder();

            var Zoom = 15;

            if (isNaN(Zoom))
                Zoom = 10;

            var Lat = 28.6355845045388, Lng = 77.364169974778;
            var latlng = new google.maps.LatLng(Lat, Lng);
            var myOptions = { zoom: Zoom, center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP, scaleControl: false }

            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            //marker = new google.maps.Marker({ map: map, position: latlng });
            //google.maps.event.addListener(map, 'click', function (event)      //Update Marker on Click
            //{
            //    var Lat = event.latLng.lat();
            //    var Lng = event.latLng.lng();
            //    marker.setPosition(new google.maps.LatLng(Lat, Lng));
            //});
        }

        function FindAddress()
        {
            geocoder.geocode({ 'address': $("#txtAddress").val() }, function (results, status)
            {
                if (status == google.maps.GeocoderStatus.OK)
                {
                    map.setCenter(results[0].geometry.location);
                    var pt = results[0].geometry.location;
                    marker.setPosition(pt);
                    $("#txtLat").val(Lat);
                    $("#txtLng").val(Lng);
                    try
                    {
                        parent.UpdateLatLng(pt.lat(), pt.lng());
                    }
                    catch (e)
                    {
                        alert("Sonia");
                    }
                }
            });
        }

        function SetCenter(Lat, Lng)
        {
            var pt = new google.maps.LatLng(parseFloat(Lat), parseFloat(Lng));
            map.setCenter(pt);
            marker.setPosition(pt);
        }

        function Resized()
        {
            var H = $(window).height() - 20;
            $("#map_canvas").height(H);
        }
</script>
</head>
<body>
    <table style="width:100%;height:800px;">
        <tr>
            <td style="width:200px;">
                <div id="divShapeList" style="overflow:auto;height:100%"></div>
            </td>
            <td>
                <div id="map_canvas" style="width: 100%; height:100%;"></div>
            </td>
        </tr>
    </table>
</body>
</html>
