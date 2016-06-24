CurrentPage = "Home";

$(document).ready(function ()
{
    var data = LoadFromLocalStorage("SelectedCity");   //Load dd selected value from Local storage

    if (data != null && $('#ddCity option:contains(' + data + ')').length && $("#ddCity option:selected").text() != data)
        location.href = BasePath + "city/" + data;

    $(window).scroll(function ()                     //scroll to top in page
    {
        if ($(this).scrollTop() > 100)
        {
            $('.scrollup').fadeIn();
        } else
        {
            $('.scrollup').fadeOut();
        }
    });

    $('.scrollup').click(function ()
    {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });

    $("#txtSearch").autocomplete({
        minLength: 2, select: function (event, ui)
        {
            window.location = ui.item.urlname;
        }, source: SendRequest
    });
});

var cache = {}, lastXhr;
function SendRequest(request, response)
{
    var term = request.term;
    if (term in cache)
    {
        response(cache[term]); return;
    }

    lastXhr = $.getJSON(BasePath + "Data.aspx?Action=AllSearch&Data1=0", request, function (data, status, xhr)
    {
        cache[term] = data;
        if (xhr === lastXhr)
        {
            //console.log(data);
            response(data);
        }
    });
}

function ShowProjects()
{
    SaveInLocalStorage("SelectedCity", $("#ddCity option:selected").text());   //save dd selected value in Local storage
    location.href = BasePath + "city/" + $("#ddCity option:selected").text().toLowerCase();
}

var FilterMinBudget = 0;
var FilterMaxBudget = 999999999;
var FilterBHK = 0;

function SelectBudget(Min, Max, obj)
{
    FilterMinBudget = Min;
    FilterMaxBudget = Max;
    $("#ltSelectedBudget").html("Budget " + $(obj).html());
    UpdateProjectList();
}

function SelectBHK(BHK, obj)
{
    FilterBHK = BHK;
    $("#ltSelectedBHK").html($(obj).html());
    UpdateProjectList();
}

function UpdateProjectList()
{
    obj = $("table.ProjectListHome");
    $(obj).find("tr:gt(1)").remove();

    var ctr = 0;
    $(ProjectList).each(function ()
    {
        var WithInBudget = false;
        var AptsList = [];

        $(this.Apts).each(function (Index)
        {
            if (Index > 4) // only till 4 bhk
                return;

            AptsList[Index] = [];
            $(this).each(function () // iterate each apartment in the same bhk
            {
                if (this.Price >= FilterMinBudget && this.Price <= FilterMaxBudget)
                {
                    AptsList[Index].push(this);
                    WithInBudget = true;
                }
            });
        });

        // apply BHK filter
        var BHKFound = FilterBHK == 0;

        $(AptsList).each(function (Index)
        {
            if (this.length > 0 && this[0].BHK == FilterBHK)
                BHKFound = true;
        });

        if (WithInBudget && BHKFound)
        {
            ctr++;
            var str = "<tr><td>" + ctr + "<td><div><a href='" + BasePath + "project/" + this.URLName + "'>" + this.Name + "</a>";
            $(AptsList).each(function (Index)
            {
                if (Index < 1)
                    return;

                if (this.length > 0)
                {
                    var area = this[0].Area;
                    var price = this[0].Price;

                    if (this.length > 1)
                    {
                        if (this[this.length - 1].Area != area)
                            area += "-" + this[this.length - 1].Area;

                        if (this[this.length - 1].Price != price)
                            price += "-" + this[this.length - 1].Price;

                        if (price == "0")
                            price = "-";
                    }

                    str += "<td class='area'>" + area + "<td class='price'>" + price;
                }
                else
                    str += "<td><td>";
            });

            str += "<td><div>" + this.SubCity + "</div>";

            $("tr:last", obj).after(str);
        }
    });

    $("#footer").css("bottom", "0");
    return false;
}

$(function ()
{
    $("#txtSearch").on("keyup", function (e)
    {
        var txt = $(this).val().toUpperCase().trim();

        $(".ProjectListHome").css("display", "none"); // hide table
        //$("#DataTable").removeHighlight();

        $(".ProjectListHome tr").not(".skp").each(function (index)
        {
            $(this).css("display", $(this).text().toUpperCase().indexOf(txt) > -1 || txt.length == 0 ? "" : "none");
        });
        $(".ProjectListHome").css("display", ""); // show table
    });
});

$.fn.removeHighlight = function ()
{
    return this.find(".highlight").each(function ()
    {
        this.parentNode.firstChild.nodeName;
        with (this.parentNode)
        {
            replaceChild(this.firstChild, this);
            normalize();
        }
    }).end();
};
