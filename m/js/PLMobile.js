/// <reference path="PLMobile.js" />
/// <reference path="jquery-1.8.0.js" />
/// <reference path="Common.js" />
/// <reference path="jquery.mobile-1.4.0.js" />

var BaseURL = "", changeTo = "";
var isChangePage = false;
function getBaseURL()
{
    return location.protocol + "//" + location.hostname + "/";
}

//(function (window, $, PhotoSwipe)
//{
//    $(document).ready(function ()
//    {
//        $('div#Details')
//            .bind('pageshow', function (e)
//            {
//                var
//                    currentPage = $(e.target),
//                    options = {},
//                    photoSwipeInstance = $("#Gallery a", e.target).photoSwipe(options, currentPage.attr('id'));
//                //console.log("Current Page:"+currentPage.attr('id'));
//                return true;
//            })
//            .bind('pagehide', function (e)
//            {
//                var
//                    currentPage = $(e.target),
//                    photoSwipeInstance = PhotoSwipe.getInstance(currentPage.attr('id'));

//                if (typeof photoSwipeInstance != "undefined" && photoSwipeInstance != null)
//                {
//                    PhotoSwipe.detatch(photoSwipeInstance);
//                }
//                return true;
//            });
//    });
//}(window, window.jQuery, window.Code.PhotoSwipe));

//$(document).ready(function ()
//{
//    $("a[data-icon^='home']").bind('click', function () { location.href = BasePath;});
//});

$(function ()
{
    //debugger;
    if (isChangePage && changeTo != "")
    {
        $.mobile.changePage(changeTo);
    }
});

$(document).bind("mobileinit", function ()
{
    $.support.cors = true;
    $.mobile.allowCrossDomainPages = true;
    $.mobile.ajaxEnabled = true;
});

$(document).bind('pageinit', function (event, data)
{
    BaseURL = getBaseURL();
    P.Load();
    $.mobile.defaultPageTransition = "none";
});

function Property()
{
    this.SelectedUserType = 1;
    this.CurrentCommand = 1;

    this.SelectedCityID = 0;
    this.SelectedBHK = 0;
    this.SelectedBudget = 0;

    this.ProjectID = 0;
    this.ProjectUrlName = "";
    this.BuilderID = 0;

    this.SelectedSociety = null;
    this.SelectedAvailabity = null;
    this.SelectedSeller = null;

    this.MinRent = 0;
    this.MaxRent = 9999999;

    this.SocietyList = new Array();
    this.AvailabilityList = new Array();
    this.FavoriteList = new Array();
    this.MyPostList = new Array();
    this.FilteredAvlList = new Array();

    this.lat = 0;
    this.lng = 0;
}
var P = new Property();

Property.prototype.Save = function ()     //save all properties in local storage
{
    for (var propertyName in this)
    {
        if (this[propertyName] != null)
        {
            if ((typeof (this[propertyName])).toString() != "function")
            {
                //console.log("TypeOF -" + propertyName + "-" + typeof (this[propertyName]));
                SaveInLocalStorage(propertyName, JSON.stringify(this[propertyName]));
            }
        }
    }
}

Property.prototype.Load = function ()    //load all properties from local storage 
{
    try
    {
        for (var propertyName in this)
        {
            var data = LoadFromLocalStorage(propertyName);

            if (data != "undefined" && data != "null" && data != null && data != undefined)
            {
                if ((typeof (this[propertyName])).toString() != "function")
                    this[propertyName] = JSON.parse(data);
            }
        }
    }
    catch (e)
    {
        ClearLocalStoreage();
    }
}

var activePageId;
$(document).bind('pagechange', function (event, data)
{
    var toPageId = data.toPage.attr("id");
    activePageId = $.mobile.activePage.attr("id");

    switch (toPageId)
    {
        case "Home":  ShowHome(); break;
        case "Rent": LoadSocietyList(); break;
        case "SocietyList": ShowPropertyListBySociety(); break;
        case "SocietyDetail": ShowSocietyDetail(); break;
        case "Availability": LoadSocietyAvailability(); break;
        case "PropertyDetail": ShowAvailabilityDetail(); break;
            //case "Favorite": ShowFavorite(); break;
        case "Post": ShowPostForm(); break;
        case "Projects":
            if ($("#selectCity option:selected").val() == 0)
                GetCurrentLocation(ShowProjectList);
            else
            {
                P.lat = 0, P.lng = 0;
                ShowProjectList();
            }
            break;
        case "Details":
            {
                if (!isChangePage)
                   ShowProjectDetail(); break;
            }
        case "BuilderDetails":
            {
                if (!isChangePage)
                ShowBuilderDetail(); break;
            }
    }
});

