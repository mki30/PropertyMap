<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" EnableViewState="false" %>
<%@ Register Src="~/UserControl/PropList.ascx" TagPrefix="uc1" TagName="PropList" %>
<%@ Register Src="~/UserControl/ProjectDetail.ascx" TagPrefix="uc1" TagName="ProjectDetail" %>
<%@ Register Src="~/UserControl/AgentMicrosite.ascx" TagPrefix="uc1" TagName="AgentMicrosite" %>
<%@ Register Src="~/UserControl/AgentEdit.ascx" TagPrefix="uc1" TagName="AgentEdit" %>
<%@ Register Src="~/UserControl/BuilderControl.ascx" TagPrefix="uc1" TagName="BuilderControl" %>
<%@ Register Src="~/UserControl/AvlDetail.ascx" TagPrefix="uc1" TagName="AvlDetail" %>
<%@ Register Src="~/UserControl/FloorPlans.ascx" TagPrefix="uc1" TagName="FloorPlans" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="msvalidate.01" content="4BB6A59C5D38BB0C4F26ED040CE38808" />
    <link rel="canonical" href="http://propertymap.info" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:AgentMicrosite runat="server" ID="AgentMicrosite" Visible="false" />
    <uc1:AgentEdit runat="server" ID="AgentEdit" Visible="false" />
    <uc1:PropList runat="server" ID="PropList" Visible="false" />
    <uc1:ProjectDetail runat="server" ID="ProjectDetail" Visible="false" />
    <uc1:BuilderControl runat="server" id="BuilderControl" Visible="false"/>
    <uc1:AvlDetail runat="server" ID="AvlDetail" Visible="false"/>
    <uc1:FloorPlans runat="server" ID="FloorPlans" Visible="false" />
    <%--<a href="#" class="scrollup">Scroll</a>--%>
</asp:Content>