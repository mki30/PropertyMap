<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgentMicrosite.ascx.cs" Inherits="UserControl_AgentMicrosite" %>
<%@ Register Src="~/UserControl/PropList.ascx" TagPrefix="uc1" TagName="PropList" %>

<style type="text/css">
    body
    {
        background: url(/images/bg.jpg) center top no-repeat #ffffff;
    }
</style>

<script type='text/javascript'>
    $(document).ready(function ()
    {
        $('.carousel').carousel({
            interval: 2500
        })
    });
</script>

<div class="row-fluid" style="background-image:url('/images/back50.png');color:white">
    <asp:Literal ID="ltLogo" runat="server"></asp:Literal>
    <h1 style="float:left;margin:20px auto; font-size:14px;"><%= agent.AgentCompany   %></h1>
    <a href="<%="/" + agent.URLName.ToLower() + "/edit"  %>" class="btn btn-primary pull-right" style="margin: 10px; " >Login</a>
</div>

<div class="row-fluid" style="position: relative">
    <div id="myCarousel" class="carousel slide" style="width: 100%;">
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
        </ol>
        <!-- Carousel items -->
        <div class="carousel-inner">
            <div class="active item">
                <img src="/images/slide21.jpg" style="width: 100%;" alt="slide1"/>
            </div>
            <div class="item">
                <img src="/images/slide22.jpg" style="width: 100%;" alt="slide2"/>
            </div>
            <div class="item">
                <img src="/images/slide24.jpg" style="width: 100%;" alt="slide3"/>
            </div>
        </div>
        <!-- Carousel nav -->
        <a class="carousel-control left" href="#myCarousel" data-slide="prev">&lsaquo;</a>
        <a class="carousel-control right" href="#myCarousel" data-slide="next">&rsaquo;</a>
    </div>

    <div style="position: absolute; top: 50px; right: 100px; color: white; font-size: 20px; z-index: 1000; padding: 10px;background-image:url('/images/back50.png') ">
        <i class="icon-user icon-white" title="Contact Person"></i>&nbsp;&nbsp;<span id="spnAgentName" runat="server"></span><br />
        <div style="margin-top: 10px">
            <i class="icon-bell icon-white"></i>&nbsp;&nbsp;<span id="lblMobile" runat="server"></span><br />
            <i class="icon-envelope icon-white"></i>&nbsp;&nbsp;<span id="lblEmail" runat="server"></span><br />
            <i class="icon-tasks icon-white" title="Address"></i>&nbsp;&nbsp;<span id="lblAddress" runat="server"></span><br />
            
       </div>
        <%--<div class="span12" id="divDetails" runat="server" style="text-align: justify;"></div>--%>

    </div>
</div>
<uc1:PropList runat="server" ID="PropList" />

