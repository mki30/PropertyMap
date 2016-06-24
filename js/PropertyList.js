/// <reference path="../Map.htm" />
/// <reference path="../fancybox/jquery.fancybox-1.3.4.js" />

var SellerID = 0;

var sellerType = 0, avlID = 0, ClientID = 0;
var ClentList = new Array();
var AvlClientList = new Array();
var AvlList = new Array();

var SocAvlList = new Array();

var PAGE_CLIENT_MAIN = 0;
var PAGE_AGENTMAIN = 1;
var PageType = PAGE_CLIENT_MAIN;
var SocietyDetail = new Array();
var FavList = new Array();

$(window).resize(Resized);

function Resized()
{
    var H = $(window).height(),W = $(window).width();
    H -= 80;

    //$("#map_canvas").height(H-40);
    // $("#map_canvas").width(W-400);
    $("#divInstruction").height(H-20);
    //$("#divInstruction").css({ left: off.left, top: off.top });
}

$(document).ready(function ()
{
    
    $("#tableFloorPlans input").bind("keyup", function ()
    {
        //alert($(this).attr('id'));
        CalculatePrice($(this).attr('id').split('_')[1]);
    });

    if (PageType == PAGE_AGENTMAIN)
        GetAvlList(SellerID);

    //$.ajax({ url: BasePath + "Data.aspx?Action=GetSocietyList&Data1=0", cache: false, success: GetSocietyListDone });
    
    $("#txtFilter").keyup(function ()                     // search in client list
    {
        var Search = $("#txtFilter").val().toLowerCase();
        var arr = new Array();
        $(ClentList).each(function ()
        {
            if (this.Name == undefined)
                return;
            if (this.Name.toLowerCase().indexOf(Search) > -1)
            {
                arr.push(this);
            }
        });
        ShowClientList(arr);
    });

    //UpdateBasePath();

    //tooltip
    //$(document).tooltip(
    //    {
    //        track: true,
    //        position: {
    //            my: "center bottom-20",
    //            at: "center top",
    //            using: function (position, feedback)
    //            {
    //                $(this).css(position);
    //                $("<div>")
    //                    .addClass("arrow")
    //                    .addClass(feedback.vertical)
    //                    .addClass(feedback.horizontal)
    //                    .appendTo(this);
    //            }
    //        }
    //    });

    //ShowHistory();

    try
    {

        $(".fancybox").fancybox(
        {
        });

        $(".fancyBox").fancybox(
          {
            'titleShow': 'true',
            'transitionIn': 'elastic',
            'transitionOut': 'elastic',
            'speedIn': 600,
            'speedOut': 200,
            'overlayShow': true,
            helpers: {
                title: {
                    type: 'outside',
                    position: 'top'
                }
            }
        });

        //$("#fancyBoxVedio").fancybox({
        //    'padding': 0,
        //    'autoScale': false,
        //    'transitionIn': 'none',
        //    'transitionOut': 'none',
        //    'title': this.title,
        //    'width': 680,
        //    'height': 495,
        //    'type': 'iframe',
        //    'swf': {
        //        'wmode': 'transparent',
        //        'allowfullscreen': 'true'
        //     }
        //  }
        //);

        $(".manualfancybox").click(function () //for Gallary
        {
            var photos = new Array();

            $(".details_gallery_min a").each(function ()
            {
                href = $(this).attr("href");
                title = $(this).attr("title");
                photos.push({ 'href': href, 'title': title })
            });

            jQuery.fancybox(photos,
                {
                    'transitionIn': 'elastic',
                    'easingIn': 'easeOutBack',
                    'transitionOut': 'elastic',
                    'easingOut': 'easeInBack',
                    'opacity': false,
                    'titleShow': true,
                    'titlePosition': 'over',
                    'type': 'image',
                    'titleFromAlt': true
                }
            );
        });
    }

    catch (e) { }
   
    var subcity = $("#hidSubCity").val();
    
    //console.log(Rate["MinRate"]);
    //alert(subcity);

    //-----------------graph Plot----------------------------
    //var plot = $.plot($("#placeholder"),
    //                  [{ data: $.globalEval(MinRate), label: "Min = -0.00" }, { data: MaxRate, label: "Max = -0.00" }, { data:AvgRate, label: "Avg = -0.00" }],
    //                   {
    //                       series: { lines: { show: true }, points: { show: true }, bars: { show: false } },
    //                       crosshair: { mode: "x" },
    //                       grid: { hoverable: true, autoHighlight: true },
    //                       legend: { backgroundColor: "none", position: "nw", container: null },
    //                       yaxis: { min: 1000 },
    //                       xaxis: { show: true, mode: "time", timeformat: "%b-%y", tickSize: [4, "month"] },
    //                       xaxes: [{ axisLabel: 'Time'}],
    //                       yaxes: [{ position: 'left', axisLabel: 'Rs/Sq ft'}]
    //                   });

    //var legends = $("#placeholder .legendLabel");
    //legends.each(function ()
    //{
    //    // fix the widths so they don't jump around
    //    //$(this).css('width', $(this).width());
    //});

    //var updateLegendTimeout = null;
    //var latestPosition = null;

    //function updateLegend()
    //{
    //    updateLegendTimeout = null;

    //    var pos = latestPosition;

    //    var axes = plot.getAxes();
    //    if (pos.x < axes.xaxis.min || pos.x > axes.xaxis.max ||
    //        pos.y < axes.yaxis.min || pos.y > axes.yaxis.max)
    //        return;

    //    var i, j, dataset = plot.getData();
    //    for (i = 0; i < dataset.length; ++i)
    //    {
    //        var series = dataset[i];

    //        // find the nearest points, x-wise
    //        for (j = 0; j < series.data.length; ++j)
    //            if (series.data[j][0] > pos.x)
    //                break;

    //        // now interpolate
    //        var y, p1 = series.data[j - 1], p2 = series.data[j];
    //        if (p1 == null)
    //            y = p2[1];
    //        else if (p2 == null)
    //            y = p1[1];
    //        else
    //            y = p1[1] + (p2[1] - p1[1]) * (pos.x - p1[0]) / (p2[0] - p1[0]);

    //        legends.eq(i).text(series.label.replace(/=.*/, "= " + parseInt(y, 10)));
    //    }
    //}

    //$("#placeholder").bind("plothover", function (event, pos, item)
    //{
    //    latestPosition = pos;
    //    if (!updateLegendTimeout)
    //        updateLegendTimeout = setTimeout(updateLegend, 50);
    //});
    //-----------------graph Plot------------------------

    Resized();
});


