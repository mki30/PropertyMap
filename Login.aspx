<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="NewLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #loginDiv
        {
            padding:10px;
            border-radius:10px;
            background: #999; /* for non-css3 browsers */
            /*filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#cccccc', endColorstr='#000000');  for IE 
            background: -webkit-gradient(linear, left top, left bottom, from(#ccc), to(#000));  for webkit browsers 
            background: -moz-linear-gradient(top,  #ccc,  #000);  for firefox 3.6+*/ 
            background-color:gainsboro; padding:10px; border-radius:10px; 
            background: -webkit-gradient(linear, left top, left bottom, from(white), to(#DBE7B3));
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <%--  <div style="height: 50px; background-color: #DBE7B3; text-align: right; background: -webkit-gradient(linear, left top, left bottom, from(white), to(#DBE7B3));">
        <br />
        <span style="margin-right: 30px; font-size: 20px;">New User? <a href="Registration.aspx" style="color: #f39005;">Regester Now!</a></span></div>--%>
    <asp:Label ID="lblResponseMessage" runat="server" Text="Label"></asp:Label>
  <div class="row-fluid">
            <div class="span4"></div>
            <div class="span4" id="loginDiv">
                <h2>Please sign in</h2>
                <asp:TextBox ID="txtUserName" runat="server" placeholder="Email address" class="input-block-level"></asp:TextBox>
                <asp:TextBox ID="txtPass" runat="server" placeholder="Password" TextMode="Password" Text="123" class="input-block-level"></asp:TextBox>
                <label class="checkbox"><input type="checkbox" value="remember-me">
                    Remember me
                </label>
                <asp:Button ID="btnLogin" runat="server" Text="Login"  class="btn btn-primary" style="width:100%;" OnClick="btnLogin_Click"/>
                <asp:Label ID="Label1" runat="server" Text="Label" Style="padding-left: 25px;"></asp:Label>
            </div>
            <div class="span4"></div>
        </div>
 </asp:Content>

