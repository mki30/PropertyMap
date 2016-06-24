<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectDetail.ascx.cs" Inherits="UserControl_ProjectDetail" %>
<%@ Register Src="~/UserControl/PropList.ascx" TagPrefix="uc1" TagName="PropList" %>

<style type="text/css">
    /*media only css*/
    @media only screen and (max-width: 640px)
    {
        table td:nth-child(3),
        table th:nth-child(3),
        table td:nth-child(6),
        table th:nth-child(6)
        {
            display: none;
        }
    }
    
    .sub-header
    {
        font-size: medium;
        text-align: right;
        color: #cbc8c8;
    }
    
    .nav-list > .active > a, .nav-list > .active > a:hover, .nav-list > .active > a:focus /*Bootstrap css overrite*/
    {
        color: #fff;
        text-shadow: 0 -1px 0 rgba(0,0,0,0.2);
        background-color: #79C7EE;
    }

    .tdminwidth
    {
        min-width: 60px;
    }

    .tdminwidth2
    {
        min-width: 75px;
    }
</style>

<script type="text/javascript">
    var url = "";
    $(document).ready(function ()
    {
        url = window.location;
        $(".fancybox").fancybox(
         {
             'width': '350',
         });
    });
    function htmlEncode(value) { return $('<div/>').text(value).html(); }
    function htmlDecode(value) { return $('<div/>').html(value).text(); }

    function GeneratePDF()
    {
        //var data = $("#aptTypeTable").html();
        //console.log($("#demo").html().replace('<tbody>', '').replace('</tbody>', ''));
        var HTML = htmlEncode($("#demo").html().replace('<tbody>', '').replace('</tbody>', ''));
        //var data = htmlEncode(text);
        //var HTML = htmlEncode($("#projdesc").html());
        //var HTML = htmlEncode($("#gallery").html());

        $.post("/Data.aspx?Action=GetPDF", { _HTML: HTML }, function (data)
        {
            window.open("/Data.aspx?Action=ShowPDF", "Menu of ", "width=800,height=800,screenX=50,left=50,screenY=50");
        });
    }
</script>

<div class="row-fluid">
    <div class="span3"></div>
    <div class="span9">
        <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
        <!-- PropertyMap.info -->
        <ins class="adsbygoogle"
            style="display: inline-block; width: 728px; height: 90px"
            data-ad-client="ca-pub-9629311196237402"
            data-ad-slot="9018992292"></ins>
        <script>
            (adsbygoogle = window.adsbygoogle || []).push({});
        </script>
    </div>
</div>