function GetSocietyListDone(data)
{
    $("#divSociety").html(ShowError(data)[1]);
    $("#societytable").HighLightRows();
    $("#societytable a").css("text-decoration", "none");
    //$("#societytable th").css("background", "-webkit-gradient(linear, left top, left bottom, from(white), to(orange))");
    $("#societytable th").addClass("tablehead");
}

function GetSocietyAvailability(SocietyID, bhk)
{
    //$.ajax({ url: "Data.aspx?Action=GetSocietyAvailability&Data1=" + SocietyID+"&Data2="+bhk, cache: false, success: GetSocietyAvailabilityDone });
    $.ajax({
        url: "Data.aspx?Action=GetAvailibilityText&Data4=1&Data1=" + SocietyID + "&Data2=" + bhk, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);
            SocAvlList.length = 0;
            $(Lines).each(function ()
            {
                SocAvlList.push({ ID: this[0], SocietyID: this[1], SocietyName: this[2], SellerID: this[3], SellerName: this[4], BHK: this[5], Amount: this[6], AvailableFrom: this[7], Area: this[8], Bathroom: this[9], FloorNo: this[10], Facing: this[11], PostedOnDate: this[12] });
            });
            //console.log(SocAvlList);
            ShowList(SocAvlList);
        }
    });
}

