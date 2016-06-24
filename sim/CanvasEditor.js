
/// <reference path="jquery-1.8.0.min.js" />
/// <reference path="kinetic-v4.0.1.js" />

/// <reference path="canvasmap.js" />

var
CMD_NONE = 0,
CMD_DELETE = 3,
CMD_INSERT = 4,
CMD_MOVE = 5,
CMD_AREA_SELECT = 6,
CMD_Shape_SELECT = 7,
CMD_Shape_MOVE = 8,
CMD_MOVE_NAME = 9,
CMD_Shape_ADD = 11,

ALIGN_LEFT = 1,
ALIGN_RIGHT = 2,
ALIGN_TOP = 3,
ALIGN_BOTTOM = 4,

MouseDown = { X: 0, Y: 0 },
MouseUp = { X: 0, Y: 0 },
MouseMove = { X: 0, Y: 0 },
SelectedPoints = new Array(),
SelectedShape = null,
MouseLeftDown = false,
AltKeyDown = false,
SelectedIndex = -1,
Command = 0,
Stn = null,
ZoomValue = 1,
OffSetX = 0.0,
OffSetY = 0.0,
stage,
layer,
context,
editLayer,
editLayerCanvas,
AreaSelection,
TimeLapse = 0,
EditPointIndex1 = -1,
EditPointIndex2 = -1,
flag = true,
CanvasWidth = 800,
CanvasHeight = 600,
MoveXAxis = true,
MoveYAxis = true,
NoCommand = false,
FlashShape = null,
MapCurrentZoom;


var CanvasX = 0,
CanvasY = 0,
CanvasMaxX = 0,
CanvasMaxY = 0,
ViewAngleDeg = 0;

