/// <reference path="markerwithlabel_packed.js" />
/// <reference path="jquery-1.9.1.min.js" />
/// <reference path="Common.js" />
//....................propertymap.aspx................................//

var map,
directionsRenderer,
directionsService;

var ProjectID = 0;
var SelectedProject;
var SocietyToDisplay = null;
var CityList = { ID: 0, ParentID: -1, Name: "City", Level: 0, Index: 0, PolyPoints: "" };
var SelectedCity = null;
var LastSociety = null;
var _markers = new Array();
var markerCluster = null;
var infoWindowContent = "";
var infowindow = new google.maps.InfoWindow({ content: infoWindowContent });
var DoNotZoom = false;

google.maps.event.addDomListener(window, 'load', initialize);

$(document).ready(function ()
{
    var q = new QueryString();       //using QueryString.js
    q.read();
    if (q.status == true)
    {
        debugger;
        SelectedCityID = q.getQueryString("CityID");
        SelectedSubCityID = q.getQueryString("SubcityID");
    }

    if (LoadMap == '2D')
    {
        Load2DMap();
        $("#ddMapSelect").val('1');
    }

    else if (LoadMap == '3D')
    {
        $("#ddMapSelect").val('2');
        Load3DMap();
    }

    $("#ddCity").on("change", function ()
    {
        SetSubcity($(this));
    });

    $("#ddMapSelect").change(function ()
    {
        if ($(this).val() == 2)
            window.location.href = BasePath + 'map3D/' + SelectedProject.URLName;
        else
            window.location.href = BasePath + 'map/' + SelectedProject.URLName;
    });
    //SetAll();
});

//function SetAll()
//{
//    SetCity();
//}
//function SetCity(CallBack)
//{
//    if (SelectedCityID == 0)
//        SelectedCityID = LoadFromLocalStorage("SelectedCityID", 4);
//    $("#ddCity").html(str).val(SelectedCityID);
//    CallBack();
//}

function SetSubcity(obj)
{
    SaveInLocalStorage("SelectedCityID", obj.val());
    LoadSubCity(obj.val());
    LoadSubCity2(obj.val());
}

function htmlEncode(value) { return $('<div/>').text(value).html(); }
function htmlDecode(value) { return $('<div/>').html(value).text(); }

function initialize()
{
    var latlng = new google.maps.LatLng(28.654742134791775, 77.4371337890625);
    var myOptions = {
        zoom: 9,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }

    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    google.maps.event.addListener(map, 'zoom_changed', UpdateZoomDisplay);

    google.maps.event.addListener(map, 'bounds_changed', function (event)
    {
        var c = map.getCenter();
        $("#txtLat").val(c.lat());
        $("#txtLng").val(c.lng());
    });

    //directionsService = new google.maps.DirectionsService();
    //directionsRenderer = new google.maps.DirectionsRenderer();
    //directionsRenderer.setMap(map);
    LoadCity();
}

function UpdateZoomDisplay()
{
    return;
    var Zoom = map.getZoom();
    $("#txtZoom").val(Zoom);

    $(SelectedCity.SubCityList).each(function ()
    {
        if (this.Label)
            this.Label.setVisible(Zoom > 15);
    });
}

// call this function after map initilize
// Load Main Cities eg Ghaziabad, Noida, Gurgaon, Greater Noida
function LoadCity()
{
    LoadSubCity(0, function ()
    {
        //console.log(SelectedCityID + "-" + SelectedSubCityID + "-" + SelectedAreaID + "-" + SelectedProjectID);
        //console.log(CityList);

        var str = "";
        $(CityList.SubCityList).each(function ()
        {
            // Ghaziabad, Noida, Gurgaon, Greater Noida
            if (this.ID == 1 || this.ID == 4 || this.ID == 173 || this.ID == 392)          //add only needed city
                str += "<option value='" + this.ID + "'>" + this.Name;
        });

        if (SelectedCityID == 0)
            SelectedCityID = LoadFromLocalStorage("SelectedCityID", 4); //Main City
        $("#ddCity").html(str).val(SelectedCityID);                                        //create city Dropdown and select city

        LoadSubCity(SelectedCityID, function ()
        {
            if (SelectedSubCityID == 0 || SelectedSubCityID == -1)                         //select subcity from local storage if no subCity is selected
            {
                SelectedSubCityID = LoadFromLocalStorage("SelectedSubCityID"); //Sub City
                //$("#ddSubCity").val(SelectedSubCityID);
                HightLightPoly(SelectedSubCityID);
                LoadSubCity2(SelectedSubCityID);
            }

            $("#ddSubCity").val(SelectedSubCityID);

            LoadProject(SelectedCityID, function ()
            {
                if (SelectedProjectID == 0)
                    SelectedProjectID = LoadFromLocalStorage("SelectedProjectID");
                else
                {
                    $("#ddProjects").val(SelectedProjectID);
                    //console.log($("#ddProjects  option:selected").val());
                    SaveInLocalStorage("SelectedProjectID", ProjectID);
                }
                ShowProject(SelectedProjectID, true);
            });
        });
        //LoadSubCity2($("#ddCity option:selected").val());
    }
   );

}

