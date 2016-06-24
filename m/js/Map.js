function ReloadMap()
{
    $("#frameMap")[0].src = "../Map.htm";
}

function FindAddress(address)
{
    $("#frameMap")[0].contentWindow.codeAddress(address);
}

function SetCenter(Lat, Lng)
{
   
    if (Lat == "" || Lng == "")
        return;
    $("#frameMap")[0].contentWindow.SetCenter(Lat, Lng);
}

function LoadNetwork(MetroID)
{
  
    $("#frameMap")[0].contentWindow.LoadNetwork(MetroID);
}
// refresh Bus
function LoadBusRoute(BusID)
{
    $("#frameMap")[0].contentWindow.LoadBusRoute(BusID);
}

function ShowBusStop(StopID)
{
    $("#frameMap")[0].contentWindow.ShowBusStop(StopID);
}


function ZoomToRoute()
{
    $("#frameMap")[0].contentWindow.ZoomToRoute();
}


function GetPath(StnFromID, StnToID)
{
    $("#frameMap")[0].contentWindow.GetPath(StnFromID, StnToID);
}

function ConnectionStations(MetroID,StationFromID, StationToID,RouteID,CallBack)
{
    var URL = "../Data.aspx?Action=CONNECTSTATIONS&Data1="
            + MetroID
            + "&Data2=" + StationFromID
            + "&Data3=" + StationToID
            + "&Data4=" + RouteID;

    $.ajax({url:URL,
        success: function (data)
        {
            CallBack(data);
            $("#frameMap")[0].contentWindow.LoadNetwork(false);
        }
    });

}

function DisConnectionStations(StationFromID, StationToID, CallBack)
{
    $.ajax({
        url: "../Data.aspx?Action=DISCONNECTSTATIONS"
            + "&Data1=" + StationFromID
            + "&Data2=" + StationToID,
        success: function (data)
        {
            CallBack(data);
            $("#frameMap")[0].contentWindow.LoadNetwork(false);
        }
    });

}