<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentMain.aspx.cs" Inherits="Edit_AgentMain"%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/Simple.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Common.js"></script>
    <script type="text/javascript">
      
        $(document).ready(function ()
        {
            $("#frameAgentList")[0].src = "AgentList.aspx?SellerType=" + sellerType;
            //LoadData(0, 1);
        });

        function LoadData(SellerID,ShowList)
        {
            if (SellerID == undefined)
                SellerID = 0;

            var Page = "EditAgent";
            switch (sellerType)
            {
                case 1:
                    Page = "EditOwner";
                    break;
                case 2:
                    Page = "EditBuilder";
                    break;
            }
            $("#frameEditAgent")[0].src = "EditAgent.aspx?ID=" + SellerID + "&UserType=" + sellerType + "&ShowList=" + ShowList;
            RefreshAvlList(SellerID);
        }

        function RefreshAvlList(SellerID)
        {
            $.ajax({
                url: "../Data.aspx?Action=GetAgentPosting&Data1=" + SellerID, cache: false, success: function (data)
                {
                    $("#divAvlList").html(ShowError(data)[1]);
                    $("#AvlTable").HighLightRows();
                    $("#AvlTable a").css("text-decoration", "none");
                    $("#AvlTable th").css("background-color", "#A9C7BC");
                }
            });
        }

        function ShowAvailabilityDetail(ID,SellerID)
        {
            $("#frameEditAvailability")[0].src = "EditAvailability.aspx?ID=" + ID;
        }

        function RefreshSocietyList()    //refresh list on add new record
        {
            $("#frameAgentList")[0].src = "AgentList.aspx?SellerType=" + sellerType;
        }

        function RefreshApartmentType(SocietyID, AvlID)
        {
            alert();
            //$("frameEditAvailability")[0].src = "EditAvailability.aspx?ID=0";
        } 
</script>
<style type="text/css">
        body
        {
            background-color:white;
            font:normal 12px Helvetica, Arial;
        }
</style>
<title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="border-spacing:1px">
            <tr>
                <td style="width: 190px;vertical-align:top">
                   <div class="edit-head"><asp:Literal runat="server" ID="ltSellerHead"></asp:Literal></div>
                   <iframe id="frameAgentList" src="AgentList.aspx" style="width: 98%; height: 700px;border:0px" ></iframe>
                </td>
                <td style="vertical-align:top;width:370px;">
                    <div class="edit-head"><asp:Literal ID="ltEditType" runat="server"></asp:Literal></div>
                    <iframe id="frameEditAgent" src="EditAgent.aspx" style="width:98%; height: 700px;border:0px" ></iframe>
                </td>
                <td style="vertical-align:top;">
                    <div style="background-color: #6997F5; text-align: center; color: #fff;"><b>Agent Postings</b></div>
                    <div id="divAvlList"></div>
                </td>
                <td style="vertical-align:top">
                    <div class="edit-head">Edit Availibility</div>
                    <iframe id="frameEditAvailability" src="EditAvailability.aspx" style="width: 350px; height: 700px;border:0px" ></iframe>
                </td>
            </tr>
          </table>
      </div>
   </form>
</body>
</html>

