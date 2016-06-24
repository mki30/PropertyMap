<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PropList.ascx.cs" Inherits="PropList" %>
<%--<link href="/css/jquery-ui-1.10.3.custom.min.css" rel="stylesheet">
<script type="text/javascript" src="/js/jquery-ui-1.10.3.custom.min.js"></script>--%>
<asp:Literal ID="ltData" runat="server"></asp:Literal>
<div class="row-fluid">
    <div class="span2">
        <div class="btn-group" style="width: 100%;">
            <a class="btn dropdown-toggle" data-toggle="dropdown" href="#" style="width: 90%;">
                <asp:Literal ID="ltCity" runat="server"></asp:Literal>
                <span class="caret"></span></a>
            <ul class="dropdown-menu">
                <asp:Literal ID="ltCityList" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>

    <div class="span2">
        <div class="btn-group" style="width: 100%;">
            <a class="btn dropdown-toggle" data-toggle="dropdown" href="#" style="width: 90%;">
                <asp:Label ID="ltSelectedBudget" runat="server" Text="Budget Any"></asp:Label>
                <span class="caret"></span>
            </a>
            <ul class="dropdown-menu">
                <asp:Literal ID="ltBudgetList" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>

    <div class="span2">
        <div class="btn-group" style="width: 100%;">
            <a class="btn dropdown-toggle" data-toggle="dropdown" href="#" style="width: 90%;">
                <asp:Label ID="ltSelectedBHK" runat="server" Text="Any BHK"></asp:Label>
                <span class="caret"></span>
            </a>
            <ul class="dropdown-menu">
                <li><a href="#" onclick="return SelectBHK(0,this)">Any BHK</a></li>
                <li><a href="#" onclick="return SelectBHK(1,this)">1 BHK</a></li>
                <li><a href="#" onclick="return SelectBHK(2,this)">2 BHK</a></li>
                <li><a href="#" onclick="return SelectBHK(3,this)">3 BHK</a></li>
                <li><a href="#" onclick="return SelectBHK(4,this)">4 BHK</a></li>
            </ul>
        </div>
    </div>
    <div class="span6  input-append" style="text-align: right;">
        <input id="txtSearch" type="text" placeholder="Search for Project/Builder/City" autofocus="autofocus" style="width: 70%;" />
        <button class="btn btn-warning" type="button">Search</button>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <asp:Literal ID="ltProjectList" runat="server"></asp:Literal>
    </div>
</div>