var CMD_FindProperty = 0,
    CMD_PostProperty = 1,
    CMD_PropertyList = 2;
    CMD_FindProjects = 3;
    CMD_FindProjectDetil = 4;
    CMD_FindBuilderDetail = 5;

function RunCommand(ID)
{
    if (ID)
        P.CurrentCommand = ID;
    P.Save();

    switch (ID)
    {
        case CMD_FindProperty:
            P.SelectedCityID = $("#selectCity").val();
            P.SelectedBHK = $("#selectBHK").val();
            P.Save();
            $.mobile.changePage("#Rent");
            break;
        case CMD_PostProperty: $.mobile.changePage("#Post"); break;
        case CMD_PropertyList: $.mobile.changePage("#SocietyList"); break;
        case CMD_FindProjects:
            P.SelectedCityID = $("#selectCity").val();
            P.SelectedBHK = $("#selectBHK").val();
            P.SelectedBudget = $("#SelectBudget").val();
            P.Save();
            $.mobile.changePage("#Projects");
            break;
        case CMD_FindProjectDetil:
            P.ProjectID = $("#ProjectID").attr('data-ProjectID');
            P.Save();
            $.mobile.changePage("#Details");
            break;
        case CMD_FindBuilderDetail:
            P.BuilderID = $("#BuilderID").attr('data-BuilderID');
            P.Save();
            $.mobile.changePage("#BuilderDetails");
            break;
    }
    return false;
}

function Seller(SellerID, Name, Company, Mobile1, Mobile2, Address)
{
    this.SellerID = SellerID;
    this.Name = Name;
    this.Company = Company;
    this.Mobile1 = Mobile1;
    this.Mobile2 = Mobile2;
    this.Address = Address;
}

function Society()
{
    this.dist1 = 0;
    this.dist2 = 0;
}
var S = new Society();

//function SetProjectId(projectID)
//{
//    isChangePage = false;
//    P.ProjectID = projectID;
//    P.Save();
//    RunCommand(CMD_FindProjectDetil);
//}

function ShowProjectList()
{
    ShowProcessingMsg(true);
    $.ajax({
        url: "/Data.aspx?Action=ProjectListMobile&Data1=" + P.SelectedCityID + "&Data2=" + P.SelectedBHK + "&Data3=" + P.SelectedBudget + "&Data4=" + P.lat + "&Data5=" + P.lng, cache: false, success: function (data)
        {
            $("#divProjectList").html(data);
            if (data == "")
                $("#divProjectList").html("<div>No project found in your criterion!</div><div>please go to home and try a different search!</div>");
            $("#divProjectList").trigger("create");
        }
    });
    ShowProcessingMsg(false);
}

function ShowBuilderDetail()
{
    isChangePage = false;
    ShowProcessingMsg(true);
    $.ajax({
        url: "/Data.aspx?Action=BuilderDetailMobile&Data1=" + P.BuilderID + "", cache: false, success: function (data)
        {
            $("#divBuilderDetails").html(data); ////change main title
            $(document).attr('title', $("#buil_Name").html());  //change main title
            $("#BuilderDetails div h1").html($("#buil_Name").html());
            $("#divBuilderDetails").trigger("create");
        }
    });
    ShowProcessingMsg(false);
}

function ShowProjectDetail()
{
    ShowProcessingMsg(true);
    //$.ajax({
    //    url: "/Data.aspx?Action=ProjectDetailMobile&Data1=" + P.ProjectID, cache: false, success: function (data)
    //    {
    //        $("#ProjectDetails").html(data);

    //        if (FoundInFavorite(P.ProjectID))
    //        {
    //            ChangeFavButtonDisplay(false);
    //        }
    //        else
    //        {
    //            ChangeFavButtonDisplay(true);
    //        }

    //        $(document).attr('title', $("#Soc_Name").html());  //change main title
    //        $("#Details div h1").html($("#Soc_Name").html());  //change title client side
    //        $("#ProjectDetails").trigger('create');
    //        //$("#Gallery a.Swipe").photoSwipe();
    //   }       
    //});
    ShowProcessingMsg(false);
}