var ShapeNames = [
     { Main: "Polygon", Short: "P", Type: 1, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Platform", Short: "P", Type: 2, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "ATM", Short: "HDFC", Type: 3, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Book Shop", Short: "BS", Type: 4, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Bus Stop", Short: "BS", Type: 5, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Cloak Room", Short: "CR", Type: 6, Color: "yellow", W: 50, H: 20 },
     { Main: "Food Court", Short: "FC", Type: 7, Color: "pink", W: 50, H: 20 },
     { Main: "Gate", Short: "No.1", Type: 8, Color: "#82f0fd", W: 50, H: 20 },
     { Main: "Help", Short: "H", Type: 9, Color: "yellow", W: 50, H: 20 },
     { Main: "Metro Station", Short: "MS", Type: 10, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Parking", Short: "Pr", Type: 11, Color: "#acfac6", W: 50, H: 20 },
     { Main: "Police Booth", Short: "PB", Type: 12, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Prepaid Booth", Short: "PB", Type: 13, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Rest Room", Short: "RR", Type: 14, Color: "#FFAADC", W: 50, H: 20 },
     { Main: "Taxi Stand", Short: "TS", Type: 15, Color: "#FFF8DC", W: 50, H: 20 },
     { Main: "Ticket Counter", Short: "TC", Type: 16, Color: "red", W: 50, H: 20 },
     { Main: "Tourist Center", Short: "TrC", Type: 17, Color: "yellow", W: 50, H: 20 },
     { Main: "Waiting Room", Short: "WR", Type: 18, Color: "pink", W: 50, H: 20 }
];


var anim = new Kinetic.Animation({
    func: function (frame)
    {
        TimeLapse += frame.timeDiff;

        if (TimeLapse > 500)
        {
            TimeLapse = 0;

            if (SelectedShape)
            {

            }

            if (FlashShape)
            {
                FlashShape.polyShape.setFill(flag ? FlashShape.FillColor : "white");
                layer.draw();
                flag = !flag;
            }
        }

    },
    node: layer

});


$(document).keyup(function (e)
{
    if (NoCommand)
        return;

    var key = e.which != undefined ? e.which : e.keyCode;
    switch (key)
    {
        case 27: SelectedShape = null; Options(CMD_NONE); break;
        case 37: MapPan(-50, 0); break; //left
        case 38: MapPan(0, -50); break; //up
        case 39: MapPan(50, 0); break; //right
        case 40: MapPan(0, 50); break; //down

    }

});

$(document).keypress(function (e)
{
    var key = e.which != undefined ? e.which : e.keyCode;

    switch (key)
    {
        case 100: case 68: Command = CMD_DELETE; CommandText = "Delete Node"; break; //D
        case 105: case 73: Command = CMD_INSERT; CommandText = "Insert Node"; break; //I
        case 109: case 77: Command = CMD_MOVE; CommandText = "Move Node"; break; //M
        case 112: case 80: Command = CMD_NONE; CommandText = "Pan"; break; //P
        case 115: case 83: Command = CMD_AREA_SELECT; CommandText = "Select Points"; break; //S
        case 97: case 65: Command = CMD_Shape_ADD; CommandText = "Add Shape"; break; //A
        case 110: case 78: Command = CMD_MOVE_NAME; CommandText = "Move Shape Name"; break; //N
        case 120: case 88: MoveXAxis = !MoveXAxis; $("#chkX").prop("checked", MoveXAxis); break; //X
        case 121: case 89: MoveYAxis = !MoveYAxis; $("#chkY").prop("checked", MoveYAxis); break; //Y
    }
    //console.log(key);
    Options(Command);
});

$(document).ready(function ()
{
    anim.start();

    //$("button").button().css("width", "100%");

    //$("#CommnadButton").buttonset();
    $('input[id^="txtProp"]').bind("keyup", UpdateProperty);

    var O = "";
    $(ShapeNames).each(function (index)
    {
        O += "<option style='background-color:" + this.Color + "' value='" + this.Short + "'>" + this.Main + "(" + this.Short + ")</option>";
    });

    $("#ddShapes").html(O);

    if (!CanvasInit())
        return;

    initialize(Stn.Lat, Stn.Lng);

    $("input[type=text]").bind("focus", function ()
    {
        var id = $(this).attr("id");

        if (id == "txtPropShortName" || id == "txtPropShortName")
            Options(CMD_MOVE_NAME);

        NoCommand = true;
    });

    $("input[type=text]").bind("blur", function () { NoCommand = false; });


    //var pos=$("$divReference").position();
    //$("#map_canvas").css({ left: pos.left, top: pos.top });
    //$("#canvas_container").css({ left: pos.left, top: pos.top });

});

function UpdateProperty()
{
    if (!SelectedShape)
        return;

    var shp = SelectedShape.polyShape;
    var S = SelectedShape;

    shp.setFill(S.FillColor = $("#txtColor").val());
    shp.setStrokeWidth(S.StrokeWidth = $("#ddLineWidth").val());
    shp.setStroke(S.StrokeColor = $("#ddLineColor").val());
    shp.setOpacity(S.FillOpacity = $("#ddOpacity").val());

    S.Name = $("#txtPropName").val();
    S.ShortName = $("#txtPropShortName").val();
    S.Platforms = parseInt($("#txtPropPlatform").val());
    S.textShape.setText(S.ShortName);

    if (S.Points.length == 1)
    {
        S.Points[0].X = parseInt($("#txtPropX").val());
        S.Points[0].Y = parseInt($("#txtPropY").val());
        LoadShapes();
    }

    layer.draw();
    DisplayShapeList();
}

function UpdatePoints(obj)
{
    var pts = [];
    $(obj.Points).each(function ()
    {
        pts.push(this.X);
        pts.push(this.Y);
    });

    if (obj.polyShape)
        obj.polyShape.setPoints(pts);
}

function CanvasInit()
{
    CanvasWidth = $("#canvas_container").width();
    CanvasHeight = $("#canvas_container").height();
    if (!CanvasWidth)
        return false;

    stage = new Kinetic.Stage({ container: canvas_container, width: CanvasWidth, height: CanvasHeight });
    layer = new Kinetic.Layer();
    editLayer = new Kinetic.Layer();

    stage.add(layer);
    stage.add(editLayer);

    context = editLayer.getCanvas().context;
    editLayerCanvas = editLayer.getCanvas().element;

    editLayer.beforeDraw(UpdateEditPoints);

    $(editLayerCanvas).bind("mousedown", MouseDownEvent);
    $(editLayerCanvas).bind("mouseup", MouseUpEvent);
    $(editLayerCanvas).bind("mousemove", MouseMoveEvent);
    $(editLayerCanvas).bind("mousewheel", MouseWheelEvent);
    $(editLayerCanvas).bind("selectstart", function ()
    {
        return MouseIcon(this);
    });

    Stn = new Station();
    LoadStation($("#selectStation").val());

    return true;
}

function MouseIcon(obj)
{
    if (Command == CMD_NONE)
        $(obj).css("cursor", 'move');
    else
        $(obj).css("cursor", 'crosshair');

    return false;
}

function MouseWheelEvent(e)
{

    var e = window.event || e; // old IE support
    var delta = Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail)));

    Zoom(delta < 0 ? -1 : 1)
}