function ShowList(Arr)
{
    var count = 1;
    var S = "<table style='border-spacing:1px;'><tr><th></th><th>Society</th><th>BHK</th><th>Rent</th><th>Area</th><th>Bath</th><th>Floor</th><th>Featutres</th><th>Avl From</th><th>Posted Bef(days)</th></tr>";
    $(Arr).each(function ()
    {
        if (this.ID == undefined)
            return;
        S += "<tr onclick='ShowAgentDetail(" + this.ID + ",0)'><td>" + count + "<td>" + this.SellerName + "<td>" + this.BHK + "<td>" + this.Amount;
        S += "<td>" + this.Area + "<td>" + this.Bathroom + "<td>" + this.FloorNo + "<td style='max-width:180px;'>" + this.Facing + "<td>" + this.AvailableFrom + "<td>" + this.PostedOnDate;
        count++;
    });
    S += "</table>";
    $("#divDetails").html(S);
    $("#divDetails table").HighLightRows();
    //$("#divDetails table th").css("background-color", "#A9C7BC");
    $("#divDetails table tr").css("cursor", "pointer");
    $("#divDetails table th").addClass("tablehead");
}

//function GetSocietyAvailabilityDone(data)
//{
//    $("#divDetails").html(ShowError(data)[1]);
//    $("#AvlTable").HighLightRows();
//    $("#AvlTable a").css("text-decoration", "none");
//    $("#AvlTable th").css("background-color", "orange");
//}


function ShowSocietyDetail(ID)
{
    //$.ajax({ url: "Data.aspx?Action=GetSocietyDetail&Data1=" + ID, cache: false, success: function (data) { $("#divSocietyDetail").html(ShowError(data)[1]); } });
    //return false;
    $.ajax({
        url: "Data.aspx?Action=GetSocietyDetail&Data1=" + ID, async: false, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);
            $(Lines).each(function ()
            {
                SocietyDetail.length = 0;
                SocietyDetail.push({ SocietyName: this[0], Address: this[1], Pin: this[2], City: this[3], BuiltYear: this[4], Country: this[5], Airport: this[6], Rail: this[7], Bus: this[8], School: this[9], Hospital: this[10], School: this[11], Shoping: this[12], PowerBack: this[13], Lat: this[14], Lng: this[15] });
            });
        }
    });

    var S = "<table cellpadding='0' cellspacing ='1'>";
    $(SocietyDetail).each(function ()
    {

        S += "<tr><td>SocietyName<td>" + this.SocietyName;
        S += "<tr><td>Address<td>" + this.Address;
        S += "<tr><td>Pin<td>" + this.Pin;
        S += "<tr><td>City<td>" + this.City;
        S += "<tr><td>BuiltYear<td>" + this.BuiltYear;
        S += "<tr><td>Country<td>" + this.Country;
        S += "<tr><td>Airport<td>" + this.Airport;
        S += "<tr><td>Rail<td>" + this.Rail;
        S += "<tr><td>Bus<td>" + this.Bus;
        S += "<tr><td>Hospital<td>" + this.Hospital;
        S += "<tr><td>Shoping<td>" + this.Shoping;
        S += "<tr><td>PowerBack<td>" + (this.PowerBack != 0) ? "Yes" : "No";
        S += "<tr><td><td><span><a href='#' onclick='ShowMap(" + this.Lat + "," + this.Lng + ")'>Map</a></span> | <span><a  id='gal' href='#' onclick='ShowImageGalary(this," + ID + ")'>Image Galary<a></span>";//
    });
    S += "</tr></table>";
    $("#divSocietyDetail").html(S);
    return false;
}