function GetCurrentLocation(callback)          //Get geolocation
{

    if (navigator && navigator.geolocation)
        navigator.geolocation.getCurrentPosition(function (pos) { geo_success(pos, callback); }, function (err) { geo_error(err); });
    else
        error('Geolocation is not supported.');
}

function geo_success(position, callback)
{
    console.log("position", position);
    P.lat = position.coords.latitude;
    P.lng = position.coords.longitude;

    if (callback != undefined)
        callback();
}

function geo_error(err)
{
    console.log("err", err);
    P.lat = 0;
    P.lng = 0;
    showError(err)
}

function showError(error)
{
    switch (error.code)
    {
        case error.PERMISSION_DENIED:
            alert('User denied the request for Geolocation.');
            break;
        case error.POSITION_UNAVAILABLE:
            alert('Location information is unavailable.');
            break;
        case error.TIMEOUT:
            alert('The request to get user location timed out.');
            break;
        case error.UNKNOWN_ERROR:
            alert('An unknown error occurred.');
            break;
    }
}

function UpdateLabel(obj)
{
    var lbl = $(obj).data("label");

    $("option", obj).each(function ()
    {
        $(this).text($(this).text().replace(lbl, ""));
    });

    var txt = $("option:selected", obj).text();
    $("option:selected", obj).text(lbl + txt);
    try
    {
        $(obj).selectmenu("refresh");
    } catch (e) { }

}

function BindLabel()
{
    $("select[data-label]").each(function () { UpdateLabel(this) });

    $("select[data-label]").bind('change',
    function ()
    {
        UpdateLabel(this);
    });
}

function AddFavouriteProjects(ID, ProjName, UrlName, City, Subcity)
{
    //console.log(ID + "-" + ProjName + "-" + City + "-" + Subcity + "-" + UrlName);
    var text = $("#btnFav").text();
    if (text == 'Remove Favorite')
    {
        $(P.FavoriteList).each(function (index)
        {
            if (this.ID == ID)
                Found = index;
        })
        P.FavoriteList.splice(Found, 1);
        ChangeFavButtonDisplay(true);
        $("#btnFav").trigger('create');
    }
    else
    {
        var Found = FoundInFavorite(ID);
        if (!Found)
            P.FavoriteList.push(
                {
                    ID: ID,
                    ProjName: ProjName,
                    City: City,
                    Subcity: Subcity,
                    UrlName: UrlName
                });
        ChangeFavButtonDisplay(false);
    }
    P.Save();
}

function ChangeFavButtonDisplay(Add)
{
    if (Add)
    {
        $("#btnFav").text("Add To Fav");
        $('#btnFav').removeClass('ui-btn-b');
        $('#btnFav').addClass('ui-btn');
    }
    else
    {
        $("#btnFav").text("Remove Favorite");
        $('#btnFav').addClass('ui-btn-b');
    }
}

function FoundInFavorite(ID)       // check if the selected property exists in the favorite list
{
    var Found = false;
    $(P.FavoriteList).each(function ()
    {
        if (this.ID == ID)
            Found = true;
    });
    return Found;
}

//function UpdateFavorite(Remove)
//{
//    var Found = -1;
//    $(P.FavoriteList).each(function (index)
//    {
//        if (this.ID == P.ProjectID)
//            Found = index;
//    })
//    if (Remove)
//        P.FavoriteList.splice(Found, 1);
//    else if (Found == -1)
//        P.FavoriteList.push();
//    P.Save();
//}

//function UpdateFavorite(Remove)
//{
//    if (!P.SelectedAvailabity)
//        return;
//    var Found = -1;
//    $(P.FavoriteList).each(function (index)
//    {
//        if (this.ID == P.SelectedAvailabity.ID)
//            Found = index;
//    })
//    if (Remove)
//        P.FavoriteList.splice(Found, 1);
//    else if (Found == -1)
//        P.FavoriteList.push(P.SelectedAvailabity);
//    P.Save();
//    $('#divPropertyOptions').popup('close');
//}

function UpadatePostList(arr)
{
    P.MyPostList.push(arr);
    P.Save();
}

function UpdateUserOptions()
{
    var V = $("#SelectUserType").val();
    P.SelectedUserType = V;
    P.Save();

    $("#UserAction1").hide();
    $("#UserAction2").hide();
    $('#selectBHK').parent().hide();

    switch (parseInt(V))
    {
        case 1:
            $("#UserAction1").show();
            $('#selectBHK').parent().show();
            ShowHome();
            break;
        case 2:
            $("#UserAction2").show();
            ShowHome();
            break;
        case 3:
            $("#UserAction2").show();
            ShowHome();
            break;
        case 4: break;
        case 5: break;
    }
}

