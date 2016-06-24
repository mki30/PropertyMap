<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FloorPlans.ascx.cs" Inherits="UserControl_FloorPlans" %>

<div class="row-fluid">
    <div class="span3">
        <asp:Literal ID="lblLogo" runat="server"></asp:Literal>
    </div>
    <div class="span6">
        <h1 style="margin-top: 2px; margin-bottom: 2px;">
            <asp:Literal ID="ltrProjectName" runat="server"></asp:Literal>
        </h1>
        <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
    </div>

    <div class="span3" style="text-align: right">
        <a id="BuilderName" runat="server"></a>
    </div>

    <div style="text-align: right">
        Expected Possession <b>:</b>
        <asp:Literal ID="ltpossessDate" runat="server"></asp:Literal>
    </div>
</div>

<div class="row-fluid">
    <asp:Label ID="lblFloorPlans" runat="server" Text="Label"></asp:Label>
</div>