function GetValueX(Value)
{
    return CanvasWidth * (Value - MapMinX) / (MapMaxX - MapMinX);
}

function GetValueY(Value)
{
    return CanvasHeight - (CanvasHeight * (Value - MapMinY) / (MapMaxY - MapMinY));
};

function LoadShapes()
{
    //console.log("LoadShapes");
    if (!layer)
        return;

    layer.removeChildren();
    editLayer.removeChildren();


    $(Stn.ShapeList).each(function (index)
    {
        var poly = new Kinetic.Polygon({
            fill: this.FillColor,
            opacity: this.FillOpacity,
            stroke: this.StrokeColor,
            strokeWidth: this.StrokeWidth,
            name: this.Name,
            id: index
        });

        poly.on("mouseup", function ()
        {
            if ($("#chkAutoSelect").prop("checked"))
                SelectShape(this.getId());
        });

        this.polyShape = poly;

        // convert from Lat Lng To Pixel
        $(this.Points).each(function ()
        {
            this.X = GetValueX(this.Lng);
            this.Y = GetValueY(this.Lat);
        });

        UpdatePoints(this);

        // add the shape to the layer
        layer.add(poly);
        this.TextX = GetValueX(this.TextLng);
        this.TextY = GetValueY(this.TextLat);

        if (MapCurrentZoom > 16)
            layer.add(this.textShape = DrawText(this.TextX, this.TextY, this.ShortName, this.TextAngle));

        if ("Platform" == this.Name)
        {
            CreatePlatforms(this.Points[0].X, this.Points[0].Y, this.Platforms);
        }

    });
    stage.draw();
}

function DrawText(X, Y, Txt, Angle)
{
    return new Kinetic.Text({ x: X, y: Y, text: Txt, fontSize: 10, rotationDeg: Angle ? Angle : 0, fontFamily: "Calibri", textFill: "blue" });
}

function AddTrack(X, Y)
{
    layer.add(DrawLine(0, Y + 8, CanvasWidth, Y + 8));
    return Y + 16;
}

function AddPlatform(X, Y, ID)
{
    layer.add(Rect(X, Y, CanvasWidth, 10, "#e0e0e0", "#e0e0e0"));
    layer.add(DrawText(X + 10, Y, "PF" + ID));
    return Y + 10;
}

function CreatePlatforms(X, Y, Platforms)
{
    for (var i = 0; i < Platforms; i++)
    {
        Y = AddPlatform(X, Y, i + 1);
        if (i > 0)
        {
            if (++i > Platforms - 1)
                break;
            Y = AddPlatform(X, Y, i + 1);
        }

        Y = AddTrack(X, Y);

        if (i < Platforms - 1)
            Y = AddTrack(X, Y);
    }
}

function DrawLine(X1, Y1, X2, Y2, Color)
{
    return new Kinetic.Line({ points: [X1, Y1, X2, Y2], stroke: Color ? Color : "black", strokeWidth: 1 });
}

function DrawLine2(Pts, Color)
{
    return new Kinetic.Line({ points: Pts, stroke: Color ? Color : "black", strokeWidth: 1 });
}

function Options(ID)
{
    MouseIcon($(editLayerCanvas));

    if (NoCommand)
        return;
    Command = ID;
    $("#Cmd" + ID).prop("checked", true);
    $("#CommnadButton").buttonset();

    EditPointIndex1 = EditPointIndex2 = -1;
    switch (ID)
    {
        case CMD_AREA_SELECT: SelectedPoints.length = 0; break;
        case CMD_Shape_ADD: EditPointIndex1 = EditPointIndex2 = -1;
    }

    stage.draw();
}