function LoadSubCity(ParentID, CallBack)
{

    //debugger;
    var Parent = GetCity(ParentID);

    if (!Parent)
        return;

    if (!Parent.SubCityList)
    {
        $.ajax({
            url: BasePath + "Data.aspx?Action=CityList&Data1=" + Parent.ID, cache: false, success:
            function (Data)
            {
                var list = JSON.parse(Data);
                $(list).each(function ()
                {
                    $.extend(this, { Parent: Parent, Poly: null, Label: null });
                });

                $.extend(Parent, { SubCityList: list });

                ShowCityList(Parent);

                if (CallBack)
                    CallBack();
            }
        });
    }
    else
        ShowCityList(Parent);
}

function LoadSubCity2(CityID)
{
    //console.log(obj);
    //var CityID = $(obj).val();
    //LoadSubCity(CityID);
    LoadProject(CityID, function ()
    {
    });
    //$("#ddProjects").val(LoadFromLocalStorage("SelectedProjectID"));
}

function LoadProject(CityID, CallBack)
{
    var City = GetCity(CityID);
    if (!City)
        return;

    if (!City.ProjectList)
    {
        $.ajax({
            url: BasePath + "Data.aspx?Action=GetProjectByCityID&Data1=" + CityID, cache: false, success:
            function (Data)
            {
                var list = JSON.parse(Data);
                $(list).each(function ()
                {
                    $.extend(this, { Parent: City, Poly: null, Label: null });
                    //ShowInfoWindow(City, map,marker);
                    CreatePoly(this, .2, 2000 + parseInt(this.ID));
                });

                $.extend(City, { ProjectList: list });

                ShowProjectList(City);

                if (CallBack)
                    CallBack();
            }
        });
    }
    else
        ShowProjectList(City);
}

function GetCity(ID, Parent)
{
    if (!Parent)
        Parent = CityList;

    if (Parent.ID == ID)
        return Parent;

    if (!Parent.SubCityList)
        return null;

    for (var i = 0; i < Parent.SubCityList.length; i++)
    {
        var Ret = GetCity(ID, Parent.SubCityList[i]);
        if (Ret != null)
            return Ret;
    }
    return null;
}

function LoadProject2()
{
    if (ProjectID == 0)
        return;

    $.ajax({
        url: BasePath + "Data.aspx?Action=GetProjectDetailJson&Data1=" + ProjectID, cache: false, success:

            function (Data)
            {
                SocietyToDisplay = JSON.parse(Data);

                var City = GetCity(SocietyToDisplay.CityID);
                if (City)
                {
                    LoadSubCity(City, function ()
                    {
                        var SubCity = GetCity(SocietyToDisplay.SubCityID);
                        LoadSubCity(SubCity, function ()
                        {
                            var Area = GetCity(SocietyToDisplay.AreaID);
                            SelectedCity = Area;
                            LoadSubCity(Area, function ()
                            {
                                ShowCityList(Area);
                                $(Area.SubCityList).each(function (Index)
                                {
                                    if (this.ID == SocietyToDisplay.ID)
                                    {
                                        ShowProject(Index);
                                    }
                                })

                            });

                        });
                    });
                }
            }
    });
}

function GenerateRoute(FromLat, FromLng, ToLat, ToLng)
{
    var end = new google.maps.LatLng(FromLat, FromLng);
    var address = new google.maps.LatLng(ToLat, ToLng);

    var Instructions = "";
    var str = "<table style='width:100%'>", ctr = 1;

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
                    str += "<tr><td>From<td> " + route.legs[ii].start_address;
                    str += "<tr><td>To <td> " + route.legs[ii].end_address;
                    str += "<tr><td>Distance <td> " + route.legs[ii].distance.text;
                    str += "<tr><td>Duration <td> " + route.legs[ii].duration.text;

                    $(route.legs[ii].steps).each(function (index)
                    {
                        str += "<tr><td>" + ctr + ". <td>" + htmlDecode(this.instructions) + "<td>" + this.distance.text + "<td>" + this.duration.text;
                        ctr++;
                    });

                    meters += route.legs[ii].distance.value;
                    Seconds += route.legs[ii].duration.value;
                }
                var distance = Math.round(meters / 1000), duration = Math.round(Seconds / 60);

                $("#RoadDistance").html("Road Distance " + distance + " km");
                $("#divInstruction").html(str + "</table>");
            }
        });
    }
    return false;
}