function ShowHome()
{
    
    $("#SelectUserType").val(P.SelectedUserType);
    $("#selectCity").val(P.SelectedCityID);
    $("#selectBHK").val(P.SelectedBHK);
    $("#SelectBudget").val(P.SelectedBudget);

    //if (P.SelectedUserType == 1)
    //{
    //    $("#divFavoriteList").html("<br/><ul data-role='listview' ><li data-role='list-divider' >Favorite"
    //        + GetAvailabilityList(P.FavoriteList)
    //        + "</ul>");
    //}
    //else if (P.SelectedUserType == 2 || P.SelectedUserType == 3)
    //{
    //    $("#divFavoriteList").html("<br/><ul data-role='listview' ><li data-role='list-divider'>MyPosts :"
    //      //+ GetMyPostList()
    //     + GetAvailabilityList($.makeArray(P.MyPostList))
    //     + "</ul>");
    //}

    var favlist = GetFavProjectList(P.FavoriteList);  //FavList of Projects
    if (favlist != "")
    {
        $("#divFavoriteList").html("<br/><ul data-role='listview' ><li data-role='list-divider'>My List<a onclick='ClearList()' style='float:right;'>Clear</a>"
         + favlist + "</ul>");
        $("#divFavoriteList").trigger("create");
    }
    BindLabel();
}

function GetFavProjectList(arr)
{
    debugger;
    var html = "";
    $(arr).each(function (index)
    {
        html += "<li><a href='/" + this.UrlName.toLowerCase() + "' data-ajax='false'>" + this.ProjName + ", <span style='font-size:.7em'>" + this.City + "</span></a></li>";
    });
    return html;
}

function GetMyPostList()
{
    //var html = "";
    //var count = 1;
    //var arr = $.makeArray(P.MyPostList);
    //$(arr).each(function (index)
    //{
    //    var b = arr[index];
    //    if ((P.SelectedUserType == 2 ? 0 : 1) == parseInt(b[8]))
    //        return;
    //    html += "<li data-mini='true' data-icon='false'><a>";
    //    html += b[1] + " (" + b[2] + ")";
    //    html+="<br/>["+b[9]+"]";
    //    html += "<br/><span class='rent'>&nbsp;" + b[3] + "&nbsp;</span> <span class='bed'>&nbsp; " + b[5] + "&nbsp;</span> <span class='bath'>&nbsp;" + b[6] + "&nbsp;</span> <span class='area'>&nbsp;" + b[4];
    //    html += "&nbsp;</span></a></li>";
    //    count++;
    //});
    //html += "<li><span class='rent'>&nbsp;Rent&nbsp;</span>&nbsp;<span class='bed'>&nbsp;Bedrooms&nbsp;</span> &nbsp;<span class='bath'>&nbsp;Bathrooms&nbsp;</span> &nbsp;<span class='area'>&nbsp;Area&nbsp;</span></li>"
    //return html;
}

function ShowPostForm()
{
    BindLabel();
    FillSociety();
}

function ShowConfirm()
{
    if (FavoriteList.length != 0)
        ShowPopup("#sure");
}

//----Show Socity List with available units bubble count----------//

function sortArray(arr)
{
    for (var x = 0; x < arr.length; x++)
    {
        for (var y = 0; y < arr.length - 1 ; y++)
        {
            if (arr[y].Lat != null || arr[y].Lat != undefined)
            {
                var dist1 = getDistance(P.lat, arr[y].Lat, P.lng, arr[y].Lng);

                var dist2 = getDistance(P.lat, arr[y + 1].Lat, P.lng, arr[y + 1].Lng);

                if (dist1 > dist2)
                {

                    var temp = arr[y + 1];
                    arr[y + 1] = arr[y];
                    arr[y] = temp;
                }
            }
        }
    }
    return arr;
}

