<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PriceTrendList.aspx.cs" Inherits="PriceTrendList" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        CurrentPage = "PriceTrend";
        $(document).ready(function ()
        {
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row-fluid">
        <div class="span4"><h1 style="margin: 2px">Price Trend</h1></div>
        <div class="span4 offset4"><asp:DropDownList ID="ddCity" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddCity_SelectedIndexChanged"></asp:DropDownList></div>
    </div>
    <div>
        <asp:Literal ID="ltrTable" runat="server"></asp:Literal>
    </div>
    <br/>
</asp:Content>