function Zoom(Value)
{
    ZoomValue = ZoomMap(Value > 0 ? 1 : -1);
}

function MapIdle()
{
    MapCurrentZoom = map.getZoom();
    Reset();
    LoadShapes();

    try
    {
        LoadStallLocation();
    }
    catch (e)
    {
    }
}

function SetZoom(Value)
{
    //console.log(Value);
    ZoomValue = Value;
    stage.setScale(ZoomValue, ZoomValue);
    stage.draw();
}

function MoveShapeName(X, Y)
{
    var S = SelectedShape;
    if (!S)
        return;


    var MapDisplace = LatLngDisplacement(X, Y);

    if (MoveXAxis)
    {
        S.TextX += X;
        S.TextLng += MapDisplace.Lng;
    }
    if (MoveYAxis)
    {
        S.TextY += Y;
        S.TextLat += MapDisplace.Lat;
    }

    $("#txtPropX").val(S.Points[0].X);
    $("#txtPropY").val(S.Points[0].Y);

    S.textShape.setX(S.TextX);
    S.textShape.setY(S.TextY);
}

function MouseDownEvent(event)
{
    //console.log(event);
    MouseDown.X = MouseMove.X = event.offsetX;
    MouseDown.Y = MouseMove.Y = event.offsetY;
    MouseLeftDown = true;
    AltKeyDown = event.altKey;
    stage.draw();
}

function LatLngDisplacement(X, Y)
{
    var PtLatLng1 = GetLatLng(X, Y);
    var PtLatLng2 = GetLatLng(0, 0);
    return { Lat: PtLatLng1.Lat - PtLatLng2.Lat, Lng: PtLatLng1.Lng - PtLatLng2.Lng };
}

function MouseMoveEvent(event)
{
    var X = (event.offsetX - MouseMove.X) / ZoomValue;
    var Y = (event.offsetY - MouseMove.Y) / ZoomValue;

    $("#spanX").html(format("000", GetX(event.offsetX)));
    $("#spanY").html(format("000", GetY(event.offsetY)));
    var MapDisplace = LatLngDisplacement(X, Y);

    if (MouseLeftDown)
    {
        switch (Command)
        {
            case CMD_NONE:
                OffSetX -= X;
                OffSetY -= Y;
                stage.setOffset(OffSetX, OffSetY);
                MapPan(-X, -Y);
                $(document).css("cursor", "move");
                break;
        }

        if (SelectedShape)
        {

            switch (Command)
            {
                case CMD_AREA_SELECT:
                    SelectedPoints.length = 0;
                    $(SelectedShape.Points).each(function (index)
                    {
                        var X1 = AreaSelection.X, X2 = AreaSelection.X + AreaSelection.W;
                        var Y1 = AreaSelection.Y, Y2 = AreaSelection.Y + AreaSelection.H;

                        if (X1 > X2)
                        {
                            var X = X2; X2 = X1; X1 = X;
                        }

                        if (Y1 > Y2)
                        {
                            var Y = Y2; Y2 = Y1; Y1 = Y;
                        }

                        if (this.X >= X1 && this.X <= X2 && this.Y >= Y1 && this.Y <= Y2)
                        {
                            SelectedPoints.push({ Pt: this, Index: index });
                        }

                    });
                    break;

                case CMD_MOVE:
                    $(SelectedPoints).each(function (index)
                    {

                        if (MoveXAxis)
                        {
                            this.Pt.Lng += MapDisplace.Lng;
                            this.Pt.X += X;
                        }
                        if (MoveYAxis)
                        {
                            this.Pt.Lat += MapDisplace.Lat;
                            this.Pt.Y += Y;
                        }
                    });
                    UpdatePoints(SelectedShape);
                    break;

                case CMD_Shape_MOVE:
                    $(SelectedShape.Points).each(function (index)
                    {
                        var PtLatLng = GetLatLng(event.offsetX, event.offsetY);

                        if (MoveXAxis)
                        {
                            this.Lng += MapDisplace.Lng;
                            this.X += X;
                        }
                        if (MoveYAxis)
                        {
                            this.Lat += MapDisplace.Lat;
                            this.Y += Y;
                        }


                    });
                    MoveShapeName(X, Y);
                    UpdatePoints(SelectedShape);
                    break;

                case CMD_MOVE_NAME:
                    if (AltKeyDown)
                    {
                        SelectedShape.TextAngle += Y
                        SelectedShape.textShape.setRotationDeg(SelectedShape.TextAngle);
                    }
                    else
                        MoveShapeName(X, Y);
                    break;
            }
        }

        stage.draw();
    }

    if (SelectedShape)
    {
        switch (Command)
        {
            case CMD_INSERT:
                editLayer.draw();
                break;
        }
    }


    MouseMove.X = event.offsetX;
    MouseMove.Y = event.offsetY;
}