function LoadSocietyList()
{
    ShowProcessingMsg(true);
    $("#divSociety").html("Loading...");
    GetSocietyList(function ()     //call GetSocietyList()
    {
        if (P.SelectedCityID == 0)
            sortArray(P.SocietyList);

        $("#RentHeader").html(P.SelectedBHK + " BHK");
        var str = "<ul data-role='listview'>";
        var strlenth = str.length;

        $(P.SocietyList).each(function (index)
        {
            var dist;

            if (this.Lat != null && this.Lng != null)
            {
                dist = getDistance(P.lat, this.Lat, P.lng, this.Lng);
            }

            var Avl = 0;
            if (P.SelectedBHK == 1) Avl = this.ForSale1BHK;
            else if (P.SelectedBHK == 2) Avl = this.ForSale2BHK;
            else if (P.SelectedBHK == 3) Avl = this.ForSale3BHK;
            else if (P.SelectedBHK == 4) Avl = this.ForSale4BHK;
            else if (P.SelectedBHK == 5) Avl = this.ForSale5BHK;

            if (Avl != 0)
            {
                if (P.SelectedCityID == 0 && dist < 100)
                {
                    str += "<li data-mini='true' data-icon='false'><a href='#' onclick = 'ShowPropertyListBySociety(" + index + ")' >" + this.SocietyName + " <span class='ui-li-count'>" + Avl + "</span><span style='color:orange;' class='ui-li-aside'>&nbsp;" + Math.round(dist) + "km</span></a></li>";
                }
                else if (P.SelectedCityID != 0)
                    str += "<li data-mini='true' data-icon='false'><a href='#' onclick = 'ShowPropertyListBySociety(" + index + ")' >" + this.SocietyName + " <span class='ui-li-count'>" + Avl + "</span></a></li>";
            }
        });

        $("#divSociety").html(str + "</ul>");
        $("#divSociety").trigger("create");

        ShowProcessingMsg(false);
    });
}

function getDistance(lat1, lat2, lon1, lon2)
{
    var R = 6371; // km
    //var R = 6378.7;

    var rad = Math.PI / 180.0;
    var lat_1 = lat1 * rad;
    var lng_1 = lon1 * rad;
    var lat_2 = lat2 * rad;
    var lng_2 = lon2 * rad;

    var d = Math.acos(Math.sin(lat_1) * Math.sin(lat_2) +
          Math.cos(lat_1) * Math.cos(lat_2) *
          Math.cos(lng_2 - lng_1)) * R;
    return d;
};

function GetSocietyList(CallBack)
{
    $.ajax({
        url: "/Data.aspx?Action=GetSocietyList&Data1=0&Data2=mobile", cache: false, success: function (data)
        {
            var Data = data.replace('~', '');
            P.SocietyList = JSON.parse(Data);
            CallBack();
        }
    });
}

//-------show Society Detail  and listing of availaibilies in selected society---------------

function ShowPropertyListBySociety(index)
{
    if (index != undefined)
    {
        P.SelectedSociety = P.SocietyList[index];
        RunCommand(CMD_PropertyList);
        return;
    }

    ShowProcessingMsg(true);

    $("#divSocietyList").html("Loading...");

    GetSocietyList(function ()
    {
        //console.log(SelectedSociety);
        $("#SocietyListHeader").html(P.SelectedBHK + " BHK");

        $.ajax({
            url: "/Data.aspx?Action=GetSocietyAvailability&Data1=" + P.SelectedSociety.ID + "&Data2=" + P.SelectedBHK + "&Data4=mobile", cache: false, success: function (data)
            {
                P.AvailabilityList = JSON.parse(ShowError(data)[1]);
                var str = "<ul data-role='listview'><li data-mini='true'><a href='#' onclick = 'ShowSocietyDetail(" + P.SelectedSociety.ID + ")' style='color:orange;'>" + P.SelectedSociety.SocietyName + "</a></li>"
                str += GetAvailabilityList(P.AvailabilityList);
                str += "</ul>";
                $("#divSocietyList").html(str);
                $("#divSocietyList").trigger("create");
                ShowProcessingMsg(false);
            }
        });
    });
}

function FilterProperty()
{
    var SelectedVal = parseInt($('#selectRent').val());

    switch (SelectedVal)
    {
        case 1: P.MinRent = 0; P.MaxRent = 5000; break;
        case 2: P.MinRent = 5000; P.MaxRent = 10000; break;
        case 3: P.MinRent = 10000; P.MaxRent = 15000; break;
        case 4: P.MinRent = 15000; P.MaxRent = 20000; break;
        case 5: P.MinRent = 20000; P.MaxRent = 25000; break;
        case 6: P.MinRent = 25000; P.MaxRent = 999999; break;
        default:
            P.MinRent = 0; P.MaxRent = 999999; break;
    }
    P.Save();

    $('#PropertyFilter').popup('close');
    RunCommand(CMD_PropertyList);
}