function SetCenter(Lat, Lng)
{
    var pt = new google.maps.LatLng(parseFloat(Lat), parseFloat(Lng));
    map.setCenter(pt);
    marker.setPosition(pt);
}

function HidePoly(City)
{
    if (!City)
        return;
    if (City.Poly)
        City.Poly.setVisible(false);

    $(City.SubCityList).each(function ()
    {
        HidePoly(this);
    });

    //if (markerCluster != null)
    //    markerCluster.setMap(null);
    //markerCluster = null;
}

function GetColor(City)
{
    var attr = $(City).attr('BuilderName');
    if (typeof attr != 'undefined')
    {
        console.log(City.Name);
        return 'black';
    }

    return "red";
    //else
    //{
    //    console.log("else");
    //    console.log(City);
    //    return 'orange';
    //}

    //else if(City.URLName!="undefined")
    //    return 'red';
    //else
    //{
    //if (City.price < 1000)
    //    return '#5555FF';
    //else if (City.price < 3000)
    //    return 'gray';
    //else if (City.price < 4000)
    //    return 'pink';
    //else if (City.price < 5000)
    //    return 'yellow';
    //else if (City.price < 7000)
    //    return 'blue';
    //else if (City.price < 10000)
    //    return 'green';
    //else
    //    return 'red';
    //}
}

function CreatePoly(City, Opacity, zIndex)
{
    console.log(City, Opacity, zIndex);
    if (City.PolyPoints == "")
        return;

    CreateLabel(City);

    var PathPoints = new google.maps.MVCArray;
    var LatLng = City.PolyPoints.split('^');

    $(LatLng).each(function ()
    {
        if (this == "")
            return;
        var pts = this.split(',');
        var pt = new google.maps.LatLng(parseFloat(pts[0]), parseFloat(pts[1]));
        PathPoints.insertAt(PathPoints.length, pt);
    });

    City.Poly = new google.maps.Polygon({
        strokeWeight: 1,
        fillColor: '#FF0000',// GetColor(City),
        fillOpacity: Opacity,
        zIndex: zIndex
    });

    City.Poly.setPaths(new google.maps.MVCArray([PathPoints]));
    City.Poly.setMap(map);

    google.maps.event.addListener(City.Poly, "click", function (event)
    {
        DoNotZoom = true;
        HightLightPoly(City.SubCityID);
        $("#ddSubCity").val(City.SubCityID);
        SaveInLocalStorage("SelectedSubCityID", City.SubCityID);
        ShowProject(City.ID, false);
        DoNotZoom = false;

        ShowInfoWindow(City, map, marker);
    });

    var marker = City.Label;
    google.maps.event.addListener(City.Poly, "mousemove", function (event)
    {
        if (map.getZoom() <= 15)
            marker.setVisible(true);
    });

    google.maps.event.addListener(City.Poly, "mouseout", function (event)
    {
        if (map.getZoom() <= 15)
            marker.setVisible(false);
    });
}

function ShowInfoWindow(City, map, marker)
{
    if (City.Lat != 0)
    {
        CreateInfowindowString(City);
        infowindow.content = infoWindowContent;
        infowindow.open(map, marker);

        $("#logo").error(function ()
        {
            $("#logo").hide();
        });
        $("#societyimg").error(function ()
        {
            $("#societyimg").hide();
        });
    }
}

function CreateInfowindowString(Object)
{
    //console.log(Object);
    infoWindowContent = '<div id="content" style="width:350px;">' +
   '<div id="siteNotice">' +
   '</div>' +
   '<h1 id="firstHeading" class="firstHeading" style="font-size:20px; color:orange;line-height:20px;margin-bottom:5px;">' + Object.Name + '&nbsp;<b  style="font-size:12px; color:black;">(' + Object.City + ')</b></h1>' +
   '<p><table><tr><td><img id="logo" src="' + BasePath + 'Data/Images_SocietyLogo/' + Object.ID + '.jpg" style="height:50px; width:100px;"></p></td>' +
   '<td><span>Builder : <a href="/builder/' + Object.BuilderName + '" target="_blank">' + Object.BuilderName + '</a></span>' +
   '<div id="bodyContent">' +
   '<p><span><a href="/project/' + Object.URLName + '" target="_blank">' +
   '<b>View Detail</b></a><span></td></tr></table>' +
   '<p><img id="societyimg" src="' + BasePath + 'Data/Images_Society/' + Object.ID + '_1.jpg" style="height:150px; width:100%;"></p>' +
   '</div>' +
   '</div>';
}