function MouseUpEvent(event)
{
    MouseLeftDown = false;
    AltKeyDown = false;
    MouseUp.X = event.offsetX;
    MouseUp.Y = event.offsetY;

    var Pt2 = GetLatLng(event.offsetX, event.offsetY);
    var Pt = { X: GetX(MouseUp.X), Y: GetY(MouseUp.Y), Lat: Pt2.Lat, Lng: Pt2.Lng };

    if (SelectedShape)
    {
        switch (Command)
        {
            case CMD_AREA_SELECT:
                if (SelectedPoints.length > 0)
                    Options(CMD_MOVE);
                break;

            case CMD_INSERT:
                if (EditPointIndex1 != -1)
                {
                    SelectedShape.Points.splice(EditPointIndex1 + 1, 0, Pt);
                    SelectPoint(EditPointIndex1 + 1);
                }
                break;
        }

        UpdatePoints(SelectedShape);
    }

    switch (Command)
    {
        case CMD_Shape_ADD:
            CreateShape(Pt);
            break;
    }


    stage.draw();
}

function MoveZIndex(Direction)
{
    var arr = Stn.ShapeList;
    if (Direction == -1 && SelectedIndex <= 0)
        return;

    if (Direction == 1 && SelectedIndex >= arr.length - 1)
        return

    var b = arr[SelectedIndex + Direction];
    arr[SelectedIndex + Direction] = arr[SelectedIndex];
    arr[SelectedIndex] = b;
    SelectedIndex += Direction;
    SelectShape(SelectedIndex);
    LoadShapes();

}

function Copy()
{
    if (SelectedShape)
    {
        SelectedShape = CopyShape(SelectedShape, 10, 10);
        Stn.ShapeList.push(SelectedShape);
        SelectShape(Stn.ShapeList.length - 1);
        LoadShapes();
        Options(CMD_Shape_MOVE);
    }
}

function Align(AlignType)
{
    if (!SelectedShape)
        return;

    if (SelectedPoints.length < 2)
        return;

    $(SelectedPoints).each(function ()
    {
        switch (AlignType)
        {
            case ALIGN_LEFT: this.Pt.X = SelectedPoints[0].Pt.X; break;
            case ALIGN_TOP: this.Pt.Y = SelectedPoints[0].Pt.Y; break;
        }
    });

    UpdatePoints(SelectedShape);
    stage.draw();

    SelectedPoints.length = 0;
    Options(CMD_AREA_SELECT);
}

function DisplayShapeList()
{
    var str = "<table style='border-spacing:0px'>";
    var arr = Stn.ShapeList;
    for (var i = arr.length - 1; i >= 0; i--)
    {
        str += "<tr><td style='background-color:" + arr[i].FillColor + "'>&nbsp;&nbsp;"
		+ "<td " + (SelectedIndex == i ? "style='background-color:wheat'" : "") + "><a href='#' onclick='SelectShape(" + i + ",true)'>"
		+ arr[i].Name + "(" + arr[i].ShortName + ") </a>";
    }

    $("#divShapeList").html(str + "</table>");
}

function GetX(X)
{
    return X / ZoomValue + OffSetX;
}

function GetY(Y)
{
    return Y / ZoomValue + OffSetY;
}