function ClearList()
{
    P.FavoriteList.length = 0;
    P.Save();
    $("#divFavoriteList").html("");
    $("#divFavoriteList").trigger("create");
    $('#sure').popup('close');
}


function GetAvailabilityList(arr)
{
    var html = "";
    $(arr).each(function (index)
    {
        var Found = FoundInFavorite(this.ID);

        var ShowProperty = true;

        if (this.Amount < P.MinRent || this.Amount > P.MaxRent)
            ShowProperty = false;

        if (P.SelectedUserType == 1)
        {
            if (ShowProperty)
            {

                html += "<li data-icon='" + (Found ? "star" : "false")
                     + "' data-mini='true'><a href='#'  onclick='ShowPropertyOptions(" + this.ID + ")'>"
                     + this.SellerName + " (" + this.SellerMobile1 + ")<span>";

                if (activePageId == "home")
                    html += " [" + this.SocietyName + "]</span>"

                html += "</br><span class='rent'>&nbsp;" + this.Amount + "&nbsp;</span>&nbsp;<span class='bath'>&nbsp;"
                     + this.Bathroom + "&nbsp;</span>&nbsp;<span class='area'>&nbsp;"
                     + this.Area + "&nbsp;</span></a></li>";
            }
        }

        var b = arr[index];
        if ((P.SelectedUserType == 2 ? 0 : 1) == parseInt(b[8]))
            return;
        if (P.SelectedUserType == 2 || P.SelectedUserType == 3)
        {
            var b = arr[index];
            if ((P.SelectedUserType == 2 ? 0 : 1) == parseInt(b[8]))
                return;
            html += "<li data-mini='true' data-icon='false'><a>";
            html += b[1] + " (" + b[2] + ")";
            html += "<br/>[" + b[9] + "]";
            html += "<br/><span class='rent'>&nbsp;" + b[3] + "&nbsp;</span> <span class='bed'>&nbsp; " + b[5] + "&nbsp;</span> <span class='bath'>&nbsp;" + b[6] + "&nbsp;</span> <span class='area'>&nbsp;" + b[4];
            html += "</a></li>";
        }
    });

    html += "<li><span class='rent'>&nbsp;Rent&nbsp;</span>";
    if (P.SelectedUserType != 1)
        html += "&nbsp;<span class='bed'>&nbsp;Bedrooms&nbsp;</span>";
    html += "&nbsp;<span class='bath'>&nbsp;Bathrooms&nbsp;</span>";
    html += "&nbsp;<span class='area'>&nbsp;Area&nbsp;</span></li>";
    return html;
}

//-----------------------------------------------------------------------------------

function ShowPropertyOptions(ID)
{
    //console.log(P.AvailabilityList);
    P.SelectedAvailabity = null;

    $(P.AvailabilityList).each(function ()
    {
        if (this.ID == ID)
        {
            P.SelectedAvailabity = this;
            //console.log(SelectedAvailabity);
        }
    });

    if (!P.SelectedAvailabity)
    {
        $(P.FavoriteList).each(function ()
        {
            if (this.ID == ID)
                P.SelectedAvailabity = this;
        });
    }
    var Found = FoundInFavorite(P.SelectedAvailabity.ID);
    $("#divFavoriteButton").html("<a data-role='button' data-theme='c' data-mini='true' onclick='UpdateFavorite(" + Found + ")'>" + (Found ? "Remove" : "Add") + " Favorite </a>");
    $("#divFavoriteButton").trigger("create");
    ShowPopup("#divPropertyOptions");
    $("#divPropertyOptionDetails").html("");
    $("#PropertyOptionsCall").attr("href", "tel:" + P.SelectedAvailabity.SellerMobile1 + "");
    return false;
}

//-------------Show agent and Property Details in popup-----------------------

