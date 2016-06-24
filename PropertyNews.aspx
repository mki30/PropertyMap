<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PropertyNews.aspx.cs" Inherits="PropertyNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        CurrentPage = "news";
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="http://www.google.com/jsapi" type="text/javascript"></script>
        <style type="text/css">
    </style>
    <script type="text/javascript">
        /*
     * The Google NewsShow embeds a news slideshow on your page, letting your users see headlines 
     * and previews of Google News Search results, based on queries that you've selected.
     *
     * This sample will show how to specify queries for the News Show.
     * http://code.google.com/apis/ajaxsearch/documentation/newsshow.html
    */
        google.load("elements", "1", { packages: ["newsshow"] });

        function onLoad()
        {
            // Set the queries to USC Football and NHL
            var options = { "queryList": [{ "title": "Delhi", "q": "Delhi Property" }] }
            var newsShow = new google.elements.NewsShow(document.getElementById('divDelhiNews'), options);

            options = { "queryList": [{ "title": "Pune", "q": "Pune Property" }] }
            newsShow = new google.elements.NewsShow(document.getElementById('divPuneNews'), options);

            options = { "queryList": [{ "title": "Mumbai", "q": "Mumbai Property" }] }
            newsShow = new google.elements.NewsShow(document.getElementById('divMumbaiNews'), options);

            options = { "queryList": [{ "title": "Chennai", "q": "Chennai Property" }] }
            newsShow = new google.elements.NewsShow(document.getElementById('divChennaiNews'), options);

            options = { "queryList": [{ "title": "Bengaluru", "q": "Bengaluru Property" }] }
            newsShow = new google.elements.NewsShow(document.getElementById('divBengaluruNews'), options);
        }
        google.setOnLoadCallback(onLoad);
    </script>
    <div id="divBengaluruNews"></div>
    <br />
    <div id="divChennaiNews"></div>
    <br />
    <div id="divDelhiNews"></div>
    <br />
    <div id="divMumbaiNews"></div>
    <br />
    <div id="divPuneNews"></div>
    <br />

</asp:Content>