function UpdateEditPoints()
{
    editLayer.removeChildren();

    if (Command == CMD_AREA_SELECT && MouseLeftDown)
    {
        AreaSelection = {
            W: GetX(MouseMove.X) - GetX(MouseDown.X),
            H: GetY(MouseMove.Y) - GetY(MouseDown.Y),
            X: GetX(MouseDown.X),
            Y: GetY(MouseDown.Y)
        };

        var rect = Rect(AreaSelection.X, AreaSelection.Y, AreaSelection.W, AreaSelection.H, null, "black");
        editLayer.add(rect);
    }

    if (SelectedShape)
    {
        var Pts = SelectedShape.Points;

        if (EditPointIndex1 == -1)
        {
            for (var i = 0; i < Pts.length; i++)
            {
                var fillclr = "white";

                var X2 = Pts[i].X, Y2 = Pts[i].Y;
                if (i == 0)
                    fillclr = "green";

                else if (i == Pts.length - 1)
                    fillclr = "red";

                else if (IsSelected(i))
                    fillclr = "yellow";


                editLayer.add(CreateRect(X2 - 4, Y2 - 4, 8, 8, fillclr, "black", i));
            }

            //console.log(Pts);
        }

        if (EditPointIndex1 != -1 && EditPointIndex2 != -1)
        {
            var l = DrawLine2([Pts[EditPointIndex1].X, Pts[EditPointIndex1].Y, GetX(MouseMove.X), GetY(MouseMove.Y), Pts[EditPointIndex2].X, Pts[EditPointIndex2].Y]);
            editLayer.add(l);
        }
    }

}

function IsSelected(index)
{
    for (var i = 0; i < SelectedPoints.length; i++)
        if (SelectedPoints[i].Index == index)
            return true;
}

function Rect(X, Y, W, H, FillColor, StrokeColor, ID, Name)
{
    return new Kinetic.Rect({
        x: X,
        y: Y,
        width: W,
        height: H,
        fill: FillColor,
        stroke: StrokeColor,
        id: ID,
        name: Name,
        strokeWidth: 1
    });
}

function CreateRect(X, Y, W, H, FillColor, StrokeColor, ID, Name)
{
    var obj = Rect(X, Y, W, H, FillColor, StrokeColor, ID, Name);

    obj.on("mouseover", function ()
    {
        //$(editLayerCanvas).css("cursor", "pointer");
    });

    obj.on("mouseout", function ()
    {
        //$(editLayerCanvas).css("cursor", "crosshair");
    });

    obj.on("mouseup", function ()
    {
        var i = this.getId();
        switch (Command)
        {
            case CMD_DELETE:
                SelectedShape.Points.splice(i, 1);
                UpdatePoints(SelectedShape);
                stage.draw();
                break;
            case CMD_INSERT:
                SelectedPoints.length = 0;
                SelectPoint(i);
                break;
        }
    });

    obj.on("mousedown", function ()
    {
        var i = this.getId();
        if (this.getName() == "AnchorPoint" && Command != CMD_DELETE)
        {
            Options(CMD_MOVE);
        }

        switch (Command)
        {
            case CMD_MOVE:
                SelectedPoints.length = 0;
                SelectedPoints.push({ Pt: SelectedShape.Points[i], Index: i });
                break;
        }

    });

    return obj;
}

function SelectPoint(index)
{
    EditPointIndex1 = index;
    EditPointIndex2 = index + 1;
    if (EditPointIndex2 > SelectedShape.Points.length - 1)
        EditPointIndex2 = 0;

    //console.log(EditPointIndex1, EditPointIndex2);
}

function SelectShape(index, ShowInCenter)
{
    SelectedPoints.length = 0;
    SelectedIndex = index;
    var S = SelectedShape = Stn.ShapeList[index];
    $("#txtColor").val(S.FillColor);
    $("#ddLineColor").val(S.StrokeColor);
    $("#ddLineWidth").val(S.StrokeWidth);
    $("#ddOpacity").val(S.FillOpacity);
    $("#txtPropName").val(S.Name);
    $("#txtPropShortName").val(S.ShortName);
    $("#txtPropX").val(S.Points[0].X);
    $("#txtPropY").val(S.Points[0].Y);
    DisplayShapeList();
    stage.draw();

    if (ShowInCenter)
    {
        var pt = GetShapeCenter(S.Points);
        pt = GetLatLng(S.TextX, S.TextY);
        MapPanTo(pt.Lat, pt.Lng);
    }
}

