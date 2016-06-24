var TabText=null ;
var TabLinks =null;

$(window).resize(Resized);

function Resized()
{
    var H = $(window).height() - 70;
    $("iframe").height(H);
}

function TabSelect(Index)
{
    //if ($("#tab-" + Index).html() == "")
    {
        $("#tab-" + Index).html("<iframe frameborder='0' style='width:100%;' src='" + TabLinks[Index] + "'></iframe>");
        Resized();
    }
}

$(document).ready(function ()
{
    var str1 = "<ul>", str2 = "";
    for (var i = 0; i < TabText.length; i++)
    {
        str1 += "<li><a href='#tab-" + i + "'>" + TabText[i] + "</a></li>";
        str2 += "<div id='tab-" + i + "'></div>";
    }
    str1 += "</ul>";

    $("#tabsProfile").html(str1 + str2);
    $("#tabsProfile").tabs();
    $('#tabsProfile').bind('tabsselect', function(event, ui) { TabSelect(ui.index); });
    TabSelect(0);
});
