<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AgentClientMain.aspx.cs" Inherits="ClientMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <title></title>
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/PropertyList.js"></script>
    <script type="text/javascript" src="js/Common.js"></script>
    <link rel="stylesheet" type="text/css" href="css/Simple.css"/>
    <style type="text/css">
    </style>
    
    <script type="text/javascript">
        $(document).ready(function ()
        {
            SellerID = 0;
            //ClientID = 1;
            //ShowAvailByClient(ClientID);
            //ShowAvailabilityByClientID(ClientID,1);
            $("#btnShow").click(function ()
            {
                alert($("#txtMobile").val());
                if ($("#txtMobile").val() != "")
                    GetClietIDByMobileNo($("#txtMobile").val());
            });
            $(".hd").hide();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="min-height:400px;">
    <asp:Panel ID="Panel1" runat="server">
        Mobile No <input type="text" id="txtMobile" style="border:1px solid #c6bebe; border-radius:4px; height:22px;" maxlength="10"/>
        <input type="button" id="btnShow" value="Show" class="btnLogin"/>
    </asp:Panel>
    <asp:Panel id="Panel2" runat="server" CssClass="hd">
    <div id="MainClientDiv" style="height:600px;">
        <div id="divClientAssignedAvl" style=" width:300px; height:400px;float:left;"></div>
        <div id="AvlDetail" style="border:1px solid gray; width:350px; height:650px; float:left;overflow:auto;">
            <iframe id="frameEditAvailability" src="Edit/EditAvailability.aspx" style="width: 350px; height: 650px; border: 0px; "></iframe>
        </div>
        <div id="DivMap" style="width:335px;height:400px; float:left; border-left:0px;">
            <iframe id="frameMap" src="Map.htm?type=1" style="height:100%; width:100%"></iframe>
        </div>
    </div>
    </asp:Panel>
   </div>
</asp:Content>

