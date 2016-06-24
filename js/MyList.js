var MyList = new Array();

function History(CityFromID,CityFromName, CityToID,CityToName)
{
    this.CityFromID=CityFromID;
    this.CityFromName=CityFromName;
    this.CityToID=CityToID;
    this.CityToName=CityToName;
}

function ShowMyList()
{
//#555
    
    var strCookie = "";
    var str = "<tr><td style='color:White; background-color:#8DC3E9; text-align:center; border-bottom: 1px solid #4C88BE;' colspan='3'><b>Search History</b>";
    for (var i = 0; i < MyList.length; i++)
    {
        var H = MyList[i];
        str += "<tr><td><a href='#' onclick='return ShowResultForHistory(" + H.CityFromID + ",\"" + H.CityFromName + "\"," + H.CityToID + ",\"" + H.CityToName + "\");'>" + H.CityFromName + " <span style='font-size:16px;color:#FF0000'> » </span> " + H.CityToName + "</a>";
        str += "<td style='width:9px'><a href='#' onclick='RemoveItemFromMyList(" + i + ");return false;'>X</a><tr>";
        strCookie += H.CityFromID + "^" + H.CityFromName + "^" + H.CityToID + "^" + H.CityToName + "~";
    }

    $("#divHistory").html("<table id='tableMyList' cellpadding='0' cellspacing='0' style='width:100%'>" + str + "</table>");
    $.cookie("MyList", strCookie, { expires: 1000 });
    //BuildMyList();
}

function BuildMyList()
{
    var strCookie = "";
    if ($.cookie("MyList") != null) strCookie = $.cookie("MyList"); //Read cookie;
    if (strCookie == "")
        return;

    var Lines = strCookie.split('~');
    for (var i = 0; i < Lines.length; i++)
    {
      
        if (Lines[i] == "")
            continue;
        var F = Lines[i].split('^');
        //alert(F[0]);
        MyList.push(new History(F[0], F[1], F[2], F[3]));
    }

    ShowMyList();
}

function RemoveItemFromMyList(Index)
{
    MyList.splice(Index, 1);
    ShowMyList();
    
}

function AddToMyList(CityFromID, CityFromName, CityToID, CityToName)
{   
    // check if the item already exists in my list
    for (var i = 0; i < MyList.length; i++)
    {
        if (MyList[i].CityFromID == CityFromID && MyList[i].CityToID == CityToID)
            return;
    };

    MyList.push(new History(CityFromID, CityFromName, CityToID, CityToName));
   
}