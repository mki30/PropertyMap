<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BuilderControl.ascx.cs" Inherits="UserControl_BuilderControl" %>
<style type="text/css">
        .page-header
        {
            margin: 0px 0 15px;
        }

        h1
        {
            margin: 0px 0;
        }

        table
        {
            font-size: 12.5px;
        }

        .table th, .table td
        {
            padding: 0px 0px 4px 6px;
        }

        .input-append .add-on
        {
            border-radius: 0px 4px 4px 0px;
        }

        .builderlogo
        {
            height: 65px;
        }

        .box-shadow
        {
            -moz-box-shadow: 0px 0 3px #ccc;
            -webkit-box-shadow: 0px 0 3px #ccc;
            -o-box-shadow: 0px 0 3px #ccc;
            box-shadow: 0px 0 3px #ccc;
        }

        .item
        {
            padding: 20px 30px 10px 30px;
        }
    </style>
    <script type="text/javascript">
        $(function ()
        {
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

                lastXhr = $.getJSON(BasePath + "Data.aspx?Action=AllSearch&Data1=1", request, function (data, status, xhr)
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
  <div class="item box-shadow">
        <div class="page-header">
            <div class="row-fluid">
                <div class="span12 input-append search-box" style="text-align: center;">
                    <input class="span6" id="txtSearch" type="text" placeholder="Search for Builders" autofocus="autofocus" />
                    <span class="add-on"><i class="icon-search"></i></span>
                    <div id="results"></div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span2">
                    <asp:Literal runat="server" ID="ltBuilderLogo"></asp:Literal>
                </div>
                <div class="span10">
                    <div class="row-fluid">
                        <div class="span9">
                            <h1>
                                <asp:Literal runat="server" ID="ltBuilderName"></asp:Literal>
                            </h1>
                        </div>
                        <div class="pull-right">
                           <asp:Literal runat="server" ID="ltURL"></asp:Literal>
                        </div>
                    </div>
                    <div>
                        <div class="Span4" style="font-size: 12px;">
                            <span style="font-weight: bold;">Address : </span>
                            <asp:Literal runat="server" ID="ltBuilderAdd"></asp:Literal>
                        </div>
                    </div>
                    <div class="row-fluid" style="font-size: 12px;">
                        <div class="Span6">
                            <span style="font-weight: bold;">Contact : </span>
                            <asp:Literal runat="server" ID="ltBuilderCont"></asp:Literal>
                        </div>
                        <div class="Span6">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid" style="text-align: justify;">
            <p>
                <asp:Literal ID="ltBuilderDetail" runat="server"></asp:Literal>
            </p>
        </div>
        <div id="ProjHead" runat="server" visible="false"></div>
        <div>
            <asp:Literal ID="ltBuilderProjects" runat="server"></asp:Literal>
        </div>
    </div>