function ShowMap(lat, lng)
{
    $.fancybox({
        'width': 744, 'height': 500, 'autoScale': false, 'scrolling': 'yes', 'transitionIn': 'elastic', 'transitionOut': 'none',
        'type': 'iframe',
        'href': "Map.htm?type=1&Lat=" + lat + "&Lng=" + lng
    });
}

function ShowAgentDetail(ID, AvlID)
{
    //$.ajax({ url: "Data.aspx?Action=GetAgentDetail&Data1=" + ID, cache: false, success:ShowDialog});
    $.ajax({ url: "Data.aspx?Action=GetAgentDetail&Data1=" + ID + "&Data2=1", cache: false, success: function (data) { $("#divAgentDetail").html(ShowError(data)[1]); } });
    return false;
}

function ShowPropertyDetail(ID)
{
    $.ajax({ url: "Data.aspx?Action=GetAvailabilityDetail&Data1=" + ID, cache: false, success: ShowDialog });
    return false;
}

function ShowDialog(data)
{
    $("#dialog").dialog('open');
    $("#dialog").dialog('option', 'title', "Society Details");
    $("#dialog").html(ShowError(data)[1]);
}

function GetAvlList(SellerID)
{
    $.ajax({
        url: "Data.aspx?Action=GetAvailibilityText&Data4=1&Data3=" + SellerID + "&Data6=" + sellerType, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);

            $(Lines).each(function ()
            {
                AvlList.push({ ID: this[0], SocietyID: this[1], SocietyName: this[2], SellerID: this[3], SellerName: this[4], BHK: this[5], Amount: this[6], AvailableFrom: this[7], Area: this[8], Bathroom: this[9], FloorNo: this[10], Facing: this[11], PostedOnDate: this[12] });
            });
            //console.log(AvlList);
            ShowAvlList(AvlList);
        }
    });
}

function ShowAvlList(Arr)
{
    var count = 1;
    var S = "<table style='border-spacing:1px;'><tr><th></th><th>Society</th><th>BHK</th><th>Rent</th><th>Avl From</th><th>Area</th><th>Bath</th><th>Floor</th><th>Featutres</th><th>PostedOn</th></tr>";
    $(Arr).each(function ()
    {
        if (this.ID == undefined)
            return;
        S += "<tr  onclick='ShowAvailabilityDetail(" + this.ID + "," + this.SellerID + ")'><td>" + count + "<td>" + this.SocietyName + "<td>" + this.BHK + "<td>" + this.Amount + "<td>" + this.AvailableFrom;
        S += "<td>" + this.Area + "<td>" + this.Bathroom + "<td>" + this.FloorNo + "<td>" + this.Facing + "<td>" + this.PostedOnDate;
        S += "<tr><td><td colspan='9'><div id='divAvl" + this.ID + "'></div>"

        count++;
    });
    S += "</table>";

    $("#divAvlList").html(S);
    $("#divAvlList table").HighLightRows();
    //$("#divAvlList table th").css("background-color", "#A9C7BC");
    $("#divAvlList table th").addClass("tablehead");
    GetClientList(SellerID);
}

function GetClientList(SellerID)
{
    //alert(SellerID);
    ClentList.length = 0;
    $.ajax({
        url: "Data.aspx?Action=GetClientList&Data1=" + SellerID, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);
            $(Lines).each(function ()
            {
                ClentList[this[0]] = ({ ID: this[0], Name: this[1], Mobile: this[2], Email: this[3] });
            });
            ShowClientList(ClentList);
            GetAvlClientList(SellerID);
        }
    });
}

function GetAvlClientList(SellerID)
{
    //alert(SellerID);
    AvlClientList.length = 0;
    $.ajax({
        url: "data.aspx?action=GetAvlClientList&Data5=" + SellerID, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);
            $(Lines).each(function ()
            {
                AvlClientList.push({ AvlID: this[0], ClientID: this[1] });
            });
            UpdateAvlList();
        }
    });
}

