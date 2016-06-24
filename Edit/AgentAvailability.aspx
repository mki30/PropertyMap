<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="AgentAvailability.aspx.cs" Inherits="Edit_AgentAvailability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body
        {
            font-size: 11px;
        }

        .table-condensed th, .table-condensed td
        {
            padding: 0px 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:literal runat="server" id="ltAvlList"></asp:literal>
</asp:Content>

