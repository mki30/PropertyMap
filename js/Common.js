var HignLightFirstRow;
$.fn.HighLightRows = function ()
{
    var thistable = this;
    ResetRowColor(thistable);

    if (thistable.attr('id') == "tableContact" || thistable.attr('id') == "tableCompany")
        $("tr", this).hover(function () { $(this).css("background-color", "#D5D5D5"); }, function () { $(this).css("background-color", jQuery.data(this, "OldClr")); });
    else
        $("tr:gt(0)", this).hover(function () { $(this).css("background-color", "rgba(4, 146, 248, 0.30)"); }, function () { $(this).css("background-color", jQuery.data(this, "OldClr")); });

    $("tr", this).click(function (index)
    {
        ResetRowColor(thistable);
        jQuery.data(this, "OldClr", "#D5D5D5");
        $(this).css("background-color", "#D5D5D5");
    });

    if (HignLightFirstRow)
    {
        $(this).find("tr:eq(1)").trigger('click');
        HignLightFirstRow = false;
    }
};

function ResetRowColor(table)
{
    $("tr", table).each(function (index)
    {
        if (table.attr('id') == "tableContact" || table.attr('id') == "tableCompany")
        {
            var clr = index % 2 == 0 ? "white" : "whiteSmoke";
            $(this).css("background-color", clr);
            jQuery.data(this, "OldClr", clr);
        }
        else
        {
            if (index != 0)
            {
                var clr = index % 2 == 0 ? "white" : "whiteSmoke";
                $(this).css("background-color", clr);
                jQuery.data(this, "OldClr", clr);
            }
        }
    });
}

function ShowError(data)         //remove error tiddle
{
    var Lines = data.split('~');
    if (Lines[0] != "")
        alert("Error-" + Lines[0]);
    return Lines;
}

function ShowError2(data)         //remove error tiddle
{
    var Lines = data.split('~');
    var DataLines = new Array();
    if (Lines[0] != "")
    {
        alert("Error-" + Lines[0]);
    }
    else
    {
        for (var i = 1; i < Lines.length; i++)
        {
            if (Lines[i] == "")
                continue;
            var F = Lines[i].split('^');
            DataLines.push(F);
        }
    }
    return DataLines;
}

function ListBoxFilter(txt, lst)        //List Box Filter
{
    var Search = $(txt).val().toLowerCase();

    $(lst + " option").each(function (index)
    {
        if ($(this).text().toLowerCase().indexOf(Search)==0)
        {
            $(lst).get(0).selectedIndex = index;
        }
    });
}

//---------Local Storage--------------------//

function SaveInLocalStorage(Key, val)
{
    if (typeof (localStorage) != 'undefined')
    {
        window.localStorage.removeItem(Key);
        window.localStorage.setItem(Key, val);
        return true;
    }
    else
    {
        alert("Your browser does not support local storage, please upgrade to latest browser");
        return false;
    }
}

function LoadFromLocalStorage(Key, DefaultValue)
{
    var valoutput;
    if (typeof (window.localStorage) != 'undefined')
    {
        valoutput = window.localStorage.getItem(Key);
    }
    else
    {
        throw "window.localStorage, not defined";
    }
    // console.log(Key + "-" + valoutput);

    if (DefaultValue && !valoutput)
        return DefaultValue;
    else
        return valoutput;
}

function RemoveFromLocalStorage(Key)
{
    window.localStorage.removeItem(Key);
}

function ClearLocalStoreage()
{
    if (typeof (window.localStorage) != 'undefined')
    {
        window.localStorage.clear();
    }
    else
    {
        throw "window.localStorage, not defined";
    }
}

function ReadClientScript()
{
    $.ajax({ url: BasePath + "Data.aspx?Action=GetClientScript", cache: false, success: function(Data)
    {
        $.globalEval(Data);
    }
    });
}

////-----------------------------//
//$.ui.autocomplete.prototype._renderItem = function (ul, item)      //Autocomplete text Highlight
//{
//    //var re = new RegExp($.trim(this.term.toLowerCase()), 'gi');
//    var re = new RegExp($.trim(this.term), 'gi');
//    var t = item.label.replace(re, "<span style='font-weight:600;color:#5C5C5C;'>" + $.trim(this.term.toLowerCase()) + "</span>");
//    return $("<li></li>")
//        .data("item.autocomplete", item)
//        .append("<a>" + t + "</a>")
//        .appendTo(ul);
//};
//-----------------------------//

var reEscape = new RegExp('(\\' + ['/', '.', '*', '+', '?', '|', '(', ')', '[', ']', '{', '}', '\\'].join('|\\') + ')', 'g');
$.ui.autocomplete.prototype._renderItem = function (ul, item)      //Autocomplete text Highlight
{
    var color = "#08c";
    var objType = "Project";
    
    switch (item.type)
    {
        case "P":
            color = "#08c";
            objType = "Project";
            break;
        case "B":
            color = "orange";
            objType = "Builder";
            break;
        case "":
            color = "voilet";
            objType = "City";
            break;
    }

    var pattern = '(' + this.term.replace(reEscape, '\\$1') + ')';
    var t = item.label.replace(new RegExp(pattern, 'gi'), '<span style="font-weight:bold;">$1</span>')+ "-<span style='color:"+color+"; font-style:italic;'>"+objType+"</span>";
    return $("<li></li>")
        .data("item.autocomplete", item)
        .append("<a>" + t + "</a>")
        .appendTo(ul);
};

