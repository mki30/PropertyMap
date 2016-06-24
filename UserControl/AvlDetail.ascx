<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AvlDetail.ascx.cs" Inherits="UserControl_AvlDetail" %>
<style type="text/css">
    .bold
    {
        font-weight:bold;
    }
</style>
<div class="row-fluid">
        <div class="span3 bs-docs-sidebar">
            <div>
                <ul class="nav nav-list bs-docs-sidenav affix">
                    <li class="active"><a href="#overview"><i class="icon-th-large"></i>Overview</a></li>
                    <li><a href="#Images"><i class="icon-picture"></i>Images</a></li>
                    <li><a href="#ProjDesc"><i class=" icon-certificate"></i>Project Description</a></li>
                    <li><a href="#ProjectGallery"><i class="icon-qrcode"></i>Project Gallery</a></li>
                    <li><a href="#ProjectLayout"><i class="icon-retweet"></i>Project Layout</a></li>
                    <li><a href="#otherprojects"><i class="icon-list"></i>Other Postings</a></li>
                    <li><a href="#contact"><i class="icon-user"></i>Contact Agent</a></li>
                    <li><a id="DownloadPdf"  href="#" runat="server" onserverclick="DownloadPdf_ServerClick"><i class="icon-file" ></i>Download PDF</a></li>
                </ul>
            </div>
        </div>
        <div class="span9">
            <section id="overview" style="padding-top: 2px;">
                <h2><asp:Literal ID="ltOverviewHead" runat="server"></asp:Literal><img runat="server" id="societyImg" class="pull-right" style="height:40px; width:50px;" alt=""/></h2>
               <div class="row-fluid">
                   <asp:Literal ID="ltAvaldetails" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="Images" style="padding-top: 0px;">
                <div class="row-fluid">
                <asp:Literal ID="ltImages" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="ProjDesc" style="padding-top: 0px;">
                <h2 id="ProjDescHead" runat="Server"><asp:Literal ID="ltProjDescHead" runat="server"></asp:Literal></h2>
                <div class="row-fluid">
                <asp:Literal ID="ltProjectDesc" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="ProjectGallery" style="padding-top: 0px;">
                <h2 id="ProjectGalleryHead" runat="Server"><asp:Literal ID="ltProjectGalleryHead" runat="server"></asp:Literal></h2>
                <div class="row-fluid">
                <asp:Literal ID="ltProjectGallery" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="ProjectLayout" style="padding-top: 0px;">
                <h2 id="ProjectLayoutHead" runat="Server"><asp:Literal ID="ltProjectLayoutHead" runat="server"></asp:Literal></h2>
                <div class="row-fluid">
                <asp:Literal ID="ltProjectLayout" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="otherprojects" style="padding-top: 0px;">
                <h2 id="otherprojectshead" runat="server">Other Postings</h2>
                <div class="row-fluid">
                    <asp:Literal ID="ltOtherPostings" runat="server"></asp:Literal>
                </div>
            </section>
            <section id="contact" style="padding-top: 0px;">
                <h2 id="contacthead" runat="server">Contact</h2>
                <div class="row-fluid">
                    <asp:Literal ID="ltContact" runat="server"></asp:Literal>
                </div>
            </section>
         </div>
</div>