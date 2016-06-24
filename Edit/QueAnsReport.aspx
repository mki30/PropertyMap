<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="QueAnsReport.aspx.cs" Inherits="Edit_EditQueAns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script src="../js/jquery.fancybox.pack.js"></script>
    <script type="text/javascript">

        $(document).ready(function ()
        {
            $(".fancybox1").fancybox(
             {
                 width: '650',
                 autoDimension: true
             });
        });        function RefreshPage()
        {
            window.location.reload();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="ltDetail" runat="server"></asp:Literal>
</asp:Content>

