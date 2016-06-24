<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="PriceTrend.aspx.cs" Inherits="Edit_PriceTrend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DropDownList ID="ddlCity" runat="server" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Value="4">Noida</asp:ListItem>
         <asp:ListItem Value="173">Gurgaon</asp:ListItem>
        <asp:ListItem value="392">Greater Noida</asp:ListItem>
        <asp:ListItem Value="1">Ghaziabad</asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Literal ID="ltDetail" runat="server"></asp:Literal>
</asp:Content>

