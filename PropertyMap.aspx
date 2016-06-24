<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PropertyMap.aspx.cs" Inherits="PropertyMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="Distance of each society from Landmark" />
    <link id="link" runat="server" rel="canonical" />

    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="http://www.google.com/jsapi?key=ABQIAAAAwbkbZLyhsmTCWXbTcjbgbRSzHs7K5SvaUdm8ua-Xxy_-2dYwMxQMhnagaawTo7L1FE1-amhuQxIlXw"></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/QueryString.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/MarkerCluster.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/Map.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/markerwithlabel_packed.js")%>'></script>

    <script type="text/javascript">

        CurrentPage = "map";

        function ShowProjects()
        {
        }

        $(window).resize(LocalResize);

        function LocalResize()
        {
            var H = $(window).height(), W = $(window).width();
            $("#divArea").height(H - 120);
            $("#divProject").height(H - 80);
            $("#map_canvas").height(H - 110);
            $("#map_canvas3D").height(H - 110);
        }

        $(function ()
        {
            $("#footer").remove();

            LocalResize();

            $("#txtSearch").autocomplete({
                minLength: 2, select: function (event, ui)
                {
                    console.log(ui.item.urlname);
                    window.location = ui.item.urlname;
                }, source: SendRequest
            });

            var cache = {}, lastXhr;
            function SendRequest(request, response)
            {
                var term = request.term;
                if (term in cache)
                {
                    response(cache[term]); return;
                }

                lastXhr = $.getJSON(BasePath + "Data.aspx?Action=AllSearch&Data1=2&Data2=map", request, function (data, status, xhr)
                {
                    cache[term] = data;
                    if (xhr === lastXhr)
                    {
                        response(data);
                    }
                });
            }
        });

    </script>
    <style type="text/css">
        /*@media only screen and (max-width: 640px)
        {
            nav ul
            {
                display: none;
            }
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <h1 style="margin-top:-50px; font-size:8px;"><asp:Label ID="ltHeading" runat="server">map</asp:Label></h1>
    <div class="row-fluid">
        <div class="span12">
            <div class="span2" style="text-align: center;">
                <select id="ddCity"></select>
            </div>
            <div class="span2" style="text-align: center">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <select id="ddSubCity" onchange="HightLightPoly($(this).val())"></select>
            </div>
            <div class="span2" style="text-align: center">
                <select id="ddProjects" onchange="ShowProject($(this).val(),true,false)"></select>
            </div>
            <div class="span6">
                <div class="span2">
                    <%--<select id="ddMapSelect" class="input-small">
                      <option value="1">2D Map</option>
                      <option value="2">3D Map</option>
                  </select>--%>
                </div>
                <div class="span10 input-append" style="text-align: center">
                    <input class="span10" id="txtSearch" type="text" placeholder="Search Projects ...">
                    <button class="btn" type="button">Search</button>
                </div>
            </div>
        </div>
        <div class="row-fluid;">
            <div id="map_canvas" style="width: 100%; height: 600px;"></div>
            <div id="map_canvas3D" style="width: 100%; height: 600px;"></div>
        </div>
    </div>
</asp:Content>


