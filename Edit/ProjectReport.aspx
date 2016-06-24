<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="ProjectReport.aspx.cs" Inherits="Edit_ProjectReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/TableSearch.js"></script>
    <script src="../js/jquery.tablesorter.min.js"></script>
    <script src="../js/Cookie.js"></script>
    <style type="text/css">
        .badge
        {
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
        }
        
        input[type="checkbox"]
        {
            width: 16px;
            height: 16px;
            vertical-align: middle;
        }
        
        table#DataTable thead tr .header {
        background-image: url(/Images/shorter-bg.gif);
        background-repeat: no-repeat;
        background-position: center right;
        cursor: pointer;
        }
        tr.active {background: red;
    }
</style>
    <script type="text/javascript">

        var cityid = 0;

        $(document).ready(function ()
        {
            $(".fancybox").fancybox();

            $(".fancybox1").fancybox(
               {
                   autoDimensions: false,
                   height: 800,
                   width: 1200,
                   enableEscapeButton: true
               });

            var link = "EditSociety.aspx?CityId=" + $.cookie("CityID");
            $("#AddProject").prop("href", link);

            $("#DataTable").tablesorter();     //Sort table
        });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top; width: 200px;">
                <%--<span><asp:CheckBox ID="chkShowDeleted" runat="server" Text="Show Deleted" /></span>--%>
                <asp:Button ID="btnReload" runat="server" Text="ReLoad" OnClick="btnReload_Click" class="btn btn-mini" />
                <asp:ListBox ID="lstProjects" runat="server" AutoPostBack="True" Height="820px" OnSelectedIndexChanged="lstProjects_SelectedIndexChanged" Width="197px"></asp:ListBox>
            </td>
            <td style="vertical-align: top;">
                <div>
                    <input id='txtName' type='text' placeholder='Search…' style='margin-top: 5px; border-radius: 2px;' autocomplete="off"/>
                    <div style="max-height: 10px; display: inline;">
                        <asp:CheckBox ID="CheckShowAll" runat="server" Text="All" AutoPostBack="True" OnCheckedChanged="CheckShowAll_CheckedChanged" />
                        <asp:CheckBox ID="chkVerified" runat="server" Text="Verified" Checked="true" AutoPostBack="True" OnCheckedChanged="chkVarified_CheckedChanged" />
                        <asp:CheckBox ID="ChkDeleted" runat="server" Text="Deleted" AutoPostBack="True" OnCheckedChanged="ChkDeleted_CheckedChanged" />
                        <asp:CheckBox ID="ChkLastUpdated" runat="server" Text="By LastUpdated" AutoPostBack="True" OnCheckedChanged="ChkLastUpdated_CheckedChanged" />
                        <asp:CheckBox ID="ChkOrderByBuilder" runat="server" Text="By Builder" AutoPostBack="True" OnCheckedChanged="ChkOrderByBuilder_CheckedChanged" />
                    </div>
                    <div style="display: inline; margin-left: 20px;"><a id="AddProject" class="fancybox fancybox.iframe">Add New Project</a></div>
                    <div style="display: inline; margin-left: 20px;">
                        <asp:CheckBox ID="ChkWishTown" Text="WishTown" runat="server" AutoPostBack="True" OnCheckedChanged="CheckWishTown_CheckedChanged" />
                    </div>
                    <div style="overflow: auto; height: 800px;">
                       <asp:Literal ID="ltList" runat="server"></asp:Literal>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>