function UpdateAvlList()
{
    $(AvlClientList).each(function ()
    {
        var C = ClentList[this.ClientID];
        $("#divAvl" + this.AvlID).html($("#divAvl" + this.AvlID).html() + "<a href='#' style='margin-right:15px;' onclick='ShowClientDetail(" + this.ClientID + "," + this.AvlID + ")'>" + C.Name + "</a>");
    });
}

function ShowClientList(Arr)
{
    var S = "<table id='clientList' style='border-spacing:1px;'><tr><th>ID</th><th>Name</th></tr>";
    $(Arr).each(function ()
    {
        if (this.ID == undefined)
            return;
        S += "<tr onclick='ShowClientDetail(" + this.ID + ")'><td>" + this.ID + "<td>" + this.Name;
    });

    S += "</table>";
    $("#divClientList").html(S);
    $("#divClientList").HighLightRows();
    $("#divclientList table").css("min-width", "200px");
    $("#divClientList table th").addClass("tablehead");
}

function AssignClientToProperty()
{
    $.ajax({
        url: "Data.aspx?Action=AssignClientToProperty&Data1=" + avlID + "&Data2=" + clientID + "&Data3=" + (Connect ? 1 : 0), cache: false, success: function (data)
        {
            GetAvlList(SellerID);
        }
    });
    //ShowAvailabilityByClientID(ClientID,0);
}

function ShowAvailabilityDetail(ID, SellerID)
{
    avlID = ID;
    clientID = 0;
    $("#btnAsignAgent").hide();
    $("#frameEditAvailability")[0].src = "Edit/EditAvailability.aspx?ID=" + ID;
    GetLatLng(SellerID);

}

function GetLatLng(SocietyID)
{
    $.ajax({
        url: "Data.aspx?Action=GetLatLng&Data1=" + SocietyID, async: false, cache: false, success: function (data)
        {
            var Lines = ShowError2(data);
            $(Lines).each(function ()
            {
                var lat = Lines[0];
                var lat = Lines[1];
                //alert(lat + "-" + lng);
                $("#frameMap")[0].src = BasePath + "Map.htm?type=1&Lat=" + lat + "&Lng=" + lng;
            });
        }
    });
    //$("#frameMap")[0].src = "Map.htm?type=1";
}

