<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SocietyMain.aspx.cs" Inherits="Edit_SocietyMain" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>PropertyList.in - Better way to find properties</title>
    <link rel="stylesheet" type="text/css" href="../css/PropertyMap.css" />
    <link href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.2.custom.min.js"></script>
    <script type="text/javascript" src="../js/Common.js"></script>

    <style type="text/css">
        body
        {
            background-color: gainsboro;
            font: normal 12px Helvetica, Arial;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function ()
        {
            $("#DivDialog").dialog({ width: 800, height: 600, autoOpen: false, show: "fade", hide: "fade", title: "Enlarge" });

            var SocietyID = LoadFromLocalStorage("SocietyID", 0);
            LoadData(SocietyID);

            $("#ui-id-1").css("width", "100%");                                //map dialoge resize 
            flag = 0;
            $("#ui-id-1").click(function ()
            {
                if (flag == 0)
                {
                    $("#DivDialog").dialog({ width: '80%', height: '850' });
                    $("#DivDialog").dialog("option", "position", 'center');
                    $("#ui-id-1").text("Srink");
                    flag = 1;
                }
                else
                {
                    $("#DivDialog").dialog({ width: 820, height: 600 });
                    $("#DivDialog").dialog("option", "position", 'center');
                    $("#ui-id-1").text("Enlarge");
                    flag = 0;
                }
            });
        });

        function ShowImageDialog(imgPath)
        {
            $("#DivDialog").dialog('open');
            $("#frameMap").hide();
            $("#aptimgtag").remove();
            $("#DivDialog").append("<img id='aptimgtag' src='" + imgPath + "' height='540' width='770'/>");
        }

        function LoadData(SocietyID)
        {
            if (SocietyID == undefined)
                SocietyID = 0;
            $("#frameEditSociety")[0].src = "EditSociety.aspx?ID=" + SocietyID;

            RefreshApartment(SocietyID);
            RefreshApartmentType(SocietyID);
            ShowApartment(0, SocietyID);
        }

        function RefreshApartment(SocietyID, ApartmentID)
        {
            if (ApartmentID == undefined)
                ApartmentID = 0;
            $("#frameApartmentList")[0].src = "ApartmentList.aspx?SocietyID=" + SocietyID + "&ApartmentID=" + ApartmentID;
        }

        function RefreshApartmentType(SocietyID, ApartmentTypeID)
        {
            if (ApartmentTypeID == undefined)
                ApartmentTypeID = 0;

            $("#frameEditApartmentType")[0].src = "EditApartmentType.aspx?SocietyID=" + SocietyID + "&ApartmentTypeID=" + ApartmentTypeID;
        }

        function ShowApartment(ApartmentID, SocietyID)
        {
            $("#frameEditApartment")[0].src = "EditApartment.aspx?ID=" + ApartmentID + "&SocietyID=" + SocietyID;
        }

        function ShowApartmentType(ID)
        {
            $("#frameEditApartmentType")[0].src = "EditApartmentType.aspx?ID=" + ID;
        }
        function RefreshSocietyList()
        {
            $("#frameSocietyList")[0].src = "SocietyList.aspx";
        }

        function FindAddress(address)
        {
            $("#frameMap")[0].contentWindow.FindAddress(address);
        }

        function SetCenter(Lat, Lng)
        {
            try
            {
                if ($("#frameMap"))
                    $("#frameMap")[0].contentWindow.SetCenter(Lat, Lng);
            }
            catch (e) { }
        }

        function UpdateLatLng(Lat, Lng)
        {
            $("#frameEditSociety")[0].contentWindow.UpdateLatLng(Lat, Lng);
        }

        function ShowMap(QueryString)
        {
            $("#aptimgtag").remove();
            $("#frameMap").show();
            $("#frameMap")[0].src = "../Map.htm?" + QueryString;
            $("#DivDialog").dialog('open');
        }

        function UpdatePolyPoints(data)
        {
            console.log(data);
            $("#frameEditSociety")[0].contentWindow.UpdatePolyPoints(data);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: auto; border-spacing: 1px;">
            <tr>
                <td style="padding-right: 4px;">
                    <div class="edit-head">List</div>
                </td>
                <td style="padding-right: 4px;">
                    <div class="edit-head">Detail</div>
                </td>
                <td style="padding-right: 4px;">
                    <div class="edit-head">Apartment</div>
                </td>
                <td style="padding-right: 4px;">
                    <div class="edit-head">Apartment Detail</div>
                </td>
                <td style="padding-right: 7px;">
                    <div class="edit-head">Apartment Type</div>
                </td>
            </tr>
            <tr>
                <td style="width: 250px; vertical-align: top;">
                    <iframe id="frameSocietyList" src="SocietyList.aspx" style="width: 98%; height: 800px; border: 0px;"></iframe>
                </td>
                <td style="width: 400px; vertical-align: top;">
                    <iframe id="frameEditSociety" src="EditSociety.aspx" style="width: 98%; height: 800px; border: 0px;"></iframe>
                </td>
                <td style="width: 100px; vertical-align: top;">
                    <iframe id="frameApartmentList" src="ApartmentList.aspx" style="width: 98%; height: 800px; border: 0px;"></iframe>
                </td>
                <td style="width: 240px; vertical-align: top;">
                    <iframe id="frameEditApartment" src="EditApartment.aspx" style="width: 98%; height:800px; border: 0px;"></iframe>
                </td>
                <td style="width: 900px; vertical-align: top; height:auto;">
                    <iframe id="frameEditApartmentType" src="EditApartmentType.aspx" style="width: 98%; height:800px; border: 0px;"></iframe>
                </td>
            </tr>
        </table>
    </form>
    <div id="DivDialog">
        <iframe id="frameMap" style="width: 99%; height: 99%; border: 0px;"></iframe>
    </div>
</body>
</html>