function GetAgentDetails()
{
    ShowProcessingMsg(true);
    $("#divPropertyOptionDetails").html("Loading...");
    $.ajax({
        url: "/Data.aspx?Action=GetAgentDetail&Data1=" + P.SelectedAvailabity.SellerID, cache: false, success: function (data)
        {
            var F = ShowError(data)[1].split('^');

            if (F.length > 4)
            {
                //SelectedSeller = new Seller(F[0], F[1], F[2], F[3], F[4], F[5]);
                $("#divPropertyOptionDetails").html(F[1] + "<br/>" + F[2] + "<br/><a class='phonelinks' href='tel:" + F[3] + "'>" + F[3] + "</a><br/><a class='phonelinks' href='tel:" + F[4] + "'>" + F[4] + "</a><br/>" + F[5]);
            }
            else
            {
                $("#divPropertyOptionDetails").html("Agent Details not found");
            }
            ShowProcessingMsg(false);
        }
    });
}

function ShowAvailabilityDetail()
{
    // Property Detail
    var A = P.SelectedAvailabity;
    var str = "Bedrooms:" + A.BHK;
    str += "<br/>Bathroom:" + A.Bathroom;
    str += "<br/>Balcony:" + A.Balcony;
    str += "<br/>Flooor No:" + A.FloorNo;
    str += "<br/>Total Floors:" + A.TotalFloors;
    str += "<br/>Facing:" + A.Facing;
    var datepostedon = new Date(parseInt(A.PostedOnDate.split('(')[1].split(')')[0])).toDateString();
    str += "<br/>Posted On:<span style='color:orange;'>" + FormatDate(datepostedon) + "</span>";
    $("#divPropertyOptionDetails").html(str);
    //$("#divPropertyOptionDetails").trigger("create");
}

function ShowSocietyDetail(ID)
{
    if (ID)
    {
        P.SelectedSocietyID = ID;
        P.Save();
        //$.mobile.changePage("#SocietyDetail");
        //return;
    }
    ShowProcessingMsg(true);
    $.ajax({
        url: "/Data.aspx?Action=GetSocietyDetail&Data1=" + P.SelectedSocietyID, cache: false, success:
        function (data)
        {
            $("#divSocietyDetails").html(ShowError(data)[1]);
            ShowPopup("#divSocietyDetail");
            ShowProcessingMsg(false);
        }
    });
}

//----------------------------------------------------------------------------
function ShowPopup(ID)
{
    $(ID).trigger("create");
    $(ID).popup();
    $(ID).popup('open');
}

function ClosePopup(ID)
{
    $(ID).popup('close');
}

function ShowMap()
{
    ShowProcessingMsg(true);
    $.mobile.changePage("#Map");
    var Lat = 25.486;
    var Lng = 81.67;

    $("#divmap").html("<iframe id='frameMap' src='Map.htm?Lat=" + Lat + "&Lng=" + Lng + "'></iframe>");
    ShowProcessingMsg(false);
}

function ShowProcessingMsg(show)
{
    if (show)
    {
        $.mobile.loading("show", {
            text: "Loading",
            textVisible: true,
            theme: "z",
            html: ""
        });
    }
    else
    {
        $.mobile.loading("hide");
    }
}

function FormatDate(dt)
{
    var day = dt.substring(0, 3);
    var month = dt.substring(4, 8);
    var date = dt.substring(8, 11);
    var year = dt.substring(13);
    return day + " " + date + "-" + month + "-" + year;
}

function FillSociety()
{
    GetSocietyList(function ()
    {
        $(P.SocietyList).each(function (index)
        {
            $('<option/>').val(this.ID).html(this.SocietyName).appendTo('#SocietySelect_Post');

        });

        $("#SocietySelect_Post")[0].selectedIndex = 0;
        $("#SocietySelect_Post").selectmenu("refresh");
        BindLabel();
    });
}

function PostAvailibility()
{
    console.log($("#SocietySelect_Post option:selected").text());
    var A = new Array($('#SocietySelect_Post').val(), $('#name_post').val(), $('#mobile_post').val(), $('#rent_post').val(), $('#area_post').val(), $('#bhk_post').val(), $('#bath_post').val(), "1", P.SelectedUserType == 2 ? "0" : "1", $("#SocietySelect_Post option:selected").text());
    $.post("http://localhost:4566/PropertyMap/Data.aspx?Action=UpdateAvailibility", { societyId: $('#SocietySelect_Post').val(), name: $('#name_post').val(), mobile: $('#mobile_post').val(), rent: $('#rent_post').val(), area: $('#area_post').val(), bhk: $('#bhk_post').val(), bath: $('#bath_post').val(), sellerId: "1", sellerType: P.SelectedUserType == 1 ? "0" : "1", availType: "0" }, function (data)
    {
        UpadatePostList(A);
        alert("Successfully Posted!");
    });
}


