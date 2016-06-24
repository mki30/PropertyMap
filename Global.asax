<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        string[] Folders = { "KMZ", "Watermark\\Images_ApartmentType", "Watermark\\Images_LayoutPlan", "Watermark\\Images_Society", "Images_Agent", "Newspaper", "Newspaper_Small", "Images_Availability" };

        foreach (string f in Folders)
            if (!Directory.Exists(Server.MapPath(@"~\") + f))
                Directory.CreateDirectory(Server.MapPath(@"~\Data\") + f);
        
        // Code that runs on application startup
        InitializeRoutes(RouteTable.Routes);
        Global.LoadGlobalData();
    }
    
    private void InitializeRoutes(RouteCollection routes)
    {
        routes.MapPageRoute(routeName: "pagenotfound", routeUrl: "pagenotfound", physicalFile: "~/NotFound.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" }, { "Data2", "" } });
        routes.MapPageRoute(routeName: "agentlist", routeUrl: "agentlist/{Data1}", physicalFile: "~/AgentList.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" } });
        //routes.MapPageRoute(routeName: "agentposting", routeUrl: "agentposting/{Data1}", physicalFile: "~/RequestForm/AgentPosting.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" } });
        //routes.MapPageRoute(routeName: "AgentSite", routeUrl: "agentsite/{Data1}/{Data2}", physicalFile: "~/Default.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Action", "agentsite" }, { "Data1", "" }, { "Data2", "" } });
        routes.MapPageRoute(routeName: "Blog", routeUrl: "blogs/{Data1}", physicalFile: "~/Blogs.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" } });
        routes.MapPageRoute(routeName: "Builder", routeUrl: "builder/{Data1}", physicalFile: "~/Builder.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" } });
        routes.MapPageRoute(routeName: "City", routeUrl: "city/{Data1}/{Data2}/{Data3}", physicalFile: "~/Default.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Action", "city" }, { "Data1", "" }, { "Data2", "" }, { "Data3", "" } });
        routes.MapPageRoute(routeName: "EMI-EquatedMonthlyInstallment", routeUrl: "EMI-EquatedMonthlyInstallment", physicalFile: "~/EMI-EquatedMonthlyInstallment.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "KML", routeUrl: "kml/{Data1}", physicalFile: "~/GenerateKML.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Data1", "" } });
        routes.MapPageRoute(routeName: "Loan", routeUrl: "Loan", physicalFile: "~/PropertyLoan.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "Login", routeUrl: "Login", physicalFile: "~/Login.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "Map", routeUrl: "map/{Data1}/{Data2}", physicalFile: "~/PropertyMap.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Action", "map" }, { "Data1", "" }, { "Data2", "" }, { "Data3", "" } });
        routes.MapPageRoute(routeName: "Map3D", routeUrl: "map3d/{Data1}/{Data2}", physicalFile: "~/PropertyMap.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Action", "map3d" }, { "Data1", "" }, { "Data2", "" }, { "Data3", "" } });
        routes.MapPageRoute(routeName: "PriceTrendList", routeUrl: "price-trend/{Data1}", physicalFile: "~/PriceTrendList.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Data1", "" } });
        routes.MapPageRoute(routeName: "Project", routeUrl: "project/{Data1}/{Data2}/{Data3}", physicalFile: "~/ProjectDetail.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" }, { "Data2", "" }, { "Data3", "" } });
        routes.MapPageRoute(routeName: "PropertyDocuments", routeUrl: "PropertyDocuments", physicalFile: "~/PropertyDocuments.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "PropertyNews", routeUrl: "PropertyNews", physicalFile: "~/PropertyNews.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "PropertyQuestionAnswer", routeUrl: "PropertyQuestionAnswer", physicalFile: "~/PropertyQuestionAnswer.aspx", checkPhysicalUrlAccess: true);
        routes.MapPageRoute(routeName: "ShowPriceTrend", routeUrl: "property-price-trend-graph/{Data1}", physicalFile: "~/ShowPriceTrend.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" } });
        routes.MapPageRoute(routeName: "PriceEstimate", routeUrl: "price-estimate", physicalFile: "~/PriceEstimate.aspx", checkPhysicalUrlAccess: true, defaults: new RouteValueDictionary() { { "Data1", "" }, { "Data2", "" } });
        routes.MapPageRoute(routeName: "Default", routeUrl: "{Data1}/{Data2}", physicalFile: "~/Default.aspx", checkPhysicalUrlAccess: false, defaults: new RouteValueDictionary() { { "Data1", "" }, { "Data2", "" } });
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
    
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

        Messages.LastException = Server.GetLastError().GetBaseException();
        Cmn.LogError(Messages.LastException, Messages.LastException.Message + " URL:" + Request.Url.ToString() + " Device:" + (Request.Browser.IsMobileDevice ? "Mobile" : "Desktop"));
        if (!Request.Url.Host.ToLower().Contains("localhost"))
            Response.Redirect("/404.htm");
    }
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
    }
    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
</script>