<div class="row-fluid">
    <div class="span3 bs-docs-sidebar">
        <div>
            <ul class="nav nav-list bs-docs-sidenav affix">
                <li class="active"><a href="#overview"><i class="icon-th-large"></i>Overview</a></li>
                <li style="display: none"><a href="#features"><i class="icon-star"></i>Features</a></li>
                <li><asp:Literal ID="ltMap" runat="server"></asp:Literal></li>
                <li><a href="#Images"><i class="icon-picture"></i>Images</a></li>
                <li><a href="#Layout"><i class="icon-retweet"></i>Layout Plan</a></li>
                <li>
                <asp:HyperLink ID="fancyBoxVedio" runat="server" Target="_blank"><i class="icon-facetime-video"></i>Video</asp:HyperLink></li>
                <asp:Literal ID="ltBoucher" runat="server"></asp:Literal>
                <asp:Literal ID="ltPriceList" runat="server"></asp:Literal>
                <asp:Literal ID="ltWebsite" runat="server"></asp:Literal>
                <li>
                <asp:HyperLink ID="GoogleEarth" runat="server"><i class="icon-fullscreen"></i>GoogleEarth</asp:HyperLink></li>
                <li><a href="#OtherProjects"><i class="icon-folder-close"></i>Other Projects</a></li>
                <%--<li><a id="AskQuestion" class="fancybox fancybox.iframe" href="/ProjectQ.aspx?ProjectID=<%=ID%>"><i class="icon-question-sign"></i>Feedback</a></li>--%>
                <li><a id="AskQuestion" href="/ProjectQ.aspx?ProjectID=<%=ID%>&url=<%=Request.Url.LocalPath.Replace("/","") %>" target="_blank"><i class="icon-question-sign"></i>Feedback</a></li>
                <li><a id="DownloadPdf"  href="#" runat="server" onserverclick="DownloadPdf_ServerClick"><i class="icon-file" ></i>Download PDF</a></li>
                <%--<li><a id="DownloadPdf" href="#" onclick="GeneratePDF()"><i class="icon-file"></i>Download PDF</a></li>--%>
                <%--<li>
                    <div style="background-color:#f1f1f1; height:190px; width:250px; border-radius:0px 0px 5px 5px;margin:5px;">SFS</div>
                </li>--%>
            </ul>
        </div>
    </div>

    <div class="span9" id="pdfdoc">
        <section id="overview" style="padding-top: 2px;">
            <%--<div class="page-header"></div>--%>
            <div class="row-fluid">
                <div class="span3">
                    <asp:Literal ID="lblLogo" runat="server"></asp:Literal>
                </div>
                <div class="span6">
                    <h1 style="margin-top: 2px; margin-bottom: 2px;">
                        <asp:Literal ID="ltrProjectName" runat="server"></asp:Literal>
                    </h1>
                    <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
                </div>

                <div class="span3" style="text-align: right">
                    <a id="BuilderName" runat="server"></a>
                </div>

                <div style="text-align: right">
                    Expected Possession <b>:</b>
                    <asp:Literal ID="ltpossessDate" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="row-fluid" style="width: 170px; float: right;">
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="ltfbLike" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltGplus" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="row-fluid" id="demo">
                <%--<h2><asp:Literal ID="ltAptTypeTableHead" runat="server">Units</asp:Literal></h2>--%>
                <asp:Literal ID="ltAptTypeTable" runat="server"></asp:Literal>
            </div>
            <div>
                <asp:Literal ID="ltrDetail" runat="server"></asp:Literal>
            </div>
        </section>

        <section id="Images" style="padding-top: 0px;">
            <h2 id="ImagesHead" runat="Server"></h2>
            <asp:Literal ID="ltrGallary" runat="server"></asp:Literal>
        </section>

        <section id="Layout" style="padding-top: 0px;">
            <h2 id="LayoutHead" runat="server"></h2>
            <div class="row-fluid">
                <asp:Literal ID="ltrLayout" runat="server"></asp:Literal>
            </div>
        </section>

        <section id="AdvSection" runat="server" visible="false" style="padding-top: 0px;">
            <h2>In Newspaper</h2>
            <div id="Ad_Image" runat="server"></div>
        </section>
        <section id="ProjectList" runat="server" visible="false">
            <uc1:PropList runat="server" ID="PropList" />
        </section>
        <section id="OtherProjects" style="padding-top: 0px; background-color: #F7F7F7; border-radius: 5px;" runat="server">
            <h2 id="BuilderProjectHead" style="padding-left: 5px;" runat="server">Other Projects <span id="builder" runat="server"></span></h2>
            <div class="row-fluid">
                <asp:Literal ID="ltProjects" runat="server"></asp:Literal>
            </div>
        </section>
        <section id="NearbyProjects" style="padding-top: 0px; background-color: #F7F7F7; border-radius: 5px;" runat="server">
            <h2 id="H1" runat="server" style="padding-left: 5px;">Nearby Projects</h2>
            <div class="row-fluid">
                <asp:Literal ID="ltNearBy" runat="server"></asp:Literal>
            </div>
        </section>
        <section id="AgentsList" style="padding-top: 0px; background-color: #F7F7F7; border-radius: 5px;" runat="server">
            <h2 id="H2" runat="server" style="padding-left: 5px;">Agents</h2>
            <div class="row-fluid">
                <asp:Literal ID="ltAgentList" runat="server"></asp:Literal>
            </div>
        </section>
        <%--<section id="DisQus">
<div id="disqus_thread"></div>
<script type="text/javascript">
/* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */
var disqus_shortname = 'propertymap'; // required: replace example with your forum shortname

/* * * DON'T EDIT BELOW THIS LINE * * */
(function ()
{
var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';
(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
})();
</script>
<noscript>Please enable JavaScript to view the <a href="http://disqus.com/?ref_noscript">comments powered by Disqus.</a></noscript>
<a href="http://disqus.com" class="dsq-brlink">comments powered by <span class="logo-disqus">Disqus</span></a>
</section>--%>
    </div>
</div>
<div style="display: none;" id="divForm">
    <%--Small Emi Calculator form--%>
    <table>
        <tr>
            <td>Loan Amount&nbsp;</td>
            <td>
                <input type="text" id="txtLoanAmount" /></td>
        </tr>
        <tr>
            <td>Interest Rates&nbsp;</td>
            <td>
                <input type="text" id="txtIntRates" /></td>
        </tr>
        <tr>
            <td>Loan Tenure&nbsp;</td>
            <td>
                <input type="text" id="txtTenure" /></td>
        </tr>
        <tr>
            <td>EMI&nbsp;</td>
            <td>
                <input type="text" id="txtEMI" />
            </td>
        </tr>
    </table>
</div>
