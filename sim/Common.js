var 
CMD_NONE = 0,
CMD_ADD_AT_START = 1,
CMD_ADD_AT_END = 2,
CMD_DELETE = 3,
CMD_INSERT = 4,
CMD_MOVE = 5,
CMD_AREA_SELECT = 6,
CMD_OBJECT_SELECT = 7,
CMD_OBJECT_MOVE = 8,
CMD_MOVE_NAME = 9,
CMD_PAN = 10;

var 
ALIGN_LEFT=1,
ALIGN_RIGHT=2,
ALIGN_TOP=3,
ALIGN_BOTTOM=4;


var MouseDown, 
MouseUp, 
MouseCurrent, 
SelectedPoints=new Array(),
SelectedObject=null,
Points = null,
InsertPoints = new Array(),
MouseLeftDown=false,
SelectedIndex = -1,
Command = -1,
Stn = null,
ZoomValue=1,
transform = new Transform(),
OffSetX = 0.0,
OffSetY = 0.0;

function Object(Name, Type, FillColor,FillOpacity, StrokeColor,StrokeWidth)
{
    this.Name = Name;
    this.Type = Type
    this.StrokeColor = StrokeColor ? StrokeColor : "black";
    this.StrokeWidth = StrokeWidth?StrokeWidth:1;
    this.FillOpacity= FillOpacity?FillOpacity:1;
    this.FillColor = FillColor ? FillColor : "white";
    this.Points=new Array();
}

function CopyObject(obj,OffSetX,OffSetY)
{
	var NewObj=new Object(obj.Name, obj.Type, obj.FillColor,obj.FillOpacity, obj.StrokeColor,obj.StrokeWidth);
	$(obj.Points).each(function()
	{
		NewObj.Points.push({X:this.X+OffSetX,Y:this.Y+OffSetY});
	});
	
	return 	NewObj;
}

function Station()
{   this.Name = "";
    this.Lat = 0;
    this.Lng = 0;
    this.Platforms = 10;
    this.Gate1Name = "";
    this.Gate2Name = "";
    this.ObjectList = new Array();
}

Station.prototype.Save = function ()
{
    for (var propertyName in this)
    {
        if (this[propertyName] != null)
        {
            if ((typeof (this[propertyName])).toString() != "function")
            {
                SaveInLocalStorage(propertyName, JSON.stringify(this[propertyName]));
            }
        }
    }

}

Station.prototype.Load = function ()
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

function DrawLine(context, X1, Y1, X2, Y2, StrokeStyle)
{
    context.beginPath();

    context.moveTo(X1, Y1);
    context.lineTo(X2, Y2);
    if (StrokeStyle) context.strokeStyle = StrokeStyle;
    context.stroke();
}

function DrawPolygon(context, Arr, FillColor, FillOpacity, StrokeStyle,StrokeWidth)
{
    context.beginPath();

    if (StrokeStyle) 
		context.strokeStyle = StrokeStyle;


	if(StrokeWidth)
		context.lineWidth=StrokeWidth;
		
    $(Arr).each(function (index)
    {
        if(index==0)
            context.moveTo(this.X, this.Y);
        else
            context.lineTo(this.X, this.Y);
    });
    
    context.closePath();
        
	if(FillOpacity)
	{
		context.globalAlpha =FillOpacity;
	}
		
    if (FillColor)
    {
        context.fillStyle = FillColor;
        context.fill();
    }
	
	context.globalAlpha =1;
}


function DrawRect2(context, X1, Y1, X2, Y2, FillColor, StrokeStyle)
{
    context.beginPath();

    context.rect(X1,Y1, X2-X1, Y2-Y1);
    if (FillColor)
    {
        context.fillStyle = FillColor;
        context.fill();
    }
    if (StrokeStyle) context.strokeStyle = StrokeStyle;
    context.stroke();
}

function DrawRect(context, X, Y, W, H, FillColor, StrokeStyle)
{
    context.beginPath();

    context.rect(X - W * .5, Y - H * .5, W, H);
    if (FillColor)
    {
        context.fillStyle = FillColor;
        context.fill();
    }
    if (StrokeStyle) context.strokeStyle = StrokeStyle;
    context.stroke();
}


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
    //console.log(Key + "-" + valoutput);

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