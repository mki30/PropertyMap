<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowPriceTrend.aspx.cs" Inherits="ShowPriceTrend" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <p>
        <br />
    </p>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/PriceTrend.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/highcharts.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/QueryString.js")%>'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        var City = "<%=City%>";
        var SubCity = "<%=SubCity%>";
        var SubCityUrlName = "<%=SubCityUrlName%>";
    </script>
    <h1 style="font-size:30px;"><asp:Literal ID="ltHeading" runat="server"></asp:Literal>
    <asp:Literal ID="ltLocality" runat="server"></asp:Literal></h1>
    <div class="row-fluid">
        <div class="span3">
            <div style="border: 1px solid gainsboro; text-align: justify; padding-left: 5px; height: 600px; overflow: scroll; overflow-x: hidden;">
                <asp:Label ID="lblCheckBox" runat="server"></asp:Label>
            </div>
        </div>
        <div class="span9">
            <div id="container" style="width: 99%; height: 100%; border: 1px solid #dbd6d6"></div>
        </div>
    </div>
    <br/>
</asp:Content>





