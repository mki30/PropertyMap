<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #divMain
        {
            background: -webkit-gradient(linear, left top, left bottom, from(#DBE7B3), to(white));
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="divMain">
        <div style="height: 50px; text-align: left;">
            <br />
            <span style="margin-left: 22px; font-size: 20px;">Registation</span>
        </div>

        <div style="width: 400px; background-color: white; border-radius: 10px; margin-left: 20px; padding-top: 10px; border: 1px solid #DBE7B3;">
            <div style="float: left; padding-left: 3px;">I Am</div>
            <div style="width: 200px; margin-left: 90px; margin-right: auto; text-align: right;">
                <input type="radio" checked="checked" name="radiolist" value="0">Agent
                <input type="radio" name="radiolist" value="1">Owner
                <input type="radio" name="radiolist" value="2">Builder                
            </div>
            <iframe id="frameEdit" style="width: 350px; height: 450px; border: 0px;"></iframe>
        </div>
    </div>
    
    <script type="text/javascript">

        var $frameEdit = null;
        var isLoaded = false;

        $(document).ready(function ()
        {
            $frameEdit = $("#frameEdit")[0];
            $frameEdit.src = "Edit/EditAgent.aspx?CSS=1&ReqType=user&ID=0";

            isLoaded = true;

            $("input[name='radiolist']").on('change', function ()
            {
                RefreshEditPage($(this).val());
            });
            
        });

        function RefreshEditPage(sellerType)
        {
            var SellerID = '0';
            $frameEdit.src = "Edit/EditAgent.aspx?CSS=1&ReqType=user&ID=" + SellerID + "&UserType=" + sellerType;
        }
    </script>
</asp:Content>

