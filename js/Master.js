var CurrentPage = "home"; //set home select by default

$(document).ready(function ()
{
    //console.log(CurrentPage);
    $('ul.nav li.' + CurrentPage).addClass("active");            //Activate selected Tab for current page
    $('ul.nav li.' + CurrentPage + " i").removeClass("icon-inverse");
    $('ul.nav li.' + CurrentPage + " i").addClass("icon-white");

    $("#txtMainSearh").keyup(GetSearchedList);
});

function GetSearchedList()
{
    txtVal = $("#txtMainSearh").val();

    if (txtVal.length == 0)
    {
        $("#divSearchedResult").hide();
        return;
    }

    $.ajax({
        url: BasePath + "Data.aspx?Action=GlobalSearch&term=" + txtVal, success: function (Data)
        {
            SearchResult = JSON.parse(Data);
            $("#divSearchedResult").html(DisplaySearchedData());
            $("#divSearchedResult").show();
            $("#divSearchedResult").css({ 'top': 40, 'left': $("li.search").position().left() });
            $("#divSearchedResult").hover(function () { Is_Search_Box = true; }, function () { Is_Search_Box = false; });
        }
    });
}

function DisplaySearchedData()
{
    var str = "<table class='table table-bordered table-hover table-striped table-condensed'>";
    str += "<tr><th>Name<th>Area<th>City";

    $(SearchResult).each(function ()
    {
        switch (this.type)
        {
            case "C":
                str += "<tr><td><td><td><a href='" + BasePath + "city/" + this.label + "'>" + this.label + "</a>";
                break;

            case "P":
                var F = this.label.split(',');
                str += "<tr><td><a href='" + BasePath + "project/" + this.urlname + "'  onclick='' target='_blank'>" + F[0] + "</a><td>" + F[1] + "<td>" + F[2];
                break;

            case "B":
                str += "<tr><td><a href='" + BasePath + "builder/" + this.urlname + "'  onclick='' target='_blank'>" + this.label + "</a><td><td>";
                break;
        }
    });
    return str + "</table>";
}

$.fn.HighLightRows2 = function ()
{
    var thistable = this;
    ResetRowColor1(thistable);

    $("tr", this).hover(function () { $(this).css({ "background-color": "#17c8e1", 'color': 'white' }); }, function () { $(this).css({ "background-color": jQuery.data(this, "OldClr"), 'color': 'black' }); });

    $("tr", this).click(function ()
    {
        ResetRowColor1(thistable);

        jQuery.data(this, "OldClr", "#17c8e1");
        $(this).css("background-color", "#17c8e1");
    });

    if (HignLightFirstRow)
    {
        $(this).find("tr:eq(1)").trigger('click');
        HignLightFirstRow = false;
    }
};

function ResetRowColor1(table)
{
    $("tr", table).each(function (index)
    {
        var clr = index % 2 == 0 ? "white" : "#dee6fe";
        $(this).css("background-color", clr);
        jQuery.data(this, "OldClr", clr);
    });
}