function CreateLabel(City)
{
    City.Label = new MarkerWithLabel({
        position: new google.maps.LatLng(parseFloat(City.Lat), parseFloat(City.Lng)),
        draggable: false,
        raiseOnDrag: false,
        map: map,
        labelContent: City.Name.replace("Sector ", "").replace(" ", "<br/>"),
        labelAnchor: new google.maps.Point(30, 20),
        labelClass: "labels", // the CSS class for the label
        labelStyle: { opacity: 0.0 },
        icon: "http://placehold.it/1x1",
        visible: false
    });
    google.maps.event.addListener(City.Label, "click", function (event)
    {
        ShowProject(Index, false);
    });
}

var CurrentProjectList = null;

function ShowProjectList(City)                                                          //ProjectList
{
    //ZoomToSelection(City.Poly);
    CurrentProjectList = City.ProjectList;

    var str = "";
    $(CurrentProjectList).each(function (Index)
    {
        str += "<option value='" + this.ID + "' >" + this.Name;
    });

    $("#ddProjects").html(str);
    DrawMarkerCluster();
    return false;
}

function ShowCityList(Parent)                                                            //City List
{
    //console.log(Parent);
    HidePoly(CityList);                                                                  //hide all  poly
    ZoomToSelection(Parent.Poly);

    var str = "";
    var ShowDropDown = false;
    var SocietyParent = null;

    str += "<option value='-1'>--All--</option>";
    $(Parent.SubCityList).each(function (Index)
    {
        str += "<option value='" + this.ID + "'>" + this.Name + "</option>";

        if (this.Poly)
            this.Poly.setVisible(true);
        else if (this.PolyPoints && this.PolyPoints != "" && this.PolyPoints != "0")
            CreatePoly(this, 0, 1000);
    });
    $("#ddSubCity").html(str);
    return false;
}

function HightLightPolyOff(Society)
{
    if (!Society || !Society.Poly)
        return;

    if (SelectedProject == LastSociety)
        return;

    Society.Poly.setOptions({ fillColor: "#5555FF" });
}

var LastHightLightCity = null;

function HightLightPoly(ObjectID)
{
    //console.log(ObjectID);
    if (ObjectID == -1)
        ObjectID = LoadFromLocalStorage("SelectedCityID");

    if ($("#ddSubCity option:selected").val() != -1)
        SaveInLocalStorage("SelectedSubCityID", $("#ddSubCity option:selected").val());

    LoadProject(ObjectID, function ()
    {
    });

    if (LastHightLightCity)
        LastHightLightCity.Poly.setOptions({ fillOpacity: 0.0 });

    var C = GetCity(ObjectID);

    if (C && C.Poly)
    {
        if (DoNotZoom == false)
            ZoomToSelection(C.Poly);

        LastHightLightCity = C;
        C.Poly.setOptions({ fillColor: "green", fillOpacity: 0.2 });
    }
}

function ScrollTo(li, div, scroll)
{
    if (scroll == undefined)
        scroll = true;
    if (!li)
        return;

    if (scroll)
    {
        var T = ($(li).height() * $(li).data("index")) - $(div).height();
        $(div).animate({ scrollTop: T }, 500);
    }

    $(div + " li").removeClass("active");
    $(li).addClass("active");
}

function ShowProject(ProjectID, Pan, scroll)
{
    SelectedProject = null;
    //console.log(CurrentProjectList);
    $(CurrentProjectList).each(function ()
    {
        if (this.Poly)
            this.Poly.setOptions({ fillColor: "#ff0000" });//red

        if (this.ID == ProjectID)
        {
            SelectedProject = this;
            if (LoadMap == '3D')
            {
                ShowKMZModel();
            }
            //console.log(SelectedProject);
        }
    });

    if (!SelectedProject)
        return;

    if (SelectedProject.Poly)
        SelectedProject.Poly.setOptions({ fillColor: "#ffff00" }); //yellow

    if (Pan)
        ShowOnMap(SelectedProject, 15);
}

