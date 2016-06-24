//----------------zebra lining---------------------------
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
//-------------------------------------------//

function ShowError(data)
{
    var Lines = data.split('~');
    if (Lines[0] != "")
        alert(Lines[0]);
   return Lines;
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

//-----------------------------//