<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="maindiv" style="height: 500px;">
        <div style="float: left; width: 760px;">
            <table class="freecontactform">
                <tr>
                    <td colspan="2">
                        <div class="freecontactformheader"><h1 class="">Contact Us Form</h1></div>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="tdalign">
                        <label for="Full_Name" >Full Name<span> * </span></label>
                    </td>
                    <td class="tdalign">
                        <input type="text" runat="server" name="Full_Name" id="Full_Name" maxlength="80"  class="Designer-input">
                    </td>
                </tr>
                <tr>
                    <td class="tdalign">
                        <label for="Email_Address" >Email Address<span> * </span></label>
                    </td>
                    <td class="tdalign">
                        <input type="text" runat="server" name="Email_Address" id="Email_Address" maxlength="100" class="Designer-input">
                    </td>
                </tr>
                <tr>
                    <td class="tdalign">
                        <label for="Telephone_Number" >Telephone Number</label>
                    </td>
                    <td class="tdalign">
                        <input type="text"  runat="server" name="Telephone_Number" id="Telephone_Number" maxlength="100" class="Designer-input">
                    </td>
                </tr>
                <tr>
                    <td class="tdalign">
                        <label for="Your_Message" >Your Message<span> * </span></label>
                    </td>
                    <td class="tdalign">
                        <textarea  runat="server" style="height: 160px" name="Your_Message" id="Your_Message" maxlength="2000" class="Designer-input"></textarea>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                       <input type="submit" runat="server" onserverclick="Submit_Click" value=" Submit Form" class="btnLogin" style="width: 185px; height: 30px; float:left; margin:0px 0px 5px -2px;">
                        <asp:Literal ID="ltMessage" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <div class="imgdiv">
        </div>
    </div>
</asp:Content>