function Reset()
{
    OffSetX = OffSetY = 0;
    stage.setOffset(0, 0);
    ZoomValue = 1;
    stage.setScale(1, 1);
    stage.draw();
}

function DeleteShape()
{
    if (SelectedShape)
    {
        Stn.ShapeList.splice(SelectedIndex, 1);
        SelectedShape = null;
        LoadShapes();
        layer.draw();
        DisplayShapeList();
    }
}

function CreateShape(Pt)
{
    var ItemType = $("#ddShapes")[0].selectedIndex;
    var Data = ShapeNames[ItemType];
    var Name = $("#ddShapes option:selected").text().split('(')[0];
    var Pts = [];

    switch (Name)
    {
        case "Polygon":
        case "Platform":
            Pts.push(Pt);
            break;
        default:
            Pts.push(Pt);
            Pts.push({ X: Pt.X + Data.W, Y: Pt.Y });
            Pts.push({ X: Pt.X + Data.W, Y: Pt.Y + Data.H });
            Pts.push({ X: Pt.X, Y: Pt.Y + Data.H });
    }

    SelectedShape = new Shape(Name, $("#ddShapes").val(), Data.Type, Data.Color, .5, "black", 1);

    SelectedShape.TextX = Pt.X; //+ Data.W * .25;
    SelectedShape.TextY = Pt.Y; //+ Data.H * .25;

    SelectedShape.TextLng = Pt.Lng;
    SelectedShape.TextLat = Pt.Lat;

    Stn.ShapeList.push(SelectedShape);
    SelectedShape.Points = Pts;
    LoadShapes();
    SelectShape(Stn.ShapeList.length - 1);

    Options(CMD_Shape_MOVE);
    if (ItemType == 0)
    {
        Options(CMD_INSERT);
        EditPointIndex1 = EditPointIndex2 = 0;
    }
}

function Shape(Name, ShortName, Type, FillColor, FillOpacity, StrokeColor, StrokeWidth)
{
    this.Name = Name;
    this.ShortName = ShortName;
    this.TextAngle = 0;
    this.Type = Type
    this.StrokeColor = StrokeColor ? StrokeColor : "black";
    this.StrokeWidth = StrokeWidth ? StrokeWidth : 1;
    this.FillOpacity = FillOpacity ? FillOpacity : .5;
    this.FillColor = FillColor ? FillColor : "black";
    this.TextX = 0;
    this.TextY = 0;
    this.TextLng = 0;
    this.TextLat = 0;
    this.Points = new Array();
}

function CopyShape(obj, OffSetX, OffSetY)
{
    var NewObj = new Shape(obj.Name, obj.ShortName, obj.Type, obj.FillColor, obj.FillOpacity, obj.StrokeColor, obj.StrokeWidth);

    NewObj.TextAngle = obj.TextAngle;
    NewObj.TextX = obj.TextX + OffSetX;
    NewObj.TextY = obj.TextY + OffSetY;

    var MapDisplace = LatLngDisplacement(OffSetX, OffSetY);

    NewObj.Name = obj.Name;
    NewObj.ShortName = obj.ShortName;
    NewObj.TextLng = obj.TextLng + MapDisplace.Lng;
    NewObj.TextLat = obj.TextLat + MapDisplace.Lat;

    console.log(obj);
    console.log(NewObj);

    $(obj.Points).each(function ()
    {
        NewObj.Points.push({ X: this.X + OffSetX, Y: this.Y + OffSetY, Lat: this.Lat + MapDisplace.Lat, Lng: this.Lng + MapDisplace.Lng });
    });

    return NewObj;
}

function SetNameInCenter()
{
    var S = SelectedShape;
    if (S)
    {
        var pt = GetShapeCenter(S.Points);
        var H = S.textShape.getBoxHeight();
        var W = S.textShape.getBoxWidth();

        S.TextX = pt.X - W * .5;
        S.TextY = pt.Y - H * .5;
        pt = GetLatLng(S.TextX, S.TextY);
        S.TextLat = pt.Lat;
        S.TextLng = pt.Lng;
        S.textShape.setX(S.TextX);
        S.textShape.setY(S.TextY);
        stage.draw();
    }
}