function ShowAvailabilityByClientID(clntID, type)
{
    var AvlIds = new Array();
    var count = 1;
    var S = "<table id='clientList' style='border-spacing:1px; width:300px;text-indent:0px;'><tr><th></th><th>Society</th><th>BHK</th><th>Rent</th><th>Area</th><th>Bath</th><th>FloorNo</th></tr>";
    if (type == 0)//Agent Case
    {
        $(AvlClientList).each(function ()
        {
            if (this.ClientID == clntID)
            {
                AvlIds.push({ ID: this.AvlID });
            }
        }
        );

        $(AvlIds).each(function ()
        {
            //console.log(this.ID);
            var id = this.ID;
            $(AvlList).each(function ()
            {
                if (this.ID == id)
                {
                    S += "<tr onclick='ShowAvailabilityDetail(" + this.ID + ")'><td>" + count + "</td><td>" + this.SocietyName + "</td><td>" + this.BHK + "</td><td>" + this.Amount + "</td><td>" + this.Area + "</td><td>" + this.Bathroom + "</td><td>" + this.FloorNo + "</td>";
                    count++;
                }
            });
        });
    }
    else if (type == 1) //client Case
    {
        SellerID = 0;
        $.ajax({
            url: "Data.aspx?Action=GetAsignClient&Data1=" + ClientID, async: false, cache: false, success: function (data)
            {
                var Lines = ShowError2(data);

                $(Lines).each(function ()
                {
                    AvlIds.push({ ID: this[1] });
                });
                //console.log(AvlIds);

                var ClientAsignedList = new Array();
                $(AvlIds).each(function ()
                {
                    $.ajax({
                        url: "Data.aspx?Action=GetAvailabilityDetail&Data1=" + this.ID, async: false, cache: false, success: function (data)
                        {
                            var Lines = ShowError2(data);
                            $(Lines).each(function ()
                            {
                                ClientAsignedList.push({ ID: this[0], SocietyName: this[1], BHK: this[2], Amount: this[3], Area: this[4], Bathroom: this[5], FloorNo: this[6], SocietyID: this[7] });
                            });
                        }
                    });
                    //    //$.ajax({
                    //    //    url: "Data.aspx?Action=GetAvailibilityText&Data5=" + this.ID, async: false, cache: false, success: function (data)
                    //    //    {
                    //    //        alert(data);
                    //    //        var Lines = ShowError2(data);
                    //    //        ClientAsignedList.push({ ID: this[0], SocietyID: this[1], SocietyName: this[2] });
                    //    //    }
                    //    //})
                    //});
                    //console.log(ClientAsignedList);
                    //$(ClientAsignedList).each(function ()
                    //{
                });

                //console.log(ClientAsignedList);
                $(ClientAsignedList).each(function ()
                {
                    S += "<tr onclick='ShowAvailabilityDetail(" + this.ID + "," + this.SocietyID + ")'><td>" + count + "</td><td>" + this.SocietyName + "</td><td>" + this.BHK + "</td><td>" + this.Amount + "</td><td>" + this.Area + "</td><td>" + this.Bathroom + "</td><td>" + this.FloorNo + "</td>";;
                    count++;
                });

            }
        });

    }
    S += "</tr></table>";
    $("#divClientAssignedAvl").html(S);
    $("#divClientAssignedAvl").HighLightRows();
    $("#divClientAssignedAvl th").css("background-color", "#A9C7BC");
    $("#divClientAssignedAvl tr:hover").css("cursor", "pointer");

}

function GetClietIDByMobileNo(mobileno)
{
    $.ajax({
        url: "Data.aspx?Action=GetClietIDByMobileNo&Data1=" + mobileno, cache: false, success: function (data)
        {
            ClientID = ShowError2(data);
            ShowAvailabilityByClientID(ClientID, 1);
            $(".hd").show();
        }
    });
}


var Connect = false;
function ShowClientDetail(ClientID, avlid)
{
    ShowAvailabilityByClientID(ClientID, 0);
    clientID = ClientID;
    $("#btnAsignAgent").show();
    Connect = true;
    $("#btnAsignAgent").val("Asign To This Client");
    $("#frameClientDetail")[0].src = BasePath + "Edit/EditAgentClient.aspx?AgentID=510&ClientID=" + ClientID + "&AvlID=" + avlID;

    if (ClientID != 0 && avlid != undefined)
    {
        avlID = avlid;
        $("#btnAsignAgent").show();
        $("#btnAsignAgent").val('Remove');
        Connect = false;
    }
}

function UpdateBasePath()    //base path update
{
    $("img").each(function ()
    {
        if ($(this).attr("src"))
            if ($(this).attr("src").indexOf(BasePath) == -1 && $(this).attr("src").indexOf("http") == -1)
                $(this).attr("src", BasePath + $(this).attr("src"));
    });

    $("a").each(function ()
    {
        if ($(this).attr("href"))
            if ($(this).attr("href").indexOf(BasePath) == -1 && $(this).attr("href").indexOf("http") != 0 && $(this).attr("href").indexOf("#") != 0)
            {
                if ($(this).data("nohrefupdate") == undefined)
                {
                    $(this).attr("href", BasePath + $(this).attr("href"));
                }
            }
    });
}

function ShowLanMark(Lat, Lng)
{
    alert(Lat + "|" + Lng);
    $("#frameMap")[0].contentWindow.ShowMarker(Lat, Lng);
}

