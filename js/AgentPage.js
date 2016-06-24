var Lat, Lng, Edit;
$(document).ready(function ()
{
    //Data table table sorting  
    $('.datatable').dataTable(      
        {
            "filter": false,
            //"order": [[3, "desc"]]
            //"paging": false,
            //"ordering": false,
            //"info": false
        });
});

$(window).bind("load", function ()
{
    Lat = Lat == undefined ? "28.58" : Lat;
    Lng = Lng == undefined ? "77.33" : Lng;
    initializeMap(Lat, Lng, Edit == 0 ? "map_canvas" : "map_canvas_edit");
});

function initializeMap(Lat, Lng, MapID)
{
    //alert(Lat + "-" + Lng+"-"+MapID);
    //alert(MapID);
    geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(Lat, Lng);
    var myOptions = { zoom: 13, center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP };
    map = new google.maps.Map(document.getElementById(MapID), myOptions);

    google.maps.event.addListener(map, 'click', function (e)         //update lat lng on map click
    {
       console.log(e);
       $('#txtLat', window.parent.document).val(e.latLng.k);
       $('#txtLng', window.parent.document).val(e.latLng.B);
       marker.setPosition(e.latLng);
    });
    marker = new google.maps.Marker({ map: map, position: latlng });
    SetMarker(Lat, Lng);
}

function SetMarker(Lat, Lng)
{
    //alert(Lat + "-" + Lng);
    var latlng = new google.maps.LatLng(Lat, Lng);
    marker.setPosition(latlng);
    map.setCenter(latlng);
}

function CodeAddress(address, cb)                                          //serch n locate address on nmap
{
    address = address == undefined ? $("#txtCodeAddress").val() : address;
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
        if (cb != undefined)
            cb();
    });
}

function Save()
{
    if ($("#txtCompanyName").val() == "")
    {
        alert("Please enter Name. It is mandatory. ");
        return;
    }
    if ($("#txtAgentName").val() == "")
    {
        alert("Please enter Address. It is mandatory. ");
        return;
    }

    if ($("#txtMobile1").val() == "")
    {
        alert("Please enter Mobile. It is mandatory. ");
        return;
    }

    $.post(BasePath + "Data.aspx?Action=UpdateAgent&Data1=" + $("#spnAgentID").text(),
        {
          Company: $("#txtCompanyName").val(),
          Name: $("#txtAgentName").val(),
          Address: $("#txtAddress").val(),
          Area: $("#txtArea").val(),
          City:$("#ddCity option:selected").val(),
          //City: $("#txtCity").val(),
          State: $("#ddState option:selected").val(),
          Pin: $("#txtPinCode").val(),
          Phone1: $("#txtPhone1").val(),
          Phone2: $("#txtPhone2").val(),
          Mobile1: $("#txtMobile1").val(),
          Mobile2: $("#txtMobile2").val(),
          Details: $("#txtDetails").val(),
        },
        function (data)
        {
            if(data=="")
            alert("Agent Saved");
        });
}

function ShowPreview()
{
    window.open(location.href.replace("edit", ""), "");
}
//function htmlEncode(value) { return $('<div/>').text(value).html(); }
//function htmlDecode(value) { return $('<div/>').html(value).text(); }
