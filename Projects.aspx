<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Projects.aspx.cs" Inherits="Projects" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .background tr td /*table bacckground*/
        {
            background-color: #ECECEC;
        }
    </style>
    
    <script>
       $(document).ready(function ()
        {
            $('#tabs').tabs({
                select: function (event, ui)
                {
                    var tabNumber = ui.index;
                    var tabName = $(ui.tab).text();
                    console.log('Tab number ' + tabNumber + ' - ' + tabName + ' - clicked');
                }
            });

            //$('#tabs').bind('tabsselect', function (event, ui)
            //{
            //    alert();
            //    ui.options // options used to intialize this widget
            //    ui.tab // anchor element of the selected (clicked) tab
            //    ui.panel // element, that contains the contents of the selected (clicked) tab
            //    ui.index // zero-based index of the selected (clicked) tab
            //});
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="PanelProjectList" runat="server" Visible="false">

        <asp:Literal ID="ltProjectList" runat="server"></asp:Literal>
        <div id="EditDialog" style="display: none;">
            <iframe id="editframe" style="border: 1px solid #c8f0f8;"></iframe>
        </div>

    </asp:Panel>
    <asp:Panel ID="PanelProjectDetail" runat="server" Visible="false">

        <div id="tabs" style="padding:10px;">
            <ul>
                <li><a href="#tabs-1">Overview</a></li>
                <li><a href="#tabs-2">Features</a></li>
                <li><a href="#tabs-3">Map</a></li>
                <li><a href="#tabs-4">Images</a></li>
                <li><a href="#tabs-7">Services</a></li>
                <li><a href="#tabs-5">Other Project</a></li>
                <li><a href="#tabs-6">Agents</a></li>
            </ul>
            <div id="tabs-1">
                <table>
                    <tr>
                        <td style="width: 170px;">
                            <asp:Literal ID="lblLogo" runat="server"></asp:Literal></td>
                        <td>
                            <div class="projnameandadd">
                                <h1>
                                    <asp:Literal ID="ltrProjectName" runat="server"></asp:Literal>
                                </h1>
                                <div>
                                    <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>

                                </div>
                                Builder:<a id="BuilderName" runat="server"></a>
                            </div>
                        </td>
                        <td>
                            <a href="#" onclick="window.print()">
                                <img src="Images/Printer-icon.png" alt="Print" title="Print This Page" /></a>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Literal ID="ltAptTypeTable" runat="server"></asp:Literal>
                <br />
                <p class="SocietyDetail">
                    <asp:Literal ID="ltrDetail" runat="server"></asp:Literal>
                </p>
            </div>
            <div id="tabs-2">
                <asp:Literal ID="ltrAminities" runat="server" Text=""></asp:Literal>


                <ul id="projectmenulist">
                    <li><a runat="server" id="MapLink">Map</a></li>
                    <li>
                        <asp:Literal ID="ltSiteplan" runat="server"></asp:Literal></li>
                    <li>
                        <a href="#" class="manualfancybox">Gallary</a>
                    </li>
                    <li>
                        <asp:HyperLink class="fancyboxvedio" ID="fancyBoxVedio" runat="server">Video</asp:HyperLink>
                    </li>
                </ul>
            </div>
            
            <div id="tabs-3">
                <asp:Literal ID="ltLndmrkSideist" runat="server"></asp:Literal>
            </div>
            
            <div id="tabs-4">
              <asp:Literal ID="ltrGallary" runat="server"></asp:Literal>
            </div>
            
            <div id="tabs-7">
                Services Details
            </div>
            <div id="tabs-5">
                <div style="border-top: 1px solid #ECECEC; border-bottom: 1px solid #ECECEC; padding: 2px 0px 2px 0px;">
                    Other Projects by 
                </div>
                <asp:Literal ID="ltOtherProjects" runat="server"></asp:Literal>
            </div>
            <div id="tabs-6">
                <asp:Literal ID="ltrEmi" runat="server"></asp:Literal>
            </div>
        </div>
        <div style="height: 0px; width: 0px;" id="DivDialog">
            <iframe id="frameMap2" style="width: 99%; height: 99%; border: 0px;"></iframe>
        </div>
    </asp:Panel>
</asp:Content>
