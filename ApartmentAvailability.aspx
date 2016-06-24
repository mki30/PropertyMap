<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApartmentAvailability.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <title></title>
    <script type="text/javascript" src="js/PropertyList.js"></script>
    <script type="text/javascript" src="js/Common.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="css/Simple.css" />--%>
    <link rel="stylesheet" type="text/css" href="fancybox/jquery.fancybox-1.3.4.css" />
    <script type="text/javascript" src="fancybox/jquery.fancybox-1.3.4.pack.js"></script>


    <link rel="Stylesheet" type="text/css" href="css/smoothDivScroll.css" />
    <script src="js/jquery.mousewheel.min.js" type="text/javascript"></script>
    <script src="js/jquery.kinetic.js" type="text/javascript"></script>
    <script src="js/jquery.smoothdivscroll-1.3-min.js" type="text/javascript"></script>


    <style type="text/css">
        #makeMeScrollable
        {
            width: 1000px;
            height: 330px;
            position: relative;
        }

            /* Replace the last selector for the type of element you have in
	        your scroller. If you have div's use #makeMeScrollable div.scrollableArea div,
	        if you have links use #makeMeScrollable div.scrollableArea a and so on. */
            #makeMeScrollable div.scrollableArea img
            {
                position: relative;
                float: left;
                margin: 0;
                padding: 0;
                /* If you don't want the images in the scroller to be selectable, try the following
	        block of code. It's just a nice feature that prevent the images from
	        accidentally becoming selected/inverted when the user interacts with the scroller. */
                -webkit-user-select: none;
                -khtml-user-select: none;
                -moz-user-select: none;
                -o-user-select: none;
                user-select: none;
            }
    </style>
    <script type="text/javascript">
        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="border-spacing: 0px;">
        <tr>
            <td>
                <table style="border-spacing: 0px;">
                    <tr>
                        <td>
                            <table style="border-spacing: 0px;">
                                <tr>
                                    <td>City
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCityList" runat="server" CssClass="drp2">
                                            <%--<asp:ListItem Value="-1">&lt;-Select-&gt;</asp:ListItem>
                                            <asp:ListItem Value="0">Delhi</asp:ListItem>--%>
                                            <asp:ListItem Value="1">Gaziabad</asp:ListItem>
                                            <%--<asp:ListItem Value="2">Noida</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </td>

                                    <td>Area
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddArea" runat="server" CssClass="drp2">
                                            <%--<asp:ListItem Value="-1">&lt;-Select-&gt;</asp:ListItem>--%>
                                            <asp:ListItem Value="0">Indrapuram</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="float: right;">
                                        <asp:Button ID="btnForRent" runat="server" Text="For Rent" CssClass="btnLogin" />
                                    </td>
                                    <td style="float: right;">
                                        <asp:Button ID="btnForSale" runat="server" Text="For Sale" CssClass="btnLogin" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divSociety" style="height: 300px; overflow: auto">
                            </div>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divDetails" style="height: 300px; overflow: auto">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 0px 2px 0px 2px;">
                <div style="border-left: 1px solid black; height: 640px">
                </div>
            </td>
            <td style="width: 300px; vertical-align: top;">
                <b class="heading">Society Deatil</b>
                <div id="divSocietyDetail" style="min-height: 300px"></div>
                <hr style="border-color: #f1f1f1" />
                <b class="heading">Agent Detail</b>
                <div id="divAgentDetail"></div>
            </td>
        </tr>
    </table>
    <div id="images" runat="server"></div>
    <a id="inline" href="#DivDialog"></a>
    <div id="DivDialog" style="height: 0px; width: 0px;">
        <iframe id="frameMap" style="height: 99%; width: 100%; border: 0px;"></iframe>
    </div>

    <%--<div id="makeMeScrollable" style="height:260px;">
    </div>--%>
</asp:Content>

