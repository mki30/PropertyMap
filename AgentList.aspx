<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AgentList.aspx.cs" Inherits="AgentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row-fluid">
    <div class="span12">
    <asp:Literal ID="ltAgentList" runat="server"></asp:Literal>
    </div>
   </div>
</asp:Content>