function PolyArea(pts)
{
    var area = 0;

    var nPts = pts.length;
    var j = nPts - 1;
    var p1; var p2;

    for (var i = 0; i < nPts; j = i++)
    {
        p1 = pts[i]; p2 = pts[j];
        area += p1.X * p2.Y;
        area -= p1.Y * p2.X;
    }
    area /= 2;
    return area;
};

function GetShapeCenter(pts)
{
    var nPts = pts.length;
    var x = 0; var y = 0;
    var f;
    var j = nPts - 1;
    var p1; var p2;

    for (var i = 0; i < nPts; j = i++)
    {
        p1 = pts[i]; p2 = pts[j];
        f = p1.X * p2.Y - p2.X * p1.Y;
        x += (p1.X + p2.X) * f;
        y += (p1.Y + p2.Y) * f;
    }

    f = PolyArea(pts) * 6;
    return { X: x / f, Y: y / f };
}

function Station()
{
    this.Name = $("#selectStation").val();
    this.Lat = 28.615640724032573;
    this.Lng = 77.24547417924191;
    this.Zoom = 16;
    this.Platforms = 16;
    this.ShapeList = new Array();
}

function SetMapCenter()
{
    var pt = GetMapCenter();
    Stn.Lat = pt.Lat;
    Stn.Lng = pt.Lng;
    Stn.Save();
}

function Save()
{

    $(Stn.ShapeList).each(function ()
    {
        this.polyShape = null;
        this.textShape = null;
    });

    var data = JSON.stringify(Stn);
    SaveInLocalStorage("StnLayOut" + $("#selectStation").val(), data);

    $.post("../Data.aspx?Action=SaveStnLayOut&Password=2012&Data1=" + $("#selectStation").val(), { Layout: data }, function (data) { alert("Done"); });

}

function LoadStation(StnCode, FromServer)
{

    var data;
    if (FromServer)
    {
        $.ajax({ cache: false, url: BasePath + "Data.aspx?Action=LoadStnLayOut&Password=2012&Data1=" + StnCode, success: LoadStationDone });
    }
    else
        data = LoadFromLocalStorage("StnLayOut" + $("#selectStation").val());

    if (data != "undefined" && data != "null" && data != null && data != undefined)
    {
        LoadStationDone(data);
    }
}

function LoadStationDone(data)
{
    if (data == "")
        Stn = new Station();
    else
        Stn = JSON.parse(data);

    try
    {
        SetCenter(Stn.Lat, Stn.Lng);
    }
    catch (e) { }

    LoadShapes();
    DisplayShapeList();


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

function GetPolygon(Name)
{
    for (var i = 0; i < Stn.ShapeList.length; i++)
        if (Stn.ShapeList[i].ShortName == Name)
            return Stn.ShapeList[i];

    return null;
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

window.format = function (b, a)
{
    if (!b || isNaN(+a)) return a; var a = b.charAt(0) == "-" ? -a : +a, j = a < 0 ? a = -a : 0, e = b.match(/[^\d\-\+#]/g), h = e && e[e.length - 1] || ".", e = e && e[1] && e[0] || ",", b = b.split(h), a = a.toFixed(b[1] && b[1].length), a = +a + "", d = b[1] && b[1].lastIndexOf("0"), c = a.split("."); if (!c[1] || c[1] && c[1].length <= d) a = (+a).toFixed(d + 1); d = b[0].split(e); b[0] = d.join(""); var f = b[0] && b[0].indexOf("0"); if (f > -1) for (; c[0].length < b[0].length - f;) c[0] = "0" + c[0]; else +c[0] == 0 && (c[0] = ""); a = a.split("."); a[0] = c[0]; if (c = d[1] && d[d.length -
1].length) { for (var d = a[0], f = "", k = d.length % c, g = 0, i = d.length; g < i; g++) f += d.charAt(g), !((g - k + 1) % c) && g < i - c && (f += e); a[0] = f } a[1] = b[1] && a[1] ? h + a[1] : ""; return (j ? "-" : "") + a[0] + a[1]
};
