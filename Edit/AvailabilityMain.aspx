<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AvailabilityMain.aspx.cs" Inherits="Edit_AvailabilityMain" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.9.1.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Common.js"></script>
    <link href="../css/Simple.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        body
        {
            background-color:white;
            font:normal 12px Helvetica, Arial;
        }
    </style>
    <script type="text/javascript">
        function LoadData(SocietyID)
        {
            if (SocietyID == undefined)
                SocietyID = 0;

            RefreshAvlList(SocietyID);
            ShowAvailabilityDetail(0,SocietyID);
        }
          
        function ShowError(data)
        {
            var Lines = data.split('~');
            if (Lines[0] != "")
                alert(Lines[0]);
            return Lines;
        }

        function RefreshAvlList(SocietyID)
        {
          $.ajax({
                url: "../Data.aspx?Action=GetSocietyAvailability&Data1=" + SocietyID, cache: false, success: function (data)
                {
                    $("#divAvlList").html(ShowError(data)[1]);
                    $("#AvlTable").HighLightRows();
                    $("#AvlTable a").css("text-decoration", "none");
                    $("#AvlTable th").css("background-color", "#A9C7BC");
                }
          });
        }

        function ShowAvailabilityDetail(ID,SocietyID)
        {
            $("#frameEditAvailability")[0].src = "EditAvailability.aspx?ID=" + ID+"&SocietyID="+SocietyID;
        }

        function ShowAgentDetail(AgentID, AvlID)
        {
            //alert("Agent-" + AgentID);
            $("#frameEditAgent")[0].src = "EditAgent.aspx?ID=" + AgentID;
            $("#frameEditAvailability")[0].src = "EditAvailability.aspx?ID=" + AvlID;
            //RefreshAvlList(AgentID);
        }

</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="border-spacing:1px;">
            <tr>
            <td >
                <div class="edit-head">List</div></td>
            <td >
                <div class="edit-head">Postings</div></td>
            <td >
                <div class="edit-head">Detail</div></td>
            <td >
                <div class="edit-head">Agent Detail</div></td>
            </tr>
            <tr>
                <td style="width:160px; vertical-align:top">
                    <iframe id="frameSocietyList" src="SocietyList.aspx" style="width: 98%; height: 650px;border:0px;" ></iframe>
                </td>
                  <td style="vertical-align:top;">
                    <div id="divAvlList"></div>
                  </td>
                <td style="vertical-align:top;">
                    <iframe id="frameEditAvailability" src="EditAvailability.aspx" style="width: 320px; height: 650px;border:0px;" ></iframe>
                 </td>
                <td style="vertical-align:top;">
                    <iframe id="frameEditAgent" src="EditAgent.aspx" style="width: 360px; height: 700px;border:0px;"></iframe>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