function openSocietyEdit(SID)
{
    $("#EditDialog").dialog({ width: 350, height: 900, autoOpen: false, show: "fade", hide: "fade" });
    $("#EditDialog").dialog('open');
    $("#editframe").attr('src', 'Edit/EditSociety.aspx?ID=' + SID);
    $("#editframe").css({ width: $("#EditDialog").outerWidth() - 50, height: $("#EditDialog").outerHeight() - 20 });

}
function openFloorPlanEdit(SID)
{

    $("#EditDialog").dialog({ width: 450, height: 900, autoOpen: false, show: "fade", hide: "fade" });
    $("#EditDialog").dialog('open');
    $("#editframe").attr('src', 'Edit/EditApartmentType.aspx?SocietyID=' + SID);
    $("#editframe").css({ width: $("#EditDialog").outerWidth() - 30, height: $("#EditDialog").outerHeight() - 20 });

    //$("#EditApatTypeFrame").height($("#EditApatTypeFrame").contents().find("html").height());
}

function FindAddress(address)
{
    $("#frameMap2")[0].contentWindow.FindAddress(address);
}

function SetCenter(Lat, Lng)
{
    $("#frameMap2")[0].contentWindow.SetCenter(Lat, Lng);
}

function UpdateLatLng(Lat, Lng)
{
    $("#frameEditSociety")[0].contentWindow.UpdateLatLng(Lat, Lng);
}

function ShowMap(QueryString)
{
    $("#frameMap2")[0].src = BasePath + "Map.htm?" + QueryString;
    $("#DivDialog").dialog({ width: 800, height: 600, autoOpen: false, show: "fade", hide: "fade" });
    $("#DivDialog").dialog('open');
}

//---------------------------------------Project Page-----------------------------------------------------------------

function AddToFav(AptTypeID, BHK, SuperArea)
{
    var hidval = $("#hidField").val();
    var SocID = hidval.split("-")[0];
    var SocName = hidval.split("-")[1];
    AddToMyList(SocID, SocName, AptTypeID, BHK, SuperArea);   //push history to array
    ShowHistory(); //call show history after each push
};

//function ShowHistory()
//{
//    LoadPropertyList();
//    var str = ["<table id='tableHistory'><tr><th class='tableHeader' colspan='4'>Selected"];
//    str[str.length] = "<tr style='font-weight:bold;'><td style='padding-left:3px;'>Society<td>BHK<td>Area(sqft)<td>"
//    var AptId = "";
//    $(SelectedProperty).each(function (index)
//    {
//        //console.log(SelectedProperty);

//        str[str.length] = "<tr><td title='Click to view project details'><a class='historybox' href='" + BasePath + "Project/" + this.SocietyID + "' onclick=''>" + this.ProjectName + "</a><td>" + this.BHK + "<td>" + this.SupArea + "<td> <a title='Click to delete' href='#' onclick='RemoveFromHistory(" + this.SocietyID + "," + this.ApartmentID + ")'>&nbsp;X&nbsp;</a>";
//        AptId += this.ApartmentID + ","
//    });
//    str[str.length] = "<tr><td><a id='comparelink' class='historybox' title='Click Compare' href='" + BasePath + "ApartmentCompare.aspx?ID=" + AptId + "'>Compare</a>";
//    $("#lblHistory").html(str.join(''));
//}
//function RemoveFromHistory(SocietyID, AptID)
//{
//    //alert(SocietyID + "-" + AptID);
//    DeleteList(SocietyID, AptID);
//    ShowHistory();
//}
//function GoToCompare(ID, AptID)
//{
//    console.log(SelectedProperty);
//    var str = "";
//    $(SelectedProperty).each(function ()
//    {
//        str += this.SocietyID + "^" + this.ApartmentID + "~";
//    });
//    if (str == "")
//        return;
//    $.post("../Data.aspx?Action=PostHistory", { History: str }, function (data)
//    {
//        console.log(data);
//    });
//} 
//---------------------------------------------------------------------------



