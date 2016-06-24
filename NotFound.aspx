<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotFound.aspx.cs" Inherits="NotFound"  MasterPageFile="~/MasterPage.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var BasePath = '<%= ResolveClientUrl("~") %>';
    </script>
</asp:content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="span8" style="margin:200px 0px 200px 0px;">
    <h3>The page you are looking for is not available...go to <a href="/">Home</a></h3>
       <div class="success">You will be automatically redirected to Home page within 10 seconds...<img src="Images/progress2.gif" alt=""/></div>
    </div>
</asp:content>
