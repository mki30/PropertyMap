<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapCanvas.aspx.cs" Inherits="MapCanvas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <link rel="stylesheet" type="text/css" href="css/redmond/jquery-ui-1.9.1.custom.min.css" />
    <script src="sim/kinetic-v4.0.1.js"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="sim/canvasmap.js"></script>
    <script src="sim/CanvasEditor.js"></script>
    <script type="text/javascript" src="Js/QueryString.js"></script>
    <script type="text/javascript">
        //var BasePath = "http://propertylist.in/";
        var BasePath = "http://localhost:4566/PropertyMap/";
    </script>

    <script type="text/javascript">

        $(document).ready(function ()
        {
            console.log("Map Loaded");
            LoadStation('INDIRAPURAM', true);
        });

        var CenterDone = false;
        function LoadStallLocation()
        {
            if (CenterDone)
                return;

            var q = new QueryString();       //using QueryString.js
            q.read();
            var Map_Zoom = q.getQueryString("Zoom");

            var FromLat = q.getQueryString("LatFrom");
            var FromLng = q.getQueryString("LngFrom");
            var LatTo = q.getQueryString("LatTo");
            var LngTo = q.getQueryString("LngTo");
            alert(FromLat + "-" + FromLng + "/" + LatTo+"-"+LngTo);

            //var SocietyName = q.getQueryString("SocietyName");
            //$('h1').text(SocietyName);

            FlashShape = GetPolygon(StallNo);
            if (FlashShape)
            {
                var pt = GetShapeCenter(FlashShape.Points);
                pt = GetLatLng(pt.X, pt.Y);
                map.setZoom(parseInt(Map_Zoom, 10));
                SetCenter(pt.Lat, pt.Lng);
            }
            CenterDone = true;
        }
    </script>
</head>
<body>
    <meta name="description" content="Distance of society from diffferent amenities"/>
    <h1></h1>
    <form id="form1" runat="server">
    <div>
    <div id="canvas_container" style="width: 100%; height: 600px; z-index: 1000; position: absolute; left: 0px; top: 0px;"></div>
    <div id="map_canvas" style="width: 100%; height: 600px; position: absolute; left: 0px; top: 0px;"></div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    </form>
</body>
</html>