function ShowImages(id)
{
    $.ajax({
        url: BasePath + "Data.aspx?Action=ShowImages&Data1=" + id, cache: false, success:
        function (Data)
        {
            $("#SocietyImages").html(Data);
        }
    });
}

function ShowOnMap(obj, Zoom)
{
    var pt = new google.maps.LatLng(parseFloat(obj.Lat), parseFloat(obj.Lng));
    map.setZoom(Zoom);
    map.panTo(pt);
}

function ZoomToSelection(Poly)
{
    if (!Poly)
    {
        if (SelectedProject)
            Poly = SelectedProject.Poly;
        else if (SelectedCity && SelectedCity.Poly)
        {
            Poly = SelectedCity.Poly;
        }
    }

    if (!Poly)
        return;

    var bounds = new google.maps.LatLngBounds();
    var path = Poly.getPath();

    path.forEach(function (pt)
    {
        bounds.extend(pt);
    });

    map.fitBounds(bounds);
    return false;
}

function ClearMarkers()
{
    if (!map)
        return;
    $(_markers).each(function ()
    {
        this.setMap(null);
    });
    _markers.length = 0;
    if (markerCluster != null)
        markerCluster.setMap(null);
}

function DrawMarkerCluster()
{
    ClearMarkers();
    var ctr = 0;
    var Icon = BasePath + 'Images/icon_red_dot.png';
    $(CurrentProjectList).each(function (index)
    {
        if (this.PolyPoints == "")
        {
            var marker = new google.maps.Marker({ 'position': new google.maps.LatLng(this.Lat, this.Lng), title: this.Name, icon: Icon })
            _markers.push(marker);
        }
    });
    markerCluster = new MarkerClusterer(map, _markers);
}

function Load2DMap()
{
    $("#map_canvas").show();
    $("#map_canvas3D").hide();
}

// 3D-MapDraw
google.load("earth", "1", { "other_params": "sensor=false" });
google.setOnLoadCallback(init);

var DS_ge;
var DS_geHelpers;
var DS_map;
var DS_placemarks = {};

function Load3DMap()
{
    $("#map_canvas3D").show();
    $("#map_canvas").hide();

    //ShowKMZModel();
    //GetCity();
}

function init()
{
    google.earth.createInstance('map_canvas3D', initCB, failureCB);
}

function initCB(instance)
{
    DS_ge = instance;
    DS_ge.getWindow().setVisibility(true);
    DS_ge.getNavigationControl().setVisibility(DS_ge.VISIBILITY_SHOW);
    DS_ge.getWindow().setVisibility(true);
    DS_ge.getNavigationControl().setVisibility(DS_ge.VISIBILITY_AUTO);

    //console.log(SelectedProject);

    //event.preventDefault();

    var balloon = DS_ge.createHtmlStringBalloon('');
    //balloon.setFeature(placemark); // optional
    balloon.setContentString('<div style="padding:5px;">' + SelectedProject.Name + '<div>');
    //console.log("ballon" + balloon);

    DS_ge.setBalloon(balloon);
}

function failureCB(errorCode) { /*alert("Error loading Google Earth");*/ }

//function closeBalloon()
//{
//    alert();
//    DS_ge.setBalloon(null);
//}

function ShowKMZModel()
{
    if (SelectedProject)
    {
        href = 'http://localhost:38592/Data/KMZ/' + SelectedProject.Name.split(' ').join('_') + '.kmz';
        hreftxt = 'http://localhost:38592/Data/KMZ/' + SelectedProject.Name.split(' ').join('_') + '.txt';

        $.get(href)
        .done(function ()
        {
            fetchKML(href);
        })
        .fail(function ()
        {
            //console.log(hreftxt);
            $.get(hreftxt, function (data)
            {
                var UrlS = data.split(',');
                //console.log(UrlS);
                $(UrlS).each(function ()
                {
                    href = 'http://localhost:38592/Data/KMZ/' + this.trim() + '.kmz';
                    //console.log(href);
                    //console.log(href);
                    fetchKML(href);
                });
            });
        })

        function fetchKML(href)
        {
            google.earth.fetchKml(DS_ge, href, function (kmlObject)
            {
                //console.log(kmlObject);
                if (kmlObject)
                    DS_ge.getFeatures().appendChild(kmlObject);
                if (kmlObject.getAbstractView())
                    DS_ge.getView().setAbstractView(kmlObject.getAbstractView());
            });
        }
    }
}